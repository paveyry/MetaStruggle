using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items.ListItems
{
    public class ListButtons : Item
    {
        public enum StatusListButtons
        {
            Horizontal, Vertical
        }
        public List<MenuButton> Buttons { get; set; }
        private MenuButton ButtonSelected { get; set; }
        private int Interval { get; set; }
        private StatusListButtons Status { get; set; }
        private bool NormalSelect { get; set; }

        public ListButtons(Vector2 pos, int interval, IEnumerable<PartialButton> partialButtons, SpriteFont font,
            Color colorNormal, Color colorSelected, StatusListButtons statusMenu, bool normalSelect = true, bool isDrawable = true)
            : base(new Rectangle((int)pos.X, (int)pos.Y, 0, 0), isDrawable)
        {
            MenuButton.StatusMenuButton statusButtons;
            Enum.TryParse(statusMenu.ToString(), out statusButtons);

            Interval = interval;
            Status = statusMenu;
            NormalSelect = normalSelect;

            Buttons = new List<MenuButton>();
            foreach (var partialButton in partialButtons)
                Buttons.Add(new MenuButton(partialButton.ID, new Vector2(0, 0), statusButtons, false, font, colorNormal,
                                           colorSelected, partialButton.OnClick, normalSelect));
            if (!NormalSelect)
            {
                ButtonSelected = Buttons[0];
                ButtonSelected.IsSelect = true;
            }
            UpdateResolution();
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsDrawable)
                return;
            foreach (var menuButton in Buttons)
                menuButton.DrawItem(gameTime, spriteBatch);
            base.DrawItem(gameTime, spriteBatch);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            base.UpdateItem(gameTime);
            if (!IsDrawable)
                return;
            if (NormalSelect)
                foreach (var menuButton in Buttons)
                    menuButton.UpdateItem(gameTime);
            else
            {
                foreach (var menuButton in Buttons)
                {
                    menuButton.UpdateItem(gameTime);
                    if (!menuButton.IsSelect || menuButton.Equals(ButtonSelected)) continue;
                    ButtonSelected.IsSelect = false;
                    ButtonSelected = menuButton;
                }
            }
        }

        public void UpdateRectangles()
        {
            foreach (var menuButton in Buttons)
                menuButton.UpdateRectangles();
        }

        internal override void UpdateResolution()
        {
            base.UpdateResolution();
            Vector2 posButton = Position;
            foreach (var menuButton in Buttons)
            {
                menuButton.MiddlePos = posButton;
                menuButton.UpdateRectangles();
                if (Status == StatusListButtons.Vertical)
                    posButton.Y += menuButton.DimRectangles.Y + Interval;
                else
                    posButton.X += menuButton.DimRectangles.X + Interval;
            }
        }
    }

    public class PartialButton
    {
        public string ID { get; set; }
        public Item.Event OnClick { get; set; }

        public PartialButton(string id, Item.Event onClick)
        {
            ID = id;
            OnClick = onClick;
        }
    }
}
