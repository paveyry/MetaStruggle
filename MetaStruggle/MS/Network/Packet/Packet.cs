using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Network.IO;


namespace Network.Packet
{
    public class Packet
    {
        public PacketHeader Header { get; set; }
        private readonly MemoryStream _stream;

        public Packet(PacketHeader header) : this(header, new MemoryStream())
        {}

        public Packet(PacketHeader header, MemoryStream stream)
        {
            _stream = stream;
            Header = header;
        }

        public Reader Reader
        {
            get { return new Reader(_stream); }
        }

        public Writer Writer
        {
            get { return new Writer(_stream); }
        }

        public void ResetStream()
        {
            _stream.Seek(0, SeekOrigin.Begin);
        }

        public void Write(Writer w)
        {
            UpdateHeader();

            w.Write(Header);
            w.Write(_stream.ToArray());
        }

        void UpdateHeader()
        {
            Header.DataSize = (int)_stream.Length;
        }
    }
}
