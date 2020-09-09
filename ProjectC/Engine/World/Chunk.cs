using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.Engine.Objects;
using ProjectC.Engine.View;
using ProjectC.Engine.World;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectC.Engine.World
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

        public bool PlaceTile(Tile tile, Vector2 position)
        {
            var (x, y) = position;
            Tiles[(int) x, (int) y] = tile;
            return true;
        }

        public void Render(SpriteBatch batch)
        {
            foreach (Tile tile in Tiles)
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