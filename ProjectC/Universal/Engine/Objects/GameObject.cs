using System;
using System.Collections.Generic;
using System.Json;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.Engine.View;
using ProjectC.Universal.World;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectC.Engine.Objects
{
    public abstract class GameObject : IChunkStorable
    {
        public Texture2D sprite;
        public Vector2 origin = new Vector2(0,0); // from bottom left so y should be negitive
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
                Vector2 newOrigin = origin;

                _spriteBatch.Draw(sprite, position - newOrigin, Color.White);
            }
        }

        public virtual void destroy()
        {
            Objects.Remove(this);
        }

        public JsonObject Save()
        {
            throw new NotImplementedException();
        }

        public bool Load(string data)
        {
            throw new NotImplementedException();
        }
    }
}