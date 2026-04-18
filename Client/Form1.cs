using System.Net.Sockets;
using System.Text;
using System.Media;

namespace Client;

public partial class Form1 : Form
{
    readonly string serverIp = "127.0.0.1";
    readonly int port = 6767;
    //declare a client;
    TcpClient? client = null;
    SoundPlayer? player;
    List<TcpClient> allClients = [];
    string[] filesToSend = [];

    public Form1()
    {
        InitializeComponent();
        Send.Enabled = false;
        Picturebox.Visible = false;
        Content.Visible = false;
    }

    private void Connect_Click(object sender, EventArgs e)
    {
        try
        {
            // Connect to the Server !!
            client = new(serverIp, port);
            if (client.Connected)
            {
                Message("Đã kết đối đến Server thành công!");
                allClients.Add(client);
                // ListenForServerData();
                Send.Enabled = true;
                Connect.Enabled = false;
                filesToSend = []; // reset file storage
            }
            else Message("Không thể kết nối đến Server!");
        }
        catch (SocketException ex)
        {
            Message($"Không thể kết nối đến Server: {ex.Message}");
        }
    }

    private void Send_Click(object sender, EventArgs e)
    {
        if (filesToSend.Length == 0)
        {
            Message("Hãy thêm File trước khi gửi!");
            return;
        }
        Thread sendThread = new(() =>
        {
            Connect.Enabled = true;
            // dot not use "using" if you wanna disposing them
            NetworkStream stream = client!.GetStream();
            BinaryWriter writer = new(stream);

            foreach (string filePath in filesToSend)
            {
                string fileName = Path.GetFileName(filePath);
                byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);
                long fileSize = new FileInfo(filePath).Length;

                // //
                // Send Metadata
                // //

                // Sent Name Length
                writer.Write(nameBytes.Length);
                // Sent Name File
                writer.Write(nameBytes);
                // Sent File Size
                writer.Write(fileSize);

                // //
                // Sent File's Data
                // //
                using FileStream File = new(filePath, FileMode.Open, FileAccess.Read);
                File.CopyTo(stream);

            }
            filesToSend = [];
            // writer.Write(0); // marker end
            writer.Flush();
            // client.Close();
        });
        sendThread.IsBackground = true; // auto close when closing app, using this preventing crash out
        sendThread.Start();
    }

    private void Addfile_Click(object sender, EventArgs e)
    {
        filesToSend = [];
        using OpenFileDialog Explorer = new()
        {
            Title = "Choose Your File!",
            Filter = "(*.*)|*.*", // Allow any file you want
            Multiselect = true
        };

        if (Explorer.ShowDialog() == DialogResult.OK)
        {
            // Thêm các file được chọn vào danh sách
            var newFiles = Explorer.FileNames;
            filesToSend = filesToSend.Concat(newFiles).ToArray();
            Message($"Đã thêm {newFiles.Length} file vào danh sách gửi.");
            // Show Number of Files
            Title.Text = $"Số lượng file: {filesToSend.Length}";
            // Show File list in listbox
            Listbox.Items.Clear();
            foreach (var file in filesToSend)
            {
                Listbox.Items.Add(Path.GetFileName(file));
            }


        }
    }

    private void Clear_Click(object sender, EventArgs e)
    {
        Title.Clear();
        Content.Clear();
        Notification.Text = "";
    }
    private void Exit_Click(object sender, EventArgs e)
    {
        foreach (var client in allClients)
        {
            try { client.Close(); } catch { }
        }
        Application.Exit();
    }

    private void Message(string mess)
    {
        Notification.Text = mess;
    }

    private void Listbox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Listbox.SelectedIndex == -1) return;

        string filePath = filesToSend[Listbox.SelectedIndex];
        string ext = Path.GetExtension(filePath).ToLower();

        if (ext == ".docx" || ext == ".txt" || ext == ".log" || ext == ".csv" || ext == ".json" || ext == ".xml")
        {
            // Preview text
            string[] lines = File.ReadLines(filePath).Take(50).ToArray(); // Read 50 Rows;
            Content.Visible = true;
            Picturebox.Visible = false;

            Content.Text = string.Join(Environment.NewLine, lines);
        }
        else if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".bmp")
        {
            // Preview IMG
            Content.Visible = false;
            Picturebox.Visible = true;
            Picturebox.Image = Image.FromFile(filePath);
        }
        else if (ext == ".wav")
        {
            player = new(filePath);
            player.Load();
            player.Play();
            Content.Text = "Play Preview";
        }
        else
        {
            // Không preview được
            Picturebox.Visible = false;
            Content.Visible = true;
            Content.Text = "Not previewable";
        }
    }
    private void ListenForServerData()
    {
        Thread recvThread = new(() =>
        {
            using NetworkStream stream = client.GetStream();
            using BinaryReader reader = new(stream);

            while (true)
            {
                try
                {
                    // 
                    // Reading MetaData
                    // 

                    // Getting NameLength
                    int nameLen = reader.ReadInt32();
                    if (nameLen == 0) return; // marker end
                    // Getting Name
                    byte[] nameBytes = reader.ReadBytes(nameLen);
                    string fileName = Encoding.UTF8.GetString(nameBytes);
                    // Getting File
                    long fileSize = reader.ReadInt64();

                    using FileStream fs = new(fileName, FileMode.Create, FileAccess.Write);
                    byte[] buffer = new byte[4096];
                    long totalRead = 0;
                    long remaining = fileSize;
                    // Writing File into Destinated Folder
                    while (remaining > 0)
                    {
                        int bytesRead = stream.Read(buffer, 0, (int)Math.Min(buffer.Length, remaining));
                        if (bytesRead == 0) break; // socket closed
                        fs.Write(buffer, 0, bytesRead);

                        totalRead += bytesRead;
                        remaining -= bytesRead;

                        // Loading Progress
                        double progress = (double)totalRead / fileSize * 100;
                        Console.WriteLine($"\rĐang nhận {fileName}: {progress:f2}%");
                        //using progress bar later, Too lazy
                    }

                    Message($"Đã nhận xong file {fileName} ({fileSize} bytes).");
                }
                catch
                {
                    Message("Server đã đóng kết nối");
                }

            }
        });
        recvThread.IsBackground = true;
        recvThread.Start();
    }



}
