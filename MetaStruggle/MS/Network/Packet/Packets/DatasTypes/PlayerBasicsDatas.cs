using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Packet.Packets.DatasTypes
{
    public class PlayerBasicsDatas
    {
        public string Name { get; set; }
        public string ModelType { get; set; }
        public Client Client { get; set; }

        public PlayerBasicsDatas(string name, string modelType, Client client)
        {
            Name = name;
            ModelType = modelType;
            Client = client;
        }
    }
}
