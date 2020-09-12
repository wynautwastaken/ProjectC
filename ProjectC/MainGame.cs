using System;
using System.Net;
using System.Threading;
using ProjectC.Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectC.Client;
using ProjectC.Engine.View;
using ProjectC.Networking.Packets;
using ProjectC.World;
using ProjectC.Networking.Server;
using ProjectC.Objects;
using ProjectC.Universal.Networking;

namespace ProjectC
{
    public class MainGame : Game
    {
        public static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public GameServer Server;
        public GameClient Client;
        
        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            new Player();
            
            Console.WriteLine("Scanning Ports");
            int port = PortScanner.ScanTcpPorts(IPAddress.Loopback);

            Server = new GameServer(IPAddress.Loopback, port);
            Thread.Sleep(1000);
            Client = new GameClient("127.0.0.1", port);
            
            base.Initialize();
        }

        protected override void EndRun()
        {
            Server.Stop();
            Client.Disconnect();
            base.EndRun();
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
