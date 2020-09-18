using System;
using System.Diagnostics;
using System.Drawing;
using System.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.view;
using Point = Microsoft.Xna.Framework.Point;

namespace ProjectC.world
{
    public class Chunk : IStorable
    {
        public const int ChunkWidth = 16;
        public const int ChunkHeight = 32;

        public Vector2 Position;
        public Vector2 ChunkspacePosition;

        public const int ChunkTData = 0;
        public const int ChunkTSide = 1;
        public const int ChunkTColor = 2;
        public const int ChunkTMeta = 3;
        public const int TileDepth = 4;

        public int[,,] Tiles = new int[ChunkWidth,ChunkHeight,TileDepth];

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
            for (var x = 0; x < ChunkWidth; x++)
            {
                for (var y = 0; y < ChunkHeight; y++)
                {
                    TileHelper.Draw(batch, chunk, x, y);
                }
            }
        }
        public static void Step(Chunk chunk)
        {
            for (var x = 0; x < ChunkWidth; x++)
            {
                for (var y = 0; y < ChunkHeight; y++)
                {
                    TileHelper.Step(chunk, x, y);
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
        
        public static bool PointValid(Point point)
        {
            return point.X < Chunk.ChunkWidth && point.X >= 0 && point.Y < Chunk.ChunkHeight && point.Y >= 0;
        }

        public void PlaceTile(int data, int side, int color, int meta, Point chunkpos)
        {
            if (!PointValid(chunkpos)) return;
            Tiles[chunkpos.X, chunkpos.Y, ChunkTData] = data;
            Tiles[chunkpos.X, chunkpos.Y, ChunkTSide] = side;
            Tiles[chunkpos.X, chunkpos.Y, ChunkTColor] = color;
            Tiles[chunkpos.X, chunkpos.Y, ChunkTMeta] = meta;
        }

        public void RemoveTile(Point chunkpos)
        {
            if (!PointValid(chunkpos)) return;
            var data = Tiles[chunkpos.X, chunkpos.Y, ChunkTData];
            if (data == 0) return;
            
            TileHelper.UpdateNearbyTiles(ChunkToWorld(chunkpos));
            Tiles[chunkpos.X, chunkpos.Y, ChunkTData] = 0;
            Tiles[chunkpos.X, chunkpos.Y, ChunkTSide] = 0;
            Tiles[chunkpos.X, chunkpos.Y, ChunkTColor] = 0;
            Tiles[chunkpos.X, chunkpos.Y, ChunkTMeta] = 0;
        }

        public JsonObject Save()
        {
            throw new System.NotImplementedException();
        }

        public bool Load(string data)
        {
            throw new System.NotImplementedException();
        }

        public int TileFrom(Point position, int type)
        {
            if (position.X < 0 || position.Y < 0 || position.X >= ChunkWidth || position.Y >= ChunkHeight)
                return 0;
            try
            {
                return Tiles[position.X, position.Y,type];
            }
            catch(Exception e)
            {
                Debug.WriteLine(position.ToString());
                return 0;
            }
        }
    }
}