
namespace Spotify_Playlist_Downloader
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonGetSongs = new System.Windows.Forms.Button();
            this.textBox_PlaylistID = new System.Windows.Forms.TextBox();
            this.listView_SongsList = new System.Windows.Forms.ListView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paypalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.paypalToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.developedByHenryRichardJToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.socialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.youtubeChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.telegramChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonGetSongs);
            this.groupBox1.Controls.Add(this.textBox_PlaylistID);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 25);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(744, 72);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Playlist URL or ID";
            // 
            // buttonGetSongs
            // 
            this.buttonGetSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetSongs.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGetSongs.Location = new System.Drawing.Point(628, 11);
            this.buttonGetSongs.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonGetSongs.Name = "buttonGetSongs";
            this.buttonGetSongs.Size = new System.Drawing.Size(108, 51);
            this.buttonGetSongs.TabIndex = 1;
            this.buttonGetSongs.Text = "Get Songs";
            this.buttonGetSongs.UseVisualStyleBackColor = true;
            this.buttonGetSongs.Click += new System.EventHandler(this.buttonGetSongs_Click);
            // 
            // textBox_PlaylistID
            // 
            this.textBox_PlaylistID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PlaylistID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_PlaylistID.Location = new System.Drawing.Point(4, 29);
            this.textBox_PlaylistID.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_PlaylistID.Name = "textBox_PlaylistID";
            this.textBox_PlaylistID.Size = new System.Drawing.Size(608, 21);
            this.textBox_PlaylistID.TabIndex = 0;
            // 
            // listView_SongsList
            // 
            this.listView_SongsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_SongsList.HideSelection = false;
            this.listView_SongsList.Location = new System.Drawing.Point(0, 102);
            this.listView_SongsList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listView_SongsList.Name = "listView_SongsList";
            this.listView_SongsList.Size = new System.Drawing.Size(745, 455);
            this.listView_SongsList.TabIndex = 1;
            this.listView_SongsList.UseCompatibleStateImageBehavior = false;
            this.listView_SongsList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_SongsList_MouseClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.donateToolStripMenuItem,
            this.socialToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(744, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // donateToolStripMenuItem
            // 
            this.donateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.paypalToolStripMenuItem,
            this.donateToolStripMenuItem1,
            this.developedByHenryRichardJToolStripMenuItem});
            this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
            this.donateToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.donateToolStripMenuItem.Text = "About";
            this.donateToolStripMenuItem.Click += new System.EventHandler(this.donateToolStripMenuItem_Click);
            // 
            // paypalToolStripMenuItem
            // 
            this.paypalToolStripMenuItem.Name = "paypalToolStripMenuItem";
            this.paypalToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.paypalToolStripMenuItem.Text = "Github";
            this.paypalToolStripMenuItem.Click += new System.EventHandler(this.paypalToolStripMenuItem_Click);
            // 
            // donateToolStripMenuItem1
            // 
            this.donateToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.paypalToolStripMenuItem1});
            this.donateToolStripMenuItem1.Name = "donateToolStripMenuItem1";
            this.donateToolStripMenuItem1.Size = new System.Drawing.Size(231, 22);
            this.donateToolStripMenuItem1.Text = "Donate";
            // 
            // paypalToolStripMenuItem1
            // 
            this.paypalToolStripMenuItem1.Name = "paypalToolStripMenuItem1";
            this.paypalToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.paypalToolStripMenuItem1.Text = "Paypal";
            this.paypalToolStripMenuItem1.Click += new System.EventHandler(this.paypalToolStripMenuItem1_Click);
            // 
            // developedByHenryRichardJToolStripMenuItem
            // 
            this.developedByHenryRichardJToolStripMenuItem.Name = "developedByHenryRichardJToolStripMenuItem";
            this.developedByHenryRichardJToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.developedByHenryRichardJToolStripMenuItem.Text = "Developed By Henry Richard J";
            // 
            // socialToolStripMenuItem
            // 
            this.socialToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.youtubeChannelToolStripMenuItem,
            this.telegramChannelToolStripMenuItem});
            this.socialToolStripMenuItem.Name = "socialToolStripMenuItem";
            this.socialToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.socialToolStripMenuItem.Text = "Social";
            // 
            // youtubeChannelToolStripMenuItem
            // 
            this.youtubeChannelToolStripMenuItem.Name = "youtubeChannelToolStripMenuItem";
            this.youtubeChannelToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.youtubeChannelToolStripMenuItem.Text = "Youtube Channel";
            this.youtubeChannelToolStripMenuItem.Click += new System.EventHandler(this.youtubeChannelToolStripMenuItem_Click);
            // 
            // telegramChannelToolStripMenuItem
            // 
            this.telegramChannelToolStripMenuItem.Name = "telegramChannelToolStripMenuItem";
            this.telegramChannelToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.telegramChannelToolStripMenuItem.Text = "Telegram Channel";
            this.telegramChannelToolStripMenuItem.Click += new System.EventHandler(this.telegramChannelToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 556);
            this.Controls.Add(this.listView_SongsList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Spotify Playlist Downloader";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonGetSongs;
        private System.Windows.Forms.TextBox textBox_PlaylistID;
        private System.Windows.Forms.ListView listView_SongsList;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem paypalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem paypalToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem developedByHenryRichardJToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem socialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem youtubeChannelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem telegramChannelToolStripMenuItem;
    }
}

