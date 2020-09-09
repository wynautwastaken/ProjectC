using System.Net;
using System.Threading;
using ProjectC.Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectC.Engine.View;
using ProjectC.Objects;
using ProjectC.World;

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

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            new Server.Server(IPAddress.Any, 7777);
            Thread.Sleep(1000);
            new Client.Client("127.0.0.1", 7777);
            
            base.Initialize();
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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // step logic
            foreach (GameObject gameObject in GameObject.Objects)
            {
                gameObject.step();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draw logic
            Camera.startBatch(_spriteBatch);
            foreach(Chunk chunk in ChunkedWorld.ChunksLoaded)
            {
                chunk.Render(_spriteBatch);
            }
            foreach (GameObject gameObject in GameObject.Objects)
            {
                Camera.draw(_spriteBatch,gameObject);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
