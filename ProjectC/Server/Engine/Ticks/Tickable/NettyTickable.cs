using System.Collections.Generic;

namespace ProjectC.Server.Engine.Ticks.Tickable
{
    public abstract class NettyTickable
    {
        public static List<NettyTickable> Tickables = new List<NettyTickable>();
        public NettyTickable()
        {
            Tickables.Add(this);
        }
        
        public abstract void Step();

        public void Destroy()
        {
            Tickables.Remove(this);
        }
    }
}