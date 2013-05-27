using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items
{
    public class ListButtons : Item
    {
        public enum StatusListButtons
        {
            Horizontal, Vertical
        }
        private List<MenuButton> Buttons { get; set; }
        private int Interval { get; set; }
        private StatusListButtons Status { get; set; }

        public ListButtons(Vector2 pos, int interval,IEnumerable<PartialButton> partialButtons, SpriteFont font, Color colorNormal, Color colorSelected, StatusListButtons statusMenu, bool isDrawable = true)
            : base(new Rectangle((int)pos.X, (int)pos.Y, 0, 0), isDrawable)
        {
            MenuButton.StatusMenuButton statusButtons;
            Enum.TryParse(statusMenu.ToString(), out statusButtons);
            Buttons = new List<MenuButton>();
            Interval = interval;
            Status = statusMenu;

            Vector2 posButton = Position;
            foreach (var partialButton in partialButtons)
            {
                var button = new MenuButton(partialButton.ID, posButton, statusButtons, false, font, colorNormal, colorSelected, partialButton.OnClick);
                if (statusMenu == StatusListButtons.Vertical)
                    posButton.Y += button.DimRectangles.Y + interval;
                else
                    posButton.X += button.DimRectangles.X + interval;
                Buttons.Add(button);
            }
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
            if (!IsDrawable)
                return;
            foreach (var menuButton in Buttons)
                menuButton.UpdateItem(gameTime);
            base.UpdateItem(gameTime);
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
