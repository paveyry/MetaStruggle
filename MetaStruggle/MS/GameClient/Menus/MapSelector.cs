using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Characters;
using GameClient.Global;
using GameClient.Renderable.GUI;
using GameClient.Renderable.GUI.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Menus
{
    public class CharacterSelector
    {
        private readonly SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;
        private List<Character> sm;
        private Menu1 Menu;

        public CharacterSelector(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            _spriteBatch = spriteBatch;
            _graphics = graphics;
        }

        public Menu1 Create()
        {
            Menu1 mapSelector = new Menu1(RessourceProvider.MenuBackgrounds["MainMenu"]);
            int x = 50, y = 50;
            foreach (var character in RessourceProvider.CharacterFaces)
            {  
                mapSelector.Add(new ButtonSelector(character.Key,
                                                   new Rectangle(x, y, character.Value.Width/5,
                                                                 character.Value.Height/5), character.Value,
                                                   RessourceProvider.Fonts["Menu"], Color.White, Color.DarkOrange));
                x += character.Value.Width / 5 + 5;
            }

            mapSelector.Add(new Button(new Rectangle(400, 500, 50, 50), "OK", RessourceProvider.Fonts["Menu"], Color.White, Color.DarkOrange, ButtonOk));
            Menu = mapSelector;
            return mapSelector;
        }

        public void ButtonOk()
        {
            System.Threading.Thread.Sleep(200);
            string perso = "";
            foreach (ButtonSelector e in from ButtonSelector e in Menu.Items.FindAll(e => e is ButtonSelector) where e.IsSelect select e)
                perso = e.Text;
            GameEngine.DisplayStack.Push(new ServerSelector(_spriteBatch, _graphics, perso).Create());
        }
    }
}
