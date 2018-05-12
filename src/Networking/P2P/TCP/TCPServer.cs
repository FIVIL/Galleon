using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
namespace Galleon.Networking.P2P.TCP
{
    public delegate void MessageRecive(object Sender, string s);
    public delegate void MessageReciveByte(object Sender, byte[] b);
    public class TCPServer
    {
        private Socket SocServer;
        private Dictionary<IPEndPoint, Socket> SocClients;
        private IPEndPoint IP;
        public event MessageRecive OnMessageRecive;
        public event MessageReciveByte OnMessageReciveByte;
        public IPEndPoint First
        {
            get => SocClients.Keys.ElementAt(0);
        }
        public int Port { get => IP.Port; }
        public TCPServer(int port, string ip = "127.0.0.1")
        {
            Connect(port, ip);
        }
        private void Connect(int port, string ip)
        {
            if (SocClients != null) SocClients.Clear();
            SocClients = new Dictionary<IPEndPoint, Socket>();
            SocServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IP = new IPEndPoint(IPAddress.Parse(ip), port);
        }
        public void Start()
        {
            Thread TrStart = new Thread(new ThreadStart(start));
            TrStart.Start();
        }
        private void start()
        {
            try
            {
                SocServer.Bind(IP);
                SocServer.Listen(1);
                while (true)
                {
                    Socket ClientSoc = SocServer.Accept();
                    SocClients.Add(ClientSoc.RemoteEndPoint as IPEndPoint, ClientSoc);
                    Task.Run(() => GetMessage(ClientSoc));
                    // Thread tr = new Thread(getMessage);
                    // tr.Start(ClientSoc);
                }
            }
            catch { }
        }
        private void GetMessage(Socket ObjSoc)
        {
            try
            {
                Socket Soc = ObjSoc;
                while (true)
                {
                    byte[] barray = new byte[1024];
                    int RecB = Soc.Receive(barray);
                    if (RecB > 0)
                    {
                        try
                        {
                            string mess = Encoding.Unicode.GetString(barray, 0, RecB);
                            OnMessageRecive?.Invoke(Soc,mess);
                        }
                        catch { }
                        OnMessageReciveByte?.Invoke(Soc,barray);
                    }
                    Thread.Sleep(1);
                }
            }
            catch
            {
            }
        }
        public void SendAll(string s)
        {
            byte[] b=new byte[1024];
            b = Encoding.Unicode.GetBytes(s);
            SendAll(b);
        }
        public void SendAll(byte[] inbyte)
        {
            foreach (var item in SocClients.Values)
            {
                item.Send(inbyte);
            }
        }
        public void Send(IPEndPoint add,byte[] inbyte)
        {
            SocClients[add].Send(inbyte);
        }
        public void Send(IPEndPoint add,string mess)
        {
            byte[] b=new byte[1024];
            b = Encoding.Unicode.GetBytes(mess);
            Send(add, b);
        }
        public void Remove(IPEndPoint name)
        {
            SocClients[name].Shutdown(SocketShutdown.Both);
            SocClients[name].Close();
            SocClients[name].Dispose();
            try
            {
                SocClients.Remove(name);
            }
            catch { }
        }
        public void Stop()
        {
            try
            {
                foreach (var item in SocClients.Values)
                {
                    item.Shutdown(SocketShutdown.Both);
                    item.Close();
                    item.Dispose();
                }
                SocClients.Clear();
                if (SocServer != null)
                {
                    SocServer.Shutdown(SocketShutdown.Both);
                    SocServer.Close();
                    SocServer.Dispose();
                    SocServer = null;
                }
            }
            catch { }
        }
        public void Restart(int port,string ip = "127.0.0.1")
        {
            Stop();
            Connect(port, ip);
            Start();
        }
    }
}
