using System;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectC.objects;
using ProjectC.view;
using ProjectC.world;
using SpriteFontPlus;

namespace ProjectC
{
    public class MainGame : Game
    {
        public static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Sprites.Square = this.Content.Load<Texture2D>("square");
            Sprites.TileDirtGrass = this.Content.Load<Texture2D>("dirt_grassy");
            Sprites.TileStone = this.Content.Load<Texture2D>("stone");
            Sprites.TileFresh = this.Content.Load<Texture2D>("fresh_tile");
            Sprites.PlayerHuman = this.Content.Load<Texture2D>("player_hmn");
            Sprites.Rectangle = this.Content.Load<Texture2D>("chunk");
            var font = File.ReadAllBytes(this.Content.RootDirectory + "/font.ttf");
            var res = TtfFontBaker.Bake(font,32,512,512,new CharacterRange[] {new CharacterRange(' ','~') });
            Sprites.Font = res.CreateSpriteFont(GraphicsDevice);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // step logic
            foreach (var obj in Dimention.Current.AllLoadedThings(EnumStorables.GameObjects))
            {
                var gameObject = (GameObject) obj;
                gameObject.step();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            // draw logic
            Camera.startBatch(_spriteBatch);
            foreach (var obj in Dimention.Current.AllLoadedThings(EnumStorables.Chunks))
            {
                Chunk.Draw(_spriteBatch, (Chunk)obj);
            }
            foreach (var obj in Dimention.Current.AllLoadedThings(EnumStorables.GameObjects))
            {
                Camera.draw(_spriteBatch, (GameObject)obj);
            }

            _spriteBatch.DrawString(Sprites.Font, "test text is testing a testy test.", Player.LocalClient.position + Player.LocalClient.origin, Color.White);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
