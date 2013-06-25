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

        private delegate StatusAction ActionDelegate(Character character, GameTime gameTime);
        ActionDelegate Action { get; set; }
        StatusAction ActualStatus { get; set; }
        Dictionary<ActionDelegate, int> PriorityAction { get; set; }

        private float _oldDamages;
        private double _oldTime;

        public ComputerCharacter(string playerName, string nameCharacter, string mapName, SceneManager scene, Vector3 position, Vector3 scale
            , float speed, byte handicap, byte level)
            : base(playerName, nameCharacter, mapName, -1, scene, position, scale, speed)
        {
            EnnemiesLevel = new Dictionary<Character, float>();
            Handicap = handicap;
            Level = level;
            GetKey = GetMovement;
            Action = DoNothing;
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
                    {DoNothing,-1},
                    {Attack,0},
                    {SpecialAttack,1},
                    {Track,2},
                    {AvoidAttack,3},
                    {ReturnToMap,4}
                };
        }

        #region Update & Cie
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

        void ResetMovements()
        {
            foreach (var movement in Movements.Keys.ToList())
                Movements[movement] = false;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateEnemiesLevel();

            var ennemiesLevelAlives = EnnemiesLevel.Where(kvp => !kvp.Key.IsDead);
            Character stronger = (!ennemiesLevelAlives.Any()) ? null : ennemiesLevelAlives.Aggregate((kvp1, kvp2) => (kvp1.Value < kvp2.Value) ? kvp1 : kvp2).Key;
            if (gameTime.TotalGameTime.TotalMilliseconds - _oldTime > 500)
            {
                if (stronger != null)
                {
                    if (_oldDamages < Damages)
                        SetAction(AvoidAttack);
                    if (ReturnToMap(stronger, gameTime) != StatusAction.Finished /*rnd*/)
                        Action = ReturnToMap;
                    SetAction(Track);
                    if (Action == Track && Action(stronger, gameTime) == StatusAction.Finished /*rnd*/)
                        if (stronger.Damages > 100 /*rnd*/)
                            Action = SpecialAttack;
                        else 
                            Action = Attack;
                }
                else
                    Action = DoNothing;

                ResetMovements();
                ActualStatus = Action(stronger, gameTime);

                _oldDamages = Damages;
                _oldTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            else if (Action == Track && Action(stronger, gameTime) == StatusAction.Finished)
            {
                Action = DoNothing;
                ResetMovements();
            }

            base.Update(gameTime);
        }
        #endregion

        #region Actions
        StatusAction Attack(Character character, GameTime gameTime)
        {
            Movements[Movement.Attack] = true;
            return StatusAction.InProgress;
        }

        StatusAction SpecialAttack(Character character, GameTime gameTime)
        {
            Movements[Movement.SpecialAttack] = true;
            return StatusAction.InProgress;
        }

        StatusAction Track(Character character, GameTime gameTime)
        {
            var dist = Position.X - character.Position.X;
            if (dist < 1 && dist > 0)
            {
                Movements[Movement.Right] = true;
                return StatusAction.Finished;
            }
            if (dist > -1 && dist < 0)
            {
                Movements[Movement.Left] = true;
                return StatusAction.Finished;
            }

            Movements[(Position.X < character.Position.X) ? Movement.Left : Movement.Right] = true;

            Movements[Movement.Jump] = (Position.Y < character.Position.Y);
            return StatusAction.InProgress;
        }

        StatusAction AvoidAttack(Character character, GameTime gameTime)
        {
            Movements[Movement.Jump] = true;
            Movements[(Position.X < 0) ? Movement.Left : Movement.Right] = true;
            return StatusAction.Finished;
        }

        StatusAction ReturnToMap(Character character, GameTime gameTime)
        {
            if (Position.Y < 0)
                Movements[(Position.X > 0) ? Movement.Right : Movement.Left] = true;
            if (Position.X < -24.7)
                Movements[Movement.Left] = true;
            else if (Position.X > 13.2)
                Movements[Movement.Right] = true;
            else
                return StatusAction.Finished;
            Movements[Movement.Jump] = true;

            return StatusAction.InProgress;
        }

        StatusAction DoNothing(Character character, GameTime gameTime)
        {
            return StatusAction.Finished;
        }
        #endregion
    }
}
