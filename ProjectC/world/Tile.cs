using System;
using System.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public static void Draw(SpriteBatch batch, Tile tile)
        {
            batch.Draw(tile.Sheet, tile.Position, new Rectangle(new Point((int)tile.Side % 4, (int)tile.Side / 4), new Point(8,8)), tile.Tint);
        }

        public Tile(EnumTiles type, Chunk chunk, Point position)
        {
            TileType = type;
            ChunkIn = chunk;
            Position = chunk.ChunkToWorld(position);
            chunk.PlaceTile(this, position);
            Dimention.LoadTile(this);
        }
        
        public JsonObject Save()
        {
            throw new NotImplementedException();
        }

        public bool Load(string data)
        {
            throw new NotImplementedException();
        }
    }
}