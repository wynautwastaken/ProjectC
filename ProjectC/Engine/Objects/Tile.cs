using System;
using System.Collections.Generic;
using System.Configuration;
using System.Json;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.Engine.View;
using ProjectC.World;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectC.Engine.Objects
{
    public class Tile : IChunkStorable
    {
        private Texture2D Sheet;
        private EnumSides Side = EnumSides.None;
        private Vector2 Position;
        private Vector2 _chunkpos;
        private Chunk _chunk;
        public EnumTiles TileType { get; private set; } = EnumTiles.Air;

        public void SetType(EnumTiles type)
        {
            Sheet = type switch
            {
                EnumTiles.Dirt => Sprites.TileDirtGrass,
                EnumTiles.Fresh => Sprites.TileFresh,
                EnumTiles.Grass => Sprites.TileDirtGrass,
                EnumTiles.Stone => Sprites.TileStone,
                EnumTiles.Air => null,
                _ => Sheet
            };
            TileType = type;
        }
        
        public Tile(EnumTiles type, Chunk chunk, Vector2 positionInChunk)
        {
            _chunk = chunk;
            Position = chunk.ChunkToWorld(positionInChunk);
            _chunkpos = positionInChunk;
            SetType(type);
            chunk.PlaceTile(this, _chunkpos);
        }

        public static void Render(SpriteBatch batch, Tile tile)
        {
            
            if (tile.Sheet != null)
            {
                batch.Draw(tile.Sheet, tile.Position, new Rectangle(new Point((int)tile.Side % 4, (int)tile.Side / 4), new Point(8,8)), Color.White);
            }
        }
        
        public static bool IsTileAt(Vector2 position)
        {
            var (x, y) = (position / 8) / 256;
            
            var foundchunk = ChunkedWorld.LoadChunk(new ChunkIdentifier((int)x, (int)y));
            if (foundchunk == null) return false;
            var tile = foundchunk.GetTileFrom(foundchunk.WorldToChunk(position));
            if (tile != null)
            {
                return tile.TileType != EnumTiles.Air;
            }
            return false;
        }

        private void UpdateSide()
        {
            bool left = false, right = false, up = false, down = false;
            
            if (IsTileAt(Position - new Vector2(8, 0)))
            {
                left = true;
            };
            if (IsTileAt(Position + new Vector2(8, 0)))
            {
                right = true;
            };
            if (IsTileAt(Position + new Vector2(0, 8)))
            {
                up = true;
            };
            if (IsTileAt(Position - new Vector2(0, 8)))
            {
                down = true;
            }

            if (!left && right && !up && !down)
            {
                Side = EnumSides.Left;
            }
            if (left && !right && !up && !down)
            {
                Side = EnumSides.Right;
            }
            if (!left && !right && !up && down)
            {
                Side = EnumSides.Top;
            }
            if (!left && !right && up && !down)
            {
                Side = EnumSides.Bottom;
            }
            
            if (left && right && !up && !down)
            {
                Side = EnumSides.MiddleHorizontal;
            }
            if (!left && !right && up && down)
            {
                Side = EnumSides.MiddleVertical;
            }
            if (left && !right && up && down)
            {
                Side = EnumSides.MiddleRight;
            }
            if (left && right && up && down)
            {
                Side = EnumSides.Middle;
            }
            if (!left && right && up && down)
            {
                Side = EnumSides.MiddleLeft;
            }
            
            if (!left && right && !up && down)
            {
                Side = EnumSides.TopLeft;
            }
            if (left && right && !up && down)
            {
                Side = EnumSides.TopMiddle;
            }
            if (left && !right && !up && down)
            {
                Side = EnumSides.TopRight;
            }
            
            
            if (!left && right && up && !down)
            {
                Side = EnumSides.BottomLeft;
            }
            if (left && right && up && !down)
            {
                Side = EnumSides.BottomMiddle;
            }
            if (left && !right && up && !down)
            {
                Side = EnumSides.BottomRight;
            }
        }
        
        public void UpdateTile()
        {
            UpdateSide();
        }

        public static Vector2 SnapToGrid(Vector2 toSnap)
        {
            return Vector2.Floor(toSnap / 8) * 8;
        }

        public JsonObject Save()
        {
            var json = new JsonObject();
            json.Add("type", "tile");
            var pos = new JsonObject();
            pos.Add("x", Position.X);
            pos.Add("y",Position.Y);
            json.Add("position", pos);
            json.Add("tiletype", (int)TileType);
            return json;
        }

        public bool Load(string data)
        {
            throw new NotImplementedException();
        }
    }
}