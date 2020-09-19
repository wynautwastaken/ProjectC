using System;
using System.Diagnostics;
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
            
            Sprites.ImportAll(this);

            var font = File.ReadAllBytes(this.Content.RootDirectory + "/font.ttf");
            var res = TtfFontBaker.Bake(font,32,512,512,new CharacterRange[] {new CharacterRange(' ','~') });
            Sprites.Font = res.CreateSpriteFont(GraphicsDevice);

        }

        protected override void Initialize()
        {
            WorldGenerator.instance.GenerateWorld();
            Player.LocalClient = new Player();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            WindowBounds = Window.ClientBounds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Dimension.DestroyThings();
            Dimension.LoadThings();

            // step logic
            foreach (var obj in Dimension.Current.GameObjects)
            {
                var gameObject = obj;
                gameObject.step();
            }
            foreach(var chunk in Dimension.Current.Chunks)
            {
                if (chunk != null)
                {
                    Chunk.Step(chunk);
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draw logic
            Camera.StartBatch(_spriteBatch);

            Camera.DrawBackground(_spriteBatch);
            
            foreach (var obj in Dimension.Current.Chunks)
            {
                Chunk.Draw(_spriteBatch, obj);
            }
            foreach (var obj in Dimension.Current.GameObjects)
            {
                obj.draw(_spriteBatch);
            }

            DrawCount = 0;
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
