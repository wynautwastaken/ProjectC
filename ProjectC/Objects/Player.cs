using System;
using System.CodeDom.Compiler;
using System.Net.Http.Headers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectC.world;
using ProjectC.view;
using SharpDX.MediaFoundation;

namespace ProjectC.objects
{
    public class Player : Entity
    {
        public bool FacingRight = true;
        public static Player LocalClient = new Player();

        public Vector2 CamOffset = Vector2.One * 0.5f;

        public float WalkSpeed
        {
            get => _walkSpeed / Tile.TileSize;
            set => _walkSpeed = value;
        }
        public float FallSpeed
        {
            get => _fallSpeed / Tile.TileSize;
            set => _fallSpeed = value;
        }

        private float _fallSpeed = 1;
        private float _walkSpeed = 2;

        public Dimension CurrentDimension = Dimension.Current;
        public Chunk ChunkIn = Dimension.VoidChunk;
        public Point ChunkPos = Point.Zero;
        public Vector2 Velocity = Vector2.Zero;

        public Player()
        {
            position = new Vector2(512 * Chunk.ChunkWidth,2 * Chunk.ChunkHeight);
            LocalClient = this;
            origin = new Vector2(12,24);
        }
        
        private int _oldScroll;

        public static float Lerp(float value1, float value2, float amount)
        {
            var d = (value2 - value1) * amount;
            return value1 + d;
        }
        
        public override void step()
        {
            position = Vector2.Max(position, Vector2.Zero);
            ChunkIn = Dimension.Current.ChunkAtWorldPos(position, true);
            ChunkPos = ChunkIn.WorldToChunk(position);
            
            sprite = Sprites.PlayerHuman;

            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            var a = keyState.IsKeyDown(Keys.A) ? 1 : 0;
            var d = keyState.IsKeyDown(Keys.D) ? 1 : 0;
            var collidingTile = Dimension.Current.TileAtWorldPos(position + Vector2.UnitY, true);
            var onGround = Tile.TileExists(collidingTile);
            Velocity += new Vector2((d - a) * WalkSpeed, onGround ? 0 : FallSpeed);
            Velocity.Y = Math.Clamp(Velocity.Y, -FallSpeed, FallSpeed);
            Velocity.X = Lerp(Math.Clamp(Velocity.X, -WalkSpeed * 2, WalkSpeed * 2),0,0.25f);

            position += Velocity;
            
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

            Camera.Position = position * Tile.TileSize;
            var (x, y) = new Point((int)position.X /  Chunk.ChunkWidth, (int)position.Y / Chunk.ChunkHeight);
            var click = mouseState.LeftButton == ButtonState.Pressed;
            if (click)
            {
                TryPlaceTile(mouseState.Position.ToVector2(), EnumTiles.Grass);
            }
            var rightclick = mouseState.RightButton == ButtonState.Pressed;
            if(rightclick)
            {
                TryPlaceTile(mouseState.Position.ToVector2(), EnumTiles.Air);
            }
            var save = keyState.IsKeyDown(Keys.K);
            if (save)
            {
                CurrentDimension.Save();
            }

            Camera.zoom = Math.Clamp(Camera.zoom, 0.5f, 4f);
        }

        public bool TryPlaceTile(Vector2 worldPos, EnumTiles type)
        {
            var playerpos = position * Tile.TileSize;
            var clampedpos = Vector2.Transform(worldPos, Matrix.Invert(Camera.CameraMatrix));
            clampedpos -= playerpos;
            var len = Math.Clamp(clampedpos.Length(), 8, 96);
            clampedpos.Normalize();
            clampedpos *= len;
            var npos = playerpos + clampedpos;
            npos /= Tile.TileSize;
            npos = Vector2.Round(npos);
            var placed = false;
            if (npos.X >= 0 && npos.Y >= 0)
            {
                var chunk = CurrentDimension.ChunkAtWorldPos(npos, true);
                var chunkpos = chunk.WorldToChunk(npos);
                placed = Tile.TryMakeTile(type, chunk, chunkpos, out var tile);
            }
            return placed;
        }

        public override void draw(SpriteBatch _spriteBatch)
        {
            if (sprite != null)
            {
                var facingflip = FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                _spriteBatch.Draw(sprite, position*Tile.TileSize, null, Color.White, 0, origin, Vector2.One, facingflip, 0);
            }
        }
    }
}