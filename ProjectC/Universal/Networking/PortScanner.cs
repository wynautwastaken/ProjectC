using System;
using System.Net;
using System.Net.Sockets;

namespace ProjectC.Universal.Networking
{
    public static class PortScanner
    {
        /*
         * port numbers:
         * 1-1024: well known ports
         * 1024-49151: registered port
         * 49151-65535: dynamic/private ports
         */
        
        private static int MaxPort = 65535;
        private static int MinPort = 49151;
        public static int ScanTcpPorts(IPAddress address)
        {
            Random random = new Random();
            int port = random.Next(MinPort,MaxPort);
            using (TcpClient tcpClient = new TcpClient())
            {
                while (true)
                {
                    try
                    {
                        tcpClient.Connect(address, port);
                        Console.WriteLine("Port Closed: "+port); 
                        port = random.Next(MinPort,MaxPort);
                    }
                    catch (Exception e)
                    {
                        // port closed
                        return port;
                    }
                }
            }
        }
    }
}