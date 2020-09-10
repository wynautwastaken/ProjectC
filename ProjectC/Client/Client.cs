using System;
using System.Net.Sockets;
using System.Threading;

namespace ProjectC.Client
{
    public class Client
    {
        public TcpClient ClientSocket;
        public NetworkStream Stream;
        public Thread ClientThread;
        private bool Running = true;
        public Client(string server, int port)
        {
            Console.WriteLine("trying to connect to server");
            ClientSocket = new TcpClient(server,port);
            Thread.Sleep(1000);
            Stream = ClientSocket.GetStream();
            ThreadStart start = new ThreadStart(Run);
            ClientThread = new Thread(start);
            ClientThread.Start();
        }

        public void Run()
        {
            while (Running)
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes("Test Message");
                
                Stream.Write(data,0,data.Length);
                Console.WriteLine("Message Sent to Server");
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