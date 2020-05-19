namespace Tetris_ServerApp
{
    partial class TetrisServerApp
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TetrisServerApp));
            this.btnStartServer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelServerStatus = new System.Windows.Forms.Label();
            this.txtBoxIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.monitorServerMessages = new Tetris_ServerApp.Monitor();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.listBoxConnectedClients = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartServer
            // 
            this.btnStartServer.BackColor = System.Drawing.Color.Transparent;
            this.btnStartServer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStartServer.BackgroundImage")));
            this.btnStartServer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStartServer.FlatAppearance.BorderSize = 0;
            this.btnStartServer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStartServer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStartServer.ForeColor = System.Drawing.Color.Transparent;
            this.btnStartServer.Location = new System.Drawing.Point(12, 16);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(97, 78);
            this.btnStartServer.TabIndex = 0;
            this.btnStartServer.UseVisualStyleBackColor = false;
            this.btnStartServer.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F);
            this.label1.Location = new System.Drawing.Point(168, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "SERVER LOGS :";
            // 
            // labelServerStatus
            // 
            this.labelServerStatus.AutoSize = true;
            this.labelServerStatus.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
            this.labelServerStatus.Location = new System.Drawing.Point(331, 45);
            this.labelServerStatus.Name = "labelServerStatus";
            this.labelServerStatus.Size = new System.Drawing.Size(99, 18);
            this.labelServerStatus.TabIndex = 10;
            this.labelServerStatus.Text = "Server OFF";
            // 
            // txtBoxIP
            // 
            this.txtBoxIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtBoxIP.Location = new System.Drawing.Point(115, 41);
            this.txtBoxIP.Name = "txtBoxIP";
            this.txtBoxIP.Size = new System.Drawing.Size(192, 26);
            this.txtBoxIP.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(685, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 22);
            this.label2.TabIndex = 12;
            this.label2.Text = "CLIENT LIST";
            // 
            // monitorServerMessages
            // 
            this.monitorServerMessages.BackColor = System.Drawing.Color.Black;
            this.monitorServerMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.monitorServerMessages.ForeColor = System.Drawing.Color.Green;
            this.monitorServerMessages.Location = new System.Drawing.Point(12, 148);
            this.monitorServerMessages.Multiline = true;
            this.monitorServerMessages.Name = "monitorServerMessages";
            this.monitorServerMessages.NumberOfLines = 2;
            this.monitorServerMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.monitorServerMessages.Size = new System.Drawing.Size(499, 403);
            this.monitorServerMessages.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(896, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(130, 105);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // listBoxConnectedClients
            // 
            this.listBoxConnectedClients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxConnectedClients.BackColor = System.Drawing.Color.Black;
            this.listBoxConnectedClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxConnectedClients.ForeColor = System.Drawing.Color.LimeGreen;
            this.listBoxConnectedClients.FormattingEnabled = true;
            this.listBoxConnectedClients.ItemHeight = 20;
            this.listBoxConnectedClients.Location = new System.Drawing.Point(527, 148);
            this.listBoxConnectedClients.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxConnectedClients.Name = "listBoxConnectedClients";
            this.listBoxConnectedClients.Size = new System.Drawing.Size(499, 404);
            this.listBoxConnectedClients.TabIndex = 9;
            // 
            // TetrisServerApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 587);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBoxIP);
            this.Controls.Add(this.labelServerStatus);
            this.Controls.Add(this.listBoxConnectedClients);
            this.Controls.Add(this.monitorServerMessages);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStartServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TetrisServerApp";
            this.Text = "Tetris Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TetrisServerApp_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Label label1;
        private Monitor monitorServerMessages;
        private System.Windows.Forms.Label labelServerStatus;
        private System.Windows.Forms.TextBox txtBoxIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox listBoxConnectedClients;
    }
}

