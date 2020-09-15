using System;
using System.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.objects;
using ProjectC.view;
using ProjectC.world;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System.Diagnostics;

namespace ProjectC.world
{
    public class Tile : IStorable
    {
        public const int TileSize = 8;

        private Texture2D _sheet = Sprites.TileFresh;
        private EnumSides Side = EnumSides.None;
        private Vector2 Position;
        public Color Tint = Color.White;
        public Chunk ChunkIn;
        public EnumTiles TileType { get; private set; } = EnumTiles.Air;
        public bool NeedsToUpdate;
        public EnumTiles UpdateType;


        public void UpdateRenderedSide()
        {
            var tileLeft = Dimension.Current.TileAtWorldPos(Position - Vector2.UnitX, false);
            var tileRight = Dimension.Current.TileAtWorldPos(Position + Vector2.UnitX, false);
            var tileUp = Dimension.Current.TileAtWorldPos(Position - Vector2.UnitY, false);
            var tileDown = Dimension.Current.TileAtWorldPos(Position + Vector2.UnitY, false);

            var left = TileExists(tileLeft);
            var right = TileExists(tileRight);
            var up = TileExists(tileUp);
            var down = TileExists(tileDown);

            if (left && !right && !up && !down) Side = EnumSides.Right;
            if (!left && right && !up && !down) Side = EnumSides.Left;
            if (!left && !right && up && !down) Side = EnumSides.Bottom;
            if (!left && !right && !up && down) Side = EnumSides.Top;

            if (left && right && !up && !down) Side = EnumSides.MiddleHorizontal;
            if (!left && !right && up && down) Side = EnumSides.MiddleVertical;
            if (left && right && up && down) Side = EnumSides.Middle;
            if (!left && !right && !up && !down) Side = EnumSides.None;

            if (left && right && up && !down) Side = EnumSides.BottomMiddle;
            if (left && right && !up && down) Side = EnumSides.TopMiddle;
            if (!left && right && up && down) Side = EnumSides.MiddleLeft;
            if (left && !right && up && down) Side = EnumSides.MiddleRight;

            if (!left && right && !up && down) Side = EnumSides.TopLeft;
            if (left && !right && !up && down) Side = EnumSides.TopRight;
            if (!left && right && up && !down) Side = EnumSides.BottomLeft;
            if (left && !right && up && !down) Side = EnumSides.BottomRight;
        }

        public static void Draw(SpriteBatch batch, Tile tile)
        {
            if (tile != null)
            {
                if (tile.TileType != EnumTiles.Air)
                {
                    if (Camera.OnScreen(tile.Position))
                    {
                        batch.Draw(tile._sheet, tile.Position * TileSize,
                            new Rectangle(new Point(((int)tile.Side % 4) * 8, ((int)tile.Side / 4) * 8), new Point(8, 8)),
                            tile.Tint);
                        MainGame.DrawCount++;
                    }
                }
            }
        }

        public Tile(EnumTiles type, Chunk chunk, Point position)
        {
            TileType = type;
            ChunkIn = chunk;
            Position = chunk.ChunkToWorld(position);
            chunk.PlaceTile(this, position);
            Dimension.LoadTile(this);
            NeedsToUpdate = true;
        }

        public void Step()
        {
            if (!NeedsToUpdate) return;
            UpdateTileRendering();
        }

        public void UpdateTileRendering()
        {
            NeedsToUpdate = false;
            TileType = UpdateType;
            UpdateSprite(TileType);
            UpdateRenderedSide();
            UpdateNearbyTiles(Position, this);
        }

        public static void UpdateNearbyTiles(Vector2 worldPos, Tile starter)
        {
            var tileLeft = Dimension.Current.TileAtWorldPos(worldPos - Vector2.UnitX, false);
            if (TileExists(tileLeft) && tileLeft != starter && tileLeft.NeedsToUpdate) tileLeft.UpdateTileRendering();

            var tileRight = Dimension.Current.TileAtWorldPos(worldPos + Vector2.UnitX, false);
            if (TileExists(tileRight) && tileRight != starter && tileRight.NeedsToUpdate) tileRight.UpdateTileRendering();

            var tileUp = Dimension.Current.TileAtWorldPos(worldPos - Vector2.UnitY, false);
            if (TileExists(tileUp) && tileUp != starter && tileUp.NeedsToUpdate) tileUp.UpdateTileRendering();
            
            var tileDown = Dimension.Current.TileAtWorldPos(worldPos + Vector2.UnitY, false);
            if (TileExists(tileDown) && tileDown != starter && tileDown.NeedsToUpdate) tileDown.UpdateTileRendering();
        }

        public void UpdateSprite(EnumTiles type)
        {
            _sheet = type switch
            {
                EnumTiles.Grass => Sprites.TileDirtGrass,
                EnumTiles.Stone => Sprites.TileStone,
                _ => Sprites.TileFresh,
            };
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
            if (tile == null) return false;
            
            if (tile.TileType == EnumTiles.Air)
            {
                return false;
            }
            return true;
        }

        public static bool UpdateSpriteLater(Tile tile, EnumTiles type)
        {
            tile.NeedsToUpdate = true;
            tile.UpdateType = type;
            return true;
        }

        public static bool TryMakeTile(EnumTiles tileType, Chunk chunk, Point chunkpos, out Tile tile)
        {
            tile = null;
            if (tileType == EnumTiles.Air)
            {
                chunk.RemoveTile(chunkpos);
                return true;
            }
            if (chunkpos.X < 0 || chunkpos.Y < 0) return false;
            var tilefrom = chunk.TileFrom(chunkpos);
            if (tilefrom != null)
            {
                if (tilefrom.TileType == EnumTiles.Air)
                {
                    Dimension.UnloadTile(tilefrom);
                    tile = new Tile(tileType, chunk, chunkpos);
                    Console.WriteLine("placed tile");
                    return true;
                }

                return false;
            }
            tile = new Tile(tileType, chunk, chunkpos);
            Console.WriteLine("placed tile");
            return true;
        }
    }
}