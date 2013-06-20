using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using Network.IO;
using Network.Packet;

namespace Network
{
    public class Client
    {
        public bool Connected { get; set; }
        public TcpClient BaseClient { get { return _client; } }
        private readonly TcpClient _client;
        private const int BufferSize = 4096;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly string _networkInfos;
        public delegate void ClientConnectedHandler(Client sender);
        private readonly Parser.ParserMethod _parseMethod;

        public Reader Reader
        {
            get { return new Reader(Connected ? _client.GetStream() : Stream.Null); }
        }

        public Writer Writer
        {
            get { return new Writer(Connected ? _client.GetStream() : Stream.Null); }
        }

        public delegate void ClientDisconnectedHandler(Client sender);
        public event ClientDisconnectedHandler OnDisconnect;

        public Client(TcpClient client, IEventDispatcher eventDispatcher, Parser.ParserMethod parseMethod, ClientConnectedHandler onConnect = null)
        {
            _client = client;
            Connected = true;
            _client.NoDelay = true;
            _eventDispatcher = eventDispatcher;
            _parseMethod = parseMethod;
            _networkInfos = _client.Client.RemoteEndPoint.ToString();

            if (onConnect != null)
                onConnect.BeginInvoke(this, null, null);

            new Task(Receive).Start();
        }

        public Client(string ip, int port, IEventDispatcher eventDispatcher, Parser.ParserMethod parseMethod, ClientConnectedHandler onConnect = null)
        {
            try
            {
                _client = new TcpClient(ip, port);

                Connected = true;
                _client.NoDelay = true;
                _eventDispatcher = eventDispatcher;
                _parseMethod = parseMethod;
                _networkInfos = _client.Client.RemoteEndPoint.ToString();

                if (onConnect != null)
                    onConnect.BeginInvoke(this, null, null);

                new Task(Receive).Start();
            }
            catch (SocketException se)
            {
                Connected = false;
            }
        }

        void Receive()
        {
            while (Connected)
            {
                Thread.Sleep(10);

                if (CheckIfDisconnected()) return;
                if (_client.Available < PacketHeader.HeaderSize) continue;

                var header = Reader.ReadPacketHeader();
                int read = 0;
                var packet = new Packet.Packet(header);

                while (read != header.DataSize)
                {
                    if (CheckIfDisconnected()) return;

                    byte[] tempBuffer = Reader.ReadBytes(header.DataSize - read >= BufferSize ? BufferSize : header.DataSize - read);
                    read += tempBuffer.Length;
                    packet.Writer.Write(tempBuffer);
                }

                packet.ResetStream();
                _parseMethod.BeginInvoke(this, packet, _eventDispatcher, null, null);
            }
        }

        bool CheckIfDisconnected()
        {
            try
            {
                if (_client == null || _client.Client == null ||
                    (Connected && _client.Client.Poll(-1, SelectMode.SelectRead) && _client.Available == 0))
                    Disconnect();
            }
            catch
            {
                Disconnect();
            }

            return !Connected;
        }

        public void Disconnect()
        {
            if (!Connected)
                return;

            Connected = false;

            if (_client.Client != null)
                _client.Close();

            if (OnDisconnect != null)
                foreach (var m in OnDisconnect.GetInvocationList())
                    m.DynamicInvoke(this);
        }

        public override string ToString()
        {
            return _networkInfos;
        }
    }
}
