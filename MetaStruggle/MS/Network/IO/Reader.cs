using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Network.Packet;

namespace Network.IO
{
    public class Reader : BinaryReader
    {
        public Reader(Stream input) : base(input)
        {
        }

        public PacketHeader ReadPacketHeader()
        {
            return new PacketHeader {ID = ReadByte(), DataSize = ReadInt32()};
        }
    }
}
