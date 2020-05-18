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
            this.btnAbandonner.Location = new System.Drawing.Point(958, 34);
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
            this.lblScoreMe.Click += new System.EventHandler(this.lblScoreMe_Click);
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
            // formTetrisClientApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1088, 785);
            this.Controls.Add(this.lblScoreRival);
            this.Controls.Add(this.lblScoreMe);
            this.Controls.Add(this.lblYOU);
            this.Controls.Add(this.btnAbandonner);
            this.Controls.Add(this.lblRival);
            this.Name = "formTetrisClientApp";
            this.Text = "Tetris Client Application";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblRival;
        private System.Windows.Forms.Button btnAbandonner;
        private System.Windows.Forms.Label lblYOU;
        private System.Windows.Forms.Label lblScoreMe;
        private System.Windows.Forms.Label lblScoreRival;
    }
}

