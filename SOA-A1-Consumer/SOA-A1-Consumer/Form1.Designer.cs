namespace SOA_A1_Consumer
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
            this.label2 = new System.Windows.Forms.Label();
            this.serviceList = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.responseMsg = new System.Windows.Forms.TextBox();
            this.execute = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Reg_IP = new System.Windows.Forms.TextBox();
            this.register = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.teamName = new System.Windows.Forms.Label();
            this.query = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.param1 = new System.Windows.Forms.TextBox();
            this.param2 = new System.Windows.Forms.TextBox();
            this.param3 = new System.Windows.Forms.TextBox();
            this.param4 = new System.Windows.Forms.TextBox();
            this.param5 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "SOA-A1 Service Consumer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(32, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Select a service:";
            // 
            // serviceList
            // 
            this.serviceList.FormattingEnabled = true;
            this.serviceList.Items.AddRange(new object[] {
            "GIORP-5000 Purchase Totalizer",
            "Pay Stub Generator",
            "Car Loan Calculator",
            "Canadian Postal Code Validator"});
            this.serviceList.Location = new System.Drawing.Point(179, 97);
            this.serviceList.Name = "serviceList";
            this.serviceList.Size = new System.Drawing.Size(180, 21);
            this.serviceList.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(32, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "Response:";
            // 
            // responseMsg
            // 
            this.responseMsg.Location = new System.Drawing.Point(36, 198);
            this.responseMsg.Multiline = true;
            this.responseMsg.Name = "responseMsg";
            this.responseMsg.Size = new System.Drawing.Size(284, 36);
            this.responseMsg.TabIndex = 4;
            // 
            // execute
            // 
            this.execute.Location = new System.Drawing.Point(71, 415);
            this.execute.Name = "execute";
            this.execute.Size = new System.Drawing.Size(100, 23);
            this.execute.TabIndex = 5;
            this.execute.Text = "Execute";
            this.execute.UseVisualStyleBackColor = true;
            this.execute.Click += new System.EventHandler(this.execute_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(587, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 19);
            this.label4.TabIndex = 6;
            this.label4.Text = "Register team";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(460, 246);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 19);
            this.label5.TabIndex = 7;
            this.label5.Text = "Registry IP Address:";
            // 
            // Reg_IP
            // 
            this.Reg_IP.Location = new System.Drawing.Point(604, 245);
            this.Reg_IP.Name = "Reg_IP";
            this.Reg_IP.Size = new System.Drawing.Size(140, 20);
            this.Reg_IP.TabIndex = 8;
            // 
            // register
            // 
            this.register.Location = new System.Drawing.Point(464, 305);
            this.register.Name = "register";
            this.register.Size = new System.Drawing.Size(75, 23);
            this.register.TabIndex = 9;
            this.register.Text = "Register";
            this.register.UseVisualStyleBackColor = true;
            this.register.Click += new System.EventHandler(this.Register_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(460, 199);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 19);
            this.label6.TabIndex = 10;
            this.label6.Text = "Team name:";
            // 
            // teamName
            // 
            this.teamName.AutoSize = true;
            this.teamName.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teamName.Location = new System.Drawing.Point(600, 198);
            this.teamName.Name = "teamName";
            this.teamName.Size = new System.Drawing.Size(49, 19);
            this.teamName.TabIndex = 11;
            this.teamName.Text = "Chaos";
            // 
            // query
            // 
            this.query.Location = new System.Drawing.Point(406, 96);
            this.query.Name = "query";
            this.query.Size = new System.Drawing.Size(115, 23);
            this.query.TabIndex = 12;
            this.query.Text = "Query Registry";
            this.query.UseVisualStyleBackColor = true;
            this.query.Click += new System.EventHandler(this.Query_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(32, 380);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 19);
            this.label7.TabIndex = 13;
            this.label7.Text = "Parameter 5:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(32, 350);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 19);
            this.label8.TabIndex = 14;
            this.label8.Text = "Parameter 4:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(32, 321);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 19);
            this.label9.TabIndex = 15;
            this.label9.Text = "Parameter 3:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(32, 293);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 19);
            this.label10.TabIndex = 16;
            this.label10.Text = "Parameter 2:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(32, 264);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 19);
            this.label11.TabIndex = 17;
            this.label11.Text = "Parameter 1:";
            // 
            // param1
            // 
            this.param1.Location = new System.Drawing.Point(131, 262);
            this.param1.Name = "param1";
            this.param1.Size = new System.Drawing.Size(100, 20);
            this.param1.TabIndex = 18;
            // 
            // param2
            // 
            this.param2.Location = new System.Drawing.Point(131, 293);
            this.param2.Name = "param2";
            this.param2.Size = new System.Drawing.Size(100, 20);
            this.param2.TabIndex = 19;
            // 
            // param3
            // 
            this.param3.Location = new System.Drawing.Point(131, 319);
            this.param3.Name = "param3";
            this.param3.Size = new System.Drawing.Size(100, 20);
            this.param3.TabIndex = 20;
            // 
            // param4
            // 
            this.param4.Location = new System.Drawing.Point(131, 350);
            this.param4.Name = "param4";
            this.param4.Size = new System.Drawing.Size(100, 20);
            this.param4.TabIndex = 21;
            // 
            // param5
            // 
            this.param5.Location = new System.Drawing.Point(131, 378);
            this.param5.Name = "param5";
            this.param5.Size = new System.Drawing.Size(100, 20);
            this.param5.TabIndex = 22;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.param5);
            this.Controls.Add(this.param4);
            this.Controls.Add(this.param3);
            this.Controls.Add(this.param2);
            this.Controls.Add(this.param1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.query);
            this.Controls.Add(this.teamName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.register);
            this.Controls.Add(this.Reg_IP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.execute);
            this.Controls.Add(this.responseMsg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.serviceList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox serviceList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox responseMsg;
        private System.Windows.Forms.Button execute;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Reg_IP;
        private System.Windows.Forms.Button register;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label teamName;
        private System.Windows.Forms.Button query;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox param1;
        private System.Windows.Forms.TextBox param2;
        private System.Windows.Forms.TextBox param3;
        private System.Windows.Forms.TextBox param4;
        private System.Windows.Forms.TextBox param5;
    }
}

