using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public static class Server
    {
        private static List<TcpClient> Clients { get; } = new List<TcpClient>();

        private static List<string> ConnectedClients => new List<string>();

        private static void Main()
        {
            var localIPs = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

            var ipv4Addresses = localIPs.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToArray();

            string ipAddress;
            if (ipv4Addresses.Length > 0)
            {
                Console.WriteLine("Available IP addresses:");
                for (var i = 0; i < ipv4Addresses.Length; i++)
                {
                    Console.WriteLine($"{i + 1}: {ipv4Addresses[i]}");
                }

                Console.Write("\nChoose IP address: ");
                var ipAddressIndex = Convert.ToInt32(Console.ReadLine());

                if (ipAddressIndex >= 1 && ipAddressIndex <= ipv4Addresses.Length)
                {
                    ipAddress = ipv4Addresses[ipAddressIndex - 1].ToString();
                }
                else
                {
                    ipAddress = null;
                }
            }
            else
            {
                Console.WriteLine("IP address not found");
                ipAddress = null;
            }

            var port = 5151;

            if (ipAddress != null)
            {
                var listener = new TcpListener(IPAddress.Parse(ipAddress), port);
                listener.Start();
                Console.Clear();
                Console.WriteLine($"Server started on {ipAddress}:{port}. Waiting for client connection\n");

                while (true)
                {
                    var client = listener.AcceptTcpClient();

                    Clients.Add(client);

                    var thread = new Thread(HandleClient);
                    thread.Start(client);
                }
            }
        }

        private static void HandleClient(object obj)
        {
            var client = (TcpClient)obj;
            var stream = client?.GetStream();

            if (client != null)
            {
                var buffer = new byte[client.ReceiveBufferSize];
                var clientUsername = "";

                while (true)
                {
                    int bytesRead;
                    try
                    {
                        bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
                    }
                    catch
                    {
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        break;
                    }

                    var message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    if (string.IsNullOrEmpty(clientUsername))
                    {
                        if (message != "")
                        {
                            clientUsername = message;
                            Console.WriteLine("Client connected: " + clientUsername);
                            ConnectedClients.Add(clientUsername);
                        }
                    }
                    else
                    {
                        ForwardMessage(clientUsername, message, client);
                    }
                }

                Clients.Remove(client);

                if (clientUsername != "")
                {
                    Console.WriteLine("Клиент вышел: " + clientUsername);
                    ConnectedClients.Remove(clientUsername);
                }
            }

            client?.Close();
        }

        //private static void SendClientList()
        //{
        //    string clients = "";
        //    foreach (var connectedClient in ConnectedClients)
        //    {
        //        clients += connectedClient + "\n";
        //    }

        //    byte[] clientListBuffer = Encoding.ASCII.GetBytes("clients\n" + clients);
        //    foreach (var client in Clients)
        //    {
        //        NetworkStream stream = client.GetStream();
        //        stream.Write(clientListBuffer, 0, clientListBuffer.Length);
        //        stream.Flush();
        //    }
        //}

        private static void ForwardMessage(string username, string message, TcpClient sender)
        {
            foreach (var client in Clients)
            {
                if (client != sender)
                {
                    var stream = client.GetStream();
                    var buffer = Encoding.ASCII.GetBytes(username + ":" + message);
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                }
                else
                {
                    var stream = client.GetStream();
                    var buffer = Encoding.ASCII.GetBytes("You:" + message);
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                }
            }
        }
    }
}