using System.Collections.Generic;
using System.Linq;
using GameClient.Global;
using GameClient.Renderable.GUI.Items.ListItems.Lines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items.ListItems
{
    abstract class ListLines : Item
    {
        internal List<Line> Lines { get; set; }
        Line Field { get; set; }
        public Line LineSelected { get; set; }
        internal Dictionary<string, Texture2D> Theme { get; set; }
        internal Rectangle InternalRectangle { get; set; }
        private int HeightInternalRectangle { set { InternalRectangle = new Rectangle(InternalRectangle.X, InternalRectangle.Y, InternalRectangle.Width, value); } }
        private int _cursor;
        private float _ratio = 1f;
        private int _maxLines;
        private readonly int _heightLine;
        private readonly int _heightField;
        private int _oldWheelValue;

        protected ListLines(Rectangle abstractRectangle, List<Line> lines, Line field, int heightLine, int heightField, string theme, string themeItem, bool isDrawable = true)
            : base(abstractRectangle, true, isDrawable)
        {
            Lines = lines;
            Field = field;
            Field.IsSelect = false;
            LineSelected = Field;

            _cursor = 0;
            _heightLine = heightLine;
            _heightField = heightField;
            _oldWheelValue = GameEngine.MouseState.ScrollWheelValue;

            Theme = new Dictionary<string, Texture2D>();
            foreach (var kvp in RessourceProvider.Themes[theme].Where(kvp => kvp.Key.StartsWith(themeItem)))
                Theme.Add(kvp.Key.Substring(themeItem.Length + 1), kvp.Value);

            CreateInternalRectangle();
            CreateLines();
        }

        protected ListLines(Rectangle abstractRectangle, List<Line> lines, Line field, int heightLine, string theme, string themeItem, bool isDrawable = true)
            : this(abstractRectangle, lines, field, heightLine, heightLine, theme, themeItem, isDrawable)
        {

        }

        void CreateInternalRectangle()
        {
            InternalRectangle = new Rectangle(RealRectangle.X + Theme["LeftSide"].Width, RealRectangle.Y + _heightField,
                RealRectangle.Width - Theme["LeftSide"].Width - Theme["RightSide"].Width, RealRectangle.Height - _heightField - Theme["Down"].Height);
        }

        void CreateLines()
        {
            for (int i = _cursor - 1, posInY = RealRectangle.Y; i >= 0; i--) //Lines before cursor
            {
                posInY -= _heightLine;
                Lines[i].InternalRectangle = new Rectangle(InternalRectangle.X, posInY, InternalRectangle.Width, _heightLine);
                Lines[i].IsDrawable = false;
            }

            Field.InternalRectangle = new Rectangle(InternalRectangle.X, RealRectangle.Y, InternalRectangle.Width, _heightField);
            
            _maxLines = Lines.Count;
            bool isOutOfLimits = false;
            for (int i = _cursor, posInY = InternalRectangle.Y; i < Lines.Count; i++)
            {
                Lines[i].InternalRectangle = new Rectangle(InternalRectangle.X, posInY, InternalRectangle.Width, _heightLine);
                posInY += _heightLine;

                Lines[i].IsDrawable = posInY  <= InternalRectangle.Y + InternalRectangle.Height;
                if (Lines[i].IsDrawable || isOutOfLimits) continue;

                HeightInternalRectangle = posInY - _heightLine - InternalRectangle.Y; //Update InternalRectangle if Lines are out of the limits
                _maxLines = i;
                isOutOfLimits = true;
                _ratio = (Lines.Count*_heightLine)/(float) InternalRectangle.Height;
            }

        }

        internal override void UpdateResolution()
        {
            base.UpdateResolution();
            CreateInternalRectangle();
            CreateLines();
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsDrawable)
                return;
            spriteBatch.Draw(Theme["Background"], InternalRectangle, Color.White);
            spriteBatch.Draw(Theme["LeftSide"], new Rectangle(RealRectangle.X, InternalRectangle.Y,
                Theme["LeftSide"].Width, InternalRectangle.Height), Color.White);
            spriteBatch.Draw(Theme["RightSide"], new Rectangle(InternalRectangle.X + InternalRectangle.Width, InternalRectangle.Y,
                Theme["RightSide"].Width, InternalRectangle.Height), Color.White);
            spriteBatch.Draw(Theme["Top"], new Rectangle(RealRectangle.X, RealRectangle.Y, RealRectangle.Width, _heightLine), Color.White);
            spriteBatch.Draw(Theme["Down"], new Rectangle(RealRectangle.X, InternalRectangle.Y + InternalRectangle.Height,
                RealRectangle.Width, Theme["Down"].Height), Color.White);
            spriteBatch.Draw(Theme["Scroll"], new Rectangle(InternalRectangle.X + InternalRectangle.Width,
                InternalRectangle.Y + (int)(((_cursor) * _heightLine) / (_ratio)),
                Theme["RightSide"].Width, (int)(InternalRectangle.Height / _ratio)), Color.White);

            Field.DrawItem(gameTime, spriteBatch);
            foreach (var line in Lines.Where(line => line.IsDrawable))
                line.DrawItem(gameTime, spriteBatch);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            base.UpdateItem(gameTime);
            if (!IsDrawable)
                return;

            foreach (Line line in Lines.Where(line => line.IsDrawable))
            {
                line.UpdateItem(gameTime);
                if (!line.IsSelect || line.Equals(LineSelected))
                    continue;

                LineSelected.IsSelect = false;
                LineSelected = line;
            }


            int tempHeightLine = 0;
            if (_cursor + _maxLines < Lines.Count && GameEngine.MouseState.ScrollWheelValue < _oldWheelValue)
            {
                _cursor++;
                tempHeightLine = -_heightLine;
            }
            else if (_cursor > 0 && GameEngine.MouseState.ScrollWheelValue > _oldWheelValue)
            {
                _cursor--;
                tempHeightLine = _heightLine;
            }

            for (int i = 0; i < Lines.Count && tempHeightLine != 0; i++)
            {
                Lines[i].IsDrawable = (i >= _cursor && i < _cursor + _maxLines);
                Lines[i].PosYInternalRectangle += tempHeightLine;
            }

            _oldWheelValue = GameEngine.MouseState.ScrollWheelValue;
        }
    }
}
