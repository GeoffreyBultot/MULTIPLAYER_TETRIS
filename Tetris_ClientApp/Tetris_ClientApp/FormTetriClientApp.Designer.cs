﻿namespace Tetris_ClientApp
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnAbandonner = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(711, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "YOU";
            // 
            // btnAbandonner
            // 
            this.btnAbandonner.Location = new System.Drawing.Point(977, 12);
            this.btnAbandonner.Name = "btnAbandonner";
            this.btnAbandonner.Size = new System.Drawing.Size(75, 23);
            this.btnAbandonner.TabIndex = 2;
            this.btnAbandonner.Text = "RAGEQUIT";
            this.btnAbandonner.UseVisualStyleBackColor = true;
            this.btnAbandonner.Click += new System.EventHandler(this.btnAbandonner_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(241, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "RIVAL";
            // 
            // formTetrisClientApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1088, 785);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAbandonner);
            this.Controls.Add(this.label1);
            this.Name = "formTetrisClientApp";
            this.Text = "Tetris Client Application";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAbandonner;
        private System.Windows.Forms.Label label2;
    }
}
