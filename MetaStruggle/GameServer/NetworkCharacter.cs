using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Network.Packet.Packets;

namespace GameServer
{
    public class NetworkCharacter
    {
        public byte ID { get; set; }
        public Client Client { get; set; }
        public string Name { get; set; }
        public string ModelType { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float SX { get; set; }
        public float SY { get; set; }
        public float Yaw { get; set; }
        public bool IsJumping { get; set; }
        public float Damages { get; set; }
        public float Gravity { get; set; }
        public bool IsAlive { get; set; }

        public NetworkCharacter(byte id, Client client, string name, string modelType, float spawnX, float spawnY)
        {
            ID = id;
            Client = client;
            Name = name;
            ModelType = modelType;
            SX = spawnX;
            SY = spawnY;
            X = SX;
            Y = SY;
            IsAlive = true;
            IsJumping = false;
            Damages = 0;
            Gravity = 0.005f;
        }

        
    }
}
