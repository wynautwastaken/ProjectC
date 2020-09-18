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
using System.Collections.Generic;

namespace ProjectC.world
{
    public class TileHelper
    {
        public const int TileSize = 8;

        public int StepCount;
        public int StepAmount = 8;

        public static Dictionary<EnumSides, int> MaskingSides = new Dictionary<EnumSides, int>()
        {
            {EnumSides.Left,   (int)Math.Pow(2, 0)},
            {EnumSides.Right,  (int)Math.Pow(2, 1)},
            {EnumSides.Top,    (int)Math.Pow(2, 2)},
            {EnumSides.Bottom, (int)Math.Pow(2, 3)}
        };

        public static void Draw(SpriteBatch batch, Chunk chunk, int cx, int cy)
        {
            var pos = new Point(cx, cy);
            var wpos = chunk.ChunkToWorld(pos);
            var tile = chunk.TileFrom(pos, Chunk.ChunkTData);
            var col = chunk.TileFrom(pos, Chunk.ChunkTColor);
            var side = chunk.TileFrom(pos, Chunk.ChunkTSide);
            if (tile != 0)
            {
                if (Camera.OnScreen(wpos))
                {
                    UpdateTileRendering(chunk, pos, false);
                    batch.Draw(
                        SpriteFrom(tile), 
                        wpos * TileSize,
                        new Rectangle(new Point(GetAutotileIndex(wpos), GetAutotileIndex(wpos, true)), (Vector2.One * TileSize).ToPoint()),
                        new Color((uint)col)
                        );
                    MainGame.DrawCount++;
                }
            }
        }

        public static int GetAutotileIndex(Vector2 wpos, bool y = false)
        {
            var tileLeft = Dimension.Current.TileAtWorldPos(wpos - Vector2.UnitX, Chunk.ChunkTData, false);
            var tileRight = Dimension.Current.TileAtWorldPos(wpos + Vector2.UnitX, Chunk.ChunkTData, false);
            var tileUp = Dimension.Current.TileAtWorldPos(wpos - Vector2.UnitY, Chunk.ChunkTData, false);
            var tileDown = Dimension.Current.TileAtWorldPos(wpos + Vector2.UnitY, Chunk.ChunkTData, false);

            var left = tileLeft > 0;
            var right = tileRight > 0;
            var up = tileUp > 0;
            var down = tileDown > 0;

            var auto = EnumDrawSides.None;

            if (left && !right && !up && !down) auto = EnumDrawSides.Right;
            if (!left && right && !up && !down) auto = EnumDrawSides.Left;
            if (!left && !right && up && !down) auto = EnumDrawSides.Bottom;
            if (!left && !right && !up && down) auto = EnumDrawSides.Top;

            if (left && right && !up && !down) auto = EnumDrawSides.MiddleHorizontal;
            if (!left && !right && up && down) auto = EnumDrawSides.MiddleVertical;
            if (left && right && up && down) auto = EnumDrawSides.Middle;

            if (left && right && up && !down) auto = EnumDrawSides.BottomMiddle;
            if (left && right && !up && down) auto = EnumDrawSides.TopMiddle;
            if (!left && right && up && down) auto = EnumDrawSides.MiddleLeft;
            if (left && !right && up && down) auto = EnumDrawSides.MiddleRight;

            if (!left && right && !up && down) auto = EnumDrawSides.TopLeft;
            if (left && !right && !up && down) auto = EnumDrawSides.TopRight;
            if (!left && right && up && !down) auto = EnumDrawSides.BottomLeft;
            if (left && !right && up && !down) auto = EnumDrawSides.BottomRight;

            if(up && Dimension.Current.TileAtWorldPos(wpos,Chunk.ChunkTData,false) == (int)EnumTiles.Grass)
            {
                var c = Dimension.Current.ChunkAtWorldPos(wpos, false);
                var cpos = c.WorldToChunk(wpos);
                c.PlaceTile((int)EnumTiles.Dirt, 0, c.TileFrom(cpos, Chunk.ChunkTColor), 0, cpos);
            }

            var xx = ((int)auto / 4) * TileSize;
            var yy = ((int)auto % 4) * TileSize;
            return y ? xx : yy;
        }

        public static void Step(Chunk chunk, int cx, int cy)
        {
            var pos = new Point(cx, cy);
            if (Camera.OnScreen(chunk.ChunkToWorld(pos))) {
                UpdateTile(chunk,pos,false);    
            }
        }

        public static void UpdateTile(Chunk chunk, Point pos, bool manual)
        {
            UpdateTileRendering(chunk, pos, manual);
        }

        public static void UpdateTileRendering(Chunk chunk, Point pos, bool manual)
        {
            var tileType = chunk.TileFrom(pos, Chunk.ChunkTData);
            var wpos = chunk.ChunkToWorld(pos);
            if (manual)
            {
                UpdateNearbyTiles(wpos);
            }
            chunk.PlaceTile(tileType, chunk.TileFrom(pos, Chunk.ChunkTSide), chunk.TileFrom(pos, Chunk.ChunkTColor), chunk.TileFrom(pos, Chunk.ChunkTMeta), pos);
        }

        public static void UpdateNearbyTiles(Vector2 worldPos)
        {
            var chunkLeft = Dimension.Current.ChunkAtWorldPos(worldPos - Vector2.UnitX, false);
            if (chunkLeft != null) UpdateTileRendering(chunkLeft, chunkLeft.WorldToChunk(worldPos), false);

            var chunkRight = Dimension.Current.ChunkAtWorldPos(worldPos + Vector2.UnitX, false);
            if (chunkRight != null) UpdateTileRendering(chunkRight, chunkRight.WorldToChunk(worldPos), false);

            var chunkUp = Dimension.Current.ChunkAtWorldPos(worldPos - Vector2.UnitY, false);
            if (chunkUp != null) UpdateTileRendering(chunkUp, chunkUp.WorldToChunk(worldPos), false);

            var chunkDown = Dimension.Current.ChunkAtWorldPos(worldPos + Vector2.UnitY, false);
            if (chunkDown != null) UpdateTileRendering(chunkDown, chunkDown.WorldToChunk(worldPos), false);
        }

        public static Texture2D SpriteFrom(int type)
        {
            switch(type)
            {
                case (int)EnumTiles.Grass:
                    return Sprites.TileDirtGrass;

                case (int)EnumTiles.Dirt: 
                    return Sprites.TileDirt;

                case (int)EnumTiles.Stone: 
                    return Sprites.TileStone;

                default:
                case (int)EnumTiles.Air:
                case (int)EnumTiles.Fresh: 
                    return Sprites.TileFresh;
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

        public static bool TryMakeTile(int type, int side, Color color, int meta, Chunk chunk, Point chunkpos)
        {
            if (type <= 0)
            {
                chunk.RemoveTile(chunkpos);
                return true;
            }
            if (chunkpos.X < 0 || chunkpos.Y < 0) return false;
            
            if (chunk.TileFrom(chunkpos, Chunk.ChunkTData) <= 0)
            {
                chunk.PlaceTile(type, side, (int)color.PackedValue, meta, chunkpos);
                UpdateTile(chunk, chunkpos, true);
                return true;
            }
            return false;
        }
    }
}