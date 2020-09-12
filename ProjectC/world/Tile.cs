using System;
using System.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.objects;
using ProjectC.view;
using ProjectC.world;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectC.world
{
    public class Tile : IStorable
    {
        public const int TileSize = 8;

        private Texture2D Sheet = Sprites.TileFresh;
        private EnumSides Side = EnumSides.None;
        private Vector2 Position;
        public Color Tint = Color.White;
        public Chunk ChunkIn;
        public EnumTiles TileType { get; private set; } = EnumTiles.Air;

        public static void UpdateRenderedSide(Tile tile)
        {
            var tileLeft = Dimention.Current.TileAtWorldPos(tile.Position - (Vector2.One * TileSize), false);
            var tileRight = Dimention.Current.TileAtWorldPos(tile.Position + (Vector2.One * TileSize), false);
            var tileUp = Dimention.Current.TileAtWorldPos(tile.Position - (Vector2.One * TileSize), false);
            var tileDown = Dimention.Current.TileAtWorldPos(tile.Position + (Vector2.One * TileSize), false);
            bool left = TileExists(tileLeft), right = TileExists(tileRight), up = TileExists(tileUp), down = TileExists(tileDown);
            
            if (left && !right && !up && !down)
            {
                tile.Side = EnumSides.Right;
            }
            if (!left && right && !up && !down)
            {
                tile.Side = EnumSides.Left;
            }
            if (!left && !right && up && !down)
            {
                tile.Side = EnumSides.Bottom;
            }
            if (!left && !right && !up && down)
            {
                tile.Side = EnumSides.Top;
            }
            
            if (left && right && !up && !down)
            {
                tile.Side = EnumSides.MiddleHorizontal;
            }
            if (!left && !right && up && down)
            {
                tile.Side = EnumSides.MiddleVertical;
            }
            if (left && right && up && down)
            {
                tile.Side = EnumSides.Middle;
            }
            if (!left && !right && !up && !down)
            {
                tile.Side = EnumSides.None;
            }
            
            if (left && right && up && !down)
            {
                tile.Side = EnumSides.BottomMiddle;
            }
            if (left && right && !up && down)
            {
                tile.Side = EnumSides.TopMiddle;
            }
            if (!left && right && up && down)
            {
                tile.Side = EnumSides.MiddleLeft;
            }
            if (left && !right && up && down)
            {
                tile.Side = EnumSides.MiddleRight;
            }
            
            if (!left && right && !up && down)
            {
                tile.Side = EnumSides.TopLeft;
            }
            if (left && !right && !up && down)
            {
                tile.Side = EnumSides.TopRight;
            }
            if (!left && right && up && !down)
            {
                tile.Side = EnumSides.BottomLeft;
            }
            if (left && !right && up && !down)
            {
                tile.Side = EnumSides.BottomRight;
            }
            
        }

        public static void Draw(SpriteBatch batch, Tile tile)
        {
            if (tile != null)
            {
                batch.Draw(tile.Sheet, tile.Position, new Rectangle(new Point((int) tile.Side % 4, (int) tile.Side / 4), new Point(8, 8)), tile.Tint);
            }
        }

        public Tile(EnumTiles type, Chunk chunk, Point position)
        {
            TileType = type;
            ChunkIn = chunk;
            Position = chunk.ChunkToWorld(position);
            chunk.PlaceTile(this, position);
            Dimention.LoadTile(this);
            UpdateRenderedSide(this);
            UpdateNearbyTiles(Position);
        }

        public static void UpdateNearbyTiles(Vector2 worldPos)
        {
            var tileLeft = Dimention.Current.TileAtWorldPos(worldPos - (Vector2.One * TileSize), false);
            if (TileExists(tileLeft))
            {
                UpdateRenderedSide(tileLeft);
            }

            var tileRight = Dimention.Current.TileAtWorldPos(worldPos + (Vector2.One * TileSize), false);
            if (TileExists(tileRight))
            {
                UpdateRenderedSide(tileRight);
            }

            var tileUp = Dimention.Current.TileAtWorldPos(worldPos - (Vector2.One * TileSize), false);
            if(TileExists(tileUp))
            {
                UpdateRenderedSide(tileUp);
            }
            var tileDown = Dimention.Current.TileAtWorldPos(worldPos + (Vector2.One * TileSize), false);
            if (TileExists(tileDown))
            {
                UpdateRenderedSide(tileDown);
            }
        }
        
        public JsonObject Save()
        {
            throw new NotImplementedException();
        }

        public bool Load(string data)
        {
            throw new NotImplementedException();
        }

        public static bool TileExists(Chunk chunk, Point chunkpos)
        {
            return TileExists(chunk.TileFrom(chunkpos));
        }
        
        public static bool TileExists(Tile tile)
        {
            if (tile != null)
            {
                if (tile.TileType == EnumTiles.Air)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public static bool TryMakeTile(EnumTiles tileType, Chunk chunk, Point chunkpos, out Tile tile)
        {
            tile = null;
            if (chunkpos.X < 0 || chunkpos.Y < 0) return false; 
            if (chunk.TileFrom(chunkpos) != null)
            {
                if (chunk.TileFrom(chunkpos).TileType == EnumTiles.Air)
                {
                    tile = new Tile(tileType, chunk, chunkpos);
                    return true;
                }

                return false;
            }
            tile = new Tile(tileType, chunk, chunkpos);
            return true;
        }
    }
}