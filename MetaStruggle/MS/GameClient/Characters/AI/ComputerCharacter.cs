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
        enum StatusAction
        {
            InProgress,
            Aborted,
            Finished
        }


        byte Handicap { get; set; }
        byte Level { get; set; }

        Dictionary<Character, float> EnnemiesLevel { get; set; }
        Dictionary<Movement, bool> Movements { get; set; }

        private delegate StatusAction ActionDelegate(Character character);
        ActionDelegate Action { get; set; }
        StatusAction ActualStatus { get; set; }
        Dictionary<ActionDelegate, int> PriorityAction { get; set; }

        private float _oldDamages;


        public ComputerCharacter(string playerName, string nameCharacter, string mapName, SceneManager scene, Vector3 position, Vector3 scale
            , float speed, byte handicap, byte level)
            : base(playerName, nameCharacter, mapName, -1, scene, position, scale, speed)
        {
            EnnemiesLevel = new Dictionary<Character, float>();
            Handicap = handicap;
            Level = level;
            GetKey = GetMovement;
            Action = ((c) => StatusAction.Aborted);
            _oldDamages = Damages;
            FillPriorityAction();

            Movements = new Dictionary<Movement, bool>();
            foreach (Movement move in Enum.GetValues(typeof(Movement)))
                Movements.Add(move, false);
        }

        public void Initialize()
        {
            foreach (var character in Scene.Items.OfType<Character>().Where(c => !c.Equals(this)))
                EnnemiesLevel.Add(character, 0);
        }

        private void FillPriorityAction()
        {
            PriorityAction = new Dictionary<ActionDelegate, int>()
                {
                    {AvoidAttack,2},
                    {AttackAndTrack,1},
                    {Track,0},
                };
        }

        #region Update & Cie
        public override void Update(GameTime gameTime)
        {
            foreach (var movement in Movements.Keys.ToList())
                Movements[movement] = false;
            UpdateEnemiesLevel();
            Character stronger = EnnemiesLevel.Aggregate((kvp1, kvp2) => (kvp1.Value < kvp2.Value) ? kvp1 : kvp2).Key;

            SetAction(AttackAndTrack);
            if (_oldDamages < Damages)
                SetAction(AvoidAttack);

            ActualStatus = Action(stronger);

            _oldDamages = Damages;

            base.Update(gameTime);
        }

        void SetAction(ActionDelegate action)
        {
            if (PriorityAction[action] > PriorityAction[Action] || ActualStatus != StatusAction.InProgress)
                Action = action;
        }

        public bool GetMovement(Movement movement)
        {
            return Movements[movement];
        }

        void UpdateEnemiesLevel()
        {
            foreach (var e in EnnemiesLevel.Keys.ToList())
                EnnemiesLevel[e] = (e.IsDead) ? float.MaxValue : e.Damages / 100f + e.NumberOfDeath * 2;
        }
        #endregion

        #region Actions
        StatusAction AttackAndTrack(Character character)
        {
            if (Track(character) == StatusAction.Finished)
                Movements[Movement.Attack] = true;
            return StatusAction.InProgress;
        }

        StatusAction Track(Character character)
        {

            if (MathHelper.Distance(Position.X, character.Position.X) < 1)
            {
                if (Yaw != character.Yaw)
                    Movements[(BaseYaw == Yaw) ? Movement.Right : Movement.Left] = true;
                return StatusAction.Finished;
            }
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
            Movements[Movement.Jump] = (Position.Y < character.Position.Y);
            return StatusAction.InProgress;
        }

        StatusAction AvoidAttack(Character character)
        {
            Movements[Movement.Jump] = true;
            return StatusAction.Finished;
        }
        #endregion
    }
}
