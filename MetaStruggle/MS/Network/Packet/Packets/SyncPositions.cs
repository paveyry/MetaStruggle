using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network.Packet.Packets.DatasTypes;

namespace Network.Packet.Packets
{
    public class SyncPositions : IPacket
    {
        public byte ID
        {
            get { return 11; }
        }

        public string EventID
        {
            get { return "Network.Game.SyncPositions"; }
        }

        public void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher)
        {
            byte nb = packet.Reader.ReadByte();
            var characterPosition = new List<CharacterPositionDatas>();

            for (int i = 0; i <nb; i++)
            {
                byte id = packet.Reader.ReadByte();
                float x = packet.Reader.ReadSingle(), y = packet.Reader.ReadSingle(), yaw = packet.Reader.ReadSingle();
                characterPosition.Add(new CharacterPositionDatas {ID = id, X = x, Y = y,Yaw = yaw});
            }

            eventDispatcher.ThrowNewEvent(EventID, characterPosition);
        }

        /// <summary>
        /// 0: list CharacterPositionDatas
        /// </summary>
        /// <param name="w"></param>
        /// <param name="datas"></param>
        public void Pack(IO.Writer w, params object[] datas)
        {
            var characterPositions = (List<CharacterPositionDatas>) datas[0];
            var p = new Packet(new PacketHeader {ID = ID});
            p.Writer.Write((byte)characterPositions.Count);
            foreach (var c in characterPositions)
            {
                p.Writer.Write(c.ID);
                p.Writer.Write(c.X);
                p.Writer.Write(c.Y);
                p.Writer.Write(c.Yaw);
            }

            p.Write(w);
        }
    }
}
