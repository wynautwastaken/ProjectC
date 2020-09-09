using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectC.Engine.Objects;
using ProjectC.Engine.View;

namespace ProjectC.Objects
{
    public class Player : GameObject
    {
        public Player()
        {
            origin = new Vector2(8,8);
        }

        public override void step()
        {
            sprite = Sprites.Square;
        }
    }
}