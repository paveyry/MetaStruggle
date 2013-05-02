using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Packet.Packets
{
    public class RemovePlayer : IPacket
    {
        public byte ID
        {
            get { return 9; }
        }

        public string EventID
        {
            get { return "Network.Game.RemovePlayer"; }
        }

        public void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher)
        {
            eventDispatcher.ThrowNewEvent(EventID, packet.Reader.ReadByte());
        }

        /// <summary>
        /// 0: ID
        /// </summary>
        /// <param name="w"></param>
        /// <param name="datas"></param>
        public void Pack(IO.Writer w, params object[] datas)
        {
            var p = new Packet(new PacketHeader {ID = ID});
            p.Writer.Write((byte)datas[0]);
        }
    }
}
