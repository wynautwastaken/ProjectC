using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ProjectC.Server.Engine;

namespace ProjectC.Networking.Server
{
    /**
     * This Class will run on a different thread and will accept new clients.
     */
    public class GameServer
    {
        private TcpListener ServerSocket;
        public Thread ServerThread;
        public bool Running = true;

        public static List<Netty> clientList = new List<Netty>();

        private void Run()
        {
            while (Running)
            {
                try
                {
                    TcpClient clientSocket = ServerSocket.AcceptTcpClient(); // accept connection
                    new Netty(clientSocket);
                }
                catch (SocketException e)
                {
                    if (e.ErrorCode == 10004) // the thread was interrupted
                    {
                        return;
                    }
                }
            }
        }

        public void Stop()
        {
            ServerSocket.Stop();
            Running = false;

            foreach (Netty client in clientList)
            {
                client.Disconnect();
            }
        }
        
        public GameServer(IPAddress localAddress, Int32 port)
        {
            Console.WriteLine("Initializing Server on port: "+port);
            ServerSocket = new TcpListener(localAddress, port);

            ServerSocket.Start();
            
            Console.WriteLine("Starting Server Thread");
            ServerThread = new Thread(new ThreadStart(Run));
            ServerThread.Start();
        }
    }
}