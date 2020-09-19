using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.objects;
using ProjectC.world;

namespace ProjectC.view
{
    public class Camera
    {
        public static Vector2 Position = new Vector2(0,0);
        public static Matrix CameraMatrix;
        public static float zoom = 1;
        public static Rectangle CamBounds;
        public static Camera Instance = new Camera();
        
        public Camera()
        {
        }
        
        public static void StartBatch(SpriteBatch _spriteBatch)
        {

            var bounds = MainGame._graphics.GraphicsDevice.Viewport.Bounds;
            CamBounds = bounds;
            CameraMatrix = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                           Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                           Matrix.CreateTranslation(new Vector3(bounds.Width / 2f, bounds.Height / 2f, 0));

            _spriteBatch.Begin(SpriteSortMode.Deferred,samplerState: SamplerState.PointClamp, transformMatrix: CameraMatrix);
        }

        public static void DrawBackground(SpriteBatch batch)
        {
            batch.Draw(Sprites.SkyBg, new Rectangle(Position.ToPoint(), (CamBounds.Size.ToVector2()/zoom).ToPoint()), null, Color.White, 0, CamBounds.Center.ToVector2(), SpriteEffects.None, 0);
        }

        public static bool OnScreen(Vector2 pos)
        {
            var dist = Vector2.Distance(Player.LocalClient.position, pos);
            return dist < (96 / (zoom*1.5));
        }
    }
}