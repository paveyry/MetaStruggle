using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network.Packet.Packets.DatasTypes;

namespace Network.Packet.Packets
{
    public class GameStart : IPacket
    {
        public byte ID
        {
            get { return 8; }
        }

        public string EventID
        {
            get { return "Network.Game.GameStart"; }
        }

        public void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher)
        {
            string map = packet.Reader.ReadString();
            byte nbplayer = packet.Reader.ReadByte();
            var players = new List<Player>();

            for (int i = 0; i < nbplayer; i++)
            {
                byte id = packet.Reader.ReadByte();
                string name = packet.Reader.ReadString();
                string modelType = packet.Reader.ReadString();

                players.Add(new Player(id, name, modelType));
            }

            eventDispatcher.ThrowNewEvent(EventID, new GameStartDatas {MapName = map, Players = players});
        }

        /// <summary>
        /// 0: map
        /// 1: List players
        /// </summary>
        /// <param name="w"></param>
        /// <param name="datas"></param>
        public void Pack(IO.Writer w, params object[] datas)
        {
            var p = new Packet(new PacketHeader {ID = ID});
            var players = (List<Player>) datas[1];
            p.Writer.Write((string)datas[0]);
            p.Writer.Write((byte)players.Count);

            foreach (var player in players)
            {
                p.Writer.Write(player.ID);
                p.Writer.Write(player.Name);
                p.Writer.Write(player.ModelType);
            }
        }
    }
}
