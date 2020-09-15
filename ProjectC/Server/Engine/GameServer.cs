﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ProjectC.Server.Engine.Objects;
using ProjectC.Server.Engine.Ticks;

namespace ProjectC.Server.Engine
{
    /**
     * This Class will run on a different thread and will accept new clients.
     */
    public class GameServer
    {
        private TcpListener ServerSocket;
        public Thread ServerThread;
        public bool Running = true;
        public ServerNettyTick NettyTick;

        public static List<ClientConnection> clientList = new List<ClientConnection>();

        private void Run()
        {
            // start ticks
            NettyTick = new ServerNettyTick();
            new PingClients();

            while (Running)
            {
                try
                {
                    TcpClient clientSocket = ServerSocket.AcceptTcpClient(); // accept connection
                    new ClientConnection(clientSocket);
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
            NettyTick.Running = false;

            foreach (ClientConnection client in clientList)
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
            ServerThread = new Thread(Run);
            ServerThread.Start();
        }
    }
}