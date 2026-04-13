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

            // ReceievedFile(@"..\ReceivedFile", 6767);
            //Get file from client
        }

        // Setting up server to receive file from client
        private static void ReceievedFile(string Filepath, int Port)
        {
            //Using this Thread for only Server LOOP!
            Console.WriteLine("Server's Starting...");
            Thread ServerThread = new(() =>
            {
                TcpListener server = new(IPAddress.Any, 6767);
                server.Start();
                Console.WriteLine("Server's Starting");

                while (true)
                {
                    TcpClient client_ = server.AcceptTcpClient();
                    Console.WriteLine("Client Connected with" + client_.Client.RemoteEndPoint);


                    //This Thread for Every Single CLIENT!!
                    Thread clientsThread = new(() =>
                    {
                        HandleClient(client_, Filepath);
                    });
                    clientsThread.Start();
                }
            });
        }

        private static void HandleClient(TcpClient client_, string Filepath)
        {
            try
            {
                using NetworkStream stream = client_.GetStream();
                using BinaryReader reader = new(stream);

                while (true)
                {
                    // Đọc độ dài tên file
                    int nameLength = reader.ReadInt32();
                    if (nameLength == 0) break; // đọc đến hết file thì thôi

                    // lấy tên file ra 
                    string fileName = Encoding.UTF8.GetString(reader.ReadBytes(nameLength));

                    // lấy kích thướcc ra 
                    long fileSize = reader.ReadInt64();
                    //kết hợp cả đường dẫn + tên của file.type
                    string fullPath = Path.Combine(Filepath, fileName);
                    using FileStream fs = new(fullPath, FileMode.Create);

                    // 
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
                        Console.Write($"\rLoading:{fileName}: {progress:f2}%");
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

        }
    }

}