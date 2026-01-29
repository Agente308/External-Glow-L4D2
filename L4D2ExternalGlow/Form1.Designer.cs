namespace L4D2ExternalGlow
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
            this.GlowCHKKKK = new System.Windows.Forms.CheckBox();
            this.InfectedsCHK = new System.Windows.Forms.CheckBox();
            this.GhostsCHK = new System.Windows.Forms.CheckBox();
            this.SurvivorCHK = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // GlowCHKKKK
            // 
            this.GlowCHKKKK.AutoSize = true;
            this.GlowCHKKKK.Location = new System.Drawing.Point(12, 12);
            this.GlowCHKKKK.Name = "GlowCHKKKK";
            this.GlowCHKKKK.Size = new System.Drawing.Size(84, 17);
            this.GlowCHKKKK.TabIndex = 0;
            this.GlowCHKKKK.Text = "Enable glow";
            this.GlowCHKKKK.UseVisualStyleBackColor = true;
            this.GlowCHKKKK.CheckedChanged += new System.EventHandler(this.GlowCHKKKK_CheckedChanged);
            // 
            // InfectedsCHK
            // 
            this.InfectedsCHK.AutoSize = true;
            this.InfectedsCHK.Location = new System.Drawing.Point(12, 35);
            this.InfectedsCHK.Name = "InfectedsCHK";
            this.InfectedsCHK.Size = new System.Drawing.Size(106, 17);
            this.InfectedsCHK.TabIndex = 1;
            this.InfectedsCHK.Text = "Enable Infecteds";
            this.InfectedsCHK.UseVisualStyleBackColor = true;
            this.InfectedsCHK.CheckedChanged += new System.EventHandler(this.InfectedsCHK_CheckedChanged);
            // 
            // GhostsCHK
            // 
            this.GhostsCHK.AutoSize = true;
            this.GhostsCHK.Location = new System.Drawing.Point(12, 58);
            this.GhostsCHK.Name = "GhostsCHK";
            this.GhostsCHK.Size = new System.Drawing.Size(95, 17);
            this.GhostsCHK.TabIndex = 2;
            this.GhostsCHK.Text = "Enable Ghosts";
            this.GhostsCHK.UseVisualStyleBackColor = true;
            this.GhostsCHK.CheckedChanged += new System.EventHandler(this.GhostsCHK_CheckedChanged);
            // 
            // SurvivorCHK
            // 
            this.SurvivorCHK.AutoSize = true;
            this.SurvivorCHK.Location = new System.Drawing.Point(12, 81);
            this.SurvivorCHK.Name = "SurvivorCHK";
            this.SurvivorCHK.Size = new System.Drawing.Size(106, 17);
            this.SurvivorCHK.TabIndex = 5;
            this.SurvivorCHK.Text = "Enable Survivors";
            this.SurvivorCHK.UseVisualStyleBackColor = true;
            this.SurvivorCHK.CheckedChanged += new System.EventHandler(this.SurvivorCHK_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(172, 68);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(188, 188);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 268);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.SurvivorCHK);
            this.Controls.Add(this.GhostsCHK);
            this.Controls.Add(this.InfectedsCHK);
            this.Controls.Add(this.GlowCHKKKK);
            this.Name = "Form1";
            this.Text = "External glow by aika and agente";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox GlowCHKKKK;
        private System.Windows.Forms.CheckBox InfectedsCHK;
        private System.Windows.Forms.CheckBox GhostsCHK;
        private System.Windows.Forms.CheckBox SurvivorCHK;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

