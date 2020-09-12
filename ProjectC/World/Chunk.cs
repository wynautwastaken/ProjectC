using System;
using System.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.view;

namespace ProjectC.world
{
    public class Chunk : IStorable
    {
        public const int ChunkWidth = 32;
        public const int ChunkHeight = 96;

        public Vector2 Position;
        
        public Tile[,] Tiles = new Tile[32,96];

        public Chunk(Point id, bool dontLoad = false)
        {
            Position = id.ToVector2() * new Vector2(ChunkWidth, ChunkHeight) * Tile.TileSize;
            if (!dontLoad)
            {
                Dimention.LoadChunk(this);
            }
        }
        
        public static void Draw(SpriteBatch batch, Chunk chunk)
        {
            //batch.Draw(Sprites.Rectangle,new Rectangle(chunk.Position.ToPoint(),new Point(32*8,96*8)), Color.White);
            foreach (var tile in chunk.Tiles)
            {
                if (tile != null)
                {
                    Tile.Draw(batch, tile);
                }
            }
        }

        public Point WorldToChunk(Vector2 position)
        {
            return ((position  - Position) / Tile.TileSize).ToPoint();
        }

        public Vector2 ChunkToWorld(Point position)
        {
            return position.ToVector2() * Tile.TileSize + Position;
        }
        
        public void PlaceTile(Tile tile, Point chunkpos)
        {
            Tiles[chunkpos.X,chunkpos.Y] = tile;
        }

        public JsonObject Save()
        {
            throw new System.NotImplementedException();
        }

        public bool Load(string data)
        {
            throw new System.NotImplementedException();
        }

        public Tile TileFrom(Point position)
        {
            return Tiles[position.X, position.Y];
        }
    }
}