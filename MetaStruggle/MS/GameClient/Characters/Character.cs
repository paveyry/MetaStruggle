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
        public int PlayerNb { get; set; }
        public readonly float BaseYaw;
        public Vector3 SpawnPosition;
        public Client Client { get; set; }
        public bool IsDead;
        public bool IsPermanentlyDead { get; set; }
        public DateTime DeathDate;
        public int NumberOfDeath;
        public int NumberMaxOfLives;
        public float Damages = 0;
        public string PlayerName;
        public Texture2D Face;
        public string MapName;
        private DateTime _lastSA = DateTime.Now;
        private bool _saDone = true;

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
        public int SyncRate = 3;

        //****PARTICLE****
        Dictionary<string, ParticleSystem> ParticlesCharacter { get; set; }
        private DateTime run;
        private bool running;

        //****IA****
        public delegate bool MovementActivate(Movement movement);
        public MovementActivate GetKey { get; set; }
        
        public bool CollideWithMap
        {
            get { return (Position.Y <= 0.00 && Position.Y > -1 && Position.X < 13 && Position.X > -24.5); }
        }
        #endregion

        public Character(string playerName, string nameCharacter,string mapName, int playerNb, SceneManager scene, Vector3 position, Vector3 scale
            , float speed = 1f)
            : base(nameCharacter, scene, position, scale, speed)
        {
            Playing = true;
            ID = 0;
            PlayerNb = playerNb;
            PlayerName = playerName;
            MapName = mapName;
            Face = RessourceProvider.CharacterFaces[nameCharacter];
            Pitch = -MathHelper.PiOver2;
            Yaw = MathHelper.PiOver2;
            BaseYaw = Yaw;
            Gravity = -20f;
            _gravity = new Vector3(0, Gravity, 0);
            SpawnPosition = position;
            _latteralMove = new Vector3(LatteralSpeed, 0, 0);
            GetKey = (movement) => GetUniversalKey(movement).IsPressed();
            CreateParticlesCharacter(ModelName);
        }

        void CreateParticlesCharacter(string nameCharacter)
        {
            #region FillCorrectlyDictionnary
            if (GameEngine.ParticleEngine.Particles.ContainsKey(nameCharacter))
                ParticlesCharacter = GameEngine.ParticleEngine.Particles[nameCharacter];
            else if (GameEngine.ParticleEngine.Particles.ContainsKey("defaultPerso"))
                ParticlesCharacter = GameEngine.ParticleEngine.Particles["defaultPerso"]
                    .ToDictionary(e => e.Key, e => e.Value.Clone());
            else
                ParticlesCharacter = null;
            if (ParticlesCharacter != null && GameEngine.ParticleEngine.Particles.ContainsKey("defaultPerso"))
                foreach (
                    var kvp in
                        GameEngine.ParticleEngine.Particles["defaultPerso"].Where(
                            kvp => !ParticlesCharacter.ContainsKey(kvp.Key)))
                    ParticlesCharacter.Add(kvp.Key, kvp.Value.Clone());
            #endregion

            foreach (var particleSystem in ParticlesCharacter.Where((kvp) => kvp.Key.EndsWith(MapName))
                .ToDictionary((kvp) => kvp.Key, kvp => kvp.Value))
            {
                ParticlesCharacter[particleSystem.Key.Substring(0, particleSystem.Key.Length - MapName.Length)] =
                    particleSystem.Value;
                ParticlesCharacter.Remove(particleSystem.Key);
            }

            GameEngine.ParticleEngine.AddParticles(ParticlesCharacter);
        }

        public override void Update(GameTime gameTime)
        {
            Dictionary<Movement, bool> movements = Enum.GetValues(typeof (Movement)).Cast<Movement>().ToDictionary(move => move, CallGetKey);

            #region Particle (à modifier ! -> Zone de test)
            if (ParticlesCharacter != null) //switch perso => prévoir default
            {
                foreach (var kvp in ParticlesCharacter)
                {
                    kvp.Value.ActivateParticleSystem = false;
                }
                var ParticlesJump = ParticlesCharacter["Jump"];
                var ParticlesDoubleJump = ParticlesCharacter["DoubleJump"];
                var ParticlesRun = ParticlesCharacter["Run"];
                var ParticlesRetombe = ParticlesCharacter["Retombe"];
                ParticlesRetombe.UpdatePositionEmitter(Position);
                ParticlesJump.UpdatePositionEmitter(Position);
                ParticlesDoubleJump.UpdatePositionEmitter(Position);
                ParticlesRun.UpdatePositionEmitter(Position + new Vector3(0.2f, 0, 0));
                if (movements[Movement.Right] && !_jump && !running || movements[Movement.Left] && !_jump && !running)
                    run = DateTime.Now;
                ParticlesRun.ActivateParticleSystem = movements[Movement.Right] && CollideWithMap && (DateTime.Now - run).TotalMilliseconds % 500 >= 0
                    && (DateTime.Now - run).TotalMilliseconds % 500 < 100 || movements[Movement.Left] && CollideWithMap && (DateTime.Now - run).TotalMilliseconds % 500 >= 0
                    && (DateTime.Now - run).TotalMilliseconds % 500 < 100;
                switch (ModelName)
                {
                    case "Poseidon":
                        var ParticlesAttaqueeclatdeau = ParticlesCharacter["Attaqueeclatdeau"];
                        var ParticlesAttaquegeyser = ParticlesCharacter["Attaquegeyser"];
                        ParticlesAttaqueeclatdeau.UpdatePositionEmitter(Position + new Vector3((Yaw == BaseYaw) ? 1 : -1, 0, 0));
                        ParticlesAttaquegeyser.UpdatePositionEmitter(Position + new Vector3((Yaw == BaseYaw) ? 1 : -1, 0, 0));
                        ParticlesAttaqueeclatdeau.ActivateParticleSystem = movements[Movement.Attack] && DateTime.Now.Millisecond % 1000 < 700;
                        ParticlesAttaquegeyser.ActivateParticleSystem = ParticlesAttaqueeclatdeau.ActivateParticleSystem;
                        break;
                }
            }
            #endregion

            var pendingAnim = new List<Animation>();

            #region ManageKeyboard
            if (Playing && !IsDead)
            {
                if (CurrentAnimation != Animation.Jump)
                    pendingAnim.Add(Animation.Default);

                if (movements[Movement.SpecialAttack])
                {
                    if ((DateTime.Now - _lastSA).TotalMilliseconds < 1500)
                        pendingAnim.Add(Animation.SpecialAttack);

                    if ((DateTime.Now - _lastSA).TotalMilliseconds > 3000 && _saDone)
                    {
                        _lastSA = DateTime.Now;
                        _saDone = false;
                    }
                    else if ((DateTime.Now - _lastSA).TotalMilliseconds > 600 && !_saDone)
                    {
                        Attack(gameTime, true);
                        _saDone = true;
                    }
                }
                if (movements[Movement.Attack])
                {
                    //if((DateTime.Now - _lastA).TotalMilliseconds < 1500)
                    pendingAnim.Add(Animation.Attack);
                    Attack(gameTime, false);
                    /*if ((DateTime.Now - _lastA).TotalMilliseconds > 1000 && _aDone)
                    {
                        _lastA = DateTime.Now;
                        _aDone = false;
                    }
                    else if ((DateTime.Now - _lastA).TotalMilliseconds > 600 && !_aDone)
                    {
                        Attack(gameTime, false);
                        _aDone = true;
                    }*/
                }
                if (movements[Movement.Jump] && (!_jump || !_doublejump) && (DateTime.Now - _firstjump).Milliseconds > 300)
                {
                    GiveImpulse(-(new Vector3(0, Speed.Y, 0) + _gravity / 1.4f));

                    if (_jump)
                    {
                        var ParticlesDoubleJump = ParticlesCharacter["DoubleJump"];
                        ParticlesDoubleJump.UpdatePositionEmitter(Position);
                        ParticlesDoubleJump.ActivateParticleSystem = true;
                        _doublejump = true;
                    }
                    else
                    {
                        var ParticlesJump = ParticlesCharacter["Jump"];
                        ParticlesJump.UpdatePositionEmitter(Position);
                        ParticlesJump.ActivateParticleSystem = true;
                        _jump = true;
                        _firstjump = DateTime.Now;
                    }

                    pendingAnim.Add(Animation.Jump);
                    GameEngine.EventManager.ThrowNewEvent("Character.Jump", this);
                }
                if (movements[Movement.Right])
                {
                    running = true;
                    MoveRight(gameTime);
                    pendingAnim.Add(Animation.Run);
                }

                if (movements[Movement.Left])
                {
                    running = true;
                    MoveLeft(gameTime);
                    pendingAnim.Add(Animation.Run);
                }
            }
            #endregion

            #region Death
            if (!IsDead && Position.Y < -20 || !IsDead && Position.X < -38 || !IsDead && Position.X > 33)
            {
                IsDead = true;
                NumberOfDeath++;
                IsPermanentlyDead = (NumberMaxOfLives - NumberOfDeath <= 0);
                DeathDate = DateTime.Now;
                GameEngine.EventManager.ThrowNewEvent("Character.Die", this);
            }

            if (!IsPermanentlyDead && IsDead && (DateTime.Now - DeathDate).TotalMilliseconds > 5000)
            {
                SetAnimation(Animation.Default);
                IsDead = false;
                Position = SpawnPosition;
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

            #region Network
            if (Playing && Client != null && count % SyncRate == 0)
                new SetCharacterPosition().Pack(Client.Writer, new CharacterPositionDatas { ID = ID, X = Position.X, Y = Position.Y, Yaw = Yaw, Anim = (byte)CurrentAnimation });
            else if (dI.HasValue)
                Position += dI.Value;

            count = (count + 1) % 60;
            #endregion
            
            #region Physic
            if (Playing)
            {
                ApplyGravity(gameTime);
                ApplySpeed(gameTime);
            }
            KeepOnTheGround();
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
            Yaw = BaseYaw + MathHelper.Pi;
            Position -= _latteralMove * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        void MoveLeft(GameTime gameTime)
        {
            Yaw = BaseYaw;
            Position += _latteralMove * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        void Attack(GameTime gameTime, bool special)
        {
            List<I3DElement> characters = Scene.Items.FindAll(i3de => i3de is Character && i3de != this);

            foreach (Character character in characters.Cast<Character>().Where(character => (Yaw == BaseYaw ? Position - character.Position : character.Position - Position).Length() < 1.3 && (Yaw == BaseYaw ? Position - character.Position : character.Position - Position).X < 0))
            {
                character.GiveImpulse(new Vector3((float) ((Yaw == BaseYaw ? -1 : 1) * Gravity * (1 + character.Damages/3) * 0.008f * (special ? 1 : 10f * gameTime.ElapsedGameTime.TotalMilliseconds/1000)),
                                                  special ? -Gravity * (1 + character.Damages) * 0.008f : 0.2f, 0));

                character.Damages += ((float)(special ? 10 + (Damages / 4) : ((Damages/7) + 6) * gameTime.ElapsedGameTime.TotalMilliseconds / 1000));
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
                if (CurrentAnimation != Animation.Jump && CurrentAnimation != Animation.Attack && CurrentAnimation != Animation.SpecialAttack)
                    SetAnimation(Animation.Jump);
                return;
            }

            Position = new Vector3(Position.X, 0, Position.Z);
            Speed.Y = 0;
            Speed.X *= 0.7f;
            if (_jump)
            {
                var ParticlesRetombe = ParticlesCharacter["Retombe"];
                ParticlesRetombe.UpdatePositionEmitter(Position);
                ParticlesRetombe.ActivateParticleSystem = true;
            }
            _jump = false;
            _doublejump = false;
        }

        void ApplySpeed(GameTime gameTime)
        {
            Position = Position + Speed * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
        }
        #endregion

        #region Environnement
        public void SetEnvironnementDatas(bool playing, byte playerNb)
        {
            Playing = playing;
            PlayerNb = playerNb;
        }

        public void SetEnvironnementDatas(byte id, bool playing, Client client)
        {
            Playing = playing;
            Client = client;
            ID = id;
        }

        #endregion
    }
}
