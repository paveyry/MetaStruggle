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
        private Dictionary<string, Texture2D> Theme { get; set; }
        float[] FieldsWidthAbstract { get; set; }
        int[] RealFieldsWidth { get; set; }
        Line Fields { get; set; }
        List<Line> Elements { get; set; }
        Line LineSelected { get; set; }
        public string[] Selected { get { return (LineSelected != null) ? LineSelected.Elements : null; } }

        int MaxLine { get; set; }
        private int StartPos;
        private int EndPos { get { return StartPos + MaxLine; } set { StartPos = value - MaxLine; } }
        private int _oldWheelValue;
        int HeightLine { get; set; }
        float Ratio { get; set; }

        public ListLine(Dictionary<string, float> fields, List<string[]> elements, Rectangle abstractRectangle, string theme, SpriteFont font, Color normalColor, Color selectedColor)
            : base(abstractRectangle, true)
        {
            Theme = RessourceProvider.Themes[theme];
            Elements = new List<Line>();
            FieldsWidthAbstract = fields.Values.ToArray();
            RealFieldsWidth = new int[fields.Values.Count];
            HeightLine = GetLineHeight(font);
            _oldWheelValue = GameEngine.MouseState.ScrollWheelValue;

            for (int i = 0; i < RealFieldsWidth.Length; i++)
                RealFieldsWidth[i] = (int)((FieldsWidthAbstract[i] / 100f) * Width);

            Fields = new Line(new Rectangle((int)Position.X, (int)Position.Y, RealRectangle.Width, HeightLine), fields.Keys.ToArray(), RealFieldsWidth, font, normalColor, selectedColor, true);

            int heigth = (int)Position.Y + HeightLine;
            int newHeigth = HeightLine;
            MaxLine = 0;
            for (int i = 0; i < elements.Count; heigth += HeightLine, i++)
                if ((heigth + HeightLine) < RealRectangle.Height + RealRectangle.Y)
                {
                    Elements.Add(new Line(new Rectangle((int)Position.X, heigth, RealRectangle.Width, HeightLine), elements[i], RealFieldsWidth, font, normalColor, selectedColor, true));
                    newHeigth += HeightLine;
                    MaxLine++;
                }
                else
                    Elements.Add(new Line(new Rectangle((int)Position.X, heigth, RealRectangle.Width, HeightLine), elements[i], RealFieldsWidth, font, normalColor, selectedColor, true));
            if ((newHeigth + HeightLine) > RealRectangle.Height)
                RealRectangle.Height = newHeigth;
            Ratio = (Elements.Count)/(float)(MaxLine+1);
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Theme["ListLine.LeftSide"], RealRectangle, Color.White);
            Fields.DrawItem(gameTime, spriteBatch);
            foreach (Line element in Elements.Where(element => element.IsDrawable))
                element.DrawItem(gameTime, spriteBatch);
            spriteBatch.Draw(Theme["ListLine.LeftSide"], new Rectangle(0,RealRectangle.Y+(int)(((StartPos)*HeightLine)/(Ratio)),20,(int)(((MaxLine+1)*HeightLine)/Ratio)), Color.White);
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
                    Elements[index].UpdatePosition(HeightLine);
                }
            }
            else if (EndPos < Elements.Count && GameEngine.MouseState.ScrollWheelValue < _oldWheelValue)
            {
                StartPos++;
                for (int index = 0; index < Elements.Count; index++)
                {
                    Elements[index].IsDrawable = (index >= StartPos && index < EndPos);
                    Elements[index].UpdatePosition(-HeightLine);
                }
            }
            _oldWheelValue = GameEngine.MouseState.ScrollWheelValue;
            //base.UpdateItem(gameTime); //mettre UpadateResolution en Virtual
        }
    }
}
