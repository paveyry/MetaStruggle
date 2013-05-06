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
    class SimpleText : Item
    {
        public string Text { get; set; }
        public bool IsSelect { get; set; }
        public bool UseItemRectangle { get; set; }
        private SpriteFont Font { get; set; }
        private Color ColorNormal { get; set; }
        private Color ColorSelected { get; set; }

        //temp fix for ListTexts (TO DO)
        internal SimpleText(string text, Point position, PosOnScreen pos, SpriteFont font, Color colorNormal, Color colorSelected, bool useItemRectangle)
            : base(CreateRectangle(position, font, text), pos)
        {
            Text = text;
            Font = font;
            ColorNormal = colorNormal;
            ColorSelected = colorSelected;
            UseItemRectangle = useItemRectangle;
        }
        public SimpleText(string text, Point position, PosOnScreen pos, SpriteFont font, Color colorNormal)
            : this(text, position, pos, font, colorNormal, colorNormal, false) { }

        private static Rectangle CreateRectangle(Point position, SpriteFont font, string text)
        {
            var value = font.MeasureString(text);
            return new Rectangle(position.X, position.Y, (int)value.X + 1, (int)value.Y + 1);
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, (UseItemRectangle) ? PositionItem : Position, (IsSelect) ? ColorSelected : ColorNormal);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            var mouse = new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1);
            //line
            if (IsSelect)
                return;
            //line
            if (GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                IsSelect = ItemRectangle.Intersects(mouse);
            base.UpdateItem(gameTime);
        }
    }
}
