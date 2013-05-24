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
using Microsoft.Xna.Framework.Input;

namespace GameClient.Menus
{
    public class TestMenu
    {
        private readonly SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;
        private List<Character> sm;
        private Menu1 Menu;

        public TestMenu(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, bool osef = false)
        {
            _spriteBatch = spriteBatch;
            _graphics = graphics;
        }

        public Menu1 Create()
        {
            Menu1 characterSelector = new Menu1(RessourceProvider.MenuBackgrounds["MainMenu"]);
            //Dictionary<string,Texture2D> imageButtons = new Dictionary<string, Texture2D>();
            Dictionary<string, float> t = new Dictionary<string, float>
                {
                    {"field1", 50},
                    {"field2",50}
                };

            //characterSelector.Add("ListCharacters", new ListImageButton(new Rectangle(20, 20, 60, 60), RessourceProvider.CharacterFaces, 5, RessourceProvider.Fonts["HUDlittle"], Color.White, Color.DarkOrange));
            characterSelector.Add("nop", new ListLine(t, new List<string[]>
                {
                    new []{"0 Right.", "Right.0"}, 
                    new []{"0 Left.", "Left.0"},
                    new []{"0 Jump.","Jump.0"},
                    new []{"0 Attack.","Attack.0"},
                    new []{"0 SpecialAttack.","SpecialAttack.0"},
                    new []{"1 Right.", "Right.1"}, 
                    new []{"1 Left.", "Left.1"},
                    new []{"1 Jump.","Jump.1"},
                    new []{"1 Attack.","Attack.1"},
                    new []{"1 SpecialAttack.","SpecialAttack.1"},
                    new []{"2 Right.2", "Right.2"}, 
                    new []{"2 Left.2", "Left.2"},
                    new []{"2 Jump.2","Jump.2"},
                    new []{"2 Attack.2","Attack.2"},
                    new []{"2 SpecialAttack.2","SpecialAttack.2"},
                    new []{"3 Right.3", "Right.3"}, 
                    new []{"3 Left.3", "Left.3"},
                    new []{"3 Jump.3","Jump.3"},
                    new []{"3 Attack.3","Attack.3"},
                    new []{"3 SpecialAttack.3","SpecialAttack.3"},
                },
                new Rectangle(10, 10, 80, 50),"UglyTestTheme", RessourceProvider.Fonts["HUDlittle"], Color.White, Color.DarkOrange, false));
            characterSelector.Add("o", new Textbox("",new Rectangle(10,70,500,20), "UglyTestTheme",RessourceProvider.Fonts["Menu"],Color.White ));
            characterSelector.Add("ok", new Button("OK", Item.PosOnScreen.DownRight, new Rectangle(20, 20, 50, 50), ButtonOk));
            Menu = characterSelector;
            return characterSelector;
        }

        public void ButtonOk()
        {
            System.Threading.Thread.Sleep(200);
            //var listImageButton = Menu.Items["ListCharacters"] as ListImageButton;
            //if (listImageButton != null)
            //    perso = listImageButton.NameSelected;
            GameEngine.Config.ApplyInput();
            GameEngine.DisplayStack.Pop();
            //GameEngine.DisplayStack.Push(new ServerSelector(_spriteBatch, _graphics, perso).Create());
        }
    }
}
