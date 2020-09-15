using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ProjectC.Server.Engine.Ticks.Tickable;

namespace ProjectC.Server.Engine.Ticks
{
    public class ServerNettyTick
    {
        public int TicksPerSecond;

        public Thread NettyThread;
        private long desiredTime = 1000 / 20;
        public bool Running = true;
        
        
        /**
         * meant to run every 1/20 of a second if possible
         */
        private void Tick()
        {
            Stopwatch tickWatch = new Stopwatch();
            int ticks = 0;
            
            tickWatch.Start();
            
            Stopwatch timeTaken = new Stopwatch();
            while (Running)
            {
                timeTaken.Start();
                
                // netty tick
                foreach (NettyTickable tickable in NettyTickable.Tickables)
                {
                    tickable.Step();
                }
                
                // calculate tps
                if (tickWatch.ElapsedMilliseconds >= 1000)
                {
                    if (tickWatch.ElapsedMilliseconds == 1000)
                    {
                        ticks++;
                    }
                    TicksPerSecond = ticks;
                    ticks = 0;
                    tickWatch.Reset();
                    tickWatch.Start();
                }
                else
                {
                    ticks++;
                }

                timeTaken.Stop();
                long sleepTime = desiredTime - timeTaken.ElapsedMilliseconds;
                timeTaken.Reset();

                if (sleepTime > 0)
                {
                    Thread.Sleep((int)sleepTime);
                }
            }
        }
        
        public ServerNettyTick()
        {
            NettyThread = new Thread(Tick);
            NettyThread.Start();

        }
    }
}