namespace SOA_A1_GIORP
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.regIP = new System.Windows.Forms.TextBox();
            this.regTeam = new System.Windows.Forms.Button();
            this.publish = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.teamName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(32, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(295, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "GIORP Purchase Totalizer";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Registry IP:";
            // 
            // regIP
            // 
            this.regIP.Location = new System.Drawing.Point(105, 169);
            this.regIP.Name = "regIP";
            this.regIP.Size = new System.Drawing.Size(100, 20);
            this.regIP.TabIndex = 4;
            // 
            // regTeam
            // 
            this.regTeam.Location = new System.Drawing.Point(41, 225);
            this.regTeam.Name = "regTeam";
            this.regTeam.Size = new System.Drawing.Size(123, 24);
            this.regTeam.TabIndex = 5;
            this.regTeam.Text = "Register Team";
            this.regTeam.UseVisualStyleBackColor = true;
            this.regTeam.Click += new System.EventHandler(this.regTeam_Click);
            // 
            // publish
            // 
            this.publish.Location = new System.Drawing.Point(41, 316);
            this.publish.Name = "publish";
            this.publish.Size = new System.Drawing.Size(123, 29);
            this.publish.TabIndex = 6;
            this.publish.Text = "Publish Service";
            this.publish.UseVisualStyleBackColor = true;
            this.publish.Click += new System.EventHandler(this.publish_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Team name:";
            // 
            // teamName
            // 
            this.teamName.AutoSize = true;
            this.teamName.Location = new System.Drawing.Point(120, 130);
            this.teamName.Name = "teamName";
            this.teamName.Size = new System.Drawing.Size(37, 13);
            this.teamName.TabIndex = 8;
            this.teamName.Text = "Chaos";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.teamName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.publish);
            this.Controls.Add(this.regTeam);
            this.Controls.Add(this.regIP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "GIORP-Purchase-Totalizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox regIP;
        private System.Windows.Forms.Button regTeam;
        private System.Windows.Forms.Button publish;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label teamName;
    }
}

