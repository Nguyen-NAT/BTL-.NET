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
            using NetworkStream stream = client_.GetStream();
            using BinaryReader reader = new(stream);

            while (true)
            {
                // Read Name Length
                int nameLength;
                try
                {
                    nameLength = reader.ReadInt32();
                }
                catch (EndOfStreamException)
                {
                    Console.WriteLine("Client Cooked!");
                    break;
                }
                if (nameLength == 0)
                {
                    Console.WriteLine("Client finished sending files.");
                    break; // marker kết thúc
                }


                // Read Name File
                string fileName = Encoding.UTF8.GetString(reader.ReadBytes(nameLength));
                // Read File Size 
                long fileSize = reader.ReadInt64();
                //kết hợp cả đường dẫn + tên của file.type
                string fullPath = Path.Combine(Filepath, fileName);

                using FileStream File = new(fullPath, FileMode.Create);
                CopyFixedBytes(stream, File, fileSize, fileName);


                // Legacy Code do NOT touch!
                // Write it down in server
                // byte[] buffer = new byte[4096];
                // long totalRead = 0;
                // while (totalRead < fileSize)
                // {
                //     int bytesRead = stream.Read(buffer, 0, buffer.Length);
                //     if (bytesRead == 0) throw new Exception("Client disconnected unexpectedly");
                //     File.Write(buffer, 0, bytesRead);
                //     totalRead += bytesRead;

                //     // cal percent downloaded 
                //     double progress = (double)totalRead / fileSize * 100;
                //     Console.Write($"\rLoading {fileName}:  {progress:f2}%");
                // }

                Console.WriteLine($"File {fileName} received ({fileSize} bytes).");
            }

            client_.Close();
            Console.WriteLine("Client finished sending files.");

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
    }

}