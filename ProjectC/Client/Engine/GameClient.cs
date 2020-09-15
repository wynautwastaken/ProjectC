using System;
using System.Net.Sockets;
using System.Threading;
using ProjectC.Universal.Networking.Packets;

namespace ProjectC.Client.Engine
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
                try
                {
                    NetworkStream networkStream = ClientSocket.GetStream();
                    byte[] bytesFrom = new byte[1025];
                    networkStream.Read(bytesFrom, 0, bytesFrom.Length);

                    BufferReader reader = new BufferReader(bytesFrom);

                    switch (reader.ReadEnum<PacketType>())
                    {
                        case PacketType.Ping:
                            //Console.WriteLine("Client Ping");
                            BufferWriter writer = new BufferWriter(1);
                            writer.WriteEnum(PacketType.Ping);
                            SendBuffer(writer);
                            break;
                    }
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine("Client Disconnecting");
                    Disconnect();
                }
            }
        }

        public void SendBuffer(BufferWriter buffer)
        {
            //Console.WriteLine("Sending Buffer");
            NetworkStream stream = ClientSocket.GetStream();
            stream.Write(buffer.Buffer);
            stream.Flush();
        }

        public void Disconnect()
        {
            Running = false;
            Stream.Close();
            ClientSocket.Close();
        }
    }
}