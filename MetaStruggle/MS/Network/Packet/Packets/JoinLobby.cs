using System;
using Network.Packet.Packets.DatasTypes;

namespace Network.Packet.Packets
{
    public class JoinLobby : IPacket
    {
        public byte ID
        {
            get { return 4; }
        }

        public string EventID
        {
            get { return "Network.Game.JoinLobby"; }
        }

        public void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher)
        {
            string name = packet.Reader.ReadString();
            string modeltype = packet.Reader.ReadString();

            eventDispatcher.ThrowNewEvent(EventID, new PlayerBasicsDatas(name, modeltype, client));
        }

        /// <summary>
        /// 0: name
        /// 1: model type
        /// </summary>
        /// <param name="w"></param>
        /// <param name="datas"></param>
        public void Pack(IO.Writer w, params object[] datas)
        {
            var p = new Packet(new PacketHeader {ID = ID});
            p.Writer.Write((string)datas[0]);
            p.Writer.Write((string)datas[1]);

            p.Write(w);
        }
    }
}
