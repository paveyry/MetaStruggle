using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using GameClient.Renderable.GUI;
using GameClient.Renderable.GUI.Items.ListItems;
using GameClient.Renderable.Layout;
using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Menus
{
    class PauseMenu
    {
        Menu Menu { get; set; }

        public Menu Create()
        {
            System.Threading.Thread.Sleep(200);
            Menu = new Menu(RessourceProvider.MenuBackgrounds["MainMenu"]);

            var buttons = new List<PartialButton>
                              {
                                  new PartialButton("Menu.Back", ReturnButton),
                                  new PartialButton("MainMenu.Settings", () => GameEngine.DisplayStack.Push(new SettingsMenu().MenuSettings())),
                                  new PartialButton("Pause.ReturnMainMenu", ReturnMainMenu)
                              };

            Menu.Add("Buttons.Item", new ListButtons(new Vector2(50, 44), 20, buttons, RessourceProvider.Fonts["Menu"],
                Color.White, Color.DarkOrange, ListButtons.StatusListButtons.Vertical));

            return Menu;
        }

        void ReturnButton()
        {
            GameEngine.DisplayStack.Pop();
            System.Threading.Thread.Sleep(200);
        }

        void ReturnMainMenu()
        {
            System.Threading.Thread.Sleep(200);
            var stack = GameEngine.DisplayStack.ToList();
            GameEngine.SoundCenter.Stop((stack[1] as SceneManager).MapName);
            GameEngine.DisplayStack = new LayoutStack<IBasicLayout>();
            GameEngine.DisplayStack.Push(stack.Last());
        }

    }
}
