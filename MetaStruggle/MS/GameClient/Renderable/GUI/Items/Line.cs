using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public Line(Point position, string[] elements, int[] fields, int height, SpriteFont font, Color colorNormal, Color colorSelected, bool isDrawable)
            : base(new Rectangle(position.X, position.Y, position.X + fields.Sum(), height))
        {
            Fields = fields;
            Elements = elements;
            Cells = new List<SimpleText>();
            IsDrawable = isDrawable;

            int width = position.X;
            for (int index = 0; index < Elements.Length; width += Fields[index], index++)
                Cells.Add(new SimpleText(GetCorrectString(Elements[index], Fields[index], font), new Point(width, position.Y), PosOnScreen.TopLeft, font,
                                         colorNormal, colorSelected, true));
        }

        public void UpdatePosition(int positionInY)
        {
            foreach (SimpleText simpleText in Cells)
                simpleText.ItemRectangle.Location = new Point(simpleText.ItemRectangle.X, simpleText.ItemRectangle.Y + positionInY);
            RealRectangle.Location = new Point(RealRectangle.X, RealRectangle.Y + positionInY);
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
            foreach (var t in Cells)
            {
                t.UpdateItem(gameTime);
                if (t.IsSelect)
                {
                    IsSelect = true;
                    break;
                }
                t.IsSelect = false;
            }
            //base.UpdateItem(gameTime);
        }
    }
}
