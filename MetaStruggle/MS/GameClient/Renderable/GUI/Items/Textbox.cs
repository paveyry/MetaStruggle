using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Items
{
    public class Textbox : Item
    {
        private SpriteFont Font { get; set; }
        private Color ColorText { get; set; }
        public string Text { get; private set; }
        private string DisplayText { get; set; }
        private Texture2D Cursor { get; set; }
        private Texture2D TextboxButton { get; set; }
        private int _displayPos;
        private int _displayLength;
        private int _actualPos;
        private bool _isSelect;
        private Vector2 _charLength;

        private bool _CapsLock { get { return System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock); } }
        private bool _NumLock { get { return System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.NumLock); } }

        public Textbox(string text, Rectangle rectangle, Texture2D textboxButton, SpriteFont font, Color colorText)
            : base(rectangle)
        {
            Font = font;
            ColorText = colorText;
            _isSelect = false;
            Cursor = RessourceProvider.Cursors["Textbox"];
            TextboxButton = textboxButton;
            Text = text;
            _actualPos = Text.Length;
            _displayLength = CalculateLengthMax('m');
            _charLength = Font.MeasureString("m");
            DisplayText = Text.Length > _displayLength ? Text.Substring(_actualPos - _displayLength, _displayLength) : Text;
        }

        private int CalculateLengthMax(char c)
        {
            string str = "";
            int length;
            for (length = 0; Font.MeasureString(str).X < RealRectangle.Width; length++)
                str += c;
            return length - 1;
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextboxButton, RealRectangle, Color.White);
            spriteBatch.Draw(Cursor, new Rectangle((int)((_actualPos - _displayPos)*_charLength.X), RealRectangle.Location.Y,1,(int)_charLength.Y),ColorText );
            spriteBatch.DrawString(Font, DisplayText, Position, ColorText);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            var mouse = new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1);
            if (GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                _isSelect = RealRectangle.Intersects(mouse);
            if (!_isSelect || gameTime.TotalGameTime.Ticks % 4 != 0)
                return;
            KeyboardState ks = GameEngine.KeyboardState;
            foreach (var keyse in ks.GetPressedKeys())
            {
                char c = GetChar(keyse);
                if (c != '\0')
                    Text = Text.Substring(0, _actualPos) +
                           ((ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift)) ? GetInverseChar(c) : c) +
                           Text.Substring(_actualPos, Text.Length - _actualPos++);
                else if (keyse == Keys.Back && Text.Length > 0 && _actualPos > 0)
                    Text = Text.Substring(0, _actualPos - 1) + Text.Substring(_actualPos, Text.Length - _actualPos--);
                else if (keyse == Keys.Left && _actualPos > 0)
                    _actualPos--;
                else if (keyse == Keys.Right && _actualPos < Text.Length)
                    _actualPos++;
                else if (keyse == Keys.End)
                    _actualPos = Text.Length;
                else if (keyse == Keys.Escape || keyse == Keys.Enter)
                    _isSelect = false;
                UpdateDisplayText();
            }
            base.UpdateItem(gameTime);
        }

        private void UpdateDisplayText()
        {
            if (_actualPos >= _displayPos && _actualPos <= _displayLength && _displayLength < Text.Length)
                return;
            DisplayText = _displayLength < Text.Length
                               ? Text.Substring( _displayPos = ((_actualPos < _displayPos) ? _actualPos : _actualPos - _displayLength), _displayLength)
                               : Text.Substring(_displayPos = 0, Text.Length);
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

        private char GetInverseChar(char c)
        {
            return char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c);
        }
    }
}
