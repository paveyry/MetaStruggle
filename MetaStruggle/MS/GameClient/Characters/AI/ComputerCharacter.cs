using System;
using System.Collections.Generic;
using GameClient.Renderable.Scene;
using GameClient.Renderable._3D;
using System.Security.Cryptography;
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

        public override float Damages { get { return base.Damages; } set
        {
            base.Damages = value + Math.Abs((value - base.Damages) * (Handicap / 10f - 0.1f));
        } }
        byte Handicap { get; set; }
        byte Level { get; set; }

        Dictionary<Character, float> EnnemiesLevel { get; set; }
        Dictionary<Movement, bool> Movements { get; set; }

        private delegate StatusAction ActionDelegate(Character character, GameTime gameTime);
        ActionDelegate Action { get; set; }
        Character SelectedCharacter { get; set; }
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
            Level = (byte)(10-level);
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

        Character SelectCharacter()
        {
            var ennemiesLevelAlives = EnnemiesLevel.Where(kvp => !kvp.Key.IsDead).ToList();
            if (!ennemiesLevelAlives.Any())
                return null;

            float min = float.MaxValue, dist = float.MaxValue;
            Character stronger = null, clother = null;
            foreach (var kvp in ennemiesLevelAlives)
            {
                if (min > kvp.Value)
                {
                    stronger = kvp.Key;
                    min = kvp.Value;
                }
                var tempDist = MathHelper.Distance(Position.X, kvp.Key.Position.X);
                if (!(dist > tempDist)) continue;
                clother = kvp.Key;
                dist = tempDist;
            }

            return (MathHelper.Distance(Position.X, stronger.Position.X) < 5) ? stronger : clother;
        }

        int GetRandomNumber(int start, int end)
        {
            var rndNumber = new RNGCryptoServiceProvider();
            var rnd = new byte[1];
            rndNumber.GetBytes(rnd);
            return start + rnd[0] % end;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateEnemiesLevel();

            if (gameTime.TotalGameTime.TotalMilliseconds - _oldTime > 250 * (1 + Level / 3f - (1/3f)))
            {
                SelectedCharacter = SelectCharacter();

                if (SelectedCharacter != null)
                {
                    if (_oldDamages < Damages)
                        SetAction(AvoidAttack);
                    if (ReturnToMap(SelectedCharacter, gameTime) != StatusAction.Finished)
                        Action = ReturnToMap;
                    SetAction(Track);
                    if (Action == Track && Action(SelectedCharacter, gameTime) == StatusAction.Finished)
                        if ((SelectedCharacter.Damages > GetRandomNumber(50,150) && GetRandomNumber(0, 3) != 0) 
                            || (SelectedCharacter.Damages < 100 && GetRandomNumber(0, 3) == 0))
                            Action = SpecialAttack;
                        else
                            Action = Attack;
                }
                else
                    Action = DoNothing;

                ResetMovements();
                ActualStatus = Action(SelectedCharacter, gameTime);

                _oldDamages = Damages;
                _oldTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            else if (Action == Track && Action(SelectedCharacter, gameTime) == StatusAction.Finished)
            {
                ResetMovements();
                Action = DoNothing;
            }

            base.Update(gameTime);
        }
        #endregion

        #region Actions
        StatusAction Attack(Character character, GameTime gameTime)
        {
            Movements[Movement.Attack] = true;
            if (Yaw != BaseYaw)
            {
                if (Position.X < character.Position.X)
                    Movements[Movement.Right] = true;
            }
            else if (Position.X > character.Position.X)
                Movements[Movement.Left] = true;
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
            Movements[(Position.X < 0 || GetRandomNumber(0, 3) == 0) ? Movement.Left : Movement.Right] = true;
            return StatusAction.Finished;
        }

        StatusAction ReturnToMap(Character character, GameTime gameTime)
        {
            if (Position.Y < 0)
                Movements[(Position.X > 0) ? Movement.Right : Movement.Left] = true;
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
