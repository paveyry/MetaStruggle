using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Packet.Packets
{
    public class LeaveLobby : IPacket
    {

        public byte ID
        {
            get { return 5; }
        }

        public string EventID
        {
            get { return "Network.Game.LeaveLobby"; }
        }

        public void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher)
        {
            eventDispatcher.ThrowNewEvent(EventID, client);
            client.Disconnect();
        }

        /// <summary>
        /// 0: Name
        /// </summary>
        /// <param name="w"></param>
        /// <param name="datas"></param>
        public void Pack(IO.Writer w, params object[] datas)
        {
            new Packet(new PacketHeader {ID = ID}).Write(w);
        }
    }
}
