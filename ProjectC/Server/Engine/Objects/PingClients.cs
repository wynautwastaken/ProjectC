using System.Diagnostics;
using ProjectC.Server.Engine.Ticks.Tickable;
using ProjectC.Universal.Networking.Packets;

namespace ProjectC.Server.Engine.Objects
{
    public class PingClients : NettyTickable
    {
        BufferWriter buffer = new BufferWriter(1);
        Stopwatch _stopwatch = new Stopwatch();
        
        
        public override void Step()
        {
            foreach (ClientConnection client in GameServer.clientList)
            {
                buffer.ResetPosition();
                // send ping
                buffer.WriteEnum(PacketType.Ping);
                
                client.SendBuffer(buffer);
            }
        }
    }
}