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
        private delegate void SafeList(List<string> n, Color c);
        private delegate void SafeState(bool connect);
        private Safe SafeCall;
        private Safe SafeShowLast;
        private SafeList SafeCallList;
        private SafeState SafeSetState;

        private List<string> savedCommands = new List<string>();
        private int savedIndex = 0;
        private string textBeforeHint = "";
        private int textHintIndex = 0;

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
            SafeCallList = new SafeList(Log_List);
            SafeSetState = new SafeState(SetSt_Do);
            CommandAgent.Init(client);
        }

        private void Client_Load(object sender, EventArgs e)
        {
            SetState(false);
            textBoxText.Focus();
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
        private void LogList(List<string> ns)
        {
            if (this.InvokeRequired)
                this.Invoke(this.SafeCallList, ns, Color.White);
            else
                this.Log_List(ns, Color.White);
        }

        private void ShowLast(string n, Color c)
        {
            if (this.InvokeRequired)
                this.Invoke(this.SafeShowLast, "", Color.White);
            else
                this.listView1.Items[this.listView1.Items.Count-1].EnsureVisible();
        }
        private void SetState(bool connect)
        {
            if (this.InvokeRequired)
                this.Invoke(this.SafeSetState, connect);
            else
                this.SetSt_Do(connect);
        }

        private void Log_Local(string n, Color c)
		{
            var lvl = new ListViewItem("");
            lvl.SubItems.Add(n);
            lvl.ForeColor = c;
			this.listView1.Items.Add(lvl);
        }

        private void Log_List(List<string> dts, Color c)
        {
            foreach (var s in dts)
            {
                var lvl = new ListViewItem("");
                lvl.SubItems.Add(s);
                lvl.ForeColor = c;
                this.listView1.Items.Add(lvl);
            }
        }


        private void SetSt_Do(bool connect)
        {
            var c = connect ? Color.Black : Color.DimGray;
            listView1.BackColor = c;
            BackColor = c;
            if (connect)
            {
                buttonConnect.Text = "断开";
                buttonConnect.ForeColor = Color.Red;
            }
            else
            {
                buttonConnect.Text = "连接";
                buttonConnect.ForeColor = Color.LightGreen;
              
            }
        }

        private void client_StateChanged(object sender, NetSockStateChangedEventArgs e)
		{
			this.Log("State: " + e.PrevState.ToString() + " -> " + e.NewState.ToString());
            SetState(e.NewState == SocketState.Connected);
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
            List<string> dts = new List<string>();
            foreach (var s in str.Split('\n'))
            {
                if (s.StartsWith(">"))
                    continue;
               // Log(s);
                CommandAgent.OnReply(s);
                dts.Add(s);
            }

            if (dts.Count > 0)
                LogList(dts);

            ShowLast("", Color.AliceBlue);
        }

		private void client_Connected(object sender, NetSocketConnectedEventArgs e)
		{
			this.Log("Connected: " + e.SourceIP);

            foreach (var cmd in InitRunCmd.Cmds)
            {
                Log(string.Format("[自动指令] > {0}", cmd), Color.OrangeRed);
                CommandAgent.SetCommand(cmd);
                this.client.Send(System.Text.Encoding.Default.GetBytes(cmd + "\n"));
            }
        }

		private void buttonConnect_Click(object sender, EventArgs e)
		{
            if (buttonConnect.Text == "连接")
            {
                DoConnect();
            }
            else
            {
                this.client.Close("强制断开");
            }
		}

        private void DoConnect()
        {
            System.Net.IPEndPoint end = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(this.textBoxConnectTo.Text), 19998);
            this.client.Connect(end);
        }

        private void DoSend()
        {
            var cmd = textBoxText.Text;

            // 防止lua函数调用成员函数语法错误，先这么写写试试
            if (cmd.Contains("("))
                cmd = cmd.Replace(",", ":");

            Log(string.Format("[{0:HH:mm:ss}] > {1}", DateTime.Now, cmd), Color.LightGreen);
            
            CommandAgent.SetCommand(cmd);
            this.client.Send(System.Text.Encoding.Default.GetBytes(cmd + "\n"));

            savedCommands.RemoveAll(c => c == cmd);
            savedCommands.Add(cmd);
            savedIndex = 0;

            Application.DoEvents();

            textBoxText.Text = "";
        }

		private void buttonSendText_Click(object sender, EventArgs e)
        {
            if (textBoxText.Text == "connect")
            {
                DoConnect();
                return;
            }

            if (this.client.State != SocketState.Connected)
			{
				this.Log("请先连接网络");
				return;
			}

            DoSend();
        }

        private void textBoxText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBoxText.Text == "connect")
                {
                    DoConnect();
                    return;
                }
                if (this.client.State != SocketState.Connected)
                {
                    this.Log("请先连接网络");
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
            if (e.KeyCode == Keys.Tab)
            {
                var hintResult = AutoComplete.GetHint(textBeforeHint, textHintIndex++);
                if (hintResult != "")
                {
                    isAutoComplete = true;
                    textBoxText.Text = hintResult;
                    textBoxText.SelectionStart = textBoxText.Text.Length;
                    isAutoComplete = false;
                }

                e.Handled = true;
            }
        }

        private void textBoxText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\t')
            {
                e.Handled = true;
            }
        }

        private bool isAutoComplete;
        private void textBoxText_TextChanged(object sender, EventArgs e)
        {
            var nowText = textBoxText.Text;
            if (!isAutoComplete)
            {
                textBeforeHint = nowText;
                textHintIndex = 0;
            }
            var hintResult = AutoComplete.GetHint(nowText, 0);
            if (hintResult != "")
            {
                labelHint.Text = string.Format("{0}   -- 使用Tab完成补全", hintResult);
            }
            else
            {
                labelHint.Text = "";
            }
        }

    }
}