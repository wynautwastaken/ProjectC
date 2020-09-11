using System;
using System.Net.Sockets;
using System.Threading;
using ProjectC.Networking.Packets;
using ProjectC.Networking.Server;

namespace ProjectC.Server.Engine
{
    /**
     * This thread will handle individual player packets
     */
    public class Netty
    {
        public TcpClient ClientSocket;
        public static Thread NettyThread;
        public bool running = true;

        public void Run()
        {
            while (running)
            {
                NetworkStream networkStream = ClientSocket.GetStream();
                byte[] bytesFrom = new byte[1025];
                networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                
                BufferReader reader = new BufferReader(bytesFrom);

                switch (reader.ReadEnum<PacketType>())
                {
                    case PacketType.Ping:
                        Console.WriteLine("Ping!");
                        break;
                }
                
                return;
            }
        }

        public void Disconnect()
        {
            ClientSocket.Close();
            running = false;
        }
        
        public Netty(TcpClient socket)
        {
            ClientSocket = socket;
            GameServer.clientList.Add(this);
            ThreadStart start = new ThreadStart(Run);
            NettyThread = new Thread(start);
            NettyThread.Start();
        }
    }
}