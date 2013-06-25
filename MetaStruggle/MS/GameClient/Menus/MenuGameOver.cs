using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Characters;
using GameClient.Global;
using GameClient.Renderable.GUI;
using GameClient.Renderable.GUI.Items;
using GameClient.Renderable.GUI.Items.ListItems;
using GameClient.Renderable.Layout;
using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework;

namespace GameClient.Menus
{
    class MenuGameOver
    {
        Menu Menu { get; set; }

        public Menu Create(Stack<Character> characters)
        {
            Menu = new Menu(RessourceProvider.MenuBackgrounds["SimpleMenu"],false);

            Menu.Add("ListOfPlayer.Items", new ClassicList(new Rectangle(20,40,60,40),CreateLines(characters),new Dictionary<string, int>
                {
                    {"Rang",20},
                    {"Jouers",80}
                },RessourceProvider.Fonts["MenuLittle"],Color.White,Color.White,"MSTheme" ));
            Menu.Add("WinnerHead.Item", new ImageButton(characters.Peek().PlayerName,new Rectangle(80,80,characters.Peek().Face.Width/3,characters.Peek().Face.Height/3),
                characters.Peek().Face,RessourceProvider.Fonts["MenuLittle"]));
            var buttons = new List<PartialButton>
                              {
                                  new PartialButton("Pause.ReturnMainMenu", ReturnMainMenu)
                              };
            Menu.Add("Buttons.Item", new ListButtons(new Vector2(85, 90), 20, buttons, RessourceProvider.Fonts["Menu"],
                Color.White, Color.White, ListButtons.StatusListButtons.Vertical));
            return Menu;

        }

        void ReturnMainMenu()
        {
            for (int i = 0; i < 5; i++)
            {
                GameEngine.DisplayStack.Pop();
            }
            System.Threading.Thread.Sleep(200);
        }

        IEnumerable<string[]> CreateLines(IEnumerable<Character> characters)
        {
            int i = 1;
            return characters.Select(character => new[] {"#" + i++, character.PlayerName}).ToList();
        }
    }
}
