using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items
{
    class ListLines : Item
    {
        //private Dictionary<string, float> Fields;// { get; set; }
        string[] Fields { get; set; }
        int[] WidthFields { get; set; }
        List<string[]> Elements { get; set; }
        SpriteFont Font { get; set; }
        Color NormalColor { get; set; }
        Color SelectedColor { get; set; }
        string[] Selected { get {return Elements[IndexSelected-1];} }
        int IndexSelected { get; set; }
        List<SimpleText[]> Lines { get; set; }

        public ListLines(Dictionary<string, float> fields, List<string[]> list, Rectangle abstractRectangle, SpriteFont font, Color normalColor, Color selectedColor)
            : base(abstractRectangle, true)
        {
            //Fields = new Dictionary<string, float>();
            Fields = fields.Keys.ToArray();
            Lines = new List<SimpleText[]>();
            WidthFields = new int[fields.Count];
            Elements = list;
            Font = font;
            NormalColor = normalColor;
            SelectedColor = selectedColor;

            int width = RealRectangle.X, heigth = RealRectangle.Y, lineHeigth = GetLineHeight();
            int index = 0;
            var field = new SimpleText[fields.Count];
            foreach (KeyValuePair<string, float> kvp in fields)
            {
                WidthFields[index] = (int) ((kvp.Value/100f)*Width);
                field[index] = new SimpleText(GetCorrectString(kvp.Key, WidthFields[index]), new Point(width, heigth),
                    PosOnScreen.TopLeft, Font,NormalColor,NormalColor,true);
                width += WidthFields[index++];
            }
            heigth += lineHeigth;
            Lines.Add(field);
            for (int i = 0; i < list.Count; heigth += lineHeigth ,i++)
            {
                string[] stringse = list[i];
                var line = new SimpleText[stringse.Length];
                if (line.Length != WidthFields.Length)
                    throw new Exception("Invalid parameters...");
                width = RealRectangle.X;
                for (int j = 0; j < line.Length; width += WidthFields[j], j++)
                    line[j] = new SimpleText(GetCorrectString(stringse[j], WidthFields[j]),
                                             new Point(width, heigth), PosOnScreen.TopLeft, Font, NormalColor, SelectedColor, true);
                Lines.Add(line);
            }
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var simpleText in Lines.SelectMany(simpleTextse => simpleTextse))
                simpleText.DrawItem(gameTime, spriteBatch);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            foreach (var simpleText in Lines.SelectMany(simpleTextse => simpleTextse))
                simpleText.UpdateItem(gameTime);

            for (int i = 1; i < Lines.Count; i++)
                foreach (var s in Lines[i].Where(simpleText => simpleText.IsSelect))
                {
                    if (IndexSelected != i)
                        foreach (var lineOldIndex in Lines[IndexSelected])
                            lineOldIndex.IsSelect = false;
                    foreach (var correctLine in Lines[i])
                        correctLine.IsSelect = true;
                    IndexSelected = i;
                }

            base.UpdateItem(gameTime);
        }

        string GetCorrectString(string str, int width)
        {
            string s = "";
            foreach (char c in str)
            {
                if (Font.MeasureString(s).X > width)
                    return ((s.Length >= 4) ? s.Substring(0, s.Length - 4) : "") + "...";
                s += c;
            }
            return s;
        }

        int GetLineHeight()
        {
            return (int) Font.MeasureString("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789").Y + 1;
        }
    }
}
