using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Packet.Packets
{
    public class ServerMessage : IPacket
    {
        public byte ID
        {
            get { return 7; }
        }

        public string EventID
        {
            get { return "Network.Game.ServerMessage"; }
        }

        public void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher)
        {
            eventDispatcher.ThrowNewEvent(EventID, packet.Reader.ReadString());
        }

        /// <summary>
        /// 0: message
        /// </summary>
        /// <param name="w"></param>
        /// <param name="datas"></param>
        public void Pack(IO.Writer w, params object[] datas)
        {
            var p = new Packet(new PacketHeader {ID = ID});
            p.Writer.Write((string)datas[0]);

            p.Write(w);
        }
    }
}
