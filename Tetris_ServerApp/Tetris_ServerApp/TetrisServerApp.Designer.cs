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
            this.btnStartServer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.monitorServerMessages = new Tetris_ServerApp.Monitor();
            this.listBoxConnectedClients = new System.Windows.Forms.ListBox();
            this.labelServerStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(12, 12);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(75, 23);
            this.btnStartServer.TabIndex = 0;
            this.btnStartServer.Text = "Start Server";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(144, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // monitorServerMessages
            // 
            this.monitorServerMessages.BackColor = System.Drawing.Color.Black;
            this.monitorServerMessages.ForeColor = System.Drawing.Color.Green;
            this.monitorServerMessages.Location = new System.Drawing.Point(288, 66);
            this.monitorServerMessages.Multiline = true;
            this.monitorServerMessages.Name = "monitorServerMessages";
            this.monitorServerMessages.NumberOfLines = 2;
            this.monitorServerMessages.Size = new System.Drawing.Size(257, 275);
            this.monitorServerMessages.TabIndex = 3;
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
            this.listBoxConnectedClients.Location = new System.Drawing.Point(4, 66);
            this.listBoxConnectedClients.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxConnectedClients.Name = "listBoxConnectedClients";
            this.listBoxConnectedClients.Size = new System.Drawing.Size(279, 264);
            this.listBoxConnectedClients.TabIndex = 9;
            // 
            // labelServerStatus
            // 
            this.labelServerStatus.AutoSize = true;
            this.labelServerStatus.Location = new System.Drawing.Point(391, 22);
            this.labelServerStatus.Name = "labelServerStatus";
            this.labelServerStatus.Size = new System.Drawing.Size(32, 13);
            this.labelServerStatus.TabIndex = 10;
            this.labelServerStatus.Text = "State";
            // 
            // TetrisServerApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 353);
            this.Controls.Add(this.labelServerStatus);
            this.Controls.Add(this.listBoxConnectedClients);
            this.Controls.Add(this.monitorServerMessages);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStartServer);
            this.Name = "TetrisServerApp";
            this.Text = "Tetris Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Label label1;
        private Monitor monitorServerMessages;
        private System.Windows.Forms.ListBox listBoxConnectedClients;
        private System.Windows.Forms.Label labelServerStatus;
    }
}

