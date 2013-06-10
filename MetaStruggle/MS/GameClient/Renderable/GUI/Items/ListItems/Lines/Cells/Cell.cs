using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Items.ListItems.Lines.Cells
{
    class Cell
    {
        public delegate string NameFunc();
        public delegate void Event();

        private Event OnClick { get; set; }
        private NameFunc Text { get; set; }
        public bool IsSelect { get; set; }
        private Rectangle InternalRectangle { get; set; }
        private NameFunc displayText { get; set; }
        SpriteFont Font { get; set; }
        Color ColorNormal { get; set; }
        Color ColorSelected { get; set; }

        public Cell(NameFunc text, Event onClick, SpriteFont font, Color colorNormal, Color colorSelected)
        {
            OnClick = onClick ?? (() => { });
            Text = text;
            Font = font;
            ColorNormal = colorNormal;
            ColorSelected = colorSelected;
        }

        public void DrawCell(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font,displayText.Invoke(),new Vector2(InternalRectangle.X,InternalRectangle.Y),(IsSelect)?ColorSelected:ColorNormal);
        }

        public void UpdateCell(GameTime gameTime)
        {
        }

        public void UpdateRectangle(Rectangle rectangle)
        {
            InternalRectangle = rectangle;
            displayText = (() => GetCorrectString(Text.Invoke(), InternalRectangle.Width, Font));

        }

        string GetCorrectString(string str, int width, SpriteFont font)
        {
            string s = "";
            for (int i = 0; i < str.Length; s += str[i], i++)
                if (font.MeasureString(s).X > width)
                    return ((s.Length >= 4) ? s.Substring(0, s.Length - 4) : "") + "...";
            return s;
        }
    }
}
