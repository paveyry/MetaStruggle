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
            int x = 50, y = 50;
            //Dictionary<string,Texture2D> imageButtons = new Dictionary<string, Texture2D>();
            Dictionary<string, float> t = new Dictionary<string, float>
                {
                    {"field1", 50},
                    {"field2",50}
                };

            //characterSelector.Add("ListCharacters", new ListImageButton(new Rectangle(20, 20, 60, 60), RessourceProvider.CharacterFaces, 5, RessourceProvider.Fonts["HUDlittle"], Color.White, Color.DarkOrange));
            characterSelector.Add("nop", new ListLine(t, new List<string[]>
                {
                    new []{"1azertyuiopqsdfghjkitemwxcvbnazertyuiopoiuytrsdfghjk1", "1item2"}, 
                    new []{"2item1", "2item2"},
                    new []{"3item","item"},
                    new []{"4item","item"},
                    new []{"5item","item"},
                    new []{"6item","item"},
                    new []{"7item","item"},
                    new []{"8item","item"},
                    new []{"9item","item"},
                    new []{"10item","item"},
                    new []{"11item","item"},
                    new []{"12item","item"},
                    new []{"13item","item"},
                    new []{"14item","item"},
                    new []{"15item","item"},
                    new []{"16item","item"},
                    new []{"17item","item"},
                    new []{"18item","item"},
                    new []{"19item","item"},
                    new []{"20item","item"},
                    new []{"21item","item"},
                },
                new Rectangle(10, 10, 80, 50),"UglyTestTheme", RessourceProvider.Fonts["HUDlittle"], Color.White, Color.DarkOrange));
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
            GameEngine.DisplayStack.Pop();
            //GameEngine.DisplayStack.Push(new ServerSelector(_spriteBatch, _graphics, perso).Create());
        }
    }
}
