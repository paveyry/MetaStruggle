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
        readonly Dictionary<byte, IPacket> _packets = new Dictionary<byte, IPacket>();
        public delegate void PacketNotFoundHandler(Packet.Packet packet);
        public delegate void ParserMethod(Client client, Packet.Packet packet, IEventDispatcher eventDispatcher);
        public event PacketNotFoundHandler PacketNotFound;

        public Parser()
        {
            _packets.Add(0, new MasterAddServer());
            _packets.Add(1, new MasterRemoveServer());
            _packets.Add(2, new MasterServerList());
            _packets.Add(3, new MasterServerListRequest());
            _packets.Add(4, new JoinLobby());
            _packets.Add(5, new LeaveLobby());
            _packets.Add(6, new JoinLobbyRefused());
            _packets.Add(7, new ServerMessage());
            _packets.Add(8, new GameStart());
            _packets.Add(9, new RemovePlayer());
            _packets.Add(10, new CharacterAction());
            _packets.Add(11, new SyncPositions());
            _packets.Add(12, new SetCharacterPosition());
        }

        public void Parse(Client client, Packet.Packet packet, IEventDispatcher eventDispatcher)
        {
            if (!_packets.ContainsKey(packet.Header.ID))
            {
                if (PacketNotFound != null)
                    PacketNotFound.BeginInvoke(packet, null, null);
            }
            else
                _packets[packet.Header.ID].UnPack(client, packet, eventDispatcher);
        }
    }
}
