using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using ProjectC.Universal.Networking.Packets;

namespace ProjectC.Server.Engine
{
    /**
     * This thread will handle individual player packets
     */
    public class ClientConnection
    {
        public TcpClient ClientSocket;
        public static Thread NettyThread;
        public bool running = true;
        public int Ping = 0;

        public void Run()
        {
            while (running)
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
                            
                            break;
                    }
                }
                catch (IOException e)
                {
                    if (e.Message.Contains(
                        "Unable to read data from the transport connection: A blocking operation was interrupted by a call to WSACancelBlockingCall..")
                    )
                    {
                        // this error doesn't matter
                        running = false;
                    }
                }
            }
        }

        public void SendBuffer(BufferWriter buffer)
        {
             NetworkStream stream = ClientSocket.GetStream();
             stream.Write(buffer.Buffer);
             stream.Flush();
        }

        public void Disconnect()
        {
            ClientSocket.Close();
            running = false;
        }
        
        public ClientConnection(TcpClient socket)
        {
            ClientSocket = socket;
            GameServer.clientList.Add(this);
            ThreadStart start = new ThreadStart(Run);
            NettyThread = new Thread(start);
            NettyThread.Start();
        }
    }
}