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
                    {"MenuGameOver.Rank",20},
                    {"MenuGameOver.Players",80}
                },RessourceProvider.Fonts["MenuLittle"],Color.White,Color.White,"MSTheme" ));
            Menu.Add("WinnerHead.Item", new ImageButton(characters.Peek().PlayerName,new Rectangle(40,10,characters.Peek().Face.Width/3,characters.Peek().Face.Height/3),
                characters.Peek().Face,RessourceProvider.Fonts["MenuLittle"],true));
            (Menu.Items["WinnerHead.Item"] as ImageButton).IsSelect = true;
            Menu.Add("NextButton.Item", new MenuButton("Menu.ReturnMainMenu", new Vector2(70, 90), RessourceProvider.Fonts["MenuLittle"], Color.White,
                Color.DarkOrange, ReturnMainMenu));
            return Menu;
        }

        void ReturnMainMenu()
        {
            System.Threading.Thread.Sleep(200);
            var stack = GameEngine.DisplayStack.ToList();
            (stack[1] as SceneManager).ResetAll();
            GameEngine.DisplayStack = new LayoutStack<IBasicLayout>();
            GameEngine.DisplayStack.Push(stack.Last());
        }

        IEnumerable<string[]> CreateLines(IEnumerable<Character> characters)
        {
            int i = 1;
            return characters.Select(character => new[] {"#" + i++, character.PlayerName}).ToList();
        }
    }
}
