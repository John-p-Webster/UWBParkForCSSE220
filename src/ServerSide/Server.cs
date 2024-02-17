// Updated C# Program for Server to work with Arduino UWB sketch
// This version automatically detects the server's IP address and includes a check for new messages received
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UWBPark.src.ServerSide
{
    /// <summary>
    /// A server that listens for TCP client connections and processes data received from clients.
    /// </summary>
    public class Server
    {
        private TcpListener tcpListener;
        private TcpClient tcpClient;
        private NetworkStream stream;
        private int port = 11111; // Example port number, adjust as needed
        public ServerData serverData {get; private set;} = new ServerData();
        public delegate void NewMessageReceivedDelegate(ServerData serverData);
        public static event NewMessageReceivedDelegate NewMessageReceived;

        public void StartServer()
        {
            try
            {
                // Automatically detecting the server's IP address (IPv4)
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

                if (ipAddress == null)
                {
                    Console.WriteLine("No IPv4 address found. Exiting...");
                    return;
                }

                tcpListener = new TcpListener(ipAddress, port);

                tcpListener.Start();
                Console.WriteLine("Server started. Listening to TCP clients at " + ipAddress + ":" + port);

                // Continuously listen for client connections
                while (true)
                {
                    tcpClient = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Connected to client.");

                    // Handle client in a new thread
                    HandleClient(tcpClient);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                if (tcpListener != null)
                {
                    tcpListener.Stop();
                }
            }
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                stream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                // Read data from client
                int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
                if (bytesRead > 0)
                {
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received data: " + dataReceived);

                    // Processes data, if successful, updates the server data
                    ServerData? processedData = ProcessData(dataReceived);
                    if (processedData != null)
                    {
                        this.serverData = processedData.Value;
                        NewMessageReceived?.Invoke(this.serverData);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                }
            }
        }

        /// <summary>
        /// Processes the input data and returns a ServerData object if the data is valid.
        /// Input looks like "AnchorID1:AnchorID2:TagID:DistanceFromAnchor1:DistanceFromAnchor2"
        /// </summary>
        /// <param name="data">The input data to be processed.</param>
        /// <returns>A ServerData object if the data is valid; otherwise, null.</returns>
        private ServerData? ProcessData(string data)
        {
            try
            {
                string[] values = data.Split(':');

                if (values.Length == 5)
                {
                    ServerData serverData = new ServerData();

                    if (int.TryParse(values[0].Trim(), out int anchorID1))
                    {
                        serverData.AnchorID1 = anchorID1;
                    }
                    else
                    {
                        throw new ArgumentException("Invalid anchor ID 1");
                    }

                    if (int.TryParse(values[1].Trim(), out int anchorID2))
                    {
                        serverData.AnchorID2 = anchorID2;
                    }
                    else
                    {
                        throw new ArgumentException("Invalid anchor ID 2");
                    }

                    if (int.TryParse(values[2].Trim(), out int tagID))
                    {
                        serverData.TagID = tagID;
                    }
                    else
                    {
                        throw new ArgumentException("Invalid tag ID");
                    }

                    if (double.TryParse(values[3].Trim(), out double distance1) && double.TryParse(values[4].Trim(), out double distance2))
                    {
                        serverData.Distance1 = distance1;
                        serverData.Distance2 = distance2;
                    }
                    else
                    {
                        throw new ArgumentException("Invalid distance values");
                    }
                    return serverData;
                }
                else
                {
                    throw new ArgumentException("Invalid data format");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return null;
                // Handle the exception as needed
            }
        }
    }
}