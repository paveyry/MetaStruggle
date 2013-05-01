using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using GameClient.Lang;
using GameClient.Renderable.GUI;
using GameClient.Renderable.GUI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Menus
{
    class MainMenu
    {
        private readonly SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;

        public MainMenu(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            _spriteBatch = spriteBatch;
            _graphics = graphics;
        }

        public Menu CreateMainMenu()
        {
            Menu1 test = new Menu1(RessourceProvider.MenuBackgrounds["MainMenu"]);
            test.Add(new Button(new Rectangle(0,0,4,4), "test", RessourceProvider.Fonts["Menu"],Color.White, Color.White,null ));
            var buttons = new List<MenuButton>
                {
                    new MenuButton("play", Play),
                    new MenuButton("option", () => GameEngine.DisplayStack.Push(new SettingsMenu(_spriteBatch,_graphics).MenuSettings())),
                    new MenuButton("test", () => GameEngine.DisplayStack.Push(test)),
                    new MenuButton("quit", () => Environment.Exit(0))
                };

            var main = new Menu("MainMenu", buttons, RessourceProvider.MenuBackgrounds["MainMenu"],
                                new Point((int)((GameEngine.Config.ResolutionWidth / 2) - RessourceProvider.Fonts["Menu"].MeasureString(buttons[0].DisplayedName).X / 2),
                                          (int)(GameEngine.Config.ResolutionHeight) / 2));

            return main;
        }

        void Play()
        {
            if (GameEngine.SceneManager == null)
                GameEngine.SceneManager = Renderable.Environments.Environment1.GetScene(_spriteBatch);

            GameEngine.SoundCenter.PlayWithStatus("music1");
            GameEngine.DisplayStack.Push(GameEngine.SceneManager);
        }
    }
}
