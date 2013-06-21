using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameClient.Characters.AI;
using GameClient.Global;
using GameClient.Global.InputManager;
using GameClient.Renderable.Particle;
using GameClient.Renderable.Scene;
using GameClient.Renderable._3D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Network;
using Network.Packet.Packets;
using Network.Packet.Packets.DatasTypes;
using DPSF;

namespace GameClient.Characters
{
    public enum Movement
    {
        Left,
        Right,
        Attack,
        Jump,
        SpecialAttack
    }
    public class Character : AnimatedModel3D
    {
        #region Fields
        public byte ID { get; set; }
        public byte PlayerNb { get; set; }
        private readonly float _baseYaw;
        private readonly Vector3 _spawnPosition;
        public Client Client { get; set; }
        public bool IsDead;
        public DateTime DeathDate;
        public float Damages = 0;
        public string PlayerName;
        public Texture2D Face;

        //****PHYSIC****
        private const float LatteralSpeed = 0.005f;
        private readonly Vector3 _latteralMove;
        public Vector3 Speed;
        private readonly Vector3 _gravity;
        private bool _jump;
        private bool _doublejump;
        private DateTime _firstjump = DateTime.Now;

        //****NETWORK****
        private int count;
        public bool Playing { get; set; }
        public Vector3? F1, F2, dI;
        public int SyncRate = 5;

        //****PARTICLE****
        Dictionary<string, ParticleSystem> ParticlesCharacter { get; set; }

        //****IA****
        public delegate bool MovementActivate(Movement movement);
        public MovementActivate GetKey { get; set; }
        private ComputerCharacter ComputerCharacter { get; set; }
        public bool IsNormalPlayer { get; set; }

        public bool CollideWithMap
        {
            get { return (Position.Y <= 0.00 && Position.Y > -1 && Position.X < 13 && Position.X > -24.5); }
        }
        #endregion

        public Character(string playerName, string nameCharacter, byte playerNb, SceneManager scene, Vector3 position, Vector3 scale
            , float speed = 1f)
            : base(nameCharacter, scene, position, scale, speed)
        {
            Playing = true;
            ID = 0;
            PlayerNb = playerNb;
            PlayerName = playerName;
            Face = RessourceProvider.CharacterFaces[nameCharacter];
            CreateParticlesCharacter(nameCharacter);
            Pitch = -MathHelper.PiOver2;
            Yaw = MathHelper.PiOver2;
            _baseYaw = Yaw;
            Gravity = -20f;
            _gravity = new Vector3(0, Gravity, 0);
            _spawnPosition = position;
            _latteralMove = new Vector3(LatteralSpeed, 0, 0);
            IsNormalPlayer = true;
            GetKey = (movement) => GetUniversalKey(movement).IsPressed();
        }

        void CreateParticlesCharacter(string nameCharacter)
        {
            if (GameEngine.ParticleEngine.Particles.ContainsKey(nameCharacter))
                ParticlesCharacter = GameEngine.ParticleEngine.Particles[nameCharacter];
            else if (GameEngine.ParticleEngine.Particles.ContainsKey("defaultPerso"))
                ParticlesCharacter = GameEngine.ParticleEngine.Particles["defaultPerso"].ToDictionary(e => e.Key, e => e.Value);
            else
                ParticlesCharacter = null;
            if (ParticlesCharacter != null && GameEngine.ParticleEngine.Particles.ContainsKey("defaultPerso"))
                foreach (var kvp in GameEngine.ParticleEngine.Particles["defaultPerso"].Where(kvp => !ParticlesCharacter.ContainsKey(kvp.Key)))
                    ParticlesCharacter.Add(kvp.Key, kvp.Value);

            GameEngine.ParticleEngine.AddParticles(ParticlesCharacter);
        }

        public override void Update(GameTime gameTime)
        {
            var pendingAnim = new List<Animation>();

            #region ManageKeyboard
            if (Playing && !IsDead)
            {
                if (CurrentAnimation != Animation.Jump)
                    pendingAnim.Add(Animation.Default);

                if (CallGetKey(Movement.SpecialAttack))
                {
                    Attack(gameTime, true);
                    pendingAnim.Add(Animation.SpecialAttack);
                }
                if (CallGetKey(Movement.Attack))
                {
                    Attack(gameTime, false);
                    pendingAnim.Add(Animation.Attack);
                }
                if (CallGetKey(Movement.Jump) && (!_jump || !_doublejump) && (DateTime.Now - _firstjump).Milliseconds > 300)
                {
                    GiveImpulse(-(new Vector3(0, Speed.Y, 0) + _gravity / 1.4f));

                    if (_jump)
                        _doublejump = true;
                    else
                    {
                        _jump = true;
                        _firstjump = DateTime.Now;
                    }

                    pendingAnim.Add(Animation.Jump);
                    GameEngine.EventManager.ThrowNewEvent("Character.Jump", this);
                }
                if (CallGetKey(Movement.Right))
                {
                    MoveRight(gameTime);
                    pendingAnim.Add(Animation.Run);
                }

                if (CallGetKey(Movement.Left))
                {
                    MoveLeft(gameTime);
                    pendingAnim.Add(Animation.Run);
                }
            }
            #endregion

            #region Death
            if (!IsDead && Position.Y < -20 || !IsDead && Position.X < -38 || !IsDead && Position.X > 33)
            {
                IsDead = true;
                DeathDate = DateTime.Now;
                GameEngine.EventManager.ThrowNewEvent("Character.Die", this);
            }

            if (IsDead && (DateTime.Now - DeathDate).TotalMilliseconds > 5000)
            {
                SetAnimation(Animation.Default);
                IsDead = false;
                Position = _spawnPosition;
                Speed = Vector3.Zero;
                _jump = false;
                _doublejump = false;
                Damages = 0;
            }
            #endregion

            #region Animations
            if (Playing)
            {
                if (CollideWithMap && CurrentAnimation != Animation.Default)
                    pendingAnim.Add(Animation.Default);

                SetPriorityAnimation(pendingAnim);
            }
            #endregion

            #region Physic
            if (Playing)
            {
                ApplyGravity(gameTime);
                ApplySpeed(gameTime);
                KeepOnTheGround();
            }
            #endregion

            #region Particle
            if (ParticlesCharacter != null)
            {
                foreach (var kvp in ParticlesCharacter)
                {
                    kvp.Value.UpdatePositionEmitter(Position + new Vector3(Yaw == _baseYaw ? 0.6f : -0.6f, 1.2f, 0));
                    kvp.Value.ActivateParticleSystem = CallGetKey(Movement.Attack); //test
                }
            }
            #endregion

            #region Network
            if (Playing && Client != null && count % SyncRate == 0)
                new SetCharacterPosition().Pack(Client.Writer, new CharacterPositionDatas { ID = ID, X = Position.X, Y = Position.Y, Yaw = Yaw, Anim = (byte) CurrentAnimation});

            if (!Playing && dI.HasValue && count % SyncRate != 0)
                Position += dI.Value;

            count = (count + 1) % 60;
            #endregion

            #region AI
            if (ComputerCharacter != null)
                ComputerCharacter.Update(gameTime);
            #endregion

            base.Update(gameTime);
        }

        #region Movements
        void SetPriorityAnimation(ICollection<Animation> pendingAnim)
        {
            if (pendingAnim.Contains(Animation.SpecialAttack))
                SetAnimation(Animation.SpecialAttack);
            else if (pendingAnim.Contains(Animation.Attack))
                SetAnimation(Animation.Attack);
            else if (pendingAnim.Contains(Animation.Jump))
                SetAnimation(Animation.Jump);
            else if (pendingAnim.Contains(Animation.Run) && !_jump && !_doublejump)
                SetAnimation(Animation.Run);
            else if (pendingAnim.Count != 0 && !_jump && !_doublejump)
                SetAnimation(Animation.Default);

            if (ModelName == "Spiderman")
                AnimationController.Speed = CurrentAnimation == Animation.SpecialAttack ? 2f : 1.6f;
        }
        
        bool CallGetKey(Movement movement)
        {
            return GetKey.Invoke(movement);
        }

        UniversalKeys GetUniversalKey(Movement movement)
        {
            return RessourceProvider.InputKeys[movement + "." + PlayerNb];
        }
        
        void MoveRight(GameTime gameTime)
        {
            Yaw = _baseYaw + MathHelper.Pi;
            Position -= _latteralMove * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        void MoveLeft(GameTime gameTime)
        {
            Yaw = _baseYaw;
            Position += _latteralMove * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        void Attack(GameTime gameTime, bool special)
        {
            List<I3DElement> characters = Scene.Items.FindAll(i3de => i3de is Character && i3de != this);

            foreach (Character character in characters)
            {
                if (Yaw == _baseYaw)
                {
                    if ((Position - character.Position).Length() < 1.3 && (Position - character.Position).X < 0)
                    {
                        character.GiveImpulse(new Vector3(-Gravity*(1 + character.Damages)*0.001f,
                                                          special ? -Gravity*(1 + character.Damages)*0.001f : 0.1f, 0));

                        character.Damages += ((special ? 10 : 3) + character.Damages / 3) *
                                             (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
                    }
                }
                else
                {
                    if ((character.Position - Position).Length() < 1.3 && (character.Position - Position).X < 0)
                    {
                        character.GiveImpulse(new Vector3(Gravity*(1 + character.Damages)*0.001f,
                                                          special ? -Gravity*(1 + character.Damages)*0.001f : 0.1f, 0));

                        character.Damages += ((special ? 10 : 3) + character.Damages / 3) *
                                             (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
                    }
                }
            }
        }
        #endregion

        #region Physic
        public void GiveImpulse(Vector3 impulsion)
        {
            Speed = Speed + impulsion;
        }

        void ApplyGravity(GameTime gameTime)
        {
            Speed += _gravity * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
        }

        private void KeepOnTheGround()
        {
            if (!CollideWithMap)
            {
                if(CurrentAnimation != Animation.Jump && CurrentAnimation != Animation.Attack && CurrentAnimation != Animation.SpecialAttack)
                    SetAnimation(Animation.Jump);
                return;
            }

            Position = new Vector3(Position.X, 0, Position.Z);
            Speed.Y = 0;
            Speed.X *= 0.7f;
            _jump = false;
            _doublejump = false;
        }

        void ApplySpeed(GameTime gameTime)
        {
            Position = Position + Speed * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
        }
        #endregion

        #region Environnement
        internal void SetEnvironnementDatas(string playerName, SceneManager sm, bool playing)
        {
            PlayerName = playerName;
            Scene = sm;
            Playing = playing;
        }
        
        public void SetEnvironnementDatas(string playerName, SceneManager sm, bool playing, byte playerNb)
        {
            SetEnvironnementDatas(playerName,sm,playing);
            PlayerNb = playerNb;
        }

        public void SetEnvironnementDatas(string playerName, byte id, SceneManager sm, bool playing, Client client)
        {
            SetEnvironnementDatas(playerName,sm,playing);
            Client = client;
            ID = id;
        }

        public void SetEnvironnementDatas(string playerName,SceneManager sm, bool playing, ComputerCharacter computerCharacter)
        {
            SetEnvironnementDatas(playerName,sm,playing);
            ComputerCharacter = computerCharacter;
            IsNormalPlayer = false;
            GetKey = ComputerCharacter.GetMovement;
        }
        #endregion
    }
}
