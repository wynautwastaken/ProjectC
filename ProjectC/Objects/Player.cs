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

        public Vector2 CamOffset = Vector2.One * 0.5f;
        public float CollisionIncrement = 64;
        private float inc { get => CollisionIncrement; }

        public float WalkSpeed
        {
            get => _walkSpeed / TileHelper.TileSize;
            set => _walkSpeed = value;
        }
        public float FallSpeed
        {
            get => _fallSpeed / TileHelper.TileSize;
            set => _fallSpeed = value;
        }
        public float MaxYSpd = 8;

        private float _fallSpeed = 0.2f;
        private float _walkSpeed = 2;

        public Dimension CurrentDimension = Dimension.Current;
        public Chunk ChunkIn = Dimension.VoidChunk;
        public Point ChunkPos = Point.Zero;
        public Vector2 Velocity = Vector2.Zero;

        public Player()
        {
            position = new Vector2(512.5f * Chunk.ChunkWidth,1.5f * Chunk.ChunkHeight);
            LocalClient = this;
            origin = new Vector2(8,24);
        }
        
        private int _oldScroll;

        public static float Lerp(float value1, float value2, float amount)
        {
            var d = (value2 - value1) * amount;
            return value1 + d;
        }
        
        public bool Collides(Vector2 pos)
        {
            var hitTile = Dimension.Current.TileAtWorldPos(pos, Chunk.ChunkTData, true);
            var hasHit = hitTile > 0;
            return hasHit;
        }

        public void Collide()
        {
            var i = 0;
            while (Collides(position) && i < inc)
            {
                position -= Vector2.UnitY / inc;
                if (!Collides(position)) return;
                i++;
            }
            if (Collides(position + Velocity.X * Vector2.UnitX))
            {
                var ppos = position;
                while (Collides(ppos + Velocity.X * Vector2.UnitX))
                {
                    ppos -= Vector2.UnitY/inc;
                    i++;
                    if(i > inc)
                    {
                        ppos = position; 
                        break;
                    }
                }
                position = ppos;
                CollideHorizontally();
                return;
            }

            if (Collides(position + Velocity.Y * Vector2.UnitY))
            {
                i = 0;
                while (!Collides(position + Vector2.Normalize(Velocity.Y * Vector2.UnitY) / inc) && i < inc)
                {
                    position += Vector2.Normalize(Velocity.Y * Vector2.UnitY) / inc;
                    i++;
                }
                Velocity.Y = 0;
            }
            else if (Collides(position + Vector2.UnitY * 2) && !Collides(position + Vector2.UnitY))
            {
                position += Vector2.UnitY;
            }
            CollideHorizontally();
        }
        private void CollideHorizontally()
        {
            if (Collides(position + Velocity.X * Vector2.UnitX))
            {
                var i = 0;
                while (!Collides(position + Vector2.Normalize(Velocity.X * Vector2.UnitX) / inc) && i < inc)
                {
                    position += Vector2.Normalize(Velocity.X * Vector2.UnitX) / inc;
                    i++;
                }
                Velocity.X = 0;
            }
        }

        public void LoadNearbyChunks()
        {
            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition - Vector2.UnitY - Vector2.UnitX * 2).ToPoint(), true);
            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition - Vector2.UnitY - Vector2.UnitX).ToPoint(), true);
            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition - Vector2.UnitY).ToPoint(), true);
            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition - Vector2.UnitY + Vector2.UnitX).ToPoint(), true);
            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition - Vector2.UnitY + Vector2.UnitX * 2).ToPoint(), true);

            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition - Vector2.UnitX * 2).ToPoint(), true);
            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition - Vector2.UnitX).ToPoint(), true);
            //already loaded middle chunk
            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition + Vector2.UnitX).ToPoint(), true);
            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition + Vector2.UnitX * 2).ToPoint(), true);

            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition + Vector2.UnitY - Vector2.UnitX * 2).ToPoint(), true);
            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition + Vector2.UnitY - Vector2.UnitX).ToPoint(), true);
            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition + Vector2.UnitY).ToPoint(), true);
            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition + Vector2.UnitY + Vector2.UnitX).ToPoint(), true);
            Dimension.Current.LoadChunk((ChunkIn.ChunkspacePosition + Vector2.UnitY + Vector2.UnitX * 2).ToPoint(), true);
        }

        public override void step()
        {
            position = Vector2.Max(position, Vector2.Zero);
            ChunkIn = Dimension.Current.ChunkAtWorldPos(position, true);
            ChunkPos = ChunkIn.WorldToChunk(position);
            LoadNearbyChunks();
            
            sprite = Sprites.PlayerHuman;

            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            var a = keyState.IsKeyDown(Keys.A) ? 1 : 0;
            var d = keyState.IsKeyDown(Keys.D) ? 1 : 0;
            var onGround = Collides(position + Vector2.UnitY);
            if(onGround && position.Y != Math.Round(position.Y))
            {
                position.Y -= (position.Y - (float)Math.Floor(position.Y));
            }
            Velocity += new Vector2((d - a) * WalkSpeed, onGround ? 0 : FallSpeed);
            Velocity.Y = Math.Clamp(Velocity.Y, -MaxYSpd, onGround ? 0 : MaxYSpd);
            Velocity.X = Lerp(Math.Clamp(Velocity.X, -WalkSpeed * 2, WalkSpeed * 2), 0, 0.25f);

            Collide();

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

            Camera.Position = position * TileHelper.TileSize;
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

            Camera.zoom = Math.Clamp(Camera.zoom, 0.8f, 4f);
        }

        public bool TryPlaceTile(Vector2 worldPos, EnumTiles type)
        {
            var playerpos = position * TileHelper.TileSize;
            var clampedpos = Vector2.Transform(worldPos, Matrix.Invert(Camera.CameraMatrix));
            clampedpos -= playerpos;
            var len = Math.Clamp(clampedpos.Length(), 8, 96);
            clampedpos.Normalize();
            clampedpos *= len;
            var npos = playerpos + clampedpos;
            npos /= TileHelper.TileSize;
            npos = Vector2.Round(npos);
            var placed = false;
            if (npos.X >= 0 && npos.Y >= 0)
            {
                var chunk = CurrentDimension.ChunkAtWorldPos(npos, true);
                var chunkpos = chunk.WorldToChunk(npos);
                placed = TileHelper.TryMakeTile((int)type, 0, Color.White, 0, chunk, chunkpos);
            }
            return placed;
        }

        public override void draw(SpriteBatch _spriteBatch)
        {
            if (sprite != null)
            {
                var facingflip = FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                _spriteBatch.Draw(sprite, position*TileHelper.TileSize, null, Color.White, 0, origin, Vector2.One, facingflip, 0);
            }
        }
    }
}