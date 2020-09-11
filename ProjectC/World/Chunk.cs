#nullable enable
using System;
using System.Collections.Generic;
using System.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.Engine;
using ProjectC.Engine.Objects;
using ProjectC.Engine.View;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectC.World
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
            return $"chunkid-[{_x},{_y}]";
        }
    }
    
    public class Chunk
    {
        public const int ChunkWidth = 32;
        public const int ChunkHeight = 256;
        
        public float seed = 0;
        private Vector2 WorldPos;
        private Tile[,] Tiles = new Tile[ChunkWidth,ChunkHeight];
        public Tile[] ExistentTiles;
        public ChunkIdentifier ChunkId;
        public Chunk(Vector2 position)
        {
            ExistentTiles = new Tile[ChunkWidth*ChunkHeight];
            WorldPos = position;
            var (x, y) = position;
            ChunkId = new ChunkIdentifier((int)x,(int)y);
        }
        public Chunk(JsonObject json)
        {
            ExistentTiles = new Tile[ChunkWidth*ChunkHeight];
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
                        var ntile = new Tile(tile, this, new Vector2(x, y));
                        Tiles[(x / ChunkWidth) / Tile.TileSize,(y / ChunkHeight) / Tile.TileSize] = ntile;
                        ExistentTiles[((x/Tile.TileSize)/ChunkWidth) + (y/Tile.TileSize/ChunkHeight)] = ntile;
                    }
                }
            }
        }

        public Chunk FindTouchingChunk(EnumSides direction)
        {
            var x = ChunkId.Pos.X;
            var y = ChunkId.Pos.Y;
            switch (direction)
            {
                case EnumSides.Left:
                {
                    var chunkid = new ChunkIdentifier((int) x - 1, (int) y);
                    if (ChunkedWorld.DoesChunkExist(chunkid))
                    {
                        return ChunkedWorld.LoadChunk(chunkid);
                    }

                    break;
                }
                case EnumSides.Right:
                {
                    var chunkid = new ChunkIdentifier((int) x + 1, (int) y);
                    if (ChunkedWorld.DoesChunkExist(chunkid))
                    {
                        return ChunkedWorld.LoadChunk(chunkid);
                    }
                    break;
                }
                default:
                    return null;
            }
            return null;
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
            if (tile == null) return false;
            var (x, y) = position;
            Tiles[(int) x % ChunkWidth, (int) y % ChunkHeight] = tile;
            ExistentTiles[(int)((x/ChunkHeight) + y)] = tile;
            return true;
        }

        public static void Render(SpriteBatch batch, Chunk chunk)
        {
            if (chunk == null) return;
            if (batch == null) return;
            foreach (var tile in chunk.ExistentTiles)
            {
                if (tile == null) return;
                Tile.Render(batch, tile);
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
            var pos = (position  / Tile.TileSize) - RealWorldPos(WorldPos);
            pos.X = (int)pos.X % ChunkWidth;
            pos.Y = (int)pos.Y % ChunkHeight;
            if (pos.X < 0) pos.X += ChunkWidth;
            if (pos.Y < 0) pos.Y += ChunkHeight; 
            return pos;
        }

        public static Vector2 RealWorldPos(Vector2 pos)
        {
            return pos * new Vector2(ChunkWidth, ChunkHeight);
        }
        
        public Vector2 ChunkToWorld(Vector2 position)
        {
            Console.WriteLine(position.ToString());
            var r = (position + RealWorldPos(WorldPos)) *  Tile.TileSize;
            Console.WriteLine(r.ToString());
            return r;
        }
    }
}