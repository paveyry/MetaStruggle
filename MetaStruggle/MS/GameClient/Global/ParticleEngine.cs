using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DPSF;
using GameClient.Renderable.Particle;
using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Global
{
    public class ParticleEngine
    {
        public ParticleSystemManager ParticleSystemManager { get; set; }
        public Dictionary<string, Dictionary<string, ParticleSystem>> Particles = new Dictionary<string, Dictionary<string, ParticleSystem>>();

        public ParticleEngine(ContentManager content, Game game)
        {
            ParticleSystemManager = new ParticleSystemManager();
            Particles = new Dictionary<string, Dictionary<string, ParticleSystem>>();
            FillParticles(content,game);
        }

        public void FillParticles(ContentManager content, Game game)
        {
            foreach (var mainDir in Directory.GetDirectories("Particles"))
                Particles.Add(Path.GetFileNameWithoutExtension(mainDir),
                              Directory.GetDirectories(mainDir)
                                       .ToDictionary(Path.GetFileNameWithoutExtension,
                                                     dir => new ParticleSystem(game, content, dir + '\\')));
        }

        public void SetDrawableParticles()
        {
            foreach (var kvp in Particles.SelectMany(mainKvp => mainKvp.Value).Where(kvp => kvp.Value.IsDrawable))
            {
                ParticleSystemManager.AddParticleSystem(kvp.Value);
                kvp.Value.InitializeParticle();
            }
        }

        #region AddParticles
        public void AddParticle(string mainDir, string particlesystem)
        {
            var particle = Particles[mainDir][particlesystem];
            particle.IsDrawable = true;
            ParticleSystemManager.AddParticleSystem(particle);
            particle.InitializeParticle();
        }

        public void AddParticles(string mainDir)
        {
            AddParticles(Particles[mainDir]);
        }

        public void AddParticles(Dictionary<string, ParticleSystem> dictionary)
        {
            if (dictionary == null)
                return;
            foreach (var kvp in dictionary.Where(kvp => !kvp.Value.IsDrawable))
            {
                kvp.Value.IsDrawable = true;
                ParticleSystemManager.AddParticleSystem(kvp.Value);
                kvp.Value.InitializeParticle();
            }
        }
        #endregion

        public void RemoveAndDestroyAll()
        {
            ParticleSystemManager.DestroyAndRemoveAllParticleSystems();
        }

        public void Update(GameTime gameTime, Camera3D camera)
        {
            ParticleSystemManager.SetCameraPositionForAllParticleSystems(camera.Position);
            ParticleSystemManager.UpdateAllParticleSystems((float)gameTime.ElapsedGameTime.TotalSeconds);
            
        }

        public void Draw(SpriteBatch spriteBatch, Camera3D camera)
        {
            ParticleSystemManager.DrawAllParticleSystems();
            
            ParticleSystemManager.SetWorldViewProjectionMatricesForAllParticleSystems(Matrix.Identity,
                camera.ViewMatrix, Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45),
                spriteBatch.GraphicsDevice.DisplayMode.AspectRatio, 1f, 100f));

            spriteBatch.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
