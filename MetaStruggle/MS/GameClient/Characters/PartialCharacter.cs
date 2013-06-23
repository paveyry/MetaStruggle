using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Characters.AI;
using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework;

namespace GameClient.Characters
{
    public class PartialCharacter
    {
        string NameModel { get; set; }
        float Speed { get; set; }
        Vector3 Spawn { get; set; }
        Vector3 Scale { get; set; }

        public PartialCharacter(string nameModel, float speed)
        {
            NameModel = nameModel;
            Speed = speed;
            Spawn = new Vector3(0, 0, -17);
            Scale = new Vector3(1);
        }

        public Character ConvertToCharacter(string playerName,string mapName, int playerNb, SceneManager scene)
        {
            return new Character(playerName, NameModel,mapName, playerNb, scene, Spawn, Scale, Speed);
        }

        public ComputerCharacter ConvertToComputerCharacter(string playerName, string mapName, SceneManager scene, byte handicap, byte level)
        {
            return new ComputerCharacter(playerName, NameModel,mapName, scene, Spawn, Scale, Speed, handicap, level);
        }
    }
}
