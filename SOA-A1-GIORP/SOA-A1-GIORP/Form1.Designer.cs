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
            this.publish = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.teamName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.regResponse = new System.Windows.Forms.TextBox();
            this.teamIDText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pubPortText = new System.Windows.Forms.TextBox();
            this.pubIPText = new System.Windows.Forms.TextBox();
            this.regPortTxt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.beginListen = new System.Windows.Forms.Button();
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
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Team ID:";
            // 
            // regIP
            // 
            this.regIP.Location = new System.Drawing.Point(440, 213);
            this.regIP.Name = "regIP";
            this.regIP.Size = new System.Drawing.Size(100, 20);
            this.regIP.TabIndex = 4;
            // 
            // publish
            // 
            this.publish.Location = new System.Drawing.Point(38, 282);
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
            this.teamName.Size = new System.Drawing.Size(36, 13);
            this.teamName.TabIndex = 8;
            this.teamName.Text = "jTeam";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 374);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Registry Response:";
            // 
            // regResponse
            // 
            this.regResponse.Location = new System.Drawing.Point(146, 371);
            this.regResponse.Multiline = true;
            this.regResponse.Name = "regResponse";
            this.regResponse.Size = new System.Drawing.Size(245, 67);
            this.regResponse.TabIndex = 10;
            // 
            // teamIDText
            // 
            this.teamIDText.Location = new System.Drawing.Point(123, 172);
            this.teamIDText.Name = "teamIDText";
            this.teamIDText.Size = new System.Drawing.Size(100, 20);
            this.teamIDText.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Published IP:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 246);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Published port:";
            // 
            // pubPortText
            // 
            this.pubPortText.Location = new System.Drawing.Point(123, 246);
            this.pubPortText.Name = "pubPortText";
            this.pubPortText.Size = new System.Drawing.Size(100, 20);
            this.pubPortText.TabIndex = 14;
            // 
            // pubIPText
            // 
            this.pubIPText.Location = new System.Drawing.Point(123, 213);
            this.pubIPText.Name = "pubIPText";
            this.pubIPText.Size = new System.Drawing.Size(100, 20);
            this.pubIPText.TabIndex = 15;
            // 
            // regPortTxt
            // 
            this.regPortTxt.Location = new System.Drawing.Point(440, 246);
            this.regPortTxt.Name = "regPortTxt";
            this.regPortTxt.Size = new System.Drawing.Size(100, 20);
            this.regPortTxt.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(365, 216);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Registry IP:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(365, 253);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Registry port:";
            // 
            // beginListen
            // 
            this.beginListen.Location = new System.Drawing.Point(604, 202);
            this.beginListen.Name = "beginListen";
            this.beginListen.Size = new System.Drawing.Size(123, 31);
            this.beginListen.TabIndex = 19;
            this.beginListen.Text = "Begin listening";
            this.beginListen.UseVisualStyleBackColor = true;
            this.beginListen.Click += new System.EventHandler(this.beginListen_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.beginListen);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.regPortTxt);
            this.Controls.Add(this.pubIPText);
            this.Controls.Add(this.pubPortText);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.teamIDText);
            this.Controls.Add(this.regResponse);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.teamName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.publish);
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
        private System.Windows.Forms.Button publish;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label teamName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox regResponse;
        private System.Windows.Forms.TextBox teamIDText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox pubPortText;
        private System.Windows.Forms.TextBox pubIPText;
        private System.Windows.Forms.TextBox regPortTxt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button beginListen;
    }
}

