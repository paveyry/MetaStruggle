using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Items
{
    internal class ImageButton : Item
    {
        #region Fields
        public string Text { get; set; }
        SpriteFont Font { get; set; }
        Texture2D Image { get; set; }
        Vector2 TextSize { get; set; }
        public bool IsSelect { get; set; }
        #endregion

        public ImageButton(string text, Rectangle rectangle, Texture2D image, SpriteFont font)
            : base(rectangle)
        {
            Text = text;
            Image = image;
            Font = font;
            TextSize = font.MeasureString(text);
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsSelect)
            {
                spriteBatch.Draw(Image, ItemRectangle, Color.White);
                spriteBatch.DrawString(Font, Text, new Vector2(ItemRectangle.X + (ItemRectangle.Width - TextSize.X) / 2,
                    ItemRectangle.Height + ItemRectangle.Y), Color.White);
            }
            else
            {
                spriteBatch.Draw(Image, ItemRectangle, new Color(55, 55, 55, 0));
                spriteBatch.DrawString(Font, Text, new Vector2(ItemRectangle.X + (ItemRectangle.Width - TextSize.X) / 2,
                    ItemRectangle.Height + ItemRectangle.Y), Color.White);
            }
        }

        public override void UpdateItem(GameTime gameTime)
        {
            var mouse = new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1);
            if (IsSelect)
                return;
            if (GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                IsSelect = ItemRectangle.Intersects(mouse);
        }

    }
}
