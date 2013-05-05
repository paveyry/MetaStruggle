using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.IO;
using Network.Packet.Packets.DatasTypes;

namespace Network.Packet.Packets
{
    public class MasterServerList : IPacket
    {
        public byte ID
        {
            get { return 2; }
        }

        public string EventID
        {
            get { return "Network.Master.ServerList"; }
        }

        public void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher)
        {
            var list = new List<MasterServerDatas>();
            int count = packet.Reader.ReadInt32();

            for (int i = 0; i < count; i++)
                list.Add(new MasterServerDatas
                    {
                        IP = packet.Reader.ReadString(),
                        Port = packet.Reader.ReadUInt16(),
                        Map = packet.Reader.ReadString(),
                        MaxPlayer = packet.Reader.ReadByte(),
                        ConnectedPlayer = packet.Reader.ReadByte()
                    });

            eventDispatcher.ThrowNewEvent(EventID, list);
        }

        /// <summary>
        /// 0: MasterServerDatas List
        /// </summary>
        /// <param name="w"></param>
        /// <param name="datas"></param>
        public void Pack(Writer w, params object[] datas)
        {
            var list = (List<MasterServerDatas>) datas[0];

            var p = new Packet(new PacketHeader {ID = ID});
            p.Writer.Write(list.Count);

            foreach (var masterServerDatase in list)
            {
                p.Writer.Write(masterServerDatase.IP);
                p.Writer.Write(masterServerDatase.Port);
                p.Writer.Write(masterServerDatase.Map);
                p.Writer.Write(masterServerDatase.MaxPlayer);
                p.Writer.Write(masterServerDatase.ConnectedPlayer);
            }

            p.Write(w);
        }
    }
}
