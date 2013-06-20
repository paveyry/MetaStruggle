using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Packet.Packets.DatasTypes
{
    public class CharacterPositionDatas
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Yaw { get; set; }
        public byte ID { get; set; }
        public byte Anim { get; set; }
    }
}
