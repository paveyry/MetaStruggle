using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Characters;
using GameClient.Global;
using GameClient.Renderable.GUI;
using GameClient.Renderable.GUI.Items;
using GameClient.Renderable.GUI.Items.ListItems;
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
                },RessourceProvider.Fonts["MenuLittle"],Color.DarkOrange,Color.DarkOrange,"MSTheme" ));
            Menu.Add("WinnerHead.Item", new ImageButton(characters.Peek().PlayerName,new Rectangle(40,10,characters.Peek().Face.Width/3,characters.Peek().Face.Height/3),
                characters.Peek().Face,RessourceProvider.Fonts["MenuLittle"]));

            return Menu;
        }

        IEnumerable<string[]> CreateLines(IEnumerable<Character> characters)
        {
            int i = 1;
            return characters.Select(character => new[] {"#" + i++, character.PlayerName}).ToList();
        }
    }
}
