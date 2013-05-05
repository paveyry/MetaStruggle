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
            eventDispatcher.ThrowNewEvent(EventID,
                                          new MasterServerDatas
                                              {
                                                  IP = client.ToString().Split(':')[0],
                                                  Port = packet.Reader.ReadUInt16(),
                                                  Map = packet.Reader.ReadString(),
                                                  MaxPlayer = packet.Reader.ReadByte(),
                                                  ConnectedPlayer = packet.Reader.ReadByte()
                                              });
        }

        /// <summary>
        /// 0: Port
        /// 1: map
        /// 2: maxplayer
        /// 3: connected
        /// </summary>
        /// <param name="w"></param>
        /// <param name="datas"></param>
        public void Pack(Writer w, params object[] datas)
        {
            var p = new Packet(new PacketHeader { ID = ID });
            p.Writer.Write((ushort)datas[0]);
            p.Writer.Write((string)datas[1]);
            p.Writer.Write((byte)datas[2]);
            p.Writer.Write((byte)datas[3]);

            p.Write(w);
        }
    }
}
