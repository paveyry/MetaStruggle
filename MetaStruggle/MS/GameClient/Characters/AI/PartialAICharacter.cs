using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClient.Characters.AI
{
    class PartialAICharacter
    {

        public byte PlayerNb { get; set; }
        public byte Handicap { get; set; }
        public byte Level { get; set; }
        public string PlayerName { get; set; }
        public string ModelName { get; set; }
        public bool IsAI { get; set; }


        public PartialAICharacter(string playerName, string modelName, byte playerNb)
        {
            PlayerName = playerName;
            ModelName = modelName;
            PlayerNb = playerNb;
        }

        public PartialAICharacter(string playerName, string modelName, byte handicap, byte level)
            : this(playerName, modelName,0)
        {
            IsAI = true;
            Handicap = handicap;
            Level = level;
        }
    }
}
