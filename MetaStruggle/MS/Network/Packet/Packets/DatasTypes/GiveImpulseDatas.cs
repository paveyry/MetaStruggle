using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Packet.Packets.DatasTypes
{
    public class GiveImpulseDatas
    {
        public byte ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Damages { get; set; }
    }
}
