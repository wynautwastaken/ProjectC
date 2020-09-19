using Microsoft.Xna.Framework.Graphics;

namespace ProjectC.view
{
    public static class Sprites
    {
        public static Texture2D Square;
        public static Texture2D TileDirtGrass;
        public static Texture2D TileDirt;
        public static Texture2D TileStone;
        public static Texture2D TileFresh;
        public static Texture2D TileFancyWall;
        public static Texture2D TileCobbledBricks;
        public static Texture2D TileCopperPipe;
        public static Texture2D WallCave;
        public static Texture2D PlayerHuman;
        public static Texture2D Rectangle;
        public static SpriteFont Font;
        public static Texture2D SkyBg;
        public static Texture2D Sun;
        public static Texture2D CloudBg;
        public static Texture2D MountainBg;
        public static Texture2D Mountain2Bg;
        public static Texture2D TreeBg;
        public static Texture2D Tree2Bg;
        public static Texture2D CoinsSmall;
        public static Texture2D CopperOre;

        public static void ImportAll(MainGame game)
        {
            Square = game.Content.Load<Texture2D>("square");
            TileDirtGrass = game.Content.Load<Texture2D>("grass_jungle");
            TileDirt = game.Content.Load<Texture2D>("dirt_jungle");
            TileStone = game.Content.Load<Texture2D>("stone");
            TileFresh = game.Content.Load<Texture2D>("fresh_tile");
            TileFancyWall = game.Content.Load<Texture2D>("fancy_wall");
            TileCobbledBricks = game.Content.Load<Texture2D>("bordered_wall");
            TileCopperPipe = game.Content.Load<Texture2D>("copper_pipe");
            PlayerHuman = game.Content.Load<Texture2D>("player_hmn");
            SkyBg = game.Content.Load<Texture2D>("sky_bg");
            WallCave = game.Content.Load<Texture2D>("bg_underground");
            CloudBg = game.Content.Load<Texture2D>("clouds");
            Sun = game.Content.Load<Texture2D>("sun");
            CoinsSmall = game.Content.Load<Texture2D>("coin_small");
            CopperOre = game.Content.Load<Texture2D>("copper_ore");
        }
    }
}