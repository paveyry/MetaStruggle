using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using GameClient.Renderable.GUI;
using GameClient.Renderable.GUI.Items.ListItems;
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
                                  new PartialButton("MainMenu.Settings", () => GameEngine.DisplayStack.Push(new SettingsMenu().MenuSettings())),
                                  new PartialButton("Menu.Back", ReturnButton)
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
    }
}
