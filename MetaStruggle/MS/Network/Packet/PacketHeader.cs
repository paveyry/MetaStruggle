using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.IO;

namespace Network.Packet
{
    public class PacketHeader
    {
        public const int HeaderSize = 5;
        public byte ID { get; set; }
        public int DataSize { get; set; }

        public static PacketHeader ReadHeader(Reader r)
        {
            return new PacketHeader {ID = r.ReadByte(), DataSize = r.ReadInt32()};
        }
    }
}
