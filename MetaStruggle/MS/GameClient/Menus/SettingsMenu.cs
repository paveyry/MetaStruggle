using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using GameClient.Global.InputManager;
using GameClient.Renderable.GUI;
using GameClient.Renderable.GUI.Items;
using GameClient.Renderable.GUI.Items.ListItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Menus
{
    class SettingsMenu
    {
        Menu Menu { get; set; }
        private SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;
        
        public SettingsMenu(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            _spriteBatch = spriteBatch;
            _graphics = graphics;
        }
        
        #region Settings
        public Menu MenuSettings()
        {
            System.Threading.Thread.Sleep(200);
            Menu = new Menu(RessourceProvider.MenuBackgrounds["MainMenu"]);
            var buttons = new List<PartialButton>
                              {
                                  new PartialButton("MenuSettings.Language", ChangeLanguage),
                                  new PartialButton("MenuSettings.Controls", () => GameEngine.DisplayStack.Push(MenuControls())),
                                  new PartialButton("MenuSettings.Graphics", () => GameEngine.DisplayStack.Push(MenuGraphics())),
                                  new PartialButton("MenuSettings.Sounds", () => GameEngine.DisplayStack.Push(MenuSounds())),
                                  new PartialButton("Menu.Back", ReturnButton)
                              };

            Menu.Add("Buttons.Item", new ListButtons(new Vector2(50, 44), 20, buttons, RessourceProvider.Fonts["Menu"],
                Color.White, Color.DarkOrange, ListButtons.StatusListButtons.Vertical));
            return Menu;
        }

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

            foreach (var menu in GameEngine.DisplayStack.OfType<Menu>())
            {
                foreach (var listButton in menu.Items.Values.OfType<ListButtons>())
                    listButton.UpdateRectangles();
                foreach (var menubutton in menu.Items.Values.OfType<MenuButton>())
                    menubutton.UpdateRectangles();
            }
        }
        #endregion

        #region Graphics
        public Menu MenuGraphics()
        {
            System.Threading.Thread.Sleep(200);
            Menu = new Menu(RessourceProvider.MenuBackgrounds["SimpleMenu"]);
            Menu.Add("Graphics.Text.Fullscreen", new SimpleText("MenuGraphics.Fullscreen", new Vector2(20, 20), Item.PosOnScreen.TopLeft,
                RessourceProvider.Fonts["Menu"], Color.White));
            Menu.Add("Graphics.Checkbox.Fullscreen", new CheckBox(new Vector2(72, 20), "UglyTestTheme", GameEngine.Config.FullScreen));
            Menu.Add("Graphics.ClassicList.Resolution", new ClassicList(new Rectangle(20, 40, 60, 30), CreateResolutions(),
                new Dictionary<string, int> { { "MenuGraphics.Resolution", 100 } }, RessourceProvider.Fonts["HUDlittle"], Color.White,
                Color.DarkOrange, "UglyTestTheme"));
            Menu.Add("ApplyButton.Item", new MenuButton("MenuSettings.Apply", new Vector2(70, 80), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange, ApplyButtonGraphics));
            Menu.Add("ReturnButton.Item", new MenuButton("Menu.Back", new Vector2(10, 80), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange, ReturnButton));

            (Menu.Items["Graphics.ClassicList.Resolution"] as ClassicList).SetSelectedLine(new[]
                {
                    GameEngine.Config.ResolutionWidth + "x" + GameEngine.Config.ResolutionHeight
                });

            return Menu;
        }

        private List<string[]> CreateResolutions()
        {
            var value = new List<string[]>();
            var resolution = new List<string>()
                {
                    "800x600",
                    "1024x720",
                    "1280x768",
                    "1280x800",
                    "1280x1024",
                    "1680x1050",
                    "1920x1080"
                };
            foreach (var str in resolution)
            {
                var resStr = str.Split('x');
                var res = new[] { int.Parse(resStr[0]), int.Parse(resStr[1]) };
                if (GameEngine.PrimaryWidthOfWindows >= res[0] && GameEngine.PrimaryHeightOfWindows >= res[1])
                    value.Add(new[] { str });
            }
            if (!resolution.Contains(GameEngine.PrimaryWidthOfWindows + "x" + GameEngine.PrimaryHeightOfWindows))
                value.Add(new[] { GameEngine.PrimaryWidthOfWindows + "x" + GameEngine.PrimaryHeightOfWindows });
            return value;
        }

        void ApplyButtonGraphics()
        {
            var resSelected = (Menu.Items["Graphics.ClassicList.Resolution"] as ClassicList).Selected;
            if (resSelected != null)
            {
                var res = resSelected[0].Split('x');
                GameEngine.Config.ResolutionWidth = int.Parse(res[0]);
                GameEngine.Config.ResolutionHeight = int.Parse(res[1]);
            }
            GameEngine.Config.FullScreen = (Menu.Items["Graphics.Checkbox.Fullscreen"] as CheckBox).IsSelect;
            GameEngine.Config.ApplyGraphics(_graphics);
            foreach (Menu menu in GameEngine.DisplayStack.OfType<Menu>())
                menu.UpdateResolution();
        }
        #endregion

        #region Sounds
        public Menu MenuSounds()
        {
            System.Threading.Thread.Sleep(200);
            Menu = new Menu(RessourceProvider.MenuBackgrounds["SimpleMenu"]);

            Menu.Add("Sounds.Text.Musics", new SimpleText("MenuSounds.Musics", new Vector2(10, 20), Item.PosOnScreen.TopLeft,
                RessourceProvider.Fonts["Menu"], Color.White));
            Menu.Add("Sounds.Item.Musics", new Slider(new Rectangle(60, 21, 280, 20),
                GameEngine.Config.VolumeMusic, "UglyTestTheme", RessourceProvider.Fonts["HUDlittle"]));
            Menu.Add("Sounds.Text.Effects", new SimpleText("MenuSounds.Effects", new Vector2(10, 40), Item.PosOnScreen.TopLeft,
                RessourceProvider.Fonts["Menu"], Color.White));
            Menu.Add("Sounds.Item.Effects", new Slider(new Rectangle(60, 41, 280, 20),
                GameEngine.Config.VolumeEffect, "UglyTestTheme", RessourceProvider.Fonts["HUDlittle"]));
            Menu.Add("ApplyButton.Item", new MenuButton("MenuSettings.Apply", new Vector2(70, 80), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange, ApplyButtonSounds));
            Menu.Add("ReturnButton.Item", new MenuButton("Menu.Back", new Vector2(10, 80), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange, ReturnButton));
            return Menu;
        }

        void ApplyButtonSounds()
        {
            GameEngine.DisplayStack.Push(MenuGraphics());
            return;
            GameEngine.Config.VolumeMusic = (Menu.Items["Sounds.Item.Musics"] as Slider).Value;
            GameEngine.Config.VolumeEffect = (Menu.Items["Sounds.Item.Effects"] as Slider).Value;
            GameEngine.Config.ApplySound();
        }
        #endregion

        #region Controls
        public Menu MenuControls()
        {
            System.Threading.Thread.Sleep(200);
            Menu = new Menu(RessourceProvider.MenuBackgrounds["SimpleMenu"]);

            var keys = RessourceProvider.InputKeys.ToDictionary(e => e.Key, e => e.Value);

            Menu.Add("Controls.ButtonsPlayer.Item", new ListButtons(new Vector2(10, 20), 0, new List<PartialButton>
                {
                    new PartialButton("Controls.Pl1",() => HideKeySelectorsPlayers(0)),
                    new PartialButton("Controls.Pl2",() => HideKeySelectorsPlayers(1)),
                    new PartialButton("Controls.Pl3",() => HideKeySelectorsPlayers(2)),
                    new PartialButton("Controls.Pl4",() => HideKeySelectorsPlayers(3)),
                }, RessourceProvider.Fonts["Menu"], Color.White, Color.DarkOrange, ListButtons.StatusListButtons.Horizontal, false));

            for (int i = 0; i < 4; i++)
                CreateKeySelectorList(i,keys);

            Menu.Add("ApplyButton.Item", new MenuButton("MenuSettings.Apply", new Vector2(70, 80), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange,() => ApplyButtonControls(keys)));
            Menu.Add("ReturnButton.Item", new MenuButton("Menu.Back", new Vector2(10, 80), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange, ReturnButton));

            return Menu;
        }

        void CreateKeySelectorList(int player,Dictionary<string,UniversalKeys> keys)
        {
            Menu.Add("Controls.KeySelector.Pl" + player,
                new KeySelectorList(new Rectangle(10, 30, 80, 50),
                    (from object movement in Enum.GetValues(typeof(Characters.Movement))
                     select movement.ToString() into movementStr
                     select new[] { "Controls." + movementStr, movementStr + "." + player }),keys, player,
                    new Dictionary<string, int> { { "Controls.Movements", 50 }, { "Controls.Keys", 50 } },
                    RessourceProvider.Fonts["HUD"], Color.White, Color.DarkOrange, "UglyTestTheme", player == 0));
        }

        void HideKeySelectorsPlayers(int playerSelected)
        {
            foreach (var kvp in Menu.Items.Where(kvp => kvp.Key.StartsWith("Controls.KeySelector.Pl")))
                kvp.Value.IsDrawable = kvp.Key.EndsWith(playerSelected.ToString());
        }

        void ApplyButtonControls(Dictionary<string,UniversalKeys> keys)
        {
            RessourceProvider.InputKeys = keys;
            GameEngine.Config.ApplyInput();
        }
        #endregion

        void ReturnButton()
        {
            GameEngine.DisplayStack.Pop();
            System.Threading.Thread.Sleep(200);
        }
    }
}
