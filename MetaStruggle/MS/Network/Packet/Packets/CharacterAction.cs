using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network.Packet.Packets.DatasTypes;

namespace Network.Packet.Packets
{
    public class CharacterAction : IPacket
    {
        public byte ID
        {
            get { return 10; }
        }

        public string EventID
        {
            get { return "Network.Game.CharacterAction"; }
        }

        public void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher)
        {
            byte id = packet.Reader.ReadByte();
            byte action = packet.Reader.ReadByte();

            eventDispatcher.ThrowNewEvent(EventID, new CharacterActionDatas {ID = id, Action = action});
        }

        /// <summary>
        /// 0: ID
        /// 1: action (0 = jump, 1 = right, 2 = left, 3 = attack, 4 = die, 5 = nothing)
        /// </summary>
        /// <param name="w"></param>
        /// <param name="datas"></param>
        public void Pack(IO.Writer w, params object[] datas)
        {
            var p = new Packet(new PacketHeader {ID = ID});
            p.Writer.Write((byte)datas[0]);
            p.Writer.Write((byte)datas[1]);

            p.Write(w);
        }
    }
}
