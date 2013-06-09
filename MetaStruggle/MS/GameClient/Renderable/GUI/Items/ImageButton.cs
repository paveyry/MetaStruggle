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
        Color ColorNormal { get; set; }
        Color ColorSelected { get; set; }
        public bool IsSelect { get; set; }
        #endregion

        public ImageButton(string text, Rectangle rectangle, Texture2D image, SpriteFont font, Color colorNormal, Color colorSelected)
            : base(rectangle)
        {
            Text = text;
            Image = image;
            Font = font;
            ColorNormal = colorNormal;
            ColorSelected = colorSelected;
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsSelect)
            {
                spriteBatch.Draw(Image, ItemRectangle, ColorSelected);
                spriteBatch.DrawString(Font, Text, new Vector2(ItemRectangle.X, ItemRectangle.Height + ItemRectangle.Y), ColorSelected);
            }
            else
            {
                spriteBatch.Draw(Image, ItemRectangle, ColorNormal);
                spriteBatch.DrawString(Font, Text, new Vector2(ItemRectangle.X, ItemRectangle.Height + ItemRectangle.Y), ColorNormal);
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
