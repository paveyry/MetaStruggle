using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using GameClient.Renderable.GUI;
using GameClient.Renderable.GUI.Items;
using GameClient.Renderable.GUI.Items.ListItems;
using Microsoft.Xna.Framework;

namespace GameClient.Menus
{
    class LocalGame
    {
        Menu Menu { get; set; }

        #region CharacterSelector
        public Menu Create()
        {
            System.Threading.Thread.Sleep(200);
            Menu = new Menu(RessourceProvider.MenuBackgrounds["SimpleMenu"]);
            Menu.Add("CharacterSelector.Text", new SimpleText("Text.SelectPlayer", new Vector2(5, 17),
                Item.PosOnScreen.TopLeft, RessourceProvider.Fonts["MenuLittle"], Color.White));
            for (int i = 0; i < 4; i++)
                CreateCharacterSelector(i);

            Menu.Add("ButtonsPlayer.Item", new ListButtons(new Vector2(10, 12), 0, new List<PartialButton>
                {
                    new PartialButton("Controls.Pl1",() => HideElementsPlayers(0)),
                    new PartialButton("Controls.Pl2",() => HideElementsPlayers(1)),
                    new PartialButton("Controls.Pl3",() => HideElementsPlayers(2)),
                    new PartialButton("Controls.Pl4",() => HideElementsPlayers(3)),
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

            Menu.Add("NextButton.Item", new MenuButton("Menu.Next", new Vector2(70, 90), RessourceProvider.Fonts["MenuLittle"], Color.White,
                Color.DarkOrange, NextButton));

            return Menu;
        }

        void CreateCharacterSelector(int nb)
        {
            bool status = (nb == 0);
            
            Menu.Add("Multiple.CharacterSelector.Item." + nb, new ListImageButtons(new Rectangle(5, 22, 45, 60), RessourceProvider.CharacterFaces, "MSTheme",
                RessourceProvider.Fonts["HUDlittle"], 5, status));
            Menu.Add("Multiple.Textbox.PlayerName." + nb, new Textbox(status ? "" : "Player " + (nb + 1), new Rectangle(55, 80, 300, 0), "MSTheme",
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
        }

        void OnChangeCheckboxAI(int nb)
        {
            Menu.Items["Text.Multiple.SliderLevel"].IsDrawable =
            Menu.Items["Text.Multiple.SliderHandicap"].IsDrawable =
            Menu.Items["Multiple.IA.Slider.Handicap." + nb].IsDrawable =
            Menu.Items["Multiple.IA.Slider.Level." + nb].IsDrawable =
                (Menu.Items["Multiple.IA.Checkbox." + nb] as CheckBox).IsSelect;
        }

        private void HideElementsPlayers(int playerSelected)
        {
            foreach (var kvp in Menu.Items.Where(kvp => kvp.Key.StartsWith("Multiple.")))
                kvp.Value.IsDrawable = kvp.Key.EndsWith(playerSelected.ToString());
            Menu.Items["Text.Multiple.ActivatePl"].IsDrawable = (playerSelected != 0);

            if (playerSelected == 0) return;
            OnChangeChecboxActivate(playerSelected);
            OnChangeCheckboxAI(playerSelected);
        }

        void NextButton()
        {

        }
        #endregion
    }
}
