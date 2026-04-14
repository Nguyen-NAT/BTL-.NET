using System;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection.Metadata;
using System.Text;
using System.Xml;

namespace Server
{
    public class Program
    {
        //Setting up server
        public static void Main(string[] args)
        {
            StartServer(@"../ReceivedFIle",6767);
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
                while (true)
                {
                    
                    Console.WriteLine("Server's waiting for client!!\n");
                    TcpClient client_ = server.AcceptTcpClient();
                    Console.WriteLine("\nClient Connected with" + client_.Client.RemoteEndPoint);


                    //This Thread for Every Single CLIENT!!
                    Thread clientsThread = new(() =>
                    {
                        HandleClient(client_, Filepath);
                    });
                    clientsThread.Start();
                }
            });
            ServerThread.Start();
        }

        private static void HandleClient(TcpClient client_, string Filepath)
        {
            try
            {
                using NetworkStream stream = client_.GetStream();
                using BinaryReader reader = new(stream);

                while (true)
                {
                    // Read Name Length
                    int nameLength = reader.ReadInt32();
                    if (nameLength == 0) break; // đọc đến hết tên file thì thôi

                    // // 
                    // Receieved Metadata
                    // //


                    // Read Name File
                    string fileName = Encoding.UTF8.GetString(reader.ReadBytes(nameLength));

                    // Read File Size 
                    long fileSize = reader.ReadInt64();
                    //kết hợp cả đường dẫn + tên của file.type
                    string fullPath = Path.Combine(Filepath, fileName);
                    using FileStream fs = new(fullPath, FileMode.Create);

                    // Write it down in server
                    byte[] buffer = new byte[4096];
                    long totalRead = 0;
                    while (totalRead < fileSize)
                    {
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0) throw new Exception("Client disconnected unexpectedly");
                        fs.Write(buffer, 0, bytesRead);
                        totalRead += bytesRead;

                        // cal percent downloaded 
                        double progress = (double)totalRead / fileSize * 100;
                        Console.Write($"\rLoading:{fileName}: {progress:f2}%\n");
                    }

                    Console.WriteLine($"File {fileName} received ({fileSize} bytes).");
                }

                Console.WriteLine("Client finished sending files.");
            }
            catch (Exception err)
            {
                Console.WriteLine("Client error: " + err.Message);
            }
            finally
            {
                client_.Close();

            }

            Console.WriteLine("server closed");
            
        }
    }

}