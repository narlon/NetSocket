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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxConnectTo = new System.Windows.Forms.TextBox();
            this.buttonSendText = new System.Windows.Forms.Button();
            this.textBoxText = new System.Windows.Forms.TextBox();
            this.labelHint = new System.Windows.Forms.Label();
            this.listView1 = new TestNetClient.DoubleBufferedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDisconnect.ForeColor = System.Drawing.Color.Violet;
            this.buttonDisconnect.Location = new System.Drawing.Point(244, 12);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(75, 21);
            this.buttonDisconnect.TabIndex = 0;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConnect.ForeColor = System.Drawing.Color.GreenYellow;
            this.buttonConnect.Location = new System.Drawing.Point(163, 12);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 21);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxConnectTo
            // 
            this.textBoxConnectTo.BackColor = System.Drawing.Color.Black;
            this.textBoxConnectTo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxConnectTo.Font = new System.Drawing.Font("ËÎÌו", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxConnectTo.ForeColor = System.Drawing.Color.Fuchsia;
            this.textBoxConnectTo.Location = new System.Drawing.Point(12, 14);
            this.textBoxConnectTo.Name = "textBoxConnectTo";
            this.textBoxConnectTo.Size = new System.Drawing.Size(145, 19);
            this.textBoxConnectTo.TabIndex = 3;
            this.textBoxConnectTo.Text = "127.0.0.1";
            // 
            // buttonSendText
            // 
            this.buttonSendText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSendText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSendText.ForeColor = System.Drawing.Color.GreenYellow;
            this.buttonSendText.Location = new System.Drawing.Point(767, 414);
            this.buttonSendText.Name = "buttonSendText";
            this.buttonSendText.Size = new System.Drawing.Size(75, 21);
            this.buttonSendText.TabIndex = 6;
            this.buttonSendText.Text = "Send";
            this.buttonSendText.UseVisualStyleBackColor = true;
            this.buttonSendText.Click += new System.EventHandler(this.buttonSendText_Click);
            // 
            // textBoxText
            // 
            this.textBoxText.AcceptsTab = true;
            this.textBoxText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxText.BackColor = System.Drawing.Color.Black;
            this.textBoxText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxText.Font = new System.Drawing.Font("Consolas", 12F);
            this.textBoxText.ForeColor = System.Drawing.Color.White;
            this.textBoxText.Location = new System.Drawing.Point(12, 413);
            this.textBoxText.Multiline = true;
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.Size = new System.Drawing.Size(749, 26);
            this.textBoxText.TabIndex = 7;
            this.textBoxText.Text = "ls";
            this.textBoxText.TextChanged += new System.EventHandler(this.textBoxText_TextChanged);
            this.textBoxText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxText_KeyDown);
            this.textBoxText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxText_KeyPress);
            // 
            // labelHint
            // 
            this.labelHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelHint.AutoSize = true;
            this.labelHint.ForeColor = System.Drawing.Color.GreenYellow;
            this.labelHint.Location = new System.Drawing.Point(14, 393);
            this.labelHint.Name = "labelHint";
            this.labelHint.Size = new System.Drawing.Size(11, 12);
            this.labelHint.TabIndex = 9;
            this.labelHint.Text = "-";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.BackColor = System.Drawing.Color.Black;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.ForeColor = System.Drawing.Color.White;
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.Location = new System.Drawing.Point(12, 41);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(830, 341);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 5;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 1900;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(854, 446);
            this.Controls.Add(this.labelHint);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.textBoxText);
            this.Controls.Add(this.buttonSendText);
            this.Controls.Add(this.textBoxConnectTo);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonDisconnect);
            this.Name = "Client";
            this.Text = "Test NetClient";
            this.Load += new System.EventHandler(this.Client_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonDisconnect;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.TextBox textBoxConnectTo;
		private System.Windows.Forms.Button buttonSendText;
		private System.Windows.Forms.TextBox textBoxText;
        private DoubleBufferedListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label labelHint;
    }
}

