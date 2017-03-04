namespace TS3MusicBot
{
    partial class mainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.axVLCPlugin21 = new AxAXVLC.AxVLCPlugin2();
            this.checkChannel = new System.Windows.Forms.Timer(this.components);
            this.checkForUpdates = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.sqUsername = new System.Windows.Forms.Label();
            this.sqNickname = new System.Windows.Forms.Label();
            this.serverHost = new System.Windows.Forms.Label();
            this.serverName = new System.Windows.Forms.Label();
            this.channelName = new System.Windows.Forms.Label();
            this.DoChatBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.axVLCPlugin21)).BeginInit();
            this.SuspendLayout();
            // 
            // axVLCPlugin21
            // 
            this.axVLCPlugin21.Enabled = true;
            this.axVLCPlugin21.Location = new System.Drawing.Point(13, 12);
            this.axVLCPlugin21.Name = "axVLCPlugin21";
            this.axVLCPlugin21.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVLCPlugin21.OcxState")));
            this.axVLCPlugin21.Size = new System.Drawing.Size(1, 1);
            this.axVLCPlugin21.TabIndex = 0;
            this.axVLCPlugin21.TabStop = false;
            this.axVLCPlugin21.MediaPlayerEncounteredError += new System.EventHandler(this.encounteredError);
            this.axVLCPlugin21.MediaPlayerEndReached += new System.EventHandler(this.songFinished);
            // 
            // checkChannel
            // 
            this.checkChannel.Interval = 6000;
            this.checkChannel.Tick += new System.EventHandler(this.checkChannel_Tick);
            // 
            // checkForUpdates
            // 
            this.checkForUpdates.Interval = 5000;
            this.checkForUpdates.Tick += new System.EventHandler(this.checkForUpdates_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(127, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Jack\'s Music Bot";
            // 
            // sqUsername
            // 
            this.sqUsername.AutoSize = true;
            this.sqUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sqUsername.Location = new System.Drawing.Point(9, 108);
            this.sqUsername.Name = "sqUsername";
            this.sqUsername.Size = new System.Drawing.Size(177, 20);
            this.sqUsername.TabIndex = 2;
            this.sqUsername.Text = "Serverquery username: ";
            // 
            // sqNickname
            // 
            this.sqNickname.AutoSize = true;
            this.sqNickname.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sqNickname.Location = new System.Drawing.Point(9, 144);
            this.sqNickname.Name = "sqNickname";
            this.sqNickname.Size = new System.Drawing.Size(174, 20);
            this.sqNickname.TabIndex = 3;
            this.sqNickname.Text = "Serverquery nickname: ";
            // 
            // serverHost
            // 
            this.serverHost.AutoSize = true;
            this.serverHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverHost.Location = new System.Drawing.Point(12, 183);
            this.serverHost.Name = "serverHost";
            this.serverHost.Size = new System.Drawing.Size(98, 20);
            this.serverHost.TabIndex = 4;
            this.serverHost.Text = "Server host: ";
            // 
            // serverName
            // 
            this.serverName.AutoSize = true;
            this.serverName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverName.Location = new System.Drawing.Point(12, 224);
            this.serverName.Name = "serverName";
            this.serverName.Size = new System.Drawing.Size(107, 20);
            this.serverName.TabIndex = 5;
            this.serverName.Text = "Server name: ";
            // 
            // channelName
            // 
            this.channelName.AutoSize = true;
            this.channelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelName.Location = new System.Drawing.Point(12, 260);
            this.channelName.Name = "channelName";
            this.channelName.Size = new System.Drawing.Size(120, 20);
            this.channelName.TabIndex = 6;
            this.channelName.Text = "Channel name: ";
            // 
            // DoChatBackgroundWorker
            // 
            this.DoChatBackgroundWorker.WorkerSupportsCancellation = true;
            this.DoChatBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DoChatBackgroundWorker_DoWork);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(402, 487);
            this.Controls.Add(this.channelName);
            this.Controls.Add(this.serverName);
            this.Controls.Add(this.serverHost);
            this.Controls.Add(this.sqNickname);
            this.Controls.Add(this.sqUsername);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.axVLCPlugin21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "mainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.mainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axVLCPlugin21)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxAXVLC.AxVLCPlugin2 axVLCPlugin21;
        private System.Windows.Forms.Timer checkChannel;
        private System.Windows.Forms.Timer checkForUpdates;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label sqUsername;
        private System.Windows.Forms.Label sqNickname;
        private System.Windows.Forms.Label serverHost;
        private System.Windows.Forms.Label serverName;
        private System.Windows.Forms.Label channelName;
        private System.ComponentModel.BackgroundWorker DoChatBackgroundWorker;
    }
}

