using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.Packet;
using Network.Packet.Packets;

namespace Network
{
    public class Parser
    {
        static readonly Dictionary<byte, IPacket> Packets = new Dictionary<byte, IPacket>();
        public delegate void PacketNotFoundHandler(Packet.Packet packet);
        public delegate void ParserMethod(Client client, Packet.Packet packet, IEventDispatcher eventDispatcher);
        public event PacketNotFoundHandler PacketNotFound;

        public Parser()
        {
            Packets.Add(0, new MasterAddServer());
            Packets.Add(1, new MasterRemoveServer());
            Packets.Add(2, new MasterServerList());
            Packets.Add(3, new MasterServerListRequest());
        }

        public void Parse(Client client, Packet.Packet packet, IEventDispatcher eventDispatcher)
        {
            if (!Packets.ContainsKey(packet.Header.ID))
            {
                if (PacketNotFound != null)
                    PacketNotFound.BeginInvoke(packet, null, null);
            }
            else
                Packets[packet.Header.ID].UnPack(client, packet, eventDispatcher);
        }
    }
}
