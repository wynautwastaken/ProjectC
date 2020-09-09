using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.Engine.View;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectC.Engine.Objects
{
    public abstract class GameObject
    {
        public Texture2D sprite;
        public Vector2 origin = new Vector2(0,0); // from top right so y should be negitive
        public Vector2 position = new Vector2(0,0);
        public static List<GameObject> Objects = new List<GameObject>();

        public GameObject()
        {
            Objects.Add(this);
        }

        public virtual void step()
        {
            
        }

        public virtual void draw(SpriteBatch _spriteBatch)
        {
            if (sprite != null)
            {
                Vector2 newOrgin = origin;

                _spriteBatch.Draw(sprite, new Vector2(position.X + -newOrgin.X, position.Y + -newOrgin.Y), Color.White);
            }
        }

        public virtual void destroy()
        {
            Objects.Remove(this);
        }
    }
}