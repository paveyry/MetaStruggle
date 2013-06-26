using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Network.Packet;

namespace Network.IO
{
    public class Writer : BinaryWriter
    {
        public Writer() : base(new MemoryStream())
        {
        }

        public Writer(Stream output) : base(output)
        {
        }

        public void Write(PacketHeader header)
        {
            try
            {
                Write(header.ID);
                Write(header.DataSize);
            }
            catch{}
        }
    }
}
