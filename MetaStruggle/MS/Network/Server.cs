using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Network
{
    public class Server
    {
        private TcpListener _listener;
        public bool IsRunning { get; set; }
        public List<Client> ConnectedClients { get; set; }
        public ushort Port { get; set; }

        public delegate void ClientConnectedHandler(Client sender);
        public delegate void ClientDisconnectedHandler(Client sender);

        public event ClientConnectedHandler ClientConnected;
        public event ClientDisconnectedHandler ClientDisconnected;

        private readonly IEventDispatcher _eventDispatcher;
        private readonly Parser.ParserMethod _parserMethod;

        public Server(IEventDispatcher eventDispatcher, Parser.ParserMethod parseMethod)
        {
            _eventDispatcher = eventDispatcher;
            _parserMethod = parseMethod;
        }

        public void Start(ushort port)
        {
            if (IsRunning)
                return;

            Port = port;
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            ConnectedClients = new List<Client>();
            IsRunning = true;
            new Task(ListeningThread).Start();
        }

        void ListeningThread()
        {
            while (IsRunning)
            {
                try
                {
                    var c = new Client(_listener.AcceptTcpClient(), _eventDispatcher, _parserMethod,
                                       cc => ClientConnected.BeginInvoke(cc, null, null));
                    
                    c.OnDisconnect += OnClientDisconnected;
                    ConnectedClients.Add(c);
                }
                catch {}
            }
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
            _listener.Stop();
            _listener = null;
            ConnectedClients.Clear();
        }

        void OnClientDisconnected(Client c)
        {
            ConnectedClients.Remove(c);

            if (ClientDisconnected != null)
                ClientDisconnected.BeginInvoke(c, null, null);
        }
    }
}
