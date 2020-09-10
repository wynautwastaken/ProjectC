using System;
using System.Net.Sockets;
using System.Threading;
using ProjectC.Networking.Packets;

namespace ProjectC.Client
{
    public class GameClient
    {
        public TcpClient ClientSocket;
        public NetworkStream Stream;
        public Thread ClientThread;
        private bool Running = true;
        public GameClient(string server, int port)
        {
            Console.WriteLine("trying to connect to server");
            ClientSocket = new TcpClient(server,port);
            Thread.Sleep(1000);
            Stream = ClientSocket.GetStream();
            ThreadStart start = new ThreadStart(Run);
            ClientThread = new Thread(start);
            ClientThread.Start();
        }

        private void Run()
        {
            while (Running)
            {
                Packet packet = new Packet(PacketType.Ping);

                byte[] bytes = packet.ByteArray();

                foreach (byte b in bytes)
                {
                    Console.WriteLine(b);
                }
                
                Stream.Write(bytes,0,bytes.Length);
                Disconnect();
            }
        }

        public void Disconnect()
        {
            Running = false;
            Stream.Close();
            ClientSocket.Close();
        }
    }
}