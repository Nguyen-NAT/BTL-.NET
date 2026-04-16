using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

public partial class Form1 : Form
{
    readonly string serverIp = "127.0.0.1";
    readonly int port = 6767;

    //declare a client;
    TcpClient client = null;
    string[] filesToSend = 
    { 
        "../FileToSend/test.txt",
        "../FileToSend/file1.txt"
    };

    public Form1()
    {
        InitializeComponent();
    }

    private void Connect_Click(object sender, EventArgs e)
    {
        // Connect to the Server !!
        client = new(serverIp, port);
        if (client.Connected) Message("Đã kết đối đến Server thành công!");
        else Message("Không thể kết nối đến Server!");
        
        // this will include Endpoint
    }



    private void Send_Click(object sender, EventArgs e)
    {
        using NetworkStream stream = client.GetStream();
        using BinaryWriter writer = new(stream);
        foreach (string filePath in filesToSend)
        {
            string fileName = Path.GetFileName(filePath);
            byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);
            long fileSize = new FileInfo(filePath).Length;

            // //
            // Create Metadata
            // //

            // Sent Name Length
            writer.Write(nameBytes.Length);
            // Sent Name File
            writer.Write(nameBytes);
            // Sent File Size
            writer.Write(fileSize);
            
            
            // 
            // Sent File's Data
            // 
            using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead;
            long totalRead = 0;
            while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
            {
                stream.Write(buffer, 0, bytesRead);
                totalRead += bytesRead;
                // cal percent downloaded 
                double progress = (double)totalRead / fileSize * 100;
                Console.Write($"\rLoading:{fileName}: {progress:f2}%\n");
            }

            Console.WriteLine($"Đã gửi file {fileName} ({fileSize} bytes).");
        }
        writer.Write(0);
        // Stop Writing All Files Are Sent 

    }

    private void Addfile_Click(object sender, EventArgs e)
    {
        
    }

    private void Clear_Click(object sender, EventArgs e)
    {
        Title.Clear();
        Content.Clear();
        Notification.Text = "";
    }
    private void Exit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void Message(string mess)
    {
        Notification.Text = mess;
    }

}
