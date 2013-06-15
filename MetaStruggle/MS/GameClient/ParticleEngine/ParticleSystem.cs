using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DPSF;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.ParticleEngine
{
    public class ParticleSystem : DefaultTexturedQuadParticleSystem
    {
        string TextureDir { get; set; }

        public ParticleSystem(Game game, string textureDir) : base(game)
        {
            TextureDir = textureDir;
            ParticleInitializationFunction = InitializeParticleProperties;
        }

        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeTexturedQuadParticleSystem(cGraphicsDevice, cContentManager, 1000, 50000,
                                                UpdateVertexProperties, TextureDir);
            LoadParticleSystem();
        }

        public void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;
            InitialProperties.LifetimeMin = 1.5f;
            InitialProperties.LifetimeMax = 3.0f;
            InitialProperties.PositionMin = Vector3.Zero;
            InitialProperties.PositionMax = Vector3.Zero;
            InitialProperties.VelocityMin = new Vector3(-50, 50, -50);
            InitialProperties.VelocityMax = new Vector3(50, 100, 50);
            InitialProperties.RotationMin.Z = 0.0f;
            InitialProperties.RotationMax.Z = MathHelper.Pi;
            InitialProperties.RotationalVelocityMin.Z = -MathHelper.Pi;
            InitialProperties.RotationalVelocityMax.Z = MathHelper.Pi;
            InitialProperties.StartSizeMin = 20;
            InitialProperties.StartSizeMax = 40;
            InitialProperties.EndSizeMin = 30;
            InitialProperties.EndSizeMax = 30;
            InitialProperties.StartColorMin = Color.Black;
            InitialProperties.StartColorMax = Color.White;
            InitialProperties.EndColorMin = Color.Black;
            InitialProperties.EndColorMax = Color.White;
            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();
            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleColorUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp, 100);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleToFaceTheCamera, 200);
            Emitter.ParticlesPerSecond = 50;
            Emitter.PositionData.Position = /*new Vector3(-200, -200, 0)*/ new Vector3(0, 0, 0);
        }

        public void InitializeParticleProperties(DefaultTexturedQuadParticle cParticle)
        {
            // Set the Particle's Lifetime (how long it should exist for)
            cParticle.Lifetime = 2.0f;

            // Set the Particle's initial Position to be wherever the Emitter is
            cParticle.Position = Emitter.PositionData.Position;

            // Set the Particle's Velocity
            Vector3 sVelocityMin = new Vector3(-100, 25, -100);
            Vector3 sVelocityMax = new Vector3(100, 50, 100);
            cParticle.Velocity = DPSFHelper.RandomVectorBetweenTwoVectors(sVelocityMin, sVelocityMax);

            // Adjust the Particle's Velocity direction according to the Emitter's Orientation
            cParticle.Velocity = Vector3.Transform(cParticle.Velocity, Emitter.OrientationData.Orientation);

            // Give the Particle a random Size
            // Since we have Size Lerp enabled we must also set the Start and End Size
            cParticle.Size = cParticle.StartSize = cParticle.EndSize = RandomNumber.Next(10, 20);

            // Give the Particle a random Color
            // Since we have Color Lerp enabled we must also set the Start and End Color
            cParticle.Color = cParticle.StartColor = cParticle.EndColor = DPSFHelper.RandomColor();
        }
    }
}
