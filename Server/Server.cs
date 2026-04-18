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
        static string folderPath = @"../FileToSend";//prepared file


        //Setting up server
        public static void Main(string[] args)
        {
            StartServer(@"../ReceivedFIle", 6767);

            HotkeyListener();
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
                    // Console.WriteLine("\nClient Connected with" + client_.Client.RemoteEndPoint);


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
                //add client to list
                connectedClients[clientId] = client_;
            }

            Console.WriteLine("***********************");
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
                    if (nameLength <= 0)
                    {
                        Console.WriteLine($"Client finished sending {FileCount} files.");
                        break; // Marker End here
                    }


                    // Read Name File
                    string fileName = Encoding.UTF8.GetString(reader.ReadBytes(nameLength));
                    // Read File Size 
                    long fileSize = reader.ReadInt64();
                    //Create Path 

                    var ep = (IPEndPoint)client_.Client.RemoteEndPoint;
                    string clientFolderName = $"{ep.Address.MapToIPv4()}_{ep.Port}";
                    // This will be like ../ReceivedFile/127.0.0.1:6767/
                    string clientFolderPath = Path.Combine(Filepath, clientFolderName);
                    Directory.CreateDirectory(clientFolderPath);

                    using FileStream File = new(Path.Combine(clientFolderPath, fileName), FileMode.Create);
                    CopyFixedBytes(stream, File, fileSize, fileName);

                    Console.WriteLine($"File {fileName} received ({fileSize} bytes).");
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"\nClient {clientId} đã đóng kết nối");
                lock (connectedClients)
                {
                    connectedClients.Remove(clientId);
                }
            }
            Console.WriteLine("**************************************");
            // client_.Close();

        }
        private static void CopyFixedBytes(Stream input, Stream output, long bytesToCopy, string fileName)
        {
            byte[] buffer = new byte[8192];
            long remaining = bytesToCopy;
            long totalRead = 0;
            double progress;
            int barLength = 35;
            int filledLength;
            while (remaining > 0)
            {
                int read = input.Read(buffer, 0, (int)Math.Min(buffer.Length, remaining));
                if (read == 0) throw new EndOfStreamException("Client disconnected unexpectedly");
                output.Write(buffer, 0, read);
                remaining -= read;
                totalRead += read;

                progress = (double)totalRead / bytesToCopy * 100;
                // Console.WriteLine($"Loading {fileName}: ");
                // Console.Write($"\rLoading {fileName}: {progress}%       ");
                filledLength = (int)(progress / 100 * barLength);
                string bar = new string('#', filledLength).PadRight(barLength);
                Console.Write($"\r[{bar}] {progress:f2}%                                          "); // do NOT delete that space!!!
            }
            Console.WriteLine();
        }

        private static void SendFileToClient(TcpClient client, string FilePath)
        {
            int index = 1;
            // Showing All Client 
            foreach (var kv in connectedClients)
            {
                Console.WriteLine($"Client #{index} {kv.Key}");
                index++;
            }

            Thread sendThread = new(() =>
            {
                NetworkStream stream = client.GetStream();
                BinaryWriter writer = new(stream);

                string fileName = Path.GetFileName(FilePath);
                byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);
                long fileSize = new FileInfo(FilePath).Length;

                IPEndPoint remoteEp = client.Client.RemoteEndPoint as IPEndPoint;
                string clientInfo = $"{remoteEp!.Address.MapToIPv4()}_{remoteEp.Port}";
                byte[] clientInfoBytes = Encoding.UTF8.GetBytes(clientInfo);
                //header
                writer.Write(clientInfoBytes.Length);
                writer.Write(clientInfoBytes);
                //name
                writer.Write(nameBytes.Length);
                writer.Write(nameBytes);
                //size
                writer.Write(fileSize);

                using FileStream fs = new(FilePath, FileMode.Open, FileAccess.Read);
                fs.CopyTo(stream);

                writer.Flush();
            });
            sendThread.Start();
            sendThread.IsBackground = true;
        }
        private static void HotkeyListener()
        {
            Thread listener = new(() =>
            {

                while (true)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.M)
                    {
                        // Console.Clear();
                        Menu();
                        Console.WriteLine("Go back to Log mode. Press M to open menu");
                    }
                }
            });
            listener.Start();
            listener.IsBackground = true;
            // Thread.Sleep(Timeout.Infinite);
        }

        private static void Menu()
        {
            while (true)
            {
                Console.WriteLine("=== Menu ===");
                Console.WriteLine("1. Show connected clients");
                Console.WriteLine("2. Show all files");
                Console.WriteLine("3. Sent file to client");
                Console.WriteLine("0. Exit");
                Console.Write("Option: ");

                if (!int.TryParse(Console.ReadLine(), out int option)) continue;

                switch (option)
                {
                    case 1:
                        int index = 1;
                        foreach (var kv in connectedClients)
                        {
                            Console.WriteLine($"Client #{index}: {kv.Key}");
                            index++;
                        }
                        break;
                    case 2:
                        ShowFiles();
                        break;
                    case 3:
                        SendFileInteractive();
                        break;
                    case 0:
                        return; // thoát menu, quay lại log
                }
            }
        }

        private static void ShowFiles()
        {

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"{folderPath} does not exist!");
                return;
            }

            string[] files = Directory.GetFiles(folderPath);
            if (files.Length == 0)
            {
                Console.WriteLine("NO file in this folder");
                return;
            }

            Console.WriteLine("=== File List ===");
            int index = 1;
            foreach (var f in files)
            {
                Console.WriteLine($"{index}. {Path.GetFileName(f)}");
                index++;
            }
        }
        private static void SendFileInteractive()
        {
            // Show connected client
            Console.WriteLine("=== Danh sách client ===");
            int clientIndex = 1;
            foreach (var kv in connectedClients)
            {
                Console.WriteLine($"{clientIndex}. Client ID: {kv.Key}");
                clientIndex++;
            }
            // Choose client
            Console.Write("Chọn số thứ tự client: ");
            if (!int.TryParse(Console.ReadLine(), out int selectedClientIndex)) return; // if true we have selectedClientIndex 
            if (selectedClientIndex < 1 || selectedClientIndex > connectedClients.Count) return;

            var selectedClient = connectedClients.ElementAt(selectedClientIndex - 1).Value;

            // Show available File
            ShowFiles();

            // Choose File
            string[] files = Directory.GetFiles(folderPath);
            Console.Write("Chọn số thứ tự file: ");
            if (!int.TryParse(Console.ReadLine(), out int selectedFileIndex)) return; // if true we have selectedFileIndex 
            if (selectedFileIndex < 1 || selectedFileIndex > files.Length) return;

            string selectedFile = files[selectedFileIndex - 1];

            // FInally , Sen file
            SendFileToClient(selectedClient, selectedFile);
            Console.WriteLine($"Đã gửi {Path.GetFileName(selectedFile)} tới client #{selectedClientIndex}");
        }

    }
}