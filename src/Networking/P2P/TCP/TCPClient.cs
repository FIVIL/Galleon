using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Galleon.Networking.P2P.TCP
{
    public delegate void MessageReciveClient(string s);
    public delegate void MessageReciveByteClient(byte[] b);
    public class TCPClient
    {
        private Socket SClient;
        private IPEndPoint IP;
        public event MessageReciveClient OnMessageReciveClient;
        public event MessageReciveByteClient OnMessageReciveByteClient;
        public int Port { get => IP.Port; }
        public TCPClient(int port, string ip = "127.0.0.1")
        {
            Connect(port, ip);
        }
        public void Connect()
        {
            try
            {
                SClient.Connect(IP);
                Task.Factory.StartNew(getMessage);
            }
            catch
            {
            }
        }
        private void Connect(int port, string ip)
        {
            SClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IP = new IPEndPoint(IPAddress.Parse(ip), port);
        }
        private void getMessage()
        {
            try
            {
                while (true)
                {
                    byte[] barray = new byte[1024];
                    int RecB = SClient.Receive(barray);
                    if (RecB > 0)
                    {
                        try
                        {
                            string mess = Encoding.Unicode.GetString(barray, 0, RecB);
                            OnMessageReciveClient?.Invoke(mess);
                        }
                        catch { }
                        OnMessageReciveByteClient?.Invoke(barray);
                    }
                    Thread.Sleep(1);
                }
            }
            catch
            {
            }
        }
        public void Send(string s)
        {
            byte[] b=new byte[1024];
            b = Encoding.Unicode.GetBytes(s);
            Send(b);
        }
        public void Send(byte[] inbyte)
        {
            try
            {
                SClient.Send(inbyte);
            }
            catch { }
        }
        public void Disconnect()
        {
            try
            {
                if (SClient != null)
                {
                    SClient.Shutdown(SocketShutdown.Both);
                    SClient.Close();
                    SClient.Dispose();
                    SClient = null;
                }
            }
            catch { }
        }
        public void Reconnect(int port, string ip = "127.0.0.1")
        {
            Disconnect();
            Connect(port, ip);
            Connect();
        }
    }
}
