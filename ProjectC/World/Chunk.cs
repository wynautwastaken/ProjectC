using System;
using System.Diagnostics;
using System.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.view;

namespace ProjectC.world
{
    public class Chunk : IStorable
    {
        public const int ChunkWidth = 16;
        public const int ChunkHeight = 32;

        public Vector2 Position;
        public Vector2 ChunkspacePosition;
        
        public Tile[,] Tiles = new Tile[ChunkWidth,ChunkHeight];

        public Chunk(Point id, bool dontLoad = false)
        {
            ChunkspacePosition = id.ToVector2();
            Position = ChunkspacePosition * new Vector2(ChunkWidth, ChunkHeight);
            if (!dontLoad)
            {
                Dimension.LoadChunk(this);
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
            return (position  - Position).ToPoint();
        }

        public Vector2 ChunkToWorld(Point position)
        {
            return position.ToVector2() + Position;
        }
        
        public void PlaceTile(Tile tile, Point chunkpos)
        {
            Tiles[chunkpos.X,chunkpos.Y] = tile;
        }

        public void RemoveTile(Point chunkpos)
        {
            var tile = Tiles[chunkpos.X, chunkpos.Y];
            if (tile != null)
            {
                Tiles[chunkpos.X, chunkpos.Y] = null;
                Dimension.UnloadTile(tile);
            }
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
            try
            {
                return Tiles[position.X, position.Y];
            }
            catch(Exception e)
            {
                Debug.WriteLine(position.ToString());
            }
            return Dimension.VoidTile;
        }
    }
}