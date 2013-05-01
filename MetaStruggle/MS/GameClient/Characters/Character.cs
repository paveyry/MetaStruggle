using System;
using System.Collections.Generic;
using GameClient.CollisionEngine;
using GameClient.Global;
using GameClient.Renderable.Scene;
using GameClient.Renderable._3D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Characters
{
    public class Character : AnimatedModel3D
    {
        private readonly float _baseYaw;
        private bool _jumping;
        private double _jumppos;
        private readonly Vector3 _spawnPosition;

        public bool IsDead;
        public DateTime DeathDate;
        public float Damages = 0;
        public string PlayerName;
        public Texture2D Face;
        public bool CollisionEnabled { get; set; }
        
        public BoundingObjectModel BoundingObject { get; set; }
        public float Length, Width;

        public bool CollideWithMap
        {
            get { return Position.Y < 0.1 && Position.Y > -0.1 && Position.X < 12.58 && Position.X > -23.91; }
        }

        public Character(string playerName, string nameCharacter, SceneManager scene, Vector3 position, Vector3 scale 
            ,float speed = 1f, float length = 1.8f, float weidth = 1.2f )
            : base(nameCharacter, scene, position, scale, speed)
        {
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
            #region ManageKeyboard
            KeyboardState ks = GameEngine.KeyboardState;

            var pendingAnim = new List<Animation>();

            if (CurrentAnimation != Animation.Jump)
                pendingAnim.Add(Animation.Default);

            if (ks.IsKeyDown(Keys.Z))
            {
                Attack(gameTime);
                pendingAnim.Add(Animation.Attack);
            }
            if (ks.IsKeyDown(Keys.Space) && !_jumping && CollideWithMap)
            {
                Jump(gameTime);
                pendingAnim.Add(Animation.Jump);
                GameEngine.EventManager.ThrowNewEvent("Character.Jump", this);
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                MoveRight(gameTime);
                pendingAnim.Add(Animation.Run);
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                MoveLeft(gameTime);
                pendingAnim.Add(Animation.Run);
            }
            #endregion

            #region tests

            if (ModelName == "Spiderman")
            {
                if (ks.IsKeyDown(Keys.NumPad7))
                {
                    Attack(gameTime);
                    pendingAnim.Add(Animation.Attack);
                }
                if (ks.IsKeyDown(Keys.NumPad0) && !_jumping && CollideWithMap)
                {
                    Jump(gameTime);
                    pendingAnim.Add(Animation.Jump);
                    GameEngine.EventManager.ThrowNewEvent("Character.Jump", this);
                }
                if (ks.IsKeyDown(Keys.NumPad6))
                {
                    MoveRight(gameTime);
                    pendingAnim.Add(Animation.Run);
                }
                if (ks.IsKeyDown(Keys.NumPad4))
                {
                    MoveLeft(gameTime);
                    pendingAnim.Add(Animation.Run);
                }
            }

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

            if (CollideWithMap && CurrentAnimation == Animation.Jump)
                pendingAnim.Add(Animation.Default);

            SetPriorityAnimation(pendingAnim);

            if (CollideWithSomeone() && !_jumping && CollisionEnabled)
            {
                var c = GetPlayerColliding();
                int mul = 1;

                if (c != null && c.Yaw == Yaw)
                    mul = -mul;

                CollisionEnabled = false;

                Position += new Vector3(mul * (Yaw == _baseYaw ? new Random().Next(0, 1000) / 1000f : -new Random().Next(0, 1000) / 1000f), 0, 0);
            }
            else
                CollisionEnabled = true;

            base.Update(gameTime);
        }

        void SetPriorityAnimation(ICollection<Animation> pendingAnim)
        {
            if (pendingAnim.Contains(Animation.Attack))
                SetAnimation(Animation.Attack);
            else if (pendingAnim.Contains(Animation.Jump))
                SetAnimation(Animation.Jump);
            else if (pendingAnim.Contains(Animation.Run) && !_jumping)
                SetAnimation(Animation.Run);
            else if (pendingAnim.Count != 0 && !_jumping)
                SetAnimation(Animation.Default);
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
            return !Scene.Items.GetRange(0, Scene.Items.Count).FindAll(e => e is Character && !e.Equals(this)).TrueForAll(e => !new BoundingObjectModel(this).Intersects(new BoundingObjectModel(e as Character)));
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
                _jumppos -= gameTime.ElapsedGameTime.TotalMilliseconds*Gravity;
            }

            if (!CollideWithMap) return;

            _jumping = false;
            Position = new Vector3(Position.X, 0, Position.Z);
            _jumppos = 0;
            pendingAnim.Add(Animation.Default);
        }

        void Attack(GameTime gameTime)
        {
            var pos = Position;
            Position += new Vector3(Yaw == _baseYaw ? Width/2 : -Width/2, 0, 0);
            BoundingObject.UpdateBox(this);

            if (CollideWithSomeone())
            {
                var c = GetPlayerColliding();
                if (c != null)
                {
                    c.Damages += (1 + c.Damages/50) * 0.5f;
                    c.Position += new Vector3((Yaw == _baseYaw ? 1 : -1) * (c.Damages / 100f) , c.Damages/500f, 0);
                }
            }

            Position = pos;
            BoundingObject.UpdateBox(this);
        }
        #endregion

        void ApplyGravity()
        {
            if(!CollideWithMap && !CollideWithSomeone())
                Position -= new Vector3(0, Gravity * 3, 0);
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
