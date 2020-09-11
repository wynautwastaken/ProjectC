using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectC.Engine.Objects;

namespace ProjectC.Engine.View
{
    public class Camera
    {
        public static Vector2 Position = new Vector2(0,0);
        public static Matrix CameraMatrix;
        public static float zoom = 1;
        public static void startBatch(SpriteBatch _spriteBatch)
        {
            Rectangle bounds = MainGame._graphics.GraphicsDevice.Viewport.Bounds;
            
            CameraMatrix = Matrix.CreateTranslation(new Vector3(Position.X, -Position.Y, 0)) *
                           Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                           Matrix.CreateTranslation(new Vector3(bounds.Width*0.5f,bounds.Height*0.5f,0));

            _spriteBatch.Begin(SpriteSortMode.Texture,null,SamplerState.PointClamp,null,null,null,CameraMatrix);
        }

        public static void draw(SpriteBatch _spriteBatch, GameObject gameObject)
        {
            gameObject.draw(_spriteBatch);
        }
    }
}