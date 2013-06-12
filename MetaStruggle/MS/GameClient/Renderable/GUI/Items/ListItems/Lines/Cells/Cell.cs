using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items.ListItems.Lines.Cells
{
    class Cell
    {
        public delegate string NameFunc();
        public delegate bool EventSelect(Rectangle internalRectangle, GameTime gameTime, double _oldTime);

        private EventSelect OnClick { get; set; }
        private NameFunc Text { get; set; }
        public bool IsSelect { get; set; }
        public Rectangle InternalRectangle { get; private set; }
        private NameFunc DisplayText { get; set; }
        SpriteFont Font { get; set; }
        Color ColorNormal { get; set; }
        Color ColorSelected { get; set; }
        private double _oldMilliSeconds;

        public Cell(NameFunc text, EventSelect onClick, SpriteFont font, Color colorNormal, Color colorSelected)
        {
            OnClick = onClick ?? (( a, b,c) => true);
            Text = text;
            Font = font;
            ColorNormal = colorNormal;
            ColorSelected = colorSelected;
            _oldMilliSeconds = -1;
        }

        public void DrawCell(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font,DisplayText.Invoke(),new Vector2(InternalRectangle.X,InternalRectangle.Y),(IsSelect)?ColorSelected:ColorNormal);
        }

        public void UpdateCell(GameTime gameTime)
        {
            if (IsSelect)
                IsSelect = OnClick.Invoke(InternalRectangle, gameTime, _oldMilliSeconds);
            else
                _oldMilliSeconds = gameTime.TotalGameTime.TotalMilliseconds;        
        }

        public void UpdateRectangle(Rectangle rectangle)
        {
            InternalRectangle = rectangle;
            DisplayText = (() => GetCorrectString(Text.Invoke(), InternalRectangle.Width, Font));

        }

        string GetCorrectString(string str, int width, SpriteFont font)
        {
            string s = "";
            for (int i = 0; i < str.Length; s += str[i], i++)
            {
                var t = font.MeasureString(s).X;
                if (font.MeasureString(s).X > width)
                    return ((s.Length >= 4) ? s.Substring(0, s.Length - 4) : "") + "...";}
            return s;
        }
    }
}
