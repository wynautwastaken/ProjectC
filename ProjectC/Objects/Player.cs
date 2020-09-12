using System;
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
            sprite = Sprites.PlayerHuman;

            var w = Keyboard.GetState().IsKeyDown(Keys.W) ? 1 : 0;
            var a = Keyboard.GetState().IsKeyDown(Keys.A) ? 1 : 0;
            var s = Keyboard.GetState().IsKeyDown(Keys.S) ? 1 : 0;
            var d = Keyboard.GetState().IsKeyDown(Keys.D) ? 1 : 0;
            position += new Vector2(d - a, s - w) * WalkSpeed;
            var scroll = _oldScroll - Mouse.GetState().ScrollWheelValue;
            _oldScroll = Mouse.GetState().ScrollWheelValue;
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
            var pchunk = CurrentDimention.LoadChunk(new Point(x,y), true);
            var tpos = pchunk.WorldToChunk(position);
            var click = Mouse.GetState().LeftButton == ButtonState.Pressed;
            if (click)
            {
                var pos = Mouse.GetState().Position.ToVector2();
                pos = Vector2.Transform(pos, Matrix.Invert(Camera.CameraMatrix));
                var clampedpos = pos;
                clampedpos -= position;
                var len = Math.Clamp(clampedpos.Length(), 8, 96);
                clampedpos.Normalize();
                clampedpos *= len;
                var npos = position + clampedpos;
                var chunk = CurrentDimention.LoadChunk(new Vector2((npos.X / Tile.TileSize) / Chunk.ChunkWidth, (npos.Y / Tile.TileSize) / Chunk.ChunkHeight).ToPoint(), true);
                var chunkpos = chunk.WorldToChunk(npos);
                var tile = new Tile(EnumTiles.Grass, chunk, chunkpos);
            }

            var save = Keyboard.GetState().IsKeyDown(Keys.K);
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
                Vector2 newOrigin = origin;
                var facingflip = FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                _spriteBatch.Draw(sprite, position - new Vector2(0,8), null, Color.White, 0, newOrigin, Vector2.One, facingflip, 0);
            }
        }
    }
}