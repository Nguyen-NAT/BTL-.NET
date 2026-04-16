using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // string[] filesToSend = { "../FileToSend/test.txt" };
        //Set up Client Conn
        // SendFiles("127.0.0.1", 6767, filesToSend);


        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());

    }


    private static void SendFiles(string serverIp, int port, string[] files)
    {
        // using TcpClient client = new(serverIp, port);
        // using NetworkStream stream = client.GetStream();
        // using BinaryWriter writer = new(stream);

        // foreach (string filePath in files)
        // {
        //     string fileName = Path.GetFileName(filePath);
        //     byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);
        //     long fileSize = new FileInfo(filePath).Length;

        //     // //
        //     // Create Metadata
        //     // //

        //     // Sent Name Length
        //     writer.Write(nameBytes.Length);
        //     // Sent Name File
        //     writer.Write(nameBytes);
        //     // Sent File Size
        //     writer.Write(fileSize);

        //     // Sent File Data
        //     using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read);
        //     byte[] buffer = new byte[4096];
        //     int bytesRead;
        //     long totalRead = 0;
        //     while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
        //     {
        //         stream.Write(buffer, 0, bytesRead);
        //         totalRead += bytesRead;
        //         // cal percent downloaded 
        //         double progress = (double)totalRead / fileSize * 100;
        //         Console.Write($"\rLoading:{fileName}: {progress:f2}%\n");
        //     }

        //     Console.WriteLine($"Đã gửi file {fileName} ({fileSize} bytes).");
        // }

        // // Gửi tín hiệu kết thúc (nameLength = 0)
        // writer.Write(0);

        // Console.WriteLine("Hoàn tất gửi tất cả file.");
    }

}