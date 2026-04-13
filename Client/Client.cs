using System.Net;
using System.Net.Sockets;

namespace Client;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {

        //Set up Client Conn
        SendFile(@"..\FileToSend", "127.0.0.1", 6767);


        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());

    }

    private static void SendFile(String FileName, String IPAddress, int Port)
    {
        TcpClient TcpClient = new (IPAddress, Port);
        NetworkStream NetworkStream = TcpClient.GetStream();
        using FileStream File = new(FileName, FileMode.Open, FileAccess.Read);

        byte[] FileBuffer = new byte[File.Length];

        File.ReadExactly(FileBuffer, 0, (int)File.Length);
        NetworkStream.Write(FileBuffer, 0, FileBuffer.GetLength(0));
        NetworkStream.Close();

    }
}