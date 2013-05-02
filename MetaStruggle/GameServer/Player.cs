using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;

namespace GameServer
{
    public class Player
    {
        public byte ID { get; set; }
        public string Name { get; set; }
        public string ModelType { get; set; }
        public Client Client { get; set; }

        public Player(byte id, string name, string modelType, Client client)
        {
            ID = id;
            Name = name;
            ModelType = modelType;
            Client = client;
        }
    }
}
