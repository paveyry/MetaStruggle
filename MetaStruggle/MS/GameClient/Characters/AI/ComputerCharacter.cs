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
                    {DoNothing,-42},
                    {Track,0},
                    {AttackAndTrack,1},
                    {AvoidAttack,2},
                    {ReturnToMap,42}
                };
        }

        #region Update & Cie
        public override void Update(GameTime gameTime)
        {
            UpdateEnemiesLevel();
            if (gameTime.TotalGameTime.TotalMilliseconds - _oldTime > 500)
            {
                foreach (var movement in Movements.Keys.ToList())
                    Movements[movement] = false;

                var ennemiesLevelAlives = EnnemiesLevel.Where(kvp => !kvp.Key.IsDead);
                Character stronger = (!ennemiesLevelAlives.Any())? null : ennemiesLevelAlives.Aggregate((kvp1, kvp2) => (kvp1.Value < kvp2.Value) ? kvp1 : kvp2).Key;

                if (stronger != null)
                {
                    SetAction(AttackAndTrack);
                    if (_oldDamages < Damages)
                        SetAction(AvoidAttack);
                    if (ReturnToMap(stronger, gameTime) != StatusAction.Finished)
                        Action = ReturnToMap;
                }
                else
                    Action = DoNothing;
                ActualStatus = Action(stronger, gameTime);

                _oldDamages = Damages;
                _oldTime = gameTime.TotalGameTime.TotalMilliseconds;
            }


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
        StatusAction AttackAndTrack(Character character, GameTime gameTime)
        {
            if (Track(character,gameTime) == StatusAction.Finished)
                Movements[Movement.Attack] = true;
            return StatusAction.InProgress;
        }

        StatusAction Track(Character character, GameTime gameTime)
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

        StatusAction AvoidAttack(Character character, GameTime gameTime)
        {
            Movements[Movement.Jump] = true;
            return StatusAction.Finished;
        }

        StatusAction ReturnToMap(Character character, GameTime gameTime)
        {
            if (Position.X < -24.5)
                Movements[Movement.Left] = true;
            else if (Position.X > 13)
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
