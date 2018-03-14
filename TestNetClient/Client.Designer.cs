namespace TestNetClient
{
	partial class Client
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
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxConnectTo = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxSend = new System.Windows.Forms.TextBox();
            this.buttonSendText = new System.Windows.Forms.Button();
            this.textBoxText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(266, 11);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(75, 21);
            this.buttonDisconnect.TabIndex = 0;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(12, 77);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(830, 340);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(12, 11);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 21);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxConnectTo
            // 
            this.textBoxConnectTo.Location = new System.Drawing.Point(93, 13);
            this.textBoxConnectTo.Name = "textBoxConnectTo";
            this.textBoxConnectTo.Size = new System.Drawing.Size(145, 21);
            this.textBoxConnectTo.TabIndex = 3;
            this.textBoxConnectTo.Text = "127.0.0.1";
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(490, 11);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 21);
            this.buttonSend.TabIndex = 4;
            this.buttonSend.Text = "Send Files";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBoxSend
            // 
            this.textBoxSend.Location = new System.Drawing.Point(571, 13);
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.Size = new System.Drawing.Size(271, 21);
            this.textBoxSend.TabIndex = 5;
            // 
            // buttonSendText
            // 
            this.buttonSendText.Location = new System.Drawing.Point(490, 38);
            this.buttonSendText.Name = "buttonSendText";
            this.buttonSendText.Size = new System.Drawing.Size(75, 21);
            this.buttonSendText.TabIndex = 6;
            this.buttonSendText.Text = "Send Text";
            this.buttonSendText.UseVisualStyleBackColor = true;
            this.buttonSendText.Click += new System.EventHandler(this.buttonSendText_Click);
            // 
            // textBoxText
            // 
            this.textBoxText.Location = new System.Drawing.Point(571, 40);
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.Size = new System.Drawing.Size(271, 21);
            this.textBoxText.TabIndex = 7;
            this.textBoxText.Text = "3535131";
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 427);
            this.Controls.Add(this.textBoxText);
            this.Controls.Add(this.buttonSendText);
            this.Controls.Add(this.textBoxSend);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxConnectTo);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.buttonDisconnect);
            this.Name = "Client";
            this.Text = "Test NetClient";
            this.Load += new System.EventHandler(this.Client_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonDisconnect;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.TextBox textBoxConnectTo;
		private System.Windows.Forms.Button buttonSend;
		private System.Windows.Forms.TextBox textBoxSend;
		private System.Windows.Forms.Button buttonSendText;
		private System.Windows.Forms.TextBox textBoxText;
	}
}

