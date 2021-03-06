using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;

using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
 * @package   NetSocket
 * @version   1.0
 * @author    Jeremy Messenger <jlmessengertech+githib@gmail.com>
 * @copyright 2011 Jeremy Messenger
 * @license   LGPL <http://www.gnu.org/licenses/lgpl.html>
 * @link      http://jlmessenger.com
 */
namespace JLM.NetSocket
{
	#region Enums
	public enum SocketState
	{
		Closed,
		Closing,
		Connected,
		Connecting,
		Listening,
	}
	#endregion

	#region Event Args
	public class NetSocketConnectedEventArgs : EventArgs
	{
		public IPAddress SourceIP;
		public NetSocketConnectedEventArgs(IPAddress ip)
		{
			this.SourceIP = ip;
		}
	}

	public class NetSocketDisconnectedEventArgs : EventArgs
	{
		public string Reason;
		public NetSocketDisconnectedEventArgs(string reason)
		{
			this.Reason = reason;
		}
	}

	public class NetSockStateChangedEventArgs : EventArgs
	{
		public SocketState NewState;
		public SocketState PrevState;
		public NetSockStateChangedEventArgs(SocketState newState, SocketState prevState)
		{
			this.NewState = newState;
			this.PrevState = prevState;
		}
	}

	public class NetSockDataArrivalEventArgs : EventArgs
	{
	    public NetBase Net;
        public byte[] Data;
		public NetSockDataArrivalEventArgs(NetBase net, byte[] data)
		{
		    Net = net;
			this.Data = data;
		}
	}

	public class NetSockErrorReceivedEventArgs : EventArgs
	{
		public string Function;
		public Exception Exception;
		public NetSockErrorReceivedEventArgs(string function, Exception ex)
		{
			this.Function = function;
			this.Exception = ex;
		}
	}

	public class NetSockConnectionRequestEventArgs : EventArgs
	{
		public Socket Client;
		public NetSockConnectionRequestEventArgs(Socket client)
		{
			this.Client = client;
		}
	}
	#endregion

	#region Socket Classes
	public abstract class NetBase
	{
		#region Fields
		/// <summary>Current socket state</summary>
		protected SocketState state = SocketState.Closed;
		/// <summary>The socket object, obviously</summary>
		protected Socket socket;

		/// <summary>Keep track of when data is being sent</summary>
		protected bool isSending = false;

		/// <summary>Store incoming bytes to be processed</summary>
		protected byte[] byteBuffer = new byte[8192];
        private readonly SendQueue m_SendQueue = new SendQueue();

        /// <summary>Position of the bom header in the rxBuffer</summary>
        protected int rxHeaderIndex = -1;
		/// <summary>Expected length of the message from the bom header</summary>
		protected int rxBodyLen = -1;

		/// <summary>Beginning of message indicator</summary>
		protected ArraySegment<byte> bomBytes = new ArraySegment<byte>(new byte[] { 1, 2, 1, 255 });

		/// <summary>TCP inactivity before sending keep-alive packet (ms)</summary>
		protected uint KeepAliveInactivity = 500;
		/// <summary>Interval to send keep-alive packet if acknowledgement was not received (ms)</summary>
		protected uint KeepAliveInterval = 100;

		/// <summary>Threaded timer checks if socket is busted</summary>
		protected Timer connectionTimer;
		/// <summary>Interval for socket checks (ms)</summary>
		protected int ConnectionCheckInterval = 1000;
		#endregion

		#region Public Properties
		/// <summary>Current state of the socket</summary>
		public SocketState State { get { return this.state; } }

		/// <summary>Port the socket control is listening on.</summary>
		public int LocalPort
		{
			get
			{
				try
				{
					return ((IPEndPoint)this.socket.LocalEndPoint).Port;
				}
				catch
				{
					return -1;
				}
			}
		}

		/// <summary>IP address enumeration for local computer</summary>
		public static string[] LocalIP
		{
			get
			{
				IPHostEntry h = Dns.GetHostEntry(Dns.GetHostName());
				List<string> s = new List<string>(h.AddressList.Length);
				foreach (IPAddress i in h.AddressList)
					s.Add(i.ToString());
				return s.ToArray();
			}
		}
		#endregion

		#region Events
		/// <summary>Socket is connected</summary>
		public event EventHandler<NetSocketConnectedEventArgs> Connected;
		/// <summary>Socket connection closed</summary>
		public EventHandler<NetSocketDisconnectedEventArgs> Disconnected;
		/// <summary>Socket state has changed</summary>
		/// <remarks>This has the ability to fire very rapidly during connection / disconnection.</remarks>
		public event EventHandler<NetSockStateChangedEventArgs> StateChanged;
		/// <summary>Recived a new object</summary>
		public EventHandler<NetSockDataArrivalEventArgs> DataArrived;
		/// <summary>An error has occurred</summary>
		public EventHandler<NetSockErrorReceivedEventArgs> ErrorReceived;
		#endregion

		#region Constructor
		/// <summary>Base constructor sets up buffer and timer</summary>
		public NetBase()
		{
			this.connectionTimer = new Timer(
				new TimerCallback(this.connectedTimerCallback),
				null, Timeout.Infinite, Timeout.Infinite);
        }

        #endregion

		#region Send
		/// <summary>Send data</summary>
		/// <param name="bytes">Bytes to send</param>
		public void Send(byte[] data)
		{
			try
			{
				if (data == null)
					throw new NullReferenceException("data cannot be null");
				else if (data.Length == 0)
					throw new NullReferenceException("data cannot be empty");
				else
				{
                    SendQueue.Gram gram;
                    lock (this.m_SendQueue)
                    {
                        this.m_SendQueue.Enqueue(data, data.Length);
                        gram = m_SendQueue.CheckFlushReady();

                    }
                    
                    if (gram != null && !isSending)
				    {
                        isSending = true;
                        socket.BeginSend(gram.Buffer, 0, gram.Length, SocketFlags.None, SendCallback, socket);
                    }
				}
			}
			catch (Exception ex)
			{
				this.OnErrorReceived("Send", ex);
			}
		}

		/// <summary>Callback for BeginSend</summary>
		/// <param name="ar"></param>
		private void SendCallback(IAsyncResult ar)
		{
			try
			{
				Socket sock = (Socket)ar.AsyncState;
				int didSend = sock.EndSend(ar);

				if (this.socket != sock)
				{
					this.Close("Async Connect Socket mismatched");
					return;
				}

                SendQueue.Gram gram;
                lock (m_SendQueue)
                {
                    gram = m_SendQueue.Dequeue();

                    if (gram == null && m_SendQueue.IsFlushReady)
                    {
                        gram = m_SendQueue.CheckFlushReady();
                    }
                }

                if (gram != null)
                {
                    sock.BeginSend(gram.Buffer, 0, gram.Length, SocketFlags.None, SendCallback, sock);
                }
                else
                {
                    isSending = false;
                }
			}
			catch (ObjectDisposedException)
			{
				return;
			}
			catch (SocketException ex)
			{
				if (ex.SocketErrorCode == SocketError.ConnectionReset)
					this.Close("Remote Socket Closed");
				else
					throw;
			}
			catch (Exception ex)
			{
				this.Close("Socket Send Exception");
				this.OnErrorReceived("Socket Send", ex);
			}
		}
		#endregion

		#region Close
		/// <summary>Disconnect the socket</summary>
		/// <param name="reason"></param>
		public void Close(string reason)
		{
			try
			{
				if (this.state == SocketState.Closing || this.state == SocketState.Closed)
					return; // already closing/closed

				this.OnChangeState(SocketState.Closing);

				if (this.socket != null)
				{
					this.socket.Close();
					this.socket = null;
				}
			}
			catch (Exception ex)
			{
				this.OnErrorReceived("Close", ex);
			}

			try
			{
				this.OnChangeState(SocketState.Closed);
				if (this.Disconnected != null)
					this.Disconnected(this, new NetSocketDisconnectedEventArgs(reason));
			}
			catch (Exception ex)
			{
				this.OnErrorReceived("Close Cleanup", ex);
			}
		}
		#endregion

		#region Receive
		/// <summary>Receive data asynchronously</summary>
		public void Receive()
		{
			try
			{
				this.socket.BeginReceive(this.byteBuffer, 0, this.byteBuffer.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), this.socket);
			}
			catch (Exception ex)
			{
				this.OnErrorReceived("Receive", ex);
			}
		}

		/// <summary>Callback for BeginReceive</summary>
		/// <param name="ar"></param>
		private void ReceiveCallback(IAsyncResult ar)
		{
			try
			{
				Socket sock = (Socket)ar.AsyncState;
				int size = sock.EndReceive(ar);

				if (this.socket != sock)
				{
					this.Close("Async Receive Socket mismatched");
					return;
				}

				if (size < 1)
				{
					this.Close("No Bytes Received");
					return;
				}

                if (DataArrived != null)
                {
                    var localData = new byte[size];
                    Array.Copy(byteBuffer, localData, size);
                    var net = new NetSockDataArrivalEventArgs(this, localData);

                    DataArrived(this, net);
                }

                this.socket.BeginReceive(this.byteBuffer, 0, this.byteBuffer.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), this.socket);
			}
			catch (ObjectDisposedException)
			{
				return; // socket disposed, let it die quietly
			}
			catch (SocketException ex)
			{
				if (ex.SocketErrorCode == SocketError.ConnectionReset)
					this.Close("Remote Socket Closed");
				else
					throw;
			}
			catch (Exception ex)
			{
				this.Close("Socket Receive Exception");
				this.OnErrorReceived("Socket Receive", ex);
			}
		}

		/// <summary>Find first position the specified byte within the stream, or -1 if not found</summary>
		/// <param name="ms"></param>
		/// <param name="find"></param>
		/// <returns></returns>
		private int IndexOfByteInStream(MemoryStream ms, byte find)
		{
			int b;
			do
			{
				b = ms.ReadByte();
			} while(b > -1 && b != find);

			if (b == -1)
				return -1;
			else
				return (int)ms.Position - 1; // position is +1 byte after the byte we want
		}

		/// <summary>Find first position the specified bytes within the stream, or -1 if not found</summary>
		/// <param name="ms"></param>
		/// <param name="find"></param>
		/// <returns></returns>
		private int IndexOfBytesInStream(MemoryStream ms, byte[] find)
		{
			int index;
			do
			{
				index = this.IndexOfByteInStream(ms, find[0]);

				if (index > -1)
				{
					bool found = true;
					for (int i = 1; i < find.Length; i++)
					{
						if(find[i] != ms.ReadByte())
						{
							found = false;
							ms.Position = index + 1;
							break;
						}
					}
					if (found)
						return index;
				}
			} while(index > -1);
			return -1;
		}
		#endregion

		#region OnEvents
		protected void OnErrorReceived(string function, Exception ex)
		{
			if (this.ErrorReceived != null)
				this.ErrorReceived(this, new NetSockErrorReceivedEventArgs(function, ex));
		}

		protected void OnConnected(Socket sock)
		{
			if (this.Connected != null)
				this.Connected(this, new NetSocketConnectedEventArgs(((IPEndPoint)sock.RemoteEndPoint).Address));
		}

        public void OnChangeState(SocketState newState)
		{
			SocketState prev = this.state;
			this.state = newState;
			if (this.StateChanged != null)
				this.StateChanged(this, new NetSockStateChangedEventArgs(this.state, prev));

			if (this.state == SocketState.Connected)
				this.connectionTimer.Change(0, this.ConnectionCheckInterval);
			else if (this.state == SocketState.Closed)
				this.connectionTimer.Change(Timeout.Infinite, Timeout.Infinite);
		}
		#endregion

		#region Keep-alives
		/*
		 * Note about usage of keep-alives
		 * The TCP protocol does not successfully detect "abnormal" socket disconnects at both
		 * the client and server end. These are disconnects due to a computer crash, cable 
		 * disconnect, or other failure. The keep-alive mechanism built into the TCP socket can
		 * detect these disconnects by essentially sending null data packets (header only) and
		 * waiting for acks.
		 */

		/// <summary>Structure for settings keep-alive bytes</summary>
		[StructLayout(LayoutKind.Sequential)]
		private struct tcp_keepalive
		{
			/// <summary>1 = on, 0 = off</summary>
			public uint onoff;
			/// <summary>TCP inactivity before sending keep-alive packet (ms)</summary>
			public uint keepalivetime;
			/// <summary>Interval to send keep-alive packet if acknowledgement was not received (ms)</summary>
			public uint keepaliveinterval;
		}

		/// <summary>Set up the socket to use TCP keep alive messages</summary>
		public void SetKeepAlive()
		{
			try
			{
				tcp_keepalive sioKeepAliveVals = new tcp_keepalive();
				sioKeepAliveVals.onoff = (uint)1; // 1 to enable 0 to disable
				sioKeepAliveVals.keepalivetime = this.KeepAliveInactivity;
				sioKeepAliveVals.keepaliveinterval = this.KeepAliveInterval;

				IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(sioKeepAliveVals));
				Marshal.StructureToPtr(sioKeepAliveVals, p, true);
				byte[] inBytes = new byte[Marshal.SizeOf(sioKeepAliveVals)];
				Marshal.Copy(p, inBytes, 0, inBytes.Length);
				Marshal.FreeHGlobal(p);

				byte[] outBytes = BitConverter.GetBytes(0);
				this.socket.IOControl(IOControlCode.KeepAliveValues, inBytes, outBytes);
			}
			catch (Exception ex)
			{
				this.OnErrorReceived("Keep Alive", ex);
			}
		}
		#endregion

		#region Connection Sanity Check
		private void connectedTimerCallback(object sender)
		{
			try
			{
				if (this.state == SocketState.Connected &&
					(this.socket == null || !this.socket.Connected))
					this.Close("Connect Timer");
			}
			catch (Exception ex)
			{
				this.OnErrorReceived("ConnectTimer", ex);
				this.Close("Connect Timer Exception");
			}
		}
		#endregion
	}

    #endregion
}
