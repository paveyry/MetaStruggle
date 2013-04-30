using Network.IO;

namespace Network.Packet.Packets
{
    public interface IPacket
    {
        byte ID { get; }
        string EventID { get; }

        void UnPack(Client client, Packet packet, IEventDispatcher eventDispatcher);

        void Pack(Writer w, params object[] datas);
    }
}
