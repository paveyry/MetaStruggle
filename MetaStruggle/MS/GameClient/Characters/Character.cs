using System;
using System.Collections.Generic;
using GameClient.CollisionEngine;
using GameClient.Global;
using GameClient.Global.InputManager;
using GameClient.Renderable.Scene;
using GameClient.Renderable._3D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Network;
using Network.Packet.Packets;
using Network.Packet.Packets.DatasTypes;

namespace GameClient.Characters
{

    enum Movement
    {
        Right,
        Left,
        Jump,
        Attack,
        SpecialAttack
    }
    public class Character : AnimatedModel3D
    {
        #region Fields
        public byte ID { get; set; }
        public byte PlayerNb { get; set; }
        private readonly float _baseYaw;
        public bool _jumping;
        public double _jumppos;
        private readonly Vector3 _spawnPosition;
        public Client Client { get; set; }

        public bool IsDead;
        public DateTime DeathDate;
        public float Damages = 0;
        public string PlayerName;
        public Texture2D Face;
        public bool CollisionEnabled { get; set; }
        public bool Playing { get; set; }

        public BoundingObjectModel BoundingObject { get; set; }
        public float Length, Width;
        
        public bool CollideWithMap
        {
            get { return Position.Y < 0.1 && Position.Y > -0.1 && Position.X < 12.58 && Position.X > -23.91; }
        }
        #endregion

        public Character(string playerName, string nameCharacter,byte playerNb,SceneManager scene, Vector3 position, Vector3 scale
            ,float speed = 1f, float length = 1.8f, float weidth = 1.2f )
            : base(nameCharacter, scene, position, scale, speed)
        {
            Playing = true;
            ID = 0;
            PlayerNb = playerNb;
            PlayerName = playerName;
            Face = RessourceProvider.CharacterFaces[nameCharacter];
            Pitch = -MathHelper.PiOver2;
            Yaw = MathHelper.PiOver2;
            _baseYaw = Yaw;
            Gravity = 0.005f;
            _spawnPosition = position;
            CollisionEnabled = true;

            Length = length;
            Width = weidth;
            BoundingObject = new BoundingObjectModel(this);
        }

        public override void Update(GameTime gameTime)
        {
            var pendingAnim = new List<Animation>();

            if (Playing)
            {
                #region ManageKeyboard
                if (CurrentAnimation != Animation.Jump)
                    pendingAnim.Add(Animation.Default);

                if (GetKey(Movement.Attack).IsPressed())
                {
                    Attack(gameTime);
                    pendingAnim.Add(Animation.Attack);
                }
                if (GetKey(Movement.Jump).IsPressed() && !_jumping && CollideWithMap)
                {
                    Jump(gameTime);
                    pendingAnim.Add(Animation.Jump);
                    GameEngine.EventManager.ThrowNewEvent("Character.Jump", this);
                }
                if (GetKey(Movement.Right).IsPressed())
                {
                    MoveRight(gameTime);
                    pendingAnim.Add(Animation.Run);
                }
                if (GetKey(Movement.Left).IsPressed())
                {
                    MoveLeft(gameTime);
                    pendingAnim.Add(Animation.Run);
                }

                #endregion
            }
            
            #region tests

            //if (Client == null && ModelName == "Alex")
            //{
            //    if (ks.IsKeyDown(Keys.NumPad7))
            //    {
            //        Attack(gameTime);
            //        pendingAnim.Add(Animation.Attack);
            //    }
            //    if (ks.IsKeyDown(Keys.NumPad0) && !_jumping && CollideWithMap)
            //    {
            //        Jump(gameTime);
            //        pendingAnim.Add(Animation.Jump);
            //        GameEngine.EventManager.ThrowNewEvent("Character.Jump", this);
            //    }
            //    if (ks.IsKeyDown(Keys.NumPad6))
            //    {
            //        MoveRight(gameTime);
            //        pendingAnim.Add(Animation.Run);
            //    }
            //    if (ks.IsKeyDown(Keys.NumPad4))
            //    {
            //        MoveLeft(gameTime);
            //        pendingAnim.Add(Animation.Run);
            //    }
            //}

            #endregion
            
            #region Death
            if (!IsDead && Position.Y < -10)
            {
                IsDead = true;
                DeathDate = DateTime.Now;
                GameEngine.EventManager.ThrowNewEvent("Character.Die", this);
                //jumping = true;
            }

            if (IsDead && (DateTime.Now - DeathDate).TotalMilliseconds > 5000)
            {
                SetAnimation(Animation.Default);
                IsDead = false;
                Position = _spawnPosition;
                Damages = 0;
            }
            #endregion

            #region Jump
            if (_jumping)
            {
                UpdateJump(gameTime, pendingAnim);
            }

            if (!CollideWithMap && !_jumping)
            {
                Position -= new Vector3(0, (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Gravity * 2), 0);
                pendingAnim.Add(Animation.Jump);
            }
            #endregion

            ApplyGravity();

            if (CollideWithMap && CurrentAnimation != Animation.Default)
                pendingAnim.Add(Animation.Default);

            if (!CollideWithMap && CurrentAnimation != Animation.Attack)
            {
                //CollisionEnabled = false;
            }

            CollisionEnabled = false;

            SetPriorityAnimation(pendingAnim);

            if (CollideWithSomeone() && !_jumping && CollisionEnabled)
            {
                var c = GetPlayerColliding();
                int mul = 1;

                if (c != null && c.Yaw == Yaw)
                    mul = -mul;

                //CollisionEnabled = false;

                Position += new Vector3(mul * (Yaw == _baseYaw ? new Random().Next(0, 1000) / 1000f : -new Random().Next(0, 1000) / 1000f), 0, 0);
            }

            if(Playing && Client != null)
                new SetCharacterPosition().Pack(Client.Writer, new CharacterPositionDatas(){ID = ID, X = Position.X, Y = Position.Y, Yaw = Yaw});

            base.Update(gameTime);
        }

        void SetPriorityAnimation(ICollection<Animation> pendingAnim)
        {
            if (pendingAnim.Contains(Animation.Attack))
            {
                SetAnimation(Animation.Attack);

                if (Client != null)
                    new CharacterAction().Pack(Client.Writer, ID, 3);
            }
            else if (pendingAnim.Contains(Animation.Jump))
            {
                SetAnimation(Animation.Jump);

                if (Client != null)
                    new CharacterAction().Pack(Client.Writer, ID, 0);
            }
            else if (pendingAnim.Contains(Animation.Run) && !_jumping)
            {
                SetAnimation(Animation.Run);

                if (Client != null)
                    new CharacterAction().Pack(Client.Writer, ID, Yaw == _baseYaw ? 1 : 2);
            }
            else if (pendingAnim.Count != 0 && !_jumping)
            {
                SetAnimation(Animation.Default);

                if (Client != null)
                    new CharacterAction().Pack(Client.Writer, ID, 0);
            }
        }

        UniversalKeys GetKey(Movement movement)
        {
            return RessourceProvider.InputKeys[movement.ToString() + "." + PlayerNb];
        }

        #region Movements
        void MoveRight(GameTime gameTime)
        {
            Yaw = _baseYaw + MathHelper.Pi;
            Position -= new Vector3((float)(gameTime.ElapsedGameTime.TotalMilliseconds * Gravity), 0, 0);
            
            if (CollisionEnabled && CollideWithSomeone())
                Position += new Vector3((float)(gameTime.ElapsedGameTime.TotalMilliseconds * Gravity), 0, 0);
        }

        void MoveLeft(GameTime gameTime)
        {
            Yaw = _baseYaw;
            Position += new Vector3((float)(gameTime.ElapsedGameTime.TotalMilliseconds * Gravity), 0, 0);
            
            if (CollisionEnabled && CollideWithSomeone())
                Position -= new Vector3((float)(gameTime.ElapsedGameTime.TotalMilliseconds * Gravity), 0, 0);
        }

        bool CollideWithSomeone()
        {
            return CollisionEnabled && !Scene.Items.GetRange(0, Scene.Items.Count).FindAll(e => e is Character && !e.Equals(this)).TrueForAll(e => !new BoundingObjectModel(this).Intersects(new BoundingObjectModel(e as Character)));
        }

        void Jump(GameTime gameTime)
        {
            _jumping = true;
            _jumppos = 0;
        }

        private void UpdateJump(GameTime gameTime, ICollection<Animation> pendingAnim)
        {
            _jumppos += gameTime.ElapsedGameTime.TotalMilliseconds * Gravity;
            var pos = Position;

            if (Position.X < 12.58 && Position.X > -23.91 || Position.Y > 0.1)
            {
                Position = new Vector3(Position.X, (float)(3 * Math.Sin(_jumppos)), Position.Z);
            }
            else
            {
                _jumping = false;
                Position -= new Vector3(0, (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Gravity * 2), 0);
                _jumppos = 0;
                pendingAnim.Add(Animation.Jump);
            }

            if (CollideWithSomeone())
            {
                Position = pos + new Vector3(Yaw == _baseYaw ? 0.15f : -0.15f, 0, 0);
                _jumppos -= gameTime.ElapsedGameTime.TotalMilliseconds * Gravity;
            }

            if (!CollideWithMap) return;

            _jumping = false;
            Position = new Vector3(Position.X, 0, Position.Z);
            _jumppos = 0;
            pendingAnim.Add(Animation.Default);
        }

        void Attack(GameTime gameTime)
        {
            if (CollideWithMap)
                CollisionEnabled = true;

            var pos = Position;
            Position += new Vector3(Yaw == _baseYaw ? Width/2 : -Width/2, 0, 0);
            BoundingObject.UpdateBox(this);

            if (CollideWithSomeone())
            {
                var c = GetPlayerColliding();
                if (c != null)
                {
                    //c.CollisionEnabled = false;
                    c.Damages += (1 + c.Damages/50) * 0.5f;
                    c.Position += new Vector3((Yaw == _baseYaw ? 1 : -1) * (c.Damages / 5f) , c.Damages/500f, 0);
                    c.SetAnimation(Animation.Jump);
                    c._jumping = true;
                    c._jumppos = 0;
                }
            }

            Position = pos;
            BoundingObject.UpdateBox(this);
        }
        #endregion

        void ApplyGravity()
        {
            if(!CollideWithMap && !CollideWithSomeone())
                Position -= new Vector3(0, Gravity * 1.5f, 0);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            BoundingObject.UpdateBox(this);
            base.Draw(gameTime, spriteBatch);
        }

        Character GetPlayerColliding()
        {
            return
                Scene.Items.GetRange(0, Scene.Items.Count)
                     .FindAll(e => e is Character && !e.Equals(this))
                     .Find(e => BoundingObject.Intersects(new BoundingObjectModel(e as Character))) as Character;
        }
    }
}
