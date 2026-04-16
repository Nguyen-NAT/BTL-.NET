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
    private System.Windows.Forms.Label Notification;
    private System.Windows.Forms.Button Connect;
    private System.Windows.Forms.Button Send;
    private System.Windows.Forms.Button Addfile;
    private System.Windows.Forms.Button Clear;
    private System.Windows.Forms.Button Exit;
    private void InitializeComponent()
    {
        this.Content = new System.Windows.Forms.TextBox();
        this.Content.Location = new System.Drawing.Point(64, 160);
        this.Content.Size = new System.Drawing.Size(304, 144);
        this.Content.Multiline = true;



        this.Title = new System.Windows.Forms.TextBox();
        this.Title.Location = new System.Drawing.Point(64, 56);
        this.Title.Size = new System.Drawing.Size(304, 54);
        this.Title.Multiline = true;


        this.Notification = new System.Windows.Forms.Label();
        this.Notification.Location = new System.Drawing.Point(80, 16);
        this.Notification.Size = new System.Drawing.Size(256, 24);

        this.Exit = new System.Windows.Forms.Button();
        this.Exit.Location = new System.Drawing.Point(408, 344);
        this.Exit.Size = new System.Drawing.Size(112, 40);
        this.Exit.Text = "Exit";

        this.Clear = new System.Windows.Forms.Button();
        this.Clear.Location = new System.Drawing.Point(408, 272);
        this.Clear.Size = new System.Drawing.Size(112, 40);
        this.Clear.Text = "Clear";

        this.Addfile = new System.Windows.Forms.Button();
        this.Addfile.Location = new System.Drawing.Point(408, 152);
        this.Addfile.Size = new System.Drawing.Size(112, 48);
        this.Addfile.Text = "Addfile";

        this.Send = new System.Windows.Forms.Button();
        this.Send.Location = new System.Drawing.Point(408, 88);
        this.Send.Size = new System.Drawing.Size(112, 40);
        this.Send.Text = "Send";

        this.Connect = new System.Windows.Forms.Button();
        this.Connect.Location = new System.Drawing.Point(408, 24);
        this.Connect.Size = new System.Drawing.Size(112, 40);
        this.Connect.Text = "Connect";



        this.Controls.Add(this.Content);
        this.Controls.Add(this.Title);
        this.Controls.Add(this.Notification);

        this.Exit.Click += new System.EventHandler(this.Exit_Click);
        this.Controls.Add(this.Exit);

        this.Clear.Click += new System.EventHandler(this.Clear_Click);
        this.Controls.Add(this.Clear);

        this.Addfile.Click += new System.EventHandler(this.Addfile_Click);
        this.Controls.Add(this.Addfile);

        this.Send.Click += new System.EventHandler(this.Send_Click);
        this.Controls.Add(this.Send);

        this.Connect.Click += new System.EventHandler(this.Connect_Click);
        this.Controls.Add(this.Connect);

        components = new System.ComponentModel.Container();
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(600, 450);
        Text = "Form1";
    }

    #endregion

}
