using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items
{
    class SimpleText : Item
    {
        public string Text;
        public SpriteFont Font;
        public Color ColorNormal;

        public SimpleText(string text, Point position, PosOnScreen pos,SpriteFont font, Color colorNormal) : base(CreateRectangle(position,font,text),pos)
        {
            Text = text;
            Font = font;
            ColorNormal = colorNormal;
        }

        private static Rectangle CreateRectangle(Point position, SpriteFont font, string text)
        {
            var value = font.MeasureString(text);
            return new Rectangle(position.X,position.Y,(int)value.X+1,(int)value.Y+1);
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font,Text,Position,ColorNormal);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            base.UpdateItem(gameTime);
        } 
    }
}
