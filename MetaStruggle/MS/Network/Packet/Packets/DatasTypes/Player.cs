namespace Network.Packet.Packets.DatasTypes
{
    public class Player
    {
        public byte ID { get; set; }
        public string Name { get; set; }
        public string ModelType { get; set; }
        public Client Client { get; set; }

        public Player(byte id, string name, string modelType, Client client = null)
        {
            ID = id;
            Name = name;
            ModelType = modelType;
            Client = client;
        }
    }
}
