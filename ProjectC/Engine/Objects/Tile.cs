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
        public const int TileSize = 8;

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
            var id = ChunkedWorld.NewIndex(chunk.ExistentTiles);
            if (id >= chunk.ExistentTiles.Length || id < 0) return;
            chunk.ExistentTiles[id] = this;
        }

        public static void Render(SpriteBatch batch, Tile tile)
        {
            if (tile.Sheet != null)
            {
                batch.Draw(tile.Sheet, tile.Position, new Rectangle(new Point((int)tile.Side % 4, (int)tile.Side / 4), new Point(TileSize,TileSize)), Color.White);
            }
        }
        
        public static bool IsTileAt(Vector2 position)
        {
            var (x, y) = (position / TileSize) / new Vector2(Chunk.ChunkWidth, Chunk.ChunkHeight);
            
            var foundchunk = ChunkedWorld.LoadChunk(new ChunkIdentifier((int)x, (int)y));
            var tile = foundchunk?.GetTileFrom(foundchunk.WorldToChunk(position));
            if (tile == null) return false;
            return tile.TileType != EnumTiles.Air;
        }
        public static bool IsTileAt(Vector2 position, Chunk chunk)
        {
            var (x, y) = (position / TileSize) / new Vector2(Chunk.ChunkWidth, Chunk.ChunkHeight);

            var foundchunk = chunk;
            var tile = foundchunk?.GetTileFrom(foundchunk.WorldToChunk(position));
            if (tile == null) return false;
            return tile.TileType != EnumTiles.Air;
        }

        private void UpdateSide()
        {
            bool left = false, right = false, up = false, down = false;
            
            if (IsTileAt(Position - new Vector2(TileSize, 0), _chunk))
            {
                left = true;
            };
            if (IsTileAt(Position + new Vector2(TileSize, 0), _chunk))
            {
                right = true;
            };
            if (IsTileAt(Position + new Vector2(0, TileSize), _chunk))
            {
                up = true;
            };
            if (IsTileAt(Position - new Vector2(0, TileSize), _chunk))
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
            return Vector2.Floor(toSnap / TileSize) * TileSize;
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