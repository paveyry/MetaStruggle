using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Elements
{
    public class Textbox : Element
    {
        private SpriteFont Font;
        private Color ColorText;
        public string Text;
        private string _displayText;
        private int _displayPos;
        private int _displayLength;
        private int actualPos;
        private bool isSelect;

        private bool _CapsLock { get { return System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock); } }
        private bool _NumLock { get { return System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.NumLock); } }

        public Textbox(Rectangle rectangle, SpriteFont font)
            : base(rectangle)
        {
            Font = font;
            ColorText = Color.White;
            isSelect = true;

            Text = "";
            actualPos = Text.Length;
            _displayLength = CalculateLengthMax('m');
            _displayText = Text.Length > _displayLength ? Text.Substring(actualPos - _displayLength, _displayLength) : Text;
        }

        private int CalculateLengthMax(char c)
        {
            string str = "";
            int length;
            for (length = 0; Font.MeasureString(str).X < ElementRectangle.Width; length++)
                str += c;
            return length - 1;
        }

        public override void DrawElement(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, _displayText, Position, ColorText);
        }

        public override void UpdateElement(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Milliseconds % 2 == 0)
                return;
            KeyboardState ks = GameEngine.KeyboardState;
            foreach (var keyse in ks.GetPressedKeys())
            {
                char c = GetChar(keyse);
                if (c != '\0')
                    Text = Text.Substring(0, actualPos) +
                           ((ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift)) ? GetInverse(c) : c) +
                           Text.Substring(actualPos, Text.Length - actualPos++);
                else if (keyse == Keys.Back && Text.Length > 0 && actualPos > 0)
                    Text = Text.Substring(0, actualPos - 1) + Text.Substring(actualPos, Text.Length - actualPos--);
                else if (keyse == Keys.Left && actualPos > 0)
                    actualPos--;
                else if (keyse == Keys.Right && actualPos < Text.Length)
                    actualPos++;
                UpdateDisplayText();
            }

        }

        private void UpdateDisplayText()
        {
            if (actualPos >= _displayPos && actualPos <= _displayLength && _displayLength < Text.Length)
                return;
            _displayText = _displayLength < Text.Length
                               ? Text.Substring(actualPos < _displayPos ? actualPos : actualPos - _displayLength, _displayLength)
                               : Text.Substring(0, Text.Length);
        }

        private char GetChar(Keys key)
        {
            string t = key.ToString();
            if (t.Length == 1)
                return _CapsLock ? t[0] : char.ToLower(t[0]);
            if (t.StartsWith("NumPad") && _NumLock)
                return t[6];
            if (t[0] == 'D' && (t[1] >= '0' && t[1] <= '9'))
                return t[1];
            switch (key)
            {
                case Keys.OemPeriod:
                    return '.';
                case Keys.Space:
                    return ' ';
                default:
                    return '\0';
            }
        }

        private char GetInverse(char c)
        {
            return char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c);
        }
    }
}
