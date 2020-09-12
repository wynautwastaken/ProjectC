using System;
using System.Net.Sockets;
using System.Threading;
using ProjectC.Engine;
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
            Console.WriteLine("Client Connecting to Server");
            ClientSocket = new TcpClient(server,port);
            Stream = ClientSocket.GetStream();
            ThreadStart start = new ThreadStart(Run);
            ClientThread = new Thread(start);
            ClientThread.Start();
        }

        private void Run()
        {
            while (ClientSocket.Connected)
            {
                BufferWriter buffer = new BufferWriter(1);
                buffer.WriteEnum(PacketType.Ping);

                byte[] bytes = buffer.Buffer;

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