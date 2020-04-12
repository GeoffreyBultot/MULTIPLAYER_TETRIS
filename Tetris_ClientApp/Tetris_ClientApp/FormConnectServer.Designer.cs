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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtBoxServerIP = new System.Windows.Forms.TextBox();
            this.btnGetCode = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCodeReceived
            // 
            this.lblCodeReceived.AutoSize = true;
            this.lblCodeReceived.Location = new System.Drawing.Point(134, 82);
            this.lblCodeReceived.Name = "lblCodeReceived";
            this.lblCodeReceived.Size = new System.Drawing.Size(41, 13);
            this.lblCodeReceived.TabIndex = 0;
            this.lblCodeReceived.Text = "Code : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Enter code";
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Location = new System.Drawing.Point(71, 37);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(57, 13);
            this.lblServerIP.TabIndex = 2;
            this.lblServerIP.Text = "Server IP :";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(133, 120);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 3;
            // 
            // txtBoxServerIP
            // 
            this.txtBoxServerIP.Location = new System.Drawing.Point(133, 37);
            this.txtBoxServerIP.Name = "txtBoxServerIP";
            this.txtBoxServerIP.Size = new System.Drawing.Size(100, 20);
            this.txtBoxServerIP.TabIndex = 5;
            // 
            // btnGetCode
            // 
            this.btnGetCode.Location = new System.Drawing.Point(53, 77);
            this.btnGetCode.Name = "btnGetCode";
            this.btnGetCode.Size = new System.Drawing.Size(75, 23);
            this.btnGetCode.TabIndex = 6;
            this.btnGetCode.Text = "Get Code";
            this.btnGetCode.UseVisualStyleBackColor = true;
            this.btnGetCode.Click += new System.EventHandler(this.btnGetCode_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(136, 148);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 7;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(217, 148);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormConnectServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(304, 183);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnGetCode);
            this.Controls.Add(this.txtBoxServerIP);
            this.Controls.Add(this.textBox1);
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
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtBoxServerIP;
        private System.Windows.Forms.Button btnGetCode;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnCancel;
    }
}