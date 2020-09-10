using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using ProjectC.Engine;
using SharpDX.MediaFoundation;

namespace ProjectC.Server
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
                bytesFrom = Arithmetic.TrimByteArray(bytesFrom);
                string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom).Trim();
                Console.WriteLine("MESSAGE FROM CLIENT: "+dataFromClient);
                Console.WriteLine("Client Message Complete!");
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
            Server.clientList.Add(this);
            ThreadStart start = new ThreadStart(Run);
            NettyThread = new Thread(start);
            NettyThread.Start();
        }
    }
}