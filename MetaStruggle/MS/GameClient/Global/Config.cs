using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameClient.Menus;

namespace GameClient.Global
{
    public class Config
    {
        public string GameName { get; set; }
        public string Language { get; set; }
        public int ResolutionWidth { get; set; }
        public int ResolutionHeight { get; set; }
        public int VolumeMusic { get; set; }
        public int VolumeEffect { get; set; }
        public bool FullScreen { get; set; }
        public bool PreferMultiSampling { get; set; }
        public string[] Keys { get; set; }

        public static Config GetDefaultConfig()
        {
            return new Config
                {
                    GameName = "MetaStruggle",
                    Language = "fr_FR",
                    ResolutionWidth = 800,
                    ResolutionHeight = 600,
                    VolumeMusic = 100,
                    VolumeEffect = 100,
                    FullScreen = false,
                    PreferMultiSampling = true,
                    Keys = new [] { "Keyboard.Q,Keyboard.D,Keyboard.Z,Keyboard.Space,Keyboard.A" }
                };
        }

        public void ApplyConfig(GraphicsDeviceManager graphics)
        {
            ApplyGraphics(graphics);
            ApplySound();
            ApplyInput();
        }

        public void ApplyGraphics(GraphicsDeviceManager graphics)
        {
            graphics.PreferredBackBufferWidth = GameEngine.Config.ResolutionWidth;
            graphics.PreferredBackBufferHeight = GameEngine.Config.ResolutionHeight;
            graphics.PreferMultiSampling = GameEngine.Config.PreferMultiSampling;
            graphics.GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };
            graphics.IsFullScreen = GameEngine.Config.FullScreen;
            graphics.ApplyChanges();
        }

        public void ApplySound()
        {
            GameEngine.SoundCenter.VolumeMusic = GameEngine.Config.VolumeMusic;
            GameEngine.SoundCenter.VolumeEffect = GameEngine.Config.VolumeEffect;
        }

        public void ApplyInput()
        {
            GameEngine.Config.Keys = new string[4];
            int nbMove = RessourceProvider.InputKeys.Count / GameEngine.Config.Keys.Length;
            for (int i = 0, j = 0; i < GameEngine.Config.Keys.Length; i++)
                for (int start = j; j < RessourceProvider.InputKeys.Count && j - start < nbMove; j++)
                {
                    GameEngine.Config.Keys[i] += RessourceProvider.InputKeys.ElementAt(j).Value.ToString();
                    if (j - start != nbMove - 1)
                        GameEngine.Config.Keys[i] += ",";
                }
        }
    }
}
