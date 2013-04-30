using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.IO;
using Network.Packet.Packets.DatasTypes;

namespace Network.Packet.Packets
{
    public class MasterAddServer : IPacket
    {
        public byte ID
        {
            get { return 0; }
        }

        public string EventID
        {
            get { return "Network.Master.AddServer"; }
        }

        public void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher)
        {
            string ip = client.ToString().Split(':')[0];
            short port = packet.Reader.ReadInt16();

            eventDispatcher.ThrowNewEvent(EventID, new MasterServerDatas { IP = ip, Port = port });
        }

        /// <summary>
        /// 0: Port
        /// </summary>
        /// <param name="w"></param>
        /// <param name="datas"></param>
        public void Pack(Writer w, params object[] datas)
        {
            var p = new Packet(new PacketHeader { ID = ID });
            p.Writer.Write((short)datas[0]);
            
            p.Write(w);
        }
    }
}
