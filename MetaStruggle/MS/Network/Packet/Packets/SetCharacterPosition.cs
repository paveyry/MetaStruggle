using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network.Packet.Packets.DatasTypes;

namespace Network.Packet.Packets
{
    public class SetCharacterPosition : IPacket
    {
        public byte ID
        {
            get { return 12; }
        }

        public string EventID
        {
            get { return "Network.Game.SetCharacterPosition"; }
        }

        public void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher)
        {
            byte id = packet.Reader.ReadByte(), anim = packet.Reader.ReadByte(), lives = packet.Reader.ReadByte();
            float x = packet.Reader.ReadSingle(), y = packet.Reader.ReadSingle(), yaw = packet.Reader.ReadSingle(), damages = packet.Reader.ReadSingle();

            eventDispatcher.ThrowNewEvent(EventID, new CharacterPositionDatas { ID = id, X = x, Y = y, Yaw = yaw, Anim = anim, Lives = lives, Damages = damages});
        }

        /// <summary>
        /// 0: CharacterPosition
        /// </summary>
        /// <param name="w"></param>
        /// <param name="datas"></param>
        public void Pack(IO.Writer w, params object[] datas)
        {
            var c = (CharacterPositionDatas) datas[0];
            var p = new Packet(new PacketHeader {ID = ID});

            p.Writer.Write(c.ID);
            p.Writer.Write(c.Anim);
            p.Writer.Write(c.Lives);
            p.Writer.Write(c.X);
            p.Writer.Write(c.Y);
            p.Writer.Write(c.Yaw);
            p.Writer.Write(c.Damages);

            p.Write(w);
        }
    }
}
