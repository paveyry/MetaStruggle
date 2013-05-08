using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items
{
    public class ListLine : Item
    {

        float[] FieldsWidthAbstract { get; set; }
        int[] RealFieldsWidth { get; set; }
        Line Fields { get; set; }
        List<Line> Elements { get; set; }
        Line LineSelected { get; set; }

        int MaxLine { get; set; }
        private int StartPos;
        private int EndPos { get { return StartPos + MaxLine; } set { StartPos = value - MaxLine; } }
        private int _oldWheelValue;
        int HeigthLine { get; set; }

        public ListLine(Dictionary<string, float> fields, List<string[]> elements, Rectangle abstractRectangle, SpriteFont font, Color normalColor, Color selectedColor)
            : base(abstractRectangle, true)
        {
            Elements = new List<Line>();
            FieldsWidthAbstract = fields.Values.ToArray();
            RealFieldsWidth = new int[fields.Values.Count];
            HeigthLine = GetLineHeight(font);
            _oldWheelValue = GameEngine.MouseState.ScrollWheelValue;

            for (int i = 0; i < RealFieldsWidth.Length; i++)
                RealFieldsWidth[i] = (int)((FieldsWidthAbstract[i] / 100f) * Width);

            Fields = new Line(new Rectangle((int) Position.X, (int)Position.Y,RealRectangle.Width,HeigthLine), fields.Keys.ToArray(), RealFieldsWidth, font, normalColor, selectedColor, true);

            int heigth = (int)Position.Y + HeigthLine;
            int newHeigth = HeigthLine;
            MaxLine = 0;
            for (int i = 0; i < elements.Count; heigth += HeigthLine, i++)
                if ((heigth + HeigthLine) < RealRectangle.Height + RealRectangle.Y)
                {
                    Elements.Add(new Line(new Rectangle((int)Position.X, heigth, RealRectangle.Width, HeigthLine), elements[i], RealFieldsWidth, font, normalColor, selectedColor, true));
                    newHeigth += HeigthLine;
                    MaxLine++;
                }
                else
                    Elements.Add(new Line(new Rectangle((int)Position.X, heigth, RealRectangle.Width, HeigthLine), elements[i], RealFieldsWidth, font, normalColor, selectedColor, false));
            RealRectangle = new Rectangle(RealRectangle.X, RealRectangle.Y, RealRectangle.Width, newHeigth);
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(RessourceProvider.Buttons["TextboxMulti"], RealRectangle, Color.White);
            Fields.DrawItem(gameTime, spriteBatch);
            foreach (Line element in Elements.Where(element => element.IsDrawable))
                element.DrawItem(gameTime, spriteBatch);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            foreach (Line element in Elements.Where(element => element.IsDrawable))
            {
                element.UpdateItem(gameTime);
                if (!element.IsSelect || element.Equals(LineSelected))
                    continue;

                if (LineSelected != null)
                    LineSelected.IsSelect = false;
                LineSelected = element;
            }
            if (StartPos > 0 && GameEngine.MouseState.ScrollWheelValue > _oldWheelValue)
            {
                StartPos--;
                for (int index = 0; index < Elements.Count; index++)
                {
                    Elements[index].IsDrawable = (index >= StartPos && index < EndPos);
                    Elements[index].UpdatePosition(HeigthLine);
                }
            }
            else if (EndPos < Elements.Count && GameEngine.MouseState.ScrollWheelValue < _oldWheelValue)
            {
                StartPos++;
                for (int index = 0; index < Elements.Count; index++)
                {
                    Elements[index].IsDrawable = (index >= StartPos && index < EndPos);
                    Elements[index].UpdatePosition(-HeigthLine);
                }
            }
            _oldWheelValue = GameEngine.MouseState.ScrollWheelValue;
            //base.UpdateItem(gameTime); //mettre UpadateResolution en Virtual
        }

        int GetLineHeight(SpriteFont font)
        {
            return (int)font.MeasureString("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789").Y + 1;
        }
    }
}
