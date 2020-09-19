using System;
using System.Collections.Generic;
using System.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.view;
using ProjectC.world;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectC.objects
{
    public abstract class GameObject : IStorable
    {
        public Texture2D sprite;
        public Vector2 origin = new Vector2(0,0); // from bottom left so y should be negitive
        public Vector2 position = new Vector2(255,128);
        public Vector2 WorldPosition { get => position * TileHelper.TileSize; set => position = value; }

        public GameObject()
        {
            Dimension.LoadGameObject(this);
        }

        public virtual void step()
        {
            
        }

        public virtual void draw(SpriteBatch _spriteBatch)
        {
            if (sprite != null)
            {
                _spriteBatch.Draw(sprite, WorldPosition - origin, Color.White);
            }
        }

        public virtual void destroy()
        {
            Dimension.UnloadGameObject(this);
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