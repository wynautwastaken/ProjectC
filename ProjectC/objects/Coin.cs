using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.view;
using ProjectC.world;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ProjectC.objects
{
    class Coin : GameObject
    {
        const int frameSize = 7;
        public float frameIndex = 0;
        const int frameCount = 6;
        public int coinType = 0;

        public override void draw(SpriteBatch _spriteBatch)
        {
            frameIndex += 0.2f;
            frameIndex %= frameCount;
            _spriteBatch.Draw(Sprites.CoinsSmall, WorldPosition + origin, new Rectangle((int)frameIndex * frameSize, coinType * frameSize, frameSize, frameSize), Color.White);
        }

        public Coin(Vector2 pos, int type)
        {
            Dimension.LoadGameObject(this);
            position = pos;
            coinType = type;
        }

        public override void step()
        {
            if(Vector2.Distance(Player.LocalClient.position,position) <= 3)
            {
                position = Vector2.Lerp(position, Player.LocalClient.position, 0.1f);
            }
            if(Vector2.Distance(Player.LocalClient.position,position) <= 0.3f)
            {
                destroy();
            }
        }
    }
}
