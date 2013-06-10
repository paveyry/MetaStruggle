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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Menus
{
    public class TestMenu
    {
        private readonly SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;
        private List<Character> sm;
        private Menu Menu;

        public TestMenu(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, bool osef = false)
        {
            _spriteBatch = spriteBatch;
            _graphics = graphics;
        }

        public Menu Create()
        {
            Menu characterSelector = new Menu(RessourceProvider.MenuBackgrounds["MainMenu"]);

            #region tests

//Dictionary<string,Texture2D> imageButtons = new Dictionary<string, Texture2D>();
            //Dictionary<string, float> t = new Dictionary<string, float>
            //    {
            //        {"field1", 50},
            //        {"field2",50}
            //    };

            //#region

            ////characterSelector.Add("ListCharacters", new ListImageButton(new Rectangle(20, 20, 60, 60), RessourceProvider.CharacterFaces, 5, RessourceProvider.Fonts["HUDlittle"], Color.White, Color.DarkOrange));
            ////characterSelector.Add("nop", new ListLine(t, new List<string[]>
            ////    {
            ////        new []{"0 Right.", "Right.0"}, 
            ////        new []{"0 Left.", "Left.0"},
            ////        new []{"0 Jump.","Jump.0"},
            ////        new []{"0 Attack.","Attack.0"},
            ////        new []{"0 SpecialAttack.","SpecialAttack.0"},
            ////        new []{"1 Right.", "Right.1"}, 
            ////        new []{"1 Left.", "Left.1"},
            ////        new []{"1 Jump.","Jump.1"},
            ////        new []{"1 Attack.","Attack.1"},
            ////        new []{"1 SpecialAttack.","SpecialAttack.1"},
            ////        new []{"2 Right.2", "Right.2"}, 
            ////        new []{"2 Left.2", "Left.2"},
            ////        new []{"2 Jump.2","Jump.2"},
            ////        new []{"2 Attack.2","Attack.2"},
            ////        new []{"2 SpecialAttack.2","SpecialAttack.2"},
            ////        new []{"3 Right.3", "Right.3"}, 
            ////        new []{"3 Left.3", "Left.3"},
            ////        new []{"3 Jump.3","Jump.3"},
            ////        new []{"3 Attack.3","Attack.3"},
            ////        new []{"3 SpecialAttack.3","SpecialAttack.3"},
            ////    },
            ////    new Rectangle(10, 10, 80, 50),"UglyTestTheme", RessourceProvider.Fonts["HUDlittle"], Color.White, Color.DarkOrange, false));

            //#endregion

            //List<PartialButton> partialButtons = new List<PartialButton> { 
            //    new PartialButton("play", () => (Menu.Items["t"] as ListButtons).IsDrawable = false),
            //    new PartialButton("option", () => GameEngine.DisplayStack.Push(new SettingsMenu(_spriteBatch,_graphics).MenuSettings())),
            //    new PartialButton("quit", () => GameEngine.DisplayStack.Pop())
            //};
            //characterSelector.Add("t", new ListButtons(new Vector2(50, 50), 20, partialButtons, 
            //    RessourceProvider.Fonts["Menu"], Color.White, Color.DarkOrange, ListButtons.StatusListButtons.Vertical));
            ////characterSelector.Add("o", new Textbox("", new Rectangle(10, 70, 500, 20), "UglyTestTheme", RessourceProvider.Fonts["Menu"], Color.White));
            ////characterSelector.Add("ok", new Button("OK", Item.PosOnScreen.DownRight, new Rectangle(20, 20, 50, 50), ButtonOk));
            ////characterSelector.Add("test", new MenuButton("play", new Vector2(50, 50), RessourceProvider.Fonts["Menu"], () => GameEngine.DisplayStack.Pop()));

            #endregion

            var t = new[]
                {new[] {"test1", "test", "test"}, new[] {"test2", "test", "test"}, new[] {"test3", "test", "test"},
                new[] {"test4", "test", "test"}, new[] {"test5", "test", "test"}, new[] {"test6", "test", "test"},
                new[] {"test7", "test", "test"}, new[] {"test8", "test", "test"}, new[] {"test9", "test", "test"}};

            characterSelector.Add("lol", new ClassicList(new Rectangle(20, 20, 50, 50), t, new[] { "1", "2", "3" }, new[] { 5, 10, 5 }, RessourceProvider.Fonts["Menu"], Color.White, Color.DarkOrange, "UglyTestTheme"));
            characterSelector.Add("e", new MenuButton("e", new Vector2(0, 0), RessourceProvider.Fonts["Menu"], Color.White, Color.DarkOrange, () => GameEngine.DisplayStack.Push(new SettingsMenu(_spriteBatch, _graphics).MenuSettings())));
            //characterSelector.Add("checkbox", new CheckBox(new Vector2(70,70), "UglyTestTheme",true));
            //characterSelector.Add("Slider", new Slider(new Rectangle(50, 50, 400, 10), 50, 0, 100, "UglyTestTheme"));
            Menu = characterSelector;
            return characterSelector;
        }

        public void ButtonOk()
        {
            System.Threading.Thread.Sleep(200);
            //var listImageButton = Menu.Items["ListCharacters"] as ListImageButton;
            //if (listImageButton != null)
            //    perso = listImageButton.NameSelected;
            //GameEngine.Config.ApplyInput();
            GameEngine.DisplayStack.Pop();
            //GameEngine.DisplayStack.Push(new ServerSelector(_spriteBatch, _graphics, perso).Create());
        }
    }
}
