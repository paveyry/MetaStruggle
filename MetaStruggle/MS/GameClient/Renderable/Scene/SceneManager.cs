using System;
using System.Collections.Generic;
using System.Linq;
using DPSF;
using GameClient.Characters;
using GameClient.Global;
using GameClient.Menus;
using GameClient.Renderable.GUI;
using GameClient.Renderable.Particle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameClient.Renderable._3D;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.Scene
{
    public class SceneManager : Layout.IBasicLayout
    {
        public Camera3D Camera { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public List<I3DElement> Items { get; private set; }
        public Skybox Skybox { get; set; }
        public HUD Hud { get; set; }
        public string MapName { get; set; }
        Dictionary<string, ParticleSystem> ParticlesMap { get; set; }

        public SceneManager(Camera3D camera, SpriteBatch spriteBatch, string mapName = null)
        {
            Camera = camera;
            SpriteBatch = spriteBatch;
            Items = new List<I3DElement>();
            Hud = new HUD();
            if (mapName != null)
                AddMap(mapName);
            InitializeParticleEngine();
        }

        public void AddMap(string mapName)
        {
            Skybox = new Skybox(RessourceProvider.Skyboxes[mapName]);
            Items.Add(new Model3D(this, RessourceProvider.StaticModels[mapName], new Vector3(10, 0, 0),
                          new Vector3(1f, 1f, 0.8f)));

            if (!GameEngine.ParticleEngine.Particles.ContainsKey(mapName)) return;

            MapName = mapName;
            ParticlesMap = GameEngine.ParticleEngine.Particles[mapName];
            GameEngine.ParticleEngine.AddParticles(ParticlesMap);
        }

        public void InitializeParticleEngine()
        {
            GameEngine.ParticleEngine.SetDrawableParticles();
            foreach (var kvp in ParticlesMap)
                kvp.Value.ActivateParticleSystem = true;
        }

        public void AddElement(I3DElement element)
        {
            Items.Add(element);
            if (element is Character)
                Hud.AddCharacter(element as Character);
        }

        public static SceneManager CreateScene(Vector3 cameraPosition, Vector3 cameraTarget, SpriteBatch spriteBatch, string mapName = null)
        {
            return new SceneManager(new Camera3D(cameraPosition, cameraTarget), spriteBatch, mapName);
        }

        public void Update(GameTime gameTime)
        {
            if (GameEngine.KeyboardState.IsKeyDown(Keys.Escape))
            {
                System.Threading.Thread.Sleep(200);
                GameEngine.SoundCenter.PlayWithStatus();
                GameEngine.DisplayStack.Push(new PauseMenu().Create());
                return;
            }
            if (Skybox != null)
                Skybox.Update();
            foreach (var element in Items)
                element.Update(gameTime);
            GameEngine.ParticleEngine.Update(gameTime, Camera);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Skybox != null)
                Skybox.Draw(spriteBatch);
            foreach (var element in Items)
                element.Draw(gameTime, spriteBatch);
            Camera.FollowsCharacters(Camera, Items.FindAll(e => e is Character));
            Hud.DrawHUD(spriteBatch);
            GameEngine.ParticleEngine.Draw(spriteBatch, Camera);
        }
    }
}
