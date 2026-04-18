using System;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection.Metadata;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace Server
{
    public class Program
    {
        // Storing Client Info
        // using String for storing IP and TcpClient/Socket for port
        static Dictionary<string, TcpClient> connectedClients = new();


        //Setting up server
        public static void Main(string[] args)
        {
            StartServer(@"../ReceivedFIle", 6767);
        }

        // Setting up server to receive file from client
        private static void StartServer(string Filepath, int Port)
        {
            //Using this Thread for only Server LOOP!
            Thread ServerThread = new(() =>
            {
                TcpListener server = new(IPAddress.Any, Port);
                server.Start();
                Console.WriteLine("Server's Starting");
                Console.WriteLine("Server's waiting for client!!\n");
                while (true)
                {
                    TcpClient client_ = server.AcceptTcpClient();
                    Console.WriteLine("\nClient Connected with" + client_.Client.RemoteEndPoint);


                    //This Thread for Every Single CLIENT!!
                    Thread clientsThread = new(() =>
                    {
                        HandleClient(client_, Filepath);
                    });
                    // clientsThread.IsBackground = true; 
                    clientsThread.Start();
                }
            });
            // ServerThread.IsBackground = true;
            ServerThread.Start();
        }

        private static void HandleClient(TcpClient client_, string Filepath)
        {

            // with this we will have client's IP and Port
            string clientId = client_.Client.RemoteEndPoint!.ToString()!;

            lock (connectedClients)
            {
                connectedClients[clientId] = client_;
            }

            Console.WriteLine("client info");
            foreach (var kv in connectedClients)
            {
                Console.WriteLine($"ID: {kv.Key}");
            }

            try
            {
                using NetworkStream stream = client_.GetStream();
                using BinaryReader reader = new(stream);
                // Process Client File
                while (true)
                {
                    // Read Name Length
                    int nameLength;
                    int FileCount = 0;
                    try
                    {
                        nameLength = reader.ReadInt32();
                        FileCount++;
                    }
                    catch (EndOfStreamException)
                    {
                        Console.WriteLine($"Client {clientId} đã ngắt kết nối");
                        lock (connectedClients)
                        {
                            connectedClients.Remove(clientId);
                        }
                        client_.Close();
                        break;
                    }
                    if (nameLength == 0)
                    {
                        Console.WriteLine($"Client finished sending {FileCount} files.");
                        break; // Marker End here
                    }


                    // Read Name File
                    string fileName = Encoding.UTF8.GetString(reader.ReadBytes(nameLength));
                    // Read File Size 
                    long fileSize = reader.ReadInt64();
                    //kết hợp cả đường dẫn + tên của file.type
                    string fullPath = Path.Combine(Filepath, fileName);

                    using FileStream File = new(fullPath, FileMode.Create);
                    CopyFixedBytes(stream, File, fileSize, fileName);

                    Console.WriteLine($"File {fileName} received ({fileSize} bytes).");
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"Client {clientId} đã đóng kết nối");
            }

            // client_.Close();

        }
        private static void CopyFixedBytes(Stream input, Stream output, long bytesToCopy, string fileName)
        {
            byte[] buffer = new byte[8192];
            long remaining = bytesToCopy;
            long totalRead = 0;

            while (remaining > 0)
            {
                int read = input.Read(buffer, 0, (int)Math.Min(buffer.Length, remaining));
                if (read == 0) throw new EndOfStreamException("Client disconnected unexpectedly");
                output.Write(buffer, 0, read);
                remaining -= read;
                totalRead += read;

                double progress = (double)totalRead / bytesToCopy * 100;
                // Console.WriteLine($"Loading {fileName}: ");
                Console.Write($"\rLoading {fileName}: {progress}%");
                Console.WriteLine();
            }
        }

        private static void SendFileToClient(TcpClient client, string FilePath)
        {

            // Showing All Client 
            foreach (var kv in connectedClients)
            {
                Console.WriteLine($"ID: {kv.Key}");
            }


            using NetworkStream stream = client.GetStream();
            using BinaryWriter writer = new(stream);

            string fileName = Path.GetFileName(FilePath);
            byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);
            long fileSize = new FileInfo(FilePath).Length;

            writer.Write(nameBytes.Length);
            writer.Write(nameBytes);
            writer.Write(fileSize);

            using FileStream fs = new(FilePath, FileMode.Open, FileAccess.Read);
            fs.CopyTo(stream);

            writer.Flush();





        }
    }

}