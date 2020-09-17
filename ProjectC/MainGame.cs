using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectC.objects;
using ProjectC.view;
using ProjectC.world;
using SpriteFontPlus;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace ProjectC
{
    public class MainGame : Game
    {
        public static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static int DrawCount;
        public static Rectangle WindowBounds;
        
        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "test lol";
            Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Sprites.Square = this.Content.Load<Texture2D>("square");
            Sprites.TileDirtGrass = this.Content.Load<Texture2D>("dirt_grassy");
            Sprites.TileDirt = this.Content.Load<Texture2D>("dirt");
            Sprites.TileStone = this.Content.Load<Texture2D>("stone");
            Sprites.TileFresh = this.Content.Load<Texture2D>("fresh_tile");
            Sprites.PlayerHuman = this.Content.Load<Texture2D>("player_hmn");
            Sprites.Rectangle = this.Content.Load<Texture2D>("chunk");
            var font = File.ReadAllBytes(this.Content.RootDirectory + "/font.ttf");
            var res = TtfFontBaker.Bake(font,32,512,512,new CharacterRange[] {new CharacterRange(' ','~') });
            Sprites.Font = res.CreateSpriteFont(GraphicsDevice);

        }

        protected override void Initialize()
        {
            WorldGenerator.instance.GenerateWorld();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            WindowBounds = Window.ClientBounds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // step logic
            foreach (var obj in Dimension.Current.GameObjects)
            {
                var gameObject = obj;
                gameObject.step();
            }
            foreach(var tile in Dimension.Current.Tiles)
            {
                if (tile != null)
                {
                    tile.Step();
                    tile.StepCount++;
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            // draw logic
            Camera.StartBatch(_spriteBatch);
            foreach (var obj in Dimension.Current.Chunks)
            {
                Chunk.Draw(_spriteBatch, obj);
            }
            foreach (var obj in Dimension.Current.GameObjects)
            {
                Camera.draw(_spriteBatch, obj);
            }

            _spriteBatch.DrawString(Sprites.Font, Player.LocalClient.ChunkIn.ChunkspacePosition.ToString(), Player.LocalClient.position + new Vector2(0,48), Color.White);
            DrawCount = 0;
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
