using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Packet.Packets.DatasTypes
{
    public class GameStartDatas
    {
        public string MapName { get; set; }
        public List<Player> Players { get; set; } 
    }
}
