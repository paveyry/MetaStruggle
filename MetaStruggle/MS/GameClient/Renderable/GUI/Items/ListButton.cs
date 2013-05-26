using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items
{
    public class ListMenuButton : Item
    {
        public enum StatusListButton
        {
            Horizontal, Vertical
        }
        private List<MenuButton> Buttons { get; set; }
        private int Interval { get; set; }

        public ListMenuButton(Vector2 pos, int interval,IEnumerable<PartialButton> partialButtons, SpriteFont font, Color colorNormal, Color colorSelected, StatusListButton statusMenu)
            : base(new Rectangle((int)pos.X, (int)pos.Y, 0, 0))
        {
            MenuButton.StatusMenuButton statusButtons;
            Enum.TryParse(statusMenu.ToString(), out statusButtons);
            Buttons = new List<MenuButton>();
            Interval = interval;
            
            Vector2 posButton = Position;
            foreach (var partialButton in partialButtons)
            {
                var button = new MenuButton(partialButton.ID, posButton, statusButtons, false, font, colorNormal, colorSelected, partialButton.OnClick);
                if (statusMenu == StatusListButton.Vertical)
                    posButton.Y += button.DimRectangles.Y + interval;
                else
                    posButton.X += button.DimRectangles.X + interval;
                Buttons.Add(button);
            }
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var menuButton in Buttons)
                menuButton.DrawItem(gameTime, spriteBatch);
            base.DrawItem(gameTime, spriteBatch);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            foreach (var menuButton in Buttons)
                menuButton.UpdateItem(gameTime);
            base.UpdateItem(gameTime);
        }

        protected override void UpdateResolution()
        {
            base.UpdateResolution();
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
