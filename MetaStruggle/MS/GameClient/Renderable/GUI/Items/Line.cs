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
    internal class Line : Item
    {
        int[] Fields { get; set; }
        List<SimpleText> Cells { get; set; }
        private bool _isSelect;
        public bool IsSelect
        {
            get { return _isSelect; }
            set
            {
                _isSelect = value;
                foreach (var simpleText in Cells)
                    simpleText.IsSelect = value;
            }
        }
        public bool IsDrawable;
        string[] Elements { get; set; }

        public Line(Rectangle rectangle, string[] elements, int[] fields, SpriteFont font, Color colorNormal, Color colorSelected, bool isDrawable)
            : base(rectangle)
        {
            Fields = fields;
            Elements = elements;
            Cells = new List<SimpleText>();
            IsDrawable = isDrawable;

            int width = (int)PositionItem.X;
            for (int index = 0; index < Elements.Length; width += Fields[index], index++)
                Cells.Add(new SimpleText(GetCorrectString(Elements[index], Fields[index], font), null,new Point(width, (int)PositionItem.Y),
                    PosOnScreen.TopLeft, font, colorNormal, colorSelected, true));
        }

        public void UpdatePosition(int positionInY)
        {
            foreach (SimpleText simpleText in Cells)
                simpleText.ItemRectangle.Y += positionInY;
            ItemRectangle.Y += positionInY;
        }

        string GetCorrectString(string str, int width, SpriteFont font)
        {
            string s = "";
            for (int i = 0; i < str.Length; s += str[i], i++)
                if (font.MeasureString(s).X > width)
                    return ((s.Length >= 4) ? s.Substring(0, s.Length - 4) : "") + "...";
            return s;
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (SimpleText simpleText in Cells)
                simpleText.DrawItem(gameTime, spriteBatch);
            base.DrawItem(gameTime, spriteBatch);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            if (IsSelect)
                return;
            if (GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                IsSelect = ItemRectangle.Intersects(new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1));
            //base.UpdateItem(gameTime);
        }
    }
}
