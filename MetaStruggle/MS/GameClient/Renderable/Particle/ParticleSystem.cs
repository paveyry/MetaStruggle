using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DPSF;
using GameClient.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.Particle
{
    [Serializable]
    public class ParticleSystem : DefaultTexturedQuadParticleSystem
    {
        #region Private Fields
        private ParticleFields LoadedFields { get; set; }
        private CInitialPropertiesForQuad LoadedInitialProperties { get; set; }
        private EmitterFields LoadedEmitterFields { get; set; }
        private readonly Game _game;
        private readonly ContentManager _content;
        #endregion

        #region Public Fields
        public bool ActivateParticleSystem { get { return Emitter.Enabled; } set { Emitter.Enabled = value; } }
        public bool IsDrawable { get; set; }
        #endregion

        public ParticleSystem(Game game, ContentManager content, string dir)
            : base(game)
        {
            _game = game;
            _content = content;
            IsDrawable = false;
            LoadedFields = Serialization.LoadFile(dir + "ParticleFields.xml", typeof(ParticleFields)) as ParticleFields;
            LoadedInitialProperties =
                Serialization.LoadFile(dir + "InitialProperties.xml", typeof (CInitialPropertiesForQuad))
                as CInitialPropertiesForQuad;
            LoadedEmitterFields =
                Serialization.LoadFile(dir + "EmitterFields.xml", typeof (EmitterFields)) as EmitterFields;
        }

        private ParticleSystem(Game game, ContentManager content, ParticleFields loadedFields,
                               CInitialPropertiesForQuad loadedInitialProperties, EmitterFields loadedEmitterFields)
            : base(game)
        {
            _game = game;
            _content = content;
            LoadedFields = loadedFields;
            LoadedInitialProperties = loadedInitialProperties;
            LoadedEmitterFields = loadedEmitterFields;
        }

        public void InitializeParticle()
        {
            AutoInitialize(_game.GraphicsDevice, _content, null);
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeTexturedQuadParticleSystem(cGraphicsDevice, cContentManager, 1000, 50000,
                                                UpdateVertexProperties, LoadedFields.TextureDir);
            LoadParticleSystem();
        }

        public void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;
            Serialization.GetFields(LoadedInitialProperties, InitialProperties);
            EmitterFields.CopyEmitterFieldsToParticleEmitter(LoadedEmitterFields, Emitter);

            #region ParticleEvents (With ParticleFields)
            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            if (LoadedFields.BoolUpdateParticlePositionUsingVelocity)
                ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            if (LoadedFields.BoolUpdateParticleRotationUsingRotationalVelocity)
                ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            if (LoadedFields.BoolUpdateParticleWidthAndHeightUsingLerp)
                ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
            if (LoadedFields.BoolUpdateParticleColorUsingLerp)
                ParticleEvents.AddEveryTimeEvent(UpdateParticleColorUsingLerp);
            if (LoadedFields.BoolUpdateParticleTransparencyToFadeOutUsingLerp)
                ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp, LoadedFields.IntUpdateParticleTransparencyToFadeOutUsingLerp);
            if (LoadedFields.BoolUpdateParticleToFaceTheCamera)
                ParticleEvents.AddEveryTimeEvent(UpdateParticleToFaceTheCamera, LoadedFields.IntUpdateParticleToFaceTheCamera);
            ActivateParticleSystem = false;
            #endregion
        }

        public void UpdatePositionEmitter(Vector3 pos)
        {
            Emitter.PositionData.Position = pos;
        }

        public ParticleSystem Clone()
        {
            return new ParticleSystem(_game,_content,LoadedFields,LoadedInitialProperties,LoadedEmitterFields);
        }

        public void Pause()
        {
            Enabled = Visible = false;
        }

        public void Resume()
        {
            Enabled = Visible = true;
        }
    }
}
