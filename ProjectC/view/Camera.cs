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

        public static bool OnScreen(Vector2 pos)
        {
            return Vector2.Distance(Player.LocalClient.position,pos) < CamBounds.Width * 1.5f;
        }

        public static void draw(SpriteBatch _spriteBatch, GameObject gameObject)
        {
            gameObject.draw(_spriteBatch);
        }
    }
}