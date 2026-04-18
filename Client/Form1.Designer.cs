namespace Client;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private System.Windows.Forms.TextBox Title;
    private System.Windows.Forms.TextBox Content;
    private System.Windows.Forms.Button Connect;
    private System.Windows.Forms.Button Send;
    private System.Windows.Forms.Button Addfile;
    private System.Windows.Forms.Button Clear;
    private System.Windows.Forms.Button Exit;
    private System.Windows.Forms.Label Notification;
    private System.Windows.Forms.Label LoadfileNof;
    private System.Windows.Forms.Label ContentLabel;
    private System.Windows.Forms.Label TitleLabel;
    private System.Windows.Forms.Label ClientLabel;
    private System.Windows.Forms.PictureBox Picturebox;
    private System.Windows.Forms.ListBox Listboxtitle;
    private System.Windows.Forms.ListBox ClientList;
    private System.Windows.Forms.ProgressBar Downloadbar;
    private System.Windows.Forms.Label DownloadNof;

    
    private void InitializeComponent()

    {
            this.ClientList = new System.Windows.Forms.ListBox();
            this.ClientList.Location = new System.Drawing.Point(430, 446);
            this.ClientList.Size = new System.Drawing.Size(160, 40);

            this.ClientLabel = new System.Windows.Forms.Label();
            this.ClientLabel.Location = new System.Drawing.Point(430, 426);
            this.ClientLabel.Size = new System.Drawing.Size(160, 20);
            this.ClientLabel.Text = "Danh sách:";


            this.DownloadNof = new System.Windows.Forms.Label();
            this.DownloadNof.Location = new System.Drawing.Point(430, 490);
            this.DownloadNof.Size = new System.Drawing.Size(160, 24);
            this.DownloadNof.Text = "Loading:";


            this.Downloadbar = new System.Windows.Forms.ProgressBar();
            this.Downloadbar.Location = new System.Drawing.Point(430, 510);
            this.Downloadbar.Size = new System.Drawing.Size(160, 24);

            this.LoadfileNof = new System.Windows.Forms.Label();
            this.LoadfileNof.Location = new System.Drawing.Point(104, 8);
            this.LoadfileNof.Size = new System.Drawing.Size(104, 24);
    

            this.Content = new System.Windows.Forms.TextBox();
            this.Content.Location = new System.Drawing.Point(48, 200);
            this.Content.Size = new System.Drawing.Size(288, 192);
            this.Content.Multiline = true;
            this.Content.PlaceholderText = "Nhập nội dung...";
            this.Content.ScrollBars = ScrollBars.Vertical;
            this.Content.WordWrap = true;
            this.Content.ReadOnly = true;
            
            
            this.ContentLabel = new System.Windows.Forms.Label();
            this.ContentLabel.Location = new System.Drawing.Point(48, 176);
            this.ContentLabel.Size = new System.Drawing.Size(128, 24);
            // this.ContentLabel.Text = "Nội dung/Ảnh:";

            this.Title = new System.Windows.Forms.TextBox();
            this.Title.Location = new System.Drawing.Point(48, 72);
            this.Title.Size = new System.Drawing.Size(160, 40);
            this.Title.Multiline = true;
            this.Title.PlaceholderText = "Số lượng...";
            // this.Title.ScrollBars = ScrollBars.Horizontal;
            // this.Title.WordWrap = false;
                

            this.TitleLabel = new System.Windows.Forms.Label();
            this.TitleLabel.Location = new System.Drawing.Point(48, 48);
            this.TitleLabel.Size = new System.Drawing.Size(112, 24);
            // this.TitleLabel.Text = "Tiêu đề:";

            this.Listboxtitle = new System.Windows.Forms.ListBox();
            this.Listboxtitle.Location = new System.Drawing.Point(48, 112);
            this.Listboxtitle.Size = new System.Drawing.Size(288, 48);

            this.Picturebox = new System.Windows.Forms.PictureBox();
            this.Picturebox.Location = new System.Drawing.Point(48, 200);
            this.Picturebox.Size = new System.Drawing.Size(288, 192);
            this.Picturebox.SizeMode = PictureBoxSizeMode.Zoom;

            this.Notification = new System.Windows.Forms.Label();
            this.Notification.Location = new System.Drawing.Point(50, 32);
            this.Notification.Size = new System.Drawing.Size(536, 24);
            this.Notification.TextAlign = ContentAlignment.MiddleRight;

            this.Exit = new System.Windows.Forms.Button();
            this.Exit.Location = new System.Drawing.Point(440, 352);
            this.Exit.Size = new System.Drawing.Size(112, 40);
            this.Exit.Text = "Thoát";

            this.Clear = new System.Windows.Forms.Button();
            this.Clear.Location = new System.Drawing.Point(440, 296);
            this.Clear.Size = new System.Drawing.Size(112, 40);
            this.Clear.Text = "Xóa";

            this.Addfile = new System.Windows.Forms.Button();
            this.Addfile.Location = new System.Drawing.Point(440, 160);
            this.Addfile.Size = new System.Drawing.Size(112, 40);
            this.Addfile.Text = "Thêm File";

            this.Send = new System.Windows.Forms.Button();
            this.Send.Location = new System.Drawing.Point(440, 224);
            this.Send.Size = new System.Drawing.Size(112, 40);
            this.Send.Text = "Gửi";

            this.Connect = new System.Windows.Forms.Button();
            this.Connect.Location = new System.Drawing.Point(440, 96);
            this.Connect.Size = new System.Drawing.Size(112, 40);
            this.Connect.Text = "Kết Nối";


            
            this.Picturebox.BorderStyle = BorderStyle.FixedSingle;

            this.FormBorderStyle = FormBorderStyle.Fixed3D;


            // Khung Panel bao quanh toàn bộ nội dung
            Panel mainPanel = new Panel();
            mainPanel.Location = new Point(20, 20);
            mainPanel.Size = new Size(600, 550);    
            mainPanel.BorderStyle = BorderStyle.FixedSingle; 
            


            // GroupBox cho Tiêu đề
            GroupBox gbTitle = new GroupBox();
            gbTitle.Text = "Tiêu đề";
            gbTitle.Location = new Point(40, 70);
            gbTitle.Size = new Size(360, 130);

            // Thêm các control liên quan vào GroupBox
            // this.TitleLabel.Location = new Point(16, 24);
            this.Title.Location = new Point(16, 30);
            this.Title.Size = new Size(320, 30);
            this.Listboxtitle.Location = new Point(16, 70);
            this.Listboxtitle.Size = new Size(320, 45);

            // gbTitle.Controls.Add(this.TitleLabel);
            gbTitle.Controls.Add(this.Title);
            gbTitle.Controls.Add(this.Listboxtitle);

            // GroupBox cho Nội dung/Ảnh
            GroupBox gbContent = new GroupBox();
            gbContent.Text = "Nội dung / Ảnh";
            gbContent.Location = new Point(40, 220);
            gbContent.Size = new Size(360, 320);

            // this.ContentLabel.Location = new Point(16, 24);
            this.Content.Location = new Point(16, 30);
            this.Content.Size = new Size(320, 230);
            this.Picturebox.Location = new Point(16, 30);
            this.Picturebox.Size = new Size(320, 230);

            // gbContent.Controls.Add(this.ContentLabel);
            gbContent.Controls.Add(this.Content);
            gbContent.Controls.Add(this.Picturebox);

            GroupBox gbButtons = new GroupBox();
            gbButtons.Text = "Chức năng";
            gbButtons.Location = new Point(430, 70);
            gbButtons.Size = new Size(160, 340);

            this.Connect.Location = new Point(20, 30);
            this.Addfile.Location = new Point(20, 90);
            this.Send.Location = new Point(20, 150);
            this.Clear.Location = new Point(20, 210);
            this.Exit.Location = new Point(20, 270);

            gbButtons.Controls.Add(this.Connect);
            gbButtons.Controls.Add(this.Addfile);
            gbButtons.Controls.Add(this.Send);
            gbButtons.Controls.Add(this.Clear);
            gbButtons.Controls.Add(this.Exit);

            // Thêm các GroupBox vào Form
            this.Controls.Add(gbTitle);
            this.Controls.Add(gbContent);
            this.Controls.Add(gbButtons);
            this.Controls.Add(Notification);
            // this.Controls.Add(LoadfileNof);
            this.Controls.Add(Downloadbar);
            this.Controls.Add(DownloadNof);
            this.Controls.Add(ClientLabel);
            this.Controls.Add(ClientList);

            this.Controls.Add(mainPanel);



            


            // this.Controls.Add(this.Content);
            // this.Controls.Add(this.Title);
            // this.Controls.Add(this.Notification);
            // this.Controls.Add(this.LoadfileNof);
            // this.Controls.Add(this.TitleLabel);
            // this.Controls.Add(this.ContentLabel);
            // this.Controls.Add(this.Picturebox);
            // this.Controls.Add(this.TitlePicture);

            
        this.Exit.Click += new System.EventHandler(this.Exit_Click);
        // this.Controls.Add(this.Exit);

        this.Clear.Click += new System.EventHandler(this.Clear_Click);
        // this.Controls.Add(this.Clear);

        this.Addfile.Click += new System.EventHandler(this.Addfile_Click);
        // this.Controls.Add(this.Addfile);

        this.Send.Click += new System.EventHandler(this.Send_Click);
        // this.Controls.Add(this.Send);

        this.Connect.Click += new System.EventHandler(this.Connect_Click);
        // this.Controls.Add(this.Connect);

        this.Listboxtitle.SelectedIndexChanged += new System.EventHandler(this.Listbox_SelectedIndexChanged);

        this.ClientList.SelectedIndexChanged += new System.EventHandler(this.ClientList_SelectedIndexChanged);



        components = new System.ComponentModel.Container();
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(650, 600);
        Text = "Form1";
    }

    #endregion

}

