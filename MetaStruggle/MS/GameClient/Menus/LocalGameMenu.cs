using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Characters.AI;
using GameClient.Global;
using GameClient.Renderable.GUI;
using GameClient.Renderable.GUI.Items;
using GameClient.Renderable.GUI.Items.ListItems;
using Microsoft.Xna.Framework;
using GameClient.Renderable.Environments;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Menus
{
    class LocalGameMenu
    {
        Menu Menu { get; set; }
        private readonly SpriteBatch _spriteBatch;

        public LocalGameMenu(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        #region CharacterSelector
        public Menu Create()
        {
            System.Threading.Thread.Sleep(200);
            Menu = new Menu(RessourceProvider.MenuBackgrounds["SimpleMenu"]);
            
            Menu.Add("Text.NumberOfLives", new SimpleText("Text.Lives", new Vector2(60, 3), Item.PosOnScreen.TopLeft, RessourceProvider.Fonts["MenuLittle"], Color.White));
            Menu.Add("Slider.NumberOfLives", new Slider(new Rectangle(70, 3, 200, 20), 5, 1, 99, "MSTheme", RessourceProvider.Fonts["MenuLittle"]));

            Menu.Add("CharacterSelector.Text", new SimpleText("Text.SelectPlayer", new Vector2(5, 17),
                Item.PosOnScreen.TopLeft, RessourceProvider.Fonts["MenuLittle"], Color.White));
            for (int i = 0; i < 4; i++)
                CreateCharacterSelector(i);

            Menu.Add("ButtonsPlayer.Item", new ListButtons(new Vector2(10, 12), 0, new List<PartialButton>
                {
                    new PartialButton("Player.1",() => HideElementsPlayers(0)),
                    new PartialButton("Player.2",() => HideElementsPlayers(1)),
                    new PartialButton("Player.3",() => HideElementsPlayers(2)),
                    new PartialButton("Player.4",() => HideElementsPlayers(3)),
                }, RessourceProvider.Fonts["MenuLittle"], Color.White, Color.DarkOrange, ListButtons.StatusListButtons.Horizontal, false));

            Menu.Add("PlayerName.Text", new SimpleText("Text.TextboxPlayer", new Vector2(55, 73),
                Item.PosOnScreen.TopLeft, RessourceProvider.Fonts["MenuLittle"], Color.White));


            Menu.Add("Text.Multiple.ActivatePl", new SimpleText("Text.ActivatePl", new Vector2(55, 23),
                Item.PosOnScreen.TopLeft, RessourceProvider.Fonts["MenuLittle"], Color.White, false));
            Menu.Add("Text.Multiple.ActivateAI", new SimpleText("Text.ActivateAI", new Vector2(55, 33),
                Item.PosOnScreen.TopLeft, RessourceProvider.Fonts["MenuLittle"], Color.White, false));
            Menu.Add("Text.Multiple.SliderHandicap", new SimpleText("Text.SliderHandicap", new Vector2(55, 45),
                Item.PosOnScreen.TopLeft, RessourceProvider.Fonts["MenuLittle"], Color.White, false));
            Menu.Add("Text.Multiple.SliderLevel", new SimpleText("Text.SliderLevel", new Vector2(55, 55),
                Item.PosOnScreen.TopLeft, RessourceProvider.Fonts["MenuLittle"], Color.White, false));

            Menu.Add("NextButton.Item", new MenuButton("Menu.Next", new Vector2(70, 90), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange, NextButtonCharacterSelector));
            Menu.Add("ReturnButton.Item", new MenuButton("Menu.Back", new Vector2(15, 90), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange, MainMenu.Back));

            return Menu;
        }

        void CreateCharacterSelector(int nb)
        {
            bool status = (nb == 0);

            Menu.Add("Multiple.CharacterSelector.Item." + nb, new ListImageButtons(new Rectangle(5, 22, 45, 60), RessourceProvider.CharacterFaces, "MSTheme",
                RessourceProvider.Fonts["HUDlittle"], 5, status));
            Menu.Add("Multiple.Textbox.PlayerName." + nb, new Textbox(GameEngine.LangCenter.GetString("Player." + (nb + 1)), new Rectangle(55, 80, 300, 0), "MSTheme",
                RessourceProvider.Fonts["MenuLittle"], Color.White, status));

            if (status) return;

            Menu.Add("Multiple.Activate.Checkbox." + nb, new CheckBox(new Vector2(83, 23), "MSTheme", false, () => OnChangeChecboxActivate(nb), false));
            Menu.Add("Multiple.IA.Checkbox." + nb, new CheckBox(new Vector2(83, 31), "MSTheme", false, () => OnChangeCheckboxAI(nb), false));
            Menu.Add("Multiple.IA.Slider.Handicap." + nb, new Slider(new Rectangle(70, 50, 200, 20), 1, 1, 9, "MSTheme", RessourceProvider.Fonts["MenuLittle"], false));
            Menu.Add("Multiple.IA.Slider.Level." + nb, new Slider(new Rectangle(70, 60, 200, 20), 1, 1, 9, "MSTheme", RessourceProvider.Fonts["MenuLittle"], false));
        }

        void OnChangeChecboxActivate(int nb)
        {
            Menu.Items["Text.Multiple.ActivateAI"].IsDrawable =
            Menu.Items["Multiple.IA.Checkbox." + nb].IsDrawable =
                (Menu.Items["Multiple.Activate.Checkbox." + nb] as CheckBox).IsSelect;
            OnChangeCheckboxAI(nb);
        }

        void OnChangeCheckboxAI(int nb)
        {
            var iaCheckBox = (Menu.Items["Multiple.IA.Checkbox." + nb] as CheckBox);
            Menu.Items["Text.Multiple.SliderLevel"].IsDrawable =
            Menu.Items["Text.Multiple.SliderHandicap"].IsDrawable =
            Menu.Items["Multiple.IA.Slider.Handicap." + nb].IsDrawable =
            Menu.Items["Multiple.IA.Slider.Level." + nb].IsDrawable =
                iaCheckBox.IsSelect && iaCheckBox.IsDrawable;
        }

        private void HideElementsPlayers(int playerSelected)
        {
            foreach (var kvp in Menu.Items.Where(kvp => kvp.Key.StartsWith("Multiple.")))
                kvp.Value.IsDrawable = kvp.Key.EndsWith(playerSelected.ToString());
            if (playerSelected == 0)
            {
                Menu.Items["Text.Multiple.ActivatePl"].IsDrawable = false;
                Menu.Items["Text.Multiple.ActivateAI"].IsDrawable = false;
                Menu.Items["Text.Multiple.SliderHandicap"].IsDrawable = false;
                Menu.Items["Text.Multiple.SliderLevel"].IsDrawable = false;
                return;
            }
            Menu.Items["Text.Multiple.ActivatePl"].IsDrawable = true;
            OnChangeChecboxActivate(playerSelected);
            OnChangeCheckboxAI(playerSelected);
        }

        void NextButtonCharacterSelector()
        {
            var characters = new List<PartialAICharacter>();

            for (int i = 0; i < 4; i++)
                if (!AddCreateCharacter(i, characters))
                    return;

            if (characters.Count < 2)
                return;

            GameEngine.DisplayStack.Push(new LocalGameMenu(_spriteBatch).MapSelector(characters, ((Slider)Menu.Items["Slider.NumberOfLives"]).Value));
        }

        bool AddCreateCharacter(int nb, List<PartialAICharacter> characters)
        {
            if ((nb != 0) && !(Menu.Items["Multiple.Activate.Checkbox." + nb] as CheckBox).IsSelect)
                return true;

            string playerName = (Menu.Items["Multiple.Textbox.PlayerName." + nb] as Textbox).Text;
            string modelName = (Menu.Items["Multiple.CharacterSelector.Item." + nb] as ListImageButtons).NameSelected;

            if (playerName == "" || modelName == "")
                return false;

            bool isAI = (nb != 0) && (Menu.Items["Multiple.IA.Checkbox." + nb] as CheckBox).IsSelect;

            characters.Add((isAI) ?
                new PartialAICharacter(playerName, modelName,
                (byte)(Menu.Items["Multiple.IA.Slider.Handicap." + nb] as Slider).Value,
                (byte)(Menu.Items["Multiple.IA.Slider.Level." + nb] as Slider).Value)
                : new PartialAICharacter(playerName, modelName, (byte)nb));

            return true;
        }
        #endregion

        #region MapSelector
        Menu MapSelector(List<PartialAICharacter> characters, int nbLives)
        {
            System.Threading.Thread.Sleep(200);
            Menu = new Menu(RessourceProvider.MenuBackgrounds["SimpleMenu"]);

            Menu.Add("MapSelector.Text", new SimpleText("Text.SelectMap", new Vector2(15, 15),
                Item.PosOnScreen.TopLeft, RessourceProvider.Fonts["Menu"], Color.White));
            Menu.Add("MapSelector.Item", new ListImageButtons(new Rectangle(15, 22, 70, 45), RessourceProvider.MapScreens, "MSTheme",
                RessourceProvider.Fonts["HUDlittle"], 4));

            Menu.Add("NextButton.Item", new MenuButton("Menu.Next", new Vector2(70, 90), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange, () => NextButtonMapSelector(characters, nbLives)));
            Menu.Add("ReturnButton.Item", new MenuButton("Menu.Back", new Vector2(15, 90), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange, MainMenu.Back));

            return Menu;
        }

        void NextButtonMapSelector(IEnumerable<PartialAICharacter> characters, int nbLives)
        {
            string mapSelected = (Menu.Items["MapSelector.Item"] as ListImageButtons).NameSelected;

            if (string.IsNullOrEmpty(mapSelected))
                return;

            GameEngine.DisplayStack.Push(new LocalEnvironnement(characters, _spriteBatch, "Map" + mapSelected, nbLives).SceneManager);
        }
        #endregion

    }
}
