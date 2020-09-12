using System;
using System.Net.Http.Headers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectC.world;
using ProjectC.view;

namespace ProjectC.objects
{
    public class Player : Entity
    {
        public bool FacingRight = true;
        public static Player LocalClient = new Player();
        public float WalkSpeed = 2;
        public Dimention CurrentDimention = new Dimention();
        
        public Player()
        {
            LocalClient = this;
            origin = new Vector2(12,24);
        }
        
        private int _oldScroll = 0;

        public override void step()
        {
            position = Vector2.Max(position, Vector2.Zero);
            
            sprite = Sprites.PlayerHuman;
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            
            var w = keyState.IsKeyDown(Keys.W) ? 1 : 0;
            var a = keyState.IsKeyDown(Keys.A) ? 1 : 0;
            var s = keyState.IsKeyDown(Keys.S) ? 1 : 0;
            var d = keyState.IsKeyDown(Keys.D) ? 1 : 0;
            position += new Vector2(d - a, s - w) * WalkSpeed;
            var scroll = _oldScroll - mouseState.ScrollWheelValue;
            _oldScroll = mouseState.ScrollWheelValue;
            if (scroll > 0)
            {
                Camera.zoom *= 0.9f;
            }

            if ((d - a) < 0)
            {
                FacingRight = false;
            }  
            if ((d - a) > 0)
            {
                FacingRight = true;
            }  

            if (scroll < 0)
            {
                Camera.zoom /= 0.9f;
            }

            Camera.Position = position * new Vector2(-1,1);
            var (x, y) = new Point(((int)position.X / Tile.TileSize) / Chunk.ChunkWidth, ((int)position.Y / Tile.TileSize) / Chunk.ChunkHeight);
            var pchunk = CurrentDimention.ChunkAtWorldPos(position, true);
            var tpos = pchunk.WorldToChunk(position);
            var click = mouseState.LeftButton == ButtonState.Pressed;
            if (click)
            {
                var pos = mouseState.Position.ToVector2();
                pos = Vector2.Transform(pos, Matrix.Invert(Camera.CameraMatrix));
                var clampedpos = pos;
                clampedpos -= position;
                var len = Math.Clamp(clampedpos.Length(), 8, 96);
                clampedpos.Normalize();
                clampedpos *= len;
                var npos = position + clampedpos;
                var placed = false;
                if (npos.X >= 0 && npos.Y >= 0)
                {
                    var chunk = CurrentDimention.ChunkAtWorldPos(npos, true);
                    var chunkpos = chunk.WorldToChunk(npos);
                    placed = Tile.TryMakeTile(EnumTiles.Grass, chunk, chunkpos, out var tile);
                }
            }
            var save = keyState.IsKeyDown(Keys.K);
            if (save)
            {
                CurrentDimention.Save();
            }

            Camera.zoom = Math.Clamp(Camera.zoom, 0.5f, 4f);
        }

        public override void draw(SpriteBatch _spriteBatch)
        {
            if (sprite != null)
            {
                var facingflip = FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                _spriteBatch.Draw(sprite, position - new Vector2(0,8), null, Color.White, 0, origin, Vector2.One, facingflip, 0);
            }
        }
    }
}