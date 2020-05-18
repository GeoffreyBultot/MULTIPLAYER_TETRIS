namespace Tetris_ClientApp
{
    partial class formTetrisClientApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formTetrisClientApp));
            this.lblRival = new System.Windows.Forms.Label();
            this.btnAbandonner = new System.Windows.Forms.Button();
            this.lblYOU = new System.Windows.Forms.Label();
            this.lblScoreMe = new System.Windows.Forms.Label();
            this.lblScoreRival = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtBoxChat = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRival
            // 
            this.lblRival.AutoSize = true;
            this.lblRival.BackColor = System.Drawing.Color.Transparent;
            this.lblRival.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.lblRival.ForeColor = System.Drawing.Color.Red;
            this.lblRival.Location = new System.Drawing.Point(609, 34);
            this.lblRival.Name = "lblRival";
            this.lblRival.Size = new System.Drawing.Size(76, 26);
            this.lblRival.TabIndex = 1;
            this.lblRival.Text = "RIVAL";
            // 
            // btnAbandonner
            // 
            this.btnAbandonner.BackColor = System.Drawing.Color.IndianRed;
            this.btnAbandonner.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAbandonner.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbandonner.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbandonner.Location = new System.Drawing.Point(968, 34);
            this.btnAbandonner.Name = "btnAbandonner";
            this.btnAbandonner.Size = new System.Drawing.Size(90, 52);
            this.btnAbandonner.TabIndex = 2;
            this.btnAbandonner.Text = "READY";
            this.btnAbandonner.UseVisualStyleBackColor = false;
            this.btnAbandonner.Click += new System.EventHandler(this.btnAbandonner_Click);
            // 
            // lblYOU
            // 
            this.lblYOU.AutoSize = true;
            this.lblYOU.BackColor = System.Drawing.Color.Transparent;
            this.lblYOU.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.lblYOU.ForeColor = System.Drawing.Color.Red;
            this.lblYOU.Location = new System.Drawing.Point(188, 34);
            this.lblYOU.Name = "lblYOU";
            this.lblYOU.Size = new System.Drawing.Size(61, 26);
            this.lblYOU.TabIndex = 7;
            this.lblYOU.Text = "YOU";
            // 
            // lblScoreMe
            // 
            this.lblScoreMe.AutoSize = true;
            this.lblScoreMe.BackColor = System.Drawing.Color.Transparent;
            this.lblScoreMe.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.lblScoreMe.ForeColor = System.Drawing.Color.Red;
            this.lblScoreMe.Location = new System.Drawing.Point(173, 60);
            this.lblScoreMe.Name = "lblScoreMe";
            this.lblScoreMe.Size = new System.Drawing.Size(87, 26);
            this.lblScoreMe.TabIndex = 8;
            this.lblScoreMe.Text = "Score 0";
            // 
            // lblScoreRival
            // 
            this.lblScoreRival.AutoSize = true;
            this.lblScoreRival.BackColor = System.Drawing.Color.Transparent;
            this.lblScoreRival.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.lblScoreRival.ForeColor = System.Drawing.Color.Red;
            this.lblScoreRival.Location = new System.Drawing.Point(598, 60);
            this.lblScoreRival.Name = "lblScoreRival";
            this.lblScoreRival.Size = new System.Drawing.Size(87, 26);
            this.lblScoreRival.TabIndex = 9;
            this.lblScoreRival.Text = "Score 0";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(813, 593);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(181, 105);
            this.pictureBox1.TabIndex = 201;
            this.pictureBox1.TabStop = false;
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
            this.btnSend.Location = new System.Drawing.Point(1001, 522);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(74, 65);
            this.btnSend.TabIndex = 204;
            this.btnSend.Text = "SEND";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBoxChat
            // 
            this.txtBoxChat.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtBoxChat.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
            this.txtBoxChat.Location = new System.Drawing.Point(813, 98);
            this.txtBoxChat.Multiline = true;
            this.txtBoxChat.Name = "txtBoxChat";
            this.txtBoxChat.Size = new System.Drawing.Size(262, 418);
            this.txtBoxChat.TabIndex = 205;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
            this.textBox1.Location = new System.Drawing.Point(813, 522);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(181, 65);
            this.textBox1.TabIndex = 206;
            // 
            // formTetrisClientApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1088, 707);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtBoxChat);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblScoreRival);
            this.Controls.Add(this.lblScoreMe);
            this.Controls.Add(this.lblYOU);
            this.Controls.Add(this.btnAbandonner);
            this.Controls.Add(this.lblRival);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "formTetrisClientApp";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "MULTIPLAYER TETRIS";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblRival;
        private System.Windows.Forms.Button btnAbandonner;
        private System.Windows.Forms.Label lblYOU;
        private System.Windows.Forms.Label lblScoreMe;
        private System.Windows.Forms.Label lblScoreRival;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtBoxChat;
        private System.Windows.Forms.TextBox textBox1;
    }
}

