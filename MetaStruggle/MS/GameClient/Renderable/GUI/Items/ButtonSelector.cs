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
    public class ButtonSelector : Item
    {
        public string Text { get; set; }
        SpriteFont Font { get; set; }
        Texture2D Image { get; set; }
        Color ColorNormal { get; set; }
        Color ColorSelected { get; set; }
        bool isSelect { get; set; }

        public ButtonSelector(string text, Rectangle rectangle,Texture2D image, SpriteFont font, Color colorNormal, Color colorSelected) : base(rectangle)
        {
            Text = text;
            Image = image;
            Font = font;
            ElementRectangle = rectangle;
            ColorNormal = colorNormal;
            ColorSelected = colorSelected;
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, ElementRectangle,(isSelect)?ColorSelected :ColorNormal);
            spriteBatch.DrawString(Font,Text,new Vector2(ElementRectangle.X, ElementRectangle.Y + 50), (isSelect)?ColorSelected:ColorNormal  );
        }

        public override void UpdateItem(GameTime gameTime)
        {
            var mouse = new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1);
            if (GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                isSelect = ElementRectangle.Intersects(mouse);
        }
    }
}
