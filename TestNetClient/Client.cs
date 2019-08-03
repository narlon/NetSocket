using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using JLM.NetSocket;

namespace TestNetClient
{
	public partial class Client : Form
	{
		private NetClient client = new NetClient();

		private delegate void Safe(string n, Color c);
		private Safe SafeCall;
        private Safe SafeShowLast;

        private List<string> savedCommands = new List<string>();
        private int savedIndex = 0;

        public Client()
		{
			InitializeComponent();

			this.client.Connected += new EventHandler<NetSocketConnectedEventArgs>(client_Connected);
			this.client.DataArrived += new EventHandler<NetSockDataArrivalEventArgs>(client_DataArrived);
			this.client.Disconnected += new EventHandler<NetSocketDisconnectedEventArgs>(client_Disconnected);
			this.client.ErrorReceived += new EventHandler<NetSockErrorReceivedEventArgs>(client_ErrorReceived);
			this.client.StateChanged += new EventHandler<NetSockStateChangedEventArgs>(client_StateChanged);

			this.SafeCall = new Safe(Log_Local);
            SafeShowLast = new Safe(ShowLast);
            CommandAgent.Init(client);
        }

        private void Client_Load(object sender, EventArgs e)
        {
        }

        private void Log(string n, Color c)
		{
			if (this.InvokeRequired)
				this.Invoke(this.SafeCall, n, c);
			else
				this.Log_Local(n, c);
		}
        private void Log(string n)
        {
            if (this.InvokeRequired)
                this.Invoke(this.SafeCall, n, Color.White);
            else
                this.Log_Local(n, Color.White);
        }

        private void ShowLast(string n, Color c)
        {
            if (this.InvokeRequired)
                this.Invoke(this.SafeShowLast, "", Color.White);
            else
                this.listView1.Items[this.listView1.Items.Count-1].EnsureVisible();
        }

        private void Log_Local(string n, Color c)
		{
            var lvl = new ListViewItem("");
            lvl.SubItems.Add(n);
            lvl.ForeColor = c;
			this.listView1.Items.Add(lvl);
        }

		private void client_StateChanged(object sender, NetSockStateChangedEventArgs e)
		{
			this.Log("State: " + e.PrevState.ToString() + " -> " + e.NewState.ToString());
		}

		private void client_ErrorReceived(object sender, NetSockErrorReceivedEventArgs e)
		{
			if (e.Exception.GetType() == typeof(System.Net.Sockets.SocketException))
			{
				System.Net.Sockets.SocketException s = (System.Net.Sockets.SocketException)e.Exception;
				this.Log("Error: " + e.Function + " - " + s.SocketErrorCode.ToString() + "\r\n" + s.ToString());
			}
			else
				this.Log("Error: " + e.Function + "\r\n" + e.Exception.ToString());
		}

		private void client_Disconnected(object sender, NetSocketDisconnectedEventArgs e)
		{
			this.Log("Disconnected: " + e.Reason);
		}

		private void client_DataArrived(object sender, NetSockDataArrivalEventArgs e)
		{
            string str = System.Text.Encoding.Default.GetString(e.Data);
            foreach (var s in str.Split('\n'))
            {
                if (s.StartsWith(">"))
                    continue;
                Log(s);
                CommandAgent.OnReply(s);
            }

            ShowLast("", Color.AliceBlue);
        }

		private void client_Connected(object sender, NetSocketConnectedEventArgs e)
		{
			this.Log("Connected: " + e.SourceIP);
		}

		private void buttonDisconnect_Click(object sender, EventArgs e)
		{
			this.client.Close("User forced");
		}

		private void buttonConnect_Click(object sender, EventArgs e)
		{
			System.Net.IPEndPoint end = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(this.textBoxConnectTo.Text), 19998);
			this.client.Connect(end);
		}

        private void DoSend()
        {
            Log(string.Format("[{0:hh:mm:ss}] > {1}", DateTime.Now, textBoxText.Text), Color.LightGreen);

            var cmd = textBoxText.Text;
            CommandAgent.SetCommand(cmd);
            this.client.Send(System.Text.Encoding.Default.GetBytes(cmd + "\n"));

            savedCommands.RemoveAll(c => c == cmd);
            savedCommands.Add(textBoxText.Text);
            savedIndex = 0;

            Application.DoEvents();

            textBoxText.Text = "";
        }

		private void buttonSendText_Click(object sender, EventArgs e)
		{
			if (this.client.State != SocketState.Connected)
			{
				this.Log("Send Cancelled");
				return;
			}

            DoSend();
        }

        private void textBoxText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.client.State != SocketState.Connected)
                {
                    this.Log("Send Cancelled");
                    return;
                }

                DoSend();
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (savedCommands.Count > 0)
                {
                    savedIndex--;
                    if (Math.Abs(savedIndex) > savedCommands.Count)
                        savedIndex = -savedCommands.Count;
                    textBoxText.Text = savedCommands[savedCommands.Count + savedIndex];
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (savedCommands.Count > 0)
                {
                    savedIndex++;
                    if (savedIndex >= 0)
                        savedIndex = -1;
                    textBoxText.Text = savedCommands[savedCommands.Count + savedIndex];
                }
            }
        }
    }
}