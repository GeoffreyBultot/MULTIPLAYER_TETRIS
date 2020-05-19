namespace Tetris_ClientApp
{
    partial class FormConnectServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConnectServer));
            this.lblCodeReceived = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.txtBoxCode = new System.Windows.Forms.TextBox();
            this.txtBoxServerIP = new System.Windows.Forms.TextBox();
            this.btnGetCode = new System.Windows.Forms.Button();
            this.btnGoGame = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCodeReceived
            // 
            this.lblCodeReceived.AutoSize = true;
            this.lblCodeReceived.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
            this.lblCodeReceived.Location = new System.Drawing.Point(134, 82);
            this.lblCodeReceived.Name = "lblCodeReceived";
            this.lblCodeReceived.Size = new System.Drawing.Size(63, 18);
            this.lblCodeReceived.TabIndex = 0;
            this.lblCodeReceived.Text = "Code : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
            this.label2.ForeColor = System.Drawing.Color.Cornsilk;
            this.label2.Location = new System.Drawing.Point(31, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Enter code";
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.BackColor = System.Drawing.Color.Transparent;
            this.lblServerIP.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
            this.lblServerIP.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblServerIP.Location = new System.Drawing.Point(37, 34);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(91, 18);
            this.lblServerIP.TabIndex = 2;
            this.lblServerIP.Text = "Server IP :";
            // 
            // txtBoxCode
            // 
            this.txtBoxCode.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8F);
            this.txtBoxCode.Location = new System.Drawing.Point(133, 120);
            this.txtBoxCode.Name = "txtBoxCode";
            this.txtBoxCode.Size = new System.Drawing.Size(100, 20);
            this.txtBoxCode.TabIndex = 3;
            // 
            // txtBoxServerIP
            // 
            this.txtBoxServerIP.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8F);
            this.txtBoxServerIP.Location = new System.Drawing.Point(133, 34);
            this.txtBoxServerIP.Name = "txtBoxServerIP";
            this.txtBoxServerIP.Size = new System.Drawing.Size(100, 20);
            this.txtBoxServerIP.TabIndex = 5;
            // 
            // btnGetCode
            // 
            this.btnGetCode.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8F);
            this.btnGetCode.Location = new System.Drawing.Point(53, 77);
            this.btnGetCode.Name = "btnGetCode";
            this.btnGetCode.Size = new System.Drawing.Size(75, 23);
            this.btnGetCode.TabIndex = 6;
            this.btnGetCode.Text = "Get Code";
            this.btnGetCode.UseVisualStyleBackColor = true;
            this.btnGetCode.Click += new System.EventHandler(this.btnGetCode_Click);
            // 
            // btnGoGame
            // 
            this.btnGoGame.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8F);
            this.btnGoGame.Location = new System.Drawing.Point(136, 148);
            this.btnGoGame.Name = "btnGoGame";
            this.btnGoGame.Size = new System.Drawing.Size(97, 23);
            this.btnGoGame.TabIndex = 7;
            this.btnGoGame.Text = "Go to game";
            this.btnGoGame.UseVisualStyleBackColor = true;
            this.btnGoGame.Click += new System.EventHandler(this.btnGoGame_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8F);
            this.btnCancel.Location = new System.Drawing.Point(234, 148);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8F);
            this.btnConnect.Location = new System.Drawing.Point(239, 31);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 9;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // FormConnectServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(321, 183);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGoGame);
            this.Controls.Add(this.btnGetCode);
            this.Controls.Add(this.txtBoxServerIP);
            this.Controls.Add(this.txtBoxCode);
            this.Controls.Add(this.lblServerIP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCodeReceived);
            this.Name = "FormConnectServer";
            this.Text = "Connect with your friend";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCodeReceived;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblServerIP;
        private System.Windows.Forms.TextBox txtBoxCode;
        private System.Windows.Forms.TextBox txtBoxServerIP;
        private System.Windows.Forms.Button btnGetCode;
        private System.Windows.Forms.Button btnGoGame;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConnect;
    }
}