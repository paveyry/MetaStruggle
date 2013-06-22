using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameClient.Characters
{
    public class PartialCharacter
    {
        string NameModel { get; set; }
        float Speed { get; set; }
        public PartialCharacter(string nameModel,float speed)
        {
            NameModel = nameModel;
            Speed = speed;
        }

        public Character ConvertToCharacter()
        {
            return new Character(null, NameModel, 0, null, new Vector3(0, 0, -17), new Vector3(1), Speed);
        }
    }
}
