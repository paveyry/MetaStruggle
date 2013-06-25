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
        public bool AbstractRectangle { get; set; }
        #endregion

        public ImageButton(string text, Rectangle rectangle, Texture2D image, SpriteFont font, bool abstractRectangle = false, bool isDrawable = true)
            : base(rectangle,isDrawable)
        {
            Text = text;
            Image = image;
            Font = font;
            AbstractRectangle = abstractRectangle;
            TextSize = font.MeasureString(text);
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle rectangle = (AbstractRectangle) ? RealRectangle : ItemRectangle;
            if (IsSelect)
            {
                spriteBatch.Draw(Image, rectangle, Color.White);
                spriteBatch.DrawString(Font, Text, new Vector2(rectangle.X + (rectangle.Width - TextSize.X) / 2,
                    rectangle.Height + rectangle.Y), Color.White);
            }
            else
            {
                spriteBatch.Draw(Image, rectangle, new Color(55, 55, 55, 0));
                spriteBatch.DrawString(Font, Text, new Vector2(rectangle.X + (rectangle.Width - TextSize.X) / 2,
                    rectangle.Height + rectangle.Y), Color.White);
            }
        }

        public override void UpdateItem(GameTime gameTime)
        {
            Rectangle rectangle = (AbstractRectangle) ? RealRectangle : ItemRectangle;
            var mouse = new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1);
            if (IsSelect)
                return;
            if (GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                IsSelect = rectangle.Intersects(mouse);
            //UpdateItem(gameTime);
        }

    }
}
