using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ProjectC.Server
{
    /**
     * This Class will run on a different thread and will accept new clients.
     */
    public class Server
    {
        private TcpListener ServerSocket;
        public Thread ServerThread;

        public static List<Netty> clientList = new List<Netty>();

        private void Run()
        {
            while (ServerThread.IsAlive)
            {
                Console.WriteLine("Waiting for Client");
                TcpClient clientSocket = ServerSocket.AcceptTcpClient(); // accept connection
                Console.WriteLine("Accepted Client");
                new Netty(clientSocket); 
            }
        }
        
        public Server(IPAddress localAddress, Int32 port)
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