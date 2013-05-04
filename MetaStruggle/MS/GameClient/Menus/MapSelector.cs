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
            Menu1 characterSelector = new Menu1(RessourceProvider.MenuBackgrounds["MainMenu"]);
            int x = 50, y = 50;
            Dictionary<string,Texture2D> imageButtons = new Dictionary<string, Texture2D>();

            characterSelector.Add("ListCharacters", new ListImageButton(new Rectangle(x, y, 1500, 1500), RessourceProvider.CharacterFaces, 5, RessourceProvider.Fonts["HUDlittle"], Color.White, Color.DarkOrange));

            characterSelector.Add("ok",new Button(new Rectangle(400, 450, 50, 50), "OK", RessourceProvider.Fonts["Menu"], Color.White, Color.DarkOrange, ButtonOk));
            Menu = characterSelector;
            return characterSelector;
        }

        public void ButtonOk()
        {
            System.Threading.Thread.Sleep(200);
            string perso = "";
            var listImageButton = Menu.Items["ListCharacters"] as ListImageButton;
            if (listImageButton != null)
                perso = listImageButton.Selected.Text;

            GameEngine.DisplayStack.Pop();
        }
    }
}
