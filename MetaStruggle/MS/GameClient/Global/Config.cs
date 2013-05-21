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
        private int _tempWidth;
        private int _tempHeight;

        public string GameName { get; set; }
        public string Language { get; set; }
        public int ResolutionWidth
        {
            get { return (SettingsMenu.PrimaryWidth >= _tempWidth) ? _tempWidth : SettingsMenu.PrimaryWidth ; }
            set { _tempWidth = value; }
        }
        public int ResolutionHeight
        {
            get { return (SettingsMenu.PrimaryHeight >= _tempHeight) ? _tempHeight : SettingsMenu.PrimaryHeight ; }
            set { _tempHeight = value; }
        }
        public int VolumeMusic { get; set; }
        public int VolumeEffect { get; set; }
        public bool FullScreen { get; set; }
        public string Keys { get; set; }

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
                    Keys = "Z,Q,D,Space,LeftShift"
                };
        }

        public void ApplyConfig(GraphicsDeviceManager graphics)
        {
            GameEngine.SoundCenter.VolumeMusic = GameEngine.Config.VolumeMusic;
            GameEngine.SoundCenter.VolumeEffect = GameEngine.Config.VolumeEffect;
            graphics.PreferredBackBufferWidth = GameEngine.Config.ResolutionWidth;
            graphics.PreferredBackBufferHeight = GameEngine.Config.ResolutionHeight;
            graphics.IsFullScreen = GameEngine.Config.FullScreen;
            graphics.PreferMultiSampling = true;
            graphics.GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };
            graphics.ApplyChanges();
        }
    }
}
