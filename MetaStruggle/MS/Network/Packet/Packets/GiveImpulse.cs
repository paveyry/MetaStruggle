using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network.Packet.Packets.DatasTypes;

namespace Network.Packet.Packets
{
    public class GiveImpulse : IPacket
    {
        public byte ID
        {
            get { return 10; }
        }

        public string EventID
        {
            get { return "Network.Game.GiveImpulse"; }
        }

        public void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher)
        {
            byte id = packet.Reader.ReadByte();
            float x = packet.Reader.ReadSingle(), y = packet.Reader.ReadSingle(), d = packet.Reader.ReadSingle();

            eventDispatcher.ThrowNewEvent(EventID, new GiveImpulseDatas {ID = id, X = x, Y = y, Damages = d});
        }

        /// <summary>
        /// 0:GiveImpulseDatas
        /// </summary>
        /// <param name="w"></param>
        /// <param name="datas"></param>
        public void Pack(IO.Writer w, params object[] datas)
        {
            var gid = (GiveImpulseDatas) datas[0];
            var p = new Packet(new PacketHeader {ID = ID});

            p.Writer.Write(gid.ID);
            p.Writer.Write(gid.X);
            p.Writer.Write(gid.Y);
            p.Writer.Write(gid.Damages);

            p.Write(w);
        }
    }
}
