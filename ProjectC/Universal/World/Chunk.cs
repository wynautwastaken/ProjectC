using System;
using System.Json;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.Engine;
using ProjectC.Engine.Objects;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectC.Universal.World
{
    public readonly struct ChunkIdentifier
    {
        private readonly int _x;
        private readonly int _y;
        public ChunkIdentifier(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public JsonObject Save()
        {
            var json = new JsonObject();
            json.Add("x",_x);
            json.Add("y",_y);
            return json;
        }
        
        public Vector2 Pos => new Vector2(_x, _y);

        public Chunk FindChunk()
        {
            return Chunk.GetFromId(this);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_x, _y);
        }

        public override bool Equals(object? obj)
        {
            if (obj is ChunkIdentifier chunkid)
            {
                return chunkid._x == _x && chunkid._y == _y;
            }
            return false;
        }

        public override string ToString()
        {
            return "chunkid-[" + _x.ToString() + "," + _y.ToString() + "]";
        }
    }
    
    public class Chunk
    {
        private Vector2 WorldPos;
        private Tile[,] Tiles = new Tile[256,256];
        public ChunkIdentifier ChunkId;
        public Chunk(Vector2 position)
        {
            WorldPos = position;
            var (x, y) = position;
            ChunkId = new ChunkIdentifier((int)x,(int)y);
            for(var i = 255; i > Tiles.GetUpperBound(0); i--)
            {
                for (var j = 255; j > Tiles.GetUpperBound(1); j--)
                {
                    Tiles[i, j] = null;
                }
            }
        }
        public Chunk(JsonObject json)
        {
            json.TryGetValue("position", out var position);
            ((JsonObject) position).TryGetValue("x", out var x_x);
            ((JsonObject) position).TryGetValue("y", out var y_y);
            var x_ = (int) x_x;
            var y_ = (int) y_y;
            WorldPos = new Vector2(x_,y_);
            ChunkId = new ChunkIdentifier(x_,y_);
            if (json.TryGetValue("data",out var data))
            {
                var arr = (JsonArray) data;
                foreach (var val in arr)
                {
                    ((JsonObject) val).TryGetValue("type", out var otype);
                    if (otype == "tile")
                    {
                        ((JsonObject) val).TryGetValue("tiletype", out var ttype);
                        var tile = (EnumTiles)(int)ttype;
                        ((JsonObject) val).TryGetValue("position", out var pos);
                        ((JsonObject) pos).TryGetValue("x", out var xx);
                        ((JsonObject) pos).TryGetValue("y", out var yy);
                        var x = (int) xx;
                        var y = (int) yy;
                        Tiles[(x / 256) / 8,(y / 256) / 8] = new Tile(tile, this, new Vector2(x,y));
                    }
                }
            }
        }
        
        public JsonObject Save()
        {
            var json = new JsonObject();
            var arr = new JsonArray();
            foreach (var storable in Tiles)
            {
                if (storable != null)
                {
                    arr.Add(storable.Save());
                }
            }

            json.Add("position", ChunkId.Save());
            json.Add("data", arr);
            return json;
        }

        public bool PlaceTile(Tile tile, Vector2 position)
        {
            var (x, y) = position;
            Tiles[(int) x, (int) y] = tile;
            return true;
        }

        public void Render(SpriteBatch batch)
        {
            foreach (var tile in Tiles)
            {
                tile?.Render(batch);
            }
        }
        
        public static Chunk GetFromId(ChunkIdentifier id)
        {
            ChunkedWorld.Chunks.TryGetValue(id, out var chunk);
            return chunk;
        }
        
        public Tile GetTileFrom(Vector2 position)
        {
            var (x, y) = position;
            return Tiles[(int) x, (int) y];
        }

        public Vector2 WorldToChunk(Vector2 position)
        {
            var pos = (position - WorldPos) / 8;
            pos.X = (int)pos.X % 256;
            pos.Y = (int)pos.Y % 256;
            if (pos.X < 0) pos.X += 255;
            if (pos.Y < 0) pos.Y += 255; 
            return pos;
        }
    }
}