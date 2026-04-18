using System.Net.Sockets;
using System.Text;
using System.Media;
using System.Net;

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

    string ClientFolder = "../ClientFolder";

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
                ClientList.Items.Add($"Client#{allClients.Count}:{client.Client.RemoteEndPoint!.ToString()!}");
                ListenForServerData();
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
        sendThread.Start();
        sendThread.IsBackground = true; // auto close when closing app, using this preventing crash out
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
            Listboxtitle.Items.Clear();
            foreach (var file in filesToSend)
            {
                Listboxtitle.Items.Add(Path.GetFileName(file));
            }
        }
    }

    private void Clear_Click(object sender, EventArgs e)
    {
        // basically clear everything
        Title.Clear();
        Content.Clear();
        Notification.Text = "";
        Listboxtitle.Items.Clear();
        filesToSend = [];
        Downloadbar.Value = 0;
        // also clear current client
        try
        {
            client!.Close();
            int index = allClients.IndexOf(client);
            if (index >= 0)
            {
                allClients.RemoveAt(index);//remove client from list 
                ClientList.Items.RemoveAt(index); //remove client from UI (listbox)
            }
        }
        catch (Exception ex)
        {
            Message($"Có lỗi khi xóa Client: {ex.Message}");
        }

        Connect.Enabled = true;
        // Addfile.Enabled = false;
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

    private void downloadMessage(string mess)
    {
        DownloadNof.Text = mess;
    }
    private void Listbox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Listboxtitle.SelectedIndex == -1) return;

        string filePath = filesToSend[Listboxtitle.SelectedIndex];
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
    private void ClientList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ClientList.SelectedIndex == -1) return;

        client = allClients[ClientList.SelectedIndex]; // switch to choosen client

        if (client.Connected)
        {
            Message($"Đã chọn client {ClientList.SelectedItem}");
        }
        else
        {
            Message("Client này đã ngắt kết nối!");
        }
    }
    private void ListenForServerData()
    {
        Thread recvThread = new(() =>
        {
            using NetworkStream stream = client!.GetStream();
            using BinaryReader reader = new(stream);

            while (true)
            {
                try
                {
                    // 
                    // Reading MetaData
                    // 


                    // Getting Header
                    int infoLength = reader.ReadInt32();
                    byte[] infoBytes = reader.ReadBytes(infoLength);
                    string clientInfo = Encoding.UTF8.GetString(infoBytes);
                    // Getting NameLength
                    int nameLen = reader.ReadInt32();
                    if (nameLen == 0) return; // marker end
                    // Getting Name
                    byte[] nameBytes = reader.ReadBytes(nameLen);
                    string fileName = "Server." + Encoding.UTF8.GetString(nameBytes);
                    // Getting File
                    long fileSize = reader.ReadInt64();

                    //create directory
                   
                    string ServerFolderPath = Path.Combine(ClientFolder, clientInfo);
                    Directory.CreateDirectory(ServerFolderPath);
                    string fullPath = Path.Combine(ServerFolderPath, fileName);

                    // Writing File into Destinated Folder
                    using FileStream fs = new(fullPath, FileMode.Create, FileAccess.Write);
                    byte[] buffer = new byte[4096];
                    long totalRead = 0;
                    long remaining = fileSize;
                    downloadMessage($"Đang tải {fileName}...");
                    while (remaining > 0)
                    {
                        int bytesRead = stream.Read(buffer, 0, (int)Math.Min(buffer.Length, remaining));
                        if (bytesRead == 0) break; // socket closed
                        fs.Write(buffer, 0, bytesRead);

                        totalRead += bytesRead;
                        remaining -= bytesRead;

                        // Loading Progress
                        double progress = (double)totalRead / fileSize * 100;
                        Downloadbar.Value = (int)progress;
                    }
                    downloadMessage($"Đã tải xong {fileName}");

                }
                catch (Exception ex)
                {
                    Message($"Server đã đóng kết nối {ex.Message}");

                }

            }
        });
        recvThread.Start();
        recvThread.IsBackground = true;
    }

}
