using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Packet.Packets
{
    public class MasterServerListRequest : IPacket
    {
        public byte ID
        {
            get { return 3; }
        }

        public string EventID
        {
            get { return "Network.Master.ServerListRequest"; }
        }

        public void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher)
        {
            eventDispatcher.ThrowNewEvent(EventID, client);
        }

        public void Pack(IO.Writer w, params object[] datas)
        {
            new Packet(new PacketHeader {ID = ID}).Write(w);
        }
    }
}
