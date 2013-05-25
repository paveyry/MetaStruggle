using System;
using System.Collections.Generic;
using System.Linq;
using GameClient.Global;
using GameClient.Language;
using GameClient.Renderable.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Menus
{
    class SettingsMenu
    {
        enum MenuType
        {
            Settings,
            Sounds,
            Graphics
        }
        private MenuType Id { get; set; }

        private readonly SpriteBatch _spriteBatch;
        private readonly GraphicsDeviceManager _graphics;

        public static int PrimaryHeight { get { return System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height; } }
        public static int PrimaryWidth { get { return System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width; } }

        private int _width = GameEngine.Config.ResolutionWidth;
        private int _height = GameEngine.Config.ResolutionHeight;

        private string GetFullscreen { get { return (GameEngine.Config.FullScreen) ? GameEngine.LangCenter.GetString("on") : GameEngine.LangCenter.GetString("off"); } }
        private string GetResolution { get { return _width + "x" + _height; } }

        public Dictionary<string, int[]> Resolution = new Dictionary<string, int[]>
            {
                {"800x600", new[] {800, 600}},
                {"1024x720", new[] {1024, 720}},
                {"1280x768", new[] {1280, 768}},
                {"1280x800", new[] {1280, 800}},
                {"1280x1024", new[] {1280, 1024}},
                {"1680x1050", new[] {1680, 1050}},
                {"1920x1080", new[] {1920, 1080}}
            };

        public SettingsMenu(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            _spriteBatch = spriteBatch;
            _graphics = graphics;
        }

        public Menu MenuSettings()
        {
            System.Threading.Thread.Sleep(200);
            var buttons = new List<MenuButton1>
                              {
                                  new MenuButton1("language", ChangeLanguage),
                                  new MenuButton1("graphics", () => GameEngine.DisplayStack.Push(MenuGraphics())),
                                  new MenuButton1("sounds", () => GameEngine.DisplayStack.Push(MenuSounds())),
                                  new MenuButton1("back", Return)
                              };

            Id = MenuType.Settings;
            return new Menu("Option", buttons, RessourceProvider.MenuBackgrounds["MainMenu"],
                                new Point((int)((GameEngine.Config.ResolutionWidth / 2) - RessourceProvider.Fonts["Menu"].MeasureString(buttons[0].DisplayedName).X / 2),
                                          (int)(GameEngine.Config.ResolutionHeight) / 2));
        }

        public Menu MenuGraphics()
        {
            System.Threading.Thread.Sleep(200);
            var buttons = new List<MenuButton1>
                              {
                                  new MenuButton1("fullscreen", GetFullscreen, SetFullScreen),
                                  new MenuButton1("resolution", GetResolution, ChangeResolution),
                                  new MenuButton1("back", Return)
                              };

            Id = MenuType.Graphics;
            return new Menu("Graphics", buttons, RessourceProvider.MenuBackgrounds["MainMenu"],
                                new Point((int)((GameEngine.Config.ResolutionWidth / 2) - RessourceProvider.Fonts["Menu"].MeasureString(buttons[0].DisplayedName).X / 2),
                                          (int)(GameEngine.Config.ResolutionHeight) / 2));
        }

        public Menu MenuSounds()
        {
            System.Threading.Thread.Sleep(200);
            var buttons = new List<MenuButton1>
                              {
                                  new MenuButton1("soundeffect", GetVolume(false), () => ChangeSound(false)),
                                  new MenuButton1("music", GetVolume(true), () => ChangeSound(true)),
                                  new MenuButton1("back", Return)
                              };

            Id = MenuType.Sounds;
            return new Menu("sound", buttons, RessourceProvider.MenuBackgrounds["MainMenu"],
                                new Point((int)((GameEngine.Config.ResolutionWidth / 2) - RessourceProvider.Fonts["Menu"].MeasureString(buttons[0].DisplayedName).X / 2),
                                          (int)(GameEngine.Config.ResolutionHeight) / 2));
        }

        #region NormalSettings
        void ChangeLanguage()
        {
            if (GameEngine.LangCenter.LanguageAvailable.Length == 0)
                return;
            System.Threading.Thread.Sleep(200);
            int index = 0;
            if (GameEngine.LangCenter.LanguageAvailable.Contains(GameEngine.Config.Language))
            {
                index = Array.FindIndex(GameEngine.LangCenter.LanguageAvailable, current => current == GameEngine.Config.Language);
                index++;
            }
            GameEngine.Config.Language = GameEngine.LangCenter.LanguageAvailable[index % GameEngine.LangCenter.LanguageAvailable.Length];
            ReactualizeOption();
        }
        #endregion

        #region GraphicSettings
        void ChangeResolution()
        {
            System.Threading.Thread.Sleep(100);

            AddSystemResolution();
            int index = 0;

            if (Resolution.ContainsKey(GetResolution))
            {
                index = Array.FindIndex(Resolution.Keys.ToArray(), current => current == GetResolution);
                index++;
            }
            int[] res = Resolution.ElementAt(index % Resolution.Count).Value;
            _width = res[0];
            _height = res[1];

            ReactualizeOption();
        }

        void AddSystemResolution()
        {
            string actualRes = PrimaryWidth + "x" + PrimaryHeight;

            if (Resolution.Last().Key == actualRes) return;

            int i = 0;
            while (i < Resolution.Count)
            {
                if (Resolution.ElementAt(i).Value[0] <= PrimaryWidth && Resolution.ElementAt(i).Value[1] <= PrimaryHeight)
                    i++;
                else
                    Resolution.Remove(Resolution.ElementAt(i).Key);
            }
            if (Resolution.Last().Key != actualRes)
                Resolution.Add(actualRes, new[] { PrimaryWidth, PrimaryHeight });
        }

        void SetFullScreen()
        {
            System.Threading.Thread.Sleep(200);
            GameEngine.Config.FullScreen = !GameEngine.Config.FullScreen;
            ReactualizeOption();
        }
        #endregion

        #region SoundSettings
        void ChangeSound(bool isMusic)
        {
            System.Threading.Thread.Sleep(100);
            if (isMusic)
                GameEngine.Config.VolumeMusic = ((GameEngine.Config.VolumeMusic / 10) * 10 + 10) % 110;
            else
                GameEngine.Config.VolumeEffect = ((GameEngine.Config.VolumeEffect / 10) * 10 + 10) % 110;
            ReactualizeOption();
        }

        string GetVolume(bool isMusic)
        {
            return ((isMusic) ? GameEngine.SoundCenter.VolumeMusic : GameEngine.SoundCenter.VolumeEffect).ToString();
        }
        #endregion

        void ReactualizeOption()
        {
            GameEngine.DisplayStack.Pop();
            switch (Id)
            {
                case MenuType.Graphics:
                    GameEngine.DisplayStack.Push(MenuGraphics());
                    break;
                case MenuType.Sounds:
                    GameEngine.DisplayStack.Push(MenuSounds());
                    break;
                default:
                    GameEngine.DisplayStack.Push(MenuSettings());
                    break;
            }
        }

        void Return()
        {
            System.Threading.Thread.Sleep(200);
            if (_graphics.PreferredBackBufferHeight != _height || _graphics.PreferredBackBufferWidth != _width ||
                _graphics.IsFullScreen != GameEngine.Config.FullScreen)
            {
                GameEngine.Config.ResolutionHeight = _height;
                GameEngine.Config.ResolutionWidth = _width;
                GameEngine.Config.ApplyConfig(_graphics);
            }
            GameEngine.SoundCenter.VolumeMusic = GameEngine.Config.VolumeMusic;
            GameEngine.SoundCenter.VolumeEffect = GameEngine.Config.VolumeEffect;

            GameEngine.DisplayStack.Pop();
            GameEngine.DisplayStack.Pop();
            GameEngine.DisplayStack.Push(Id == MenuType.Settings ? new MainMenu(_spriteBatch, _graphics).CreateMainMenu() : MenuSettings());
        }

    }
}
