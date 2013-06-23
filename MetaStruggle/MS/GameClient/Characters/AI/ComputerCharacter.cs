using System.Collections.Generic;
using GameClient.Renderable.Scene;
using GameClient.Renderable._3D;
using Microsoft.Xna.Framework;
using System.Linq;

namespace GameClient.Characters.AI
{
    public class ComputerCharacter : Character
    {
        private readonly SceneManager _sceneManager;
        List<Character> OtherCharacter { get; set; } 
        byte Handicap { get; set; }
        byte Level { get; set; }

        public ComputerCharacter(string playerName, string nameCharacter,string mapName, SceneManager scene, Vector3 position, Vector3 scale
            , float speed,byte handicap, byte level) : base(playerName,nameCharacter,mapName,-1,scene,position,scale,speed)
        {
            _sceneManager = scene;
            OtherCharacter = new List<Character>();
            Handicap = handicap;
            Level = level;
            GetKey = GetMovement;
        }

        public void Initialize()
        {
            foreach (var character in _sceneManager.Items.OfType<Character>().Where(c => !c.Equals(this)))
                OtherCharacter.Add(character);
        }

        public bool GetMovement(Movement movement) //
        {
            return (movement == Movement.Left);
        }

        public void Update(GameTime gameTime) //
        {
            
        }
    }
}
