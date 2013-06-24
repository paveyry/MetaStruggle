using System;
using System.Collections.Generic;
using GameClient.Renderable.Scene;
using GameClient.Renderable._3D;
using Microsoft.Xna.Framework;
using System.Linq;

namespace GameClient.Characters.AI
{
    public class ComputerCharacter : Character
    {
        Dictionary<Character, float> EnnemiesLevel { get; set; }
        Dictionary<Movement, bool> Movements { get; set; }
        byte Handicap { get; set; }
        byte Level { get; set; }

        private delegate int ActionDelegate();
        ActionDelegate Action { get; set; }

        public ComputerCharacter(string playerName, string nameCharacter, string mapName, SceneManager scene, Vector3 position, Vector3 scale
            , float speed, byte handicap, byte level)
            : base(playerName, nameCharacter, mapName, -1, scene, position, scale, speed)
        {
            EnnemiesLevel = new Dictionary<Character, float>();
            Handicap = handicap;
            Level = level;
            GetKey = GetMovement;
            Action = () => -1;

            Movements = new Dictionary<Movement, bool>();
            foreach (Movement move in Enum.GetValues(typeof(Movement)))
                Movements.Add(move, false);
        }

        public void Initialize()
        {
            foreach (var character in Scene.Items.OfType<Character>().Where(c => !c.Equals(this)))
                EnnemiesLevel.Add(character, 0);
        }

        public bool GetMovement(Movement movement)
        {
            return Movements[movement];
        }

        void UpdateEnemiesLevel()
        {
            var listEnnemies = EnnemiesLevel.Keys.ToList();
            for (int index = 0; index < listEnnemies.Count; index++)
                EnnemiesLevel[listEnnemies[index]] = listEnnemies[index].Damages/100f +
                                                     listEnnemies[index].NumberOfDeath*2;
        }

        public override void Update(GameTime gameTime) //
        {
            UpdateEnemiesLevel();
            Track(EnnemiesLevel.Keys.First());

            base.Update(gameTime);
        }

        //int AttackAndTrack(Character character)
        //{
        //    if (Track(character))
        //        Movements[Movement.Attack] = true;
        //}

        bool Track(Character character)
        {

            if (MathHelper.Distance(Position.X, character.Position.X) < 2)
                return true;
            if (Position.X < character.Position.X)
            {
                Movements[Movement.Left] = true;
                Movements[Movement.Right] = false;
            }
            else
            {
                Movements[Movement.Right] = true;
                Movements[Movement.Left] = false;
            }

            return false;
        }
    }
}
