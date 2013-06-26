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
        public virtual float Damages { get; set; }
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
        public int SyncRate = 5;

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

        public Character(string playerName, string nameCharacter, string mapName, int playerNb, SceneManager scene, Vector3 position, Vector3 scale
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
                foreach (var kvp in GameEngine.ParticleEngine.Particles["defaultPerso"].Where(
                            kvp => !ParticlesCharacter.ContainsKey(kvp.Key)))
                    try
                    {
                        ParticlesCharacter.Add(kvp.Key, kvp.Value.Clone());
                    }
                    catch { }
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
            Dictionary<Movement, bool> movements = Enum.GetValues(typeof(Movement)).Cast<Movement>().ToDictionary(move => move, CallGetKey);

            #region Particle (à modifier ! -> Zone de test)
            if (ParticlesCharacter != null) //switch perso => prévoir default
            {
                foreach (var kvp in ParticlesCharacter)
                {
                    kvp.Value.ActivateParticleSystem = false;
                }
                var ParticlesFrappe = ParticlesCharacter["Frappe"];
                var ParticlesJump = ParticlesCharacter["Jump"];
                var ParticlesDoubleJump = ParticlesCharacter["DoubleJump"];
                var ParticlesRun = ParticlesCharacter["Run"];
                var ParticlesRetombe = ParticlesCharacter["Retombe"];
                ParticlesRetombe.UpdatePositionEmitter(Position);
                ParticlesJump.UpdatePositionEmitter(Position);
                ParticlesDoubleJump.UpdatePositionEmitter(Position);
                ParticlesRun.UpdatePositionEmitter(Position + new Vector3(0.2f, 0, 0));
                ParticlesFrappe.UpdatePositionEmitter(Position + new Vector3((Yaw == BaseYaw) ? 1 : -1, 0.8f, 0));
                if (CurrentAnimation == Animation.Run && !_jump && !running)
                    run = DateTime.Now;
                ParticlesRun.ActivateParticleSystem = CurrentAnimation == Animation.Run && CollideWithMap && (DateTime.Now - run).TotalMilliseconds % 500 >= 0
                    && (DateTime.Now - run).TotalMilliseconds % 500 < 100;
                switch (ModelName)
                {
                    case "Poseidon":
                        var ParticlesAttaqueeclatdeau = ParticlesCharacter["Attaqueeclatdeau"];
                        var ParticlesAttaquegeyser = ParticlesCharacter["Attaquegeyser"];
                        ParticlesAttaqueeclatdeau.UpdatePositionEmitter(Position + new Vector3((Yaw == BaseYaw) ? 1 : -1, 0, 0));
                        ParticlesAttaquegeyser.UpdatePositionEmitter(Position + new Vector3((Yaw == BaseYaw) ? 1 : -1, 0, 0));
                        ParticlesAttaqueeclatdeau.ActivateParticleSystem = CurrentAnimation == Animation.Attack && DateTime.Now.Millisecond % 1000 < 700;
                        ParticlesAttaquegeyser.ActivateParticleSystem = ParticlesAttaqueeclatdeau.ActivateParticleSystem;
                        if (movements[Movement.Attack])
                            GameEngine.SoundCenter.Play("water");
                        break;
                    case "Ironman":
                        if (Yaw == BaseYaw)
                        {
                            var ParticlesAttaquelaserg = ParticlesCharacter["attaquenormgauche"];
                            ParticlesAttaquelaserg.UpdatePositionEmitter(Position +
                                                                        new Vector3((Yaw == BaseYaw) ? 1 : -1, 0, 0));
                            ParticlesAttaquelaserg.ActivateParticleSystem = CurrentAnimation == Animation.Attack &&
                                                                           DateTime.Now.Millisecond % 1000 < 700;

                        }
                        else
                        {
                            var ParticlesAttaquelaser = ParticlesCharacter["attaquenorm"];
                            ParticlesAttaquelaser.UpdatePositionEmitter(Position +
                                                                        new Vector3((Yaw == BaseYaw) ? 1 : -1, 0, 0));
                            ParticlesAttaquelaser.ActivateParticleSystem = CurrentAnimation == Animation.Attack &&
                                                                           DateTime.Now.Millisecond%1000 < 700;
                        }

                        if (movements[Movement.Attack])
                            GameEngine.SoundCenter.Play("laser");
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
                    if ((DateTime.Now - _lastSA).TotalMilliseconds > 700 && (DateTime.Now - _lastSA).TotalMilliseconds < 1000)
                    {
                        switch (ModelName)
                        {
                            case "Zeus":
                                var ParticlesAttaquespe = ParticlesCharacter["Attaquespe"];
                                var ParticlesAttaquesol = ParticlesCharacter["Attaquesol"];
                                ParticlesAttaquesol.UpdatePositionEmitter(Position +
                                                                          new Vector3((Yaw == BaseYaw) ? 1.2f : -1.2f, 0,
                                                                                      0));
                                ParticlesAttaquespe.UpdatePositionEmitter(Position +
                                                                          new Vector3((Yaw == BaseYaw) ? 1.2f : -1.2f,
                                                                                      4.5f, 0));
                                ParticlesAttaquesol.ActivateParticleSystem = true;
                                ParticlesAttaquespe.ActivateParticleSystem = true;
                                GameEngine.SoundCenter.Play("eclair");
                                break;
                            case "Poseidon":
                                GameEngine.SoundCenter.Play("sword");
                                break;
                            default:
                                break;
                        }
                    }
                    if ((DateTime.Now - _lastSA).TotalMilliseconds < 1000)
                        pendingAnim.Add(Animation.SpecialAttack);

                    if ((DateTime.Now - _lastSA).TotalMilliseconds > 1000 && _saDone)
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
                    pendingAnim.Add(Animation.Attack);
                    Attack(gameTime, false);
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
                new SetCharacterPosition().Pack(Client.Writer, new CharacterPositionDatas { ID = ID, X = Position.X, Y = Position.Y, Yaw = Yaw, Anim = (byte)CurrentAnimation, Damages = Damages, Lives = (byte)(NumberMaxOfLives - NumberOfDeath) });
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
                var impulse =
                    new Vector3((float)((Yaw == BaseYaw ? -1 : 1) * Gravity * (1 + character.Damages / 3) * 0.008f * (special ? 1 : 10f * gameTime.ElapsedGameTime.TotalMilliseconds / 1000)), special ? -Gravity * (1 + character.Damages) * 0.008f : 0.2f, 0);

                character.GiveImpulse(impulse);

                var damages = ((float)
                     (special ? 10 + (Damages / 4) : ((Damages / 7) + 6) * gameTime.ElapsedGameTime.TotalMilliseconds / 1000));
                var ParticlesFrappe = ParticlesCharacter["Frappe"];
                ParticlesFrappe.UpdatePositionEmitter(Position + new Vector3((Yaw == BaseYaw) ? 1 : -1, 0.8f, 0));
                ParticlesFrappe.ActivateParticleSystem = DateTime.Now.Millisecond % 150 < 25;
                character.Damages += damages;

                if (Client != null)
                    new GiveImpulse().Pack(Client.Writer, new GiveImpulseDatas { Damages = damages, ID = character.ID, X = impulse.X, Y = impulse.Y });
                switch (ModelName)
                {
                    case "Ares":
                        GameEngine.SoundCenter.Play("sword");
                        break;
                    case "Zeus":
                    case "Alex":
                    case "Spiderman":
                        GameEngine.SoundCenter.Play("degats");
                        break;
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
