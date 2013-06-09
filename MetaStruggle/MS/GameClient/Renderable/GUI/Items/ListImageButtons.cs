using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items
{
    public class ListImageButtons : Item
    {
        public string NameSelected { get { return (Selected == null) ? "" : Selected.Text; } }
        private List<ImageButton> ImageButtons { get; set; }
        private ImageButton Selected { get; set; }
        private Rectangle InternalRectangle;
        private Dictionary<string, Texture2D> Theme { get; set; }
        private readonly int _heightFont;
        
        public ListImageButtons(Rectangle rectangle, Dictionary<string, Texture2D> items, string themeName, SpriteFont font,
            Color colorNormal, Color colorSelected,int divCoef = 5, bool isDrawable = true)
            : base(rectangle, true, isDrawable)
        {
            ImageButtons = new List<ImageButton>();
            Theme = RessourceProvider.Themes[themeName];
            InternalRectangle = new Rectangle(RealRectangle.X + Theme["ListImageButtons.LeftSide"].Width, RealRectangle.Y + Theme["ListImageButtons.Top"].Height,
                RealRectangle.Width - (Theme["ListImageButtons.LeftSide"].Width + Theme["ListImageButtons.RightSide"].Width),
                RealRectangle.Height - (Theme["ListImageButtons.Top"].Height + Theme["ListImageButtons.Down"].Height));

            int x = InternalRectangle.X, y = InternalRectangle.Y;
            _heightFont = GetLineHeight(font) + 3;
            foreach (var item in items)
            {
                int width = item.Value.Width / divCoef;
                int height = item.Value.Height / divCoef;
                ImageButtons.Add(new ImageButton(item.Key,
                    new Rectangle(x, y, width, height), item.Value, font, colorNormal, colorSelected));
                x += width + 5;
                if (x + width <= Width && x + width <= InternalRectangle.X + InternalRectangle.Width)
                    continue;
                InternalRectangle.Width = x - InternalRectangle.X;
                RealRectangle.Width = InternalRectangle.Width + Theme["ListImageButtons.LeftSide"].Width +
                                      Theme["ListImageButtons.RightSide"].Width;
                x = InternalRectangle.X;
                y += height + _heightFont;
            }
        }

        internal override void UpdateResolution()
        {
            base.UpdateResolution();
            InternalRectangle = new Rectangle(RealRectangle.X + Theme["ListImageButtons.LeftSide"].Width, RealRectangle.Y + Theme["ListImageButtons.Top"].Height,
                RealRectangle.Width - (Theme["ListImageButtons.LeftSide"].Width + Theme["ListImageButtons.RightSide"].Width),
                RealRectangle.Height - (Theme["ListImageButtons.Top"].Height + Theme["ListImageButtons.Down"].Height));
            int x = ItemRectangle.X, y = ItemRectangle.Y;

            foreach (var item in ImageButtons)
            {
                item.ItemRectangle.Location = new Point(x, y);
                x += item.ItemRectangle.Width + 5;
                if (x + item.ItemRectangle.Width <= Width && x + item.ItemRectangle.Width <= RealRectangle.Width)
                    continue;
                x = ItemRectangle.X;
                y += item.ItemRectangle.Height + _heightFont;
            }
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsDrawable)
                return;
            spriteBatch.Draw(Theme["ListImageButtons.Top"], new Rectangle(RealRectangle.X, RealRectangle.Y, RealRectangle.Width,
                Theme["ListImageButtons.Top"].Height), Color.White);
            spriteBatch.Draw(Theme["ListImageButtons.Down"], new Rectangle(RealRectangle.X, InternalRectangle.Y + InternalRectangle.Height,
                RealRectangle.Width, Theme["ListImageButtons.Down"].Height), Color.White);
            spriteBatch.Draw(Theme["ListImageButtons.LeftSide"], new Rectangle(RealRectangle.X, InternalRectangle.Y,
                Theme["ListImageButtons.LeftSide"].Width, InternalRectangle.Height), Color.White);
            spriteBatch.Draw(Theme["ListImageButtons.RightSide"], new Rectangle(InternalRectangle.X + InternalRectangle.Width,
                InternalRectangle.Y, Theme["ListImageButtons.RightSide"].Width, InternalRectangle.Height), Color.White);
            spriteBatch.Draw(Theme["ListImageButtons.Background"], InternalRectangle, Color.White);
            foreach (var imageButton in ImageButtons)
                imageButton.DrawItem(gameTime, spriteBatch);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            base.UpdateItem(gameTime);
            if (!IsDrawable)
                return;

            foreach (var imageButton in ImageButtons)
            {
                imageButton.UpdateItem(gameTime);
                if (!imageButton.IsSelect)
                    continue;
                if (Selected != null && !Selected.Equals(imageButton))
                    Selected.IsSelect = false;
                Selected = imageButton;
            }
        }
    }
}
