using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using GameClient.Renderable.GUI;
using GameClient.Renderable.GUI.Items;
using GameClient.Renderable.GUI.Items.ListItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Menus
{
    class MainMenu
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly GraphicsDeviceManager _graphics;
        private Menu Menu { get; set; }

        public MainMenu(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            _spriteBatch = spriteBatch;
            _graphics = graphics;
        }

        public Menu CreateMainMenu()
        {
            var menu = new Menu(RessourceProvider.MenuBackgrounds["MainMenu"]);
            var buttons = new List<PartialButton>
                {
                    new PartialButton("MainMenu.SoloPlay", Play),
                    new PartialButton("MainMenu.Multi", () => GameEngine.DisplayStack.Push(new CharacterSelector(_spriteBatch,_graphics, true).Create())),
                    new PartialButton("MainMenu.Settings", () => GameEngine.DisplayStack.Push(new SettingsMenu(_spriteBatch,_graphics).MenuSettings())),
                    new PartialButton("MainMenu.Quit", () => Environment.Exit(0)),
                    new PartialButton("Test", () => GameEngine.DisplayStack.Push(new TestMenu(_spriteBatch,_graphics, true).Create())),
                };

            menu.Add("Buttons.Item", new ListButtons(new Vector2(50,44),20,buttons,RessourceProvider.Fonts["Menu"],
                Color.White,Color.DarkOrange,ListButtons.StatusListButtons.Vertical));
            Menu = menu;
            return menu;
        }

         public void Play()
        {
            if (GameEngine.SceneManager == null)
                GameEngine.SceneManager = Renderable.Environments.Environment1.GetScene(_spriteBatch);

            GameEngine.SoundCenter.PlayWithStatus("tardisbattlefield");
            GameEngine.DisplayStack.Push(GameEngine.SceneManager);
        }
    }
}
