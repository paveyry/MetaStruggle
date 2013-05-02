using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Packet.Packets
{
    public class JoinLobbyRefused : IPacket
    {
        public byte ID
        {
            get { return 6; }
        }

        public string EventID
        {
            get { return "Network.Game.JoinLobbyRefused"; }
        }

        public void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher)
        {
            eventDispatcher.ThrowNewEvent("Network.Game.JoinLobbyRefused", packet.Reader.ReadString());
        }

        /// <summary>
        /// 0: Raison
        /// </summary>
        /// <param name="w"></param>
        /// <param name="datas"></param>
        public void Pack(IO.Writer w, params object[] datas)
        {
            var p = new Packet(new PacketHeader {ID = ID});
            p.Writer.Write((string)datas[0]);
        }
    }
}
