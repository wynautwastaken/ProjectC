using System.Collections.Generic;

namespace Colorless.Engine.Objects
{
    public abstract class GameObject
    {
        public static List<GameObject> Objects = new List<GameObject>();

        public GameObject()
        {
            Objects.Add(this);
        }
        
        public virtual void step() {}
        
        public virtual void draw() {}

        public virtual void destroy()
        {
            Objects.Remove(this);
        }
    }
}