using System;
using System.Collections.Generic;
using System.Configuration;
using System.Json;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.Engine.View;
using ProjectC.Universal.World;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectC.Engine.Objects
{
    public class Tile : IChunkStorable
    {
        private Texture2D _sheet;
        private EnumSides _side = EnumSides.None;
        private Vector2 _position;
        private Vector2 _chunkpos;
        private Chunk _chunk;
        public EnumTiles TileType { get; private set; } = EnumTiles.Air;

        public void SetType(EnumTiles type)
        {
            _sheet = type switch
            {
                EnumTiles.Dirt => Sprites.TileDirtGrass,
                EnumTiles.Fresh => Sprites.TileFresh,
                EnumTiles.Grass => Sprites.TileDirtGrass,
                EnumTiles.Stone => Sprites.TileStone,
                EnumTiles.Air => null,
                _ => _sheet
            };
            TileType = type;
        }
        
        public Tile(EnumTiles type, Chunk chunk, Vector2 positionInWorld)
        {
            _chunk = chunk;
            _position = positionInWorld;
            _chunkpos = chunk.WorldToChunk(positionInWorld);
            SetType(type);
            chunk.PlaceTile(this, _chunkpos);
        }

        public void Render(SpriteBatch batch)
        {
            if (_sheet != null)
            {
                batch.Draw(_sheet, _position, new Rectangle(new Point((int)_side % 4, (int)_side / 4), new Point(8,8)), Color.White);
            }
        }
        
        public static bool IsTileAt(Vector2 position)
        {
            var (x, y) = (position / 8) / 256;
            
            var foundchunk = ChunkedWorld.LoadChunk(new ChunkIdentifier((int)x, (int)y));
            if (foundchunk == null) return false;
            var tile = foundchunk.GetTileFrom(foundchunk.WorldToChunk(position));
            return tile.TileType != EnumTiles.Air;
        }

        private void UpdateSide()
        {
            bool left = false, right = false, up = false, down = false;
            
            if (IsTileAt(_position - new Vector2(8, 0)))
            {
                left = true;
            };
            if (IsTileAt(_position + new Vector2(8, 0)))
            {
                right = true;
            };
            if (IsTileAt(_position + new Vector2(0, 8)))
            {
                up = true;
            };
            if (IsTileAt(_position - new Vector2(0, 8)))
            {
                down = true;
            }

            if (!left && right && !up && !down)
            {
                _side = EnumSides.Left;
            }
            if (left && !right && !up && !down)
            {
                _side = EnumSides.Right;
            }
            if (!left && !right && !up && down)
            {
                _side = EnumSides.Top;
            }
            if (!left && !right && up && !down)
            {
                _side = EnumSides.Bottom;
            }
            
            if (left && right && !up && !down)
            {
                _side = EnumSides.MiddleHorizontal;
            }
            if (!left && !right && up && down)
            {
                _side = EnumSides.MiddleVertical;
            }
            if (left && !right && up && down)
            {
                _side = EnumSides.MiddleRight;
            }
            if (left && right && up && down)
            {
                _side = EnumSides.Middle;
            }
            if (!left && right && up && down)
            {
                _side = EnumSides.MiddleLeft;
            }
            
            if (!left && right && !up && down)
            {
                _side = EnumSides.TopLeft;
            }
            if (left && right && !up && down)
            {
                _side = EnumSides.TopMiddle;
            }
            if (left && !right && !up && down)
            {
                _side = EnumSides.TopRight;
            }
            
            
            if (!left && right && up && !down)
            {
                _side = EnumSides.BottomLeft;
            }
            if (left && right && up && !down)
            {
                _side = EnumSides.BottomMiddle;
            }
            if (left && !right && up && !down)
            {
                _side = EnumSides.BottomRight;
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
            pos.Add("x", _position.X);
            pos.Add("y",_position.Y);
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