using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Packet.Packets.DatasTypes
{
    public class MasterServerDatas
    {
        public string IP { get; set; }
        public ushort Port { get; set; }
        public string Map { get; set; }
        public byte MaxPlayer { get; set; }
        public byte ConnectedPlayer { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}:{1}]", IP, Port);
        }
    }
}
