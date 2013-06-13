using System.Collections.Generic;
using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Items
{
    public class Textbox : Item
    {
        #region Fields
        private Rectangle InternalRectangle { get; set; }

        #region ThemeElementFields
        private Dictionary<string, Texture2D> Theme { get; set; }
        private SpriteFont Font { get; set; }
        private Color ColorText { get; set; }
        public string Text { get; private set; }
        #endregion

        #region TextboxElementFields
        private bool _isSelect;
        private string _displayText;
        private int _displayPos;
        private int _actualPos;
        private float _cursorPosition;
        double _previousTime;
        private Keys _previousKey;

        private bool _CapsLock { get { return System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock); } }
        private bool _NumLock { get { return System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.NumLock); } }
        #endregion
        #endregion

        public Textbox(string text, Rectangle rectangle, string theme, SpriteFont font, Color colorText, bool isDrawable = true)
            : base(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, GetLineHeight(font)),isDrawable)
        {
            Text = text;
            Theme = RessourceProvider.Themes[theme];
            Font = font;
            ColorText = colorText;

            _isSelect = false;
            _actualPos = Text.Length;
            _previousTime = 0;
            _previousKey = Keys.Escape;

            InternalRectangle = new Rectangle(RealRectangle.X + Theme["Textbox.LeftSide"].Width, RealRectangle.Y,
                RealRectangle.Width - (Theme["Textbox.LeftSide"].Width + Theme["Textbox.RightSide"].Width), RealRectangle.Height);

            UpdateDisplayText(false);
        }

        internal override void UpdateResolution()
        {
            base.UpdateResolution();
            InternalRectangle = new Rectangle(RealRectangle.X + Theme["Textbox.LeftSide"].Width, RealRectangle.Y,
                RealRectangle.Width - (Theme["Textbox.LeftSide"].Width + Theme["Textbox.RightSide"].Width), RealRectangle.Height);
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsDrawable)
                return;
            spriteBatch.Draw(Theme["Textbox.Background"], InternalRectangle, Color.White);
            spriteBatch.Draw(Theme["Textbox.LeftSide"], new Rectangle(RealRectangle.X, RealRectangle.Y, Theme["Textbox.LeftSide"].Width,
                RealRectangle.Height), Color.White);
            spriteBatch.Draw(Theme["Textbox.RightSide"], new Rectangle(InternalRectangle.X + InternalRectangle.Width, RealRectangle.Y, 
                Theme["Textbox.RightSide"].Width, RealRectangle.Height), Color.White);
            if (_isSelect && gameTime.TotalGameTime.Milliseconds / 500 % 2 == 0)
                spriteBatch.Draw(Theme["Textbox.Separator"], new Rectangle((int)_cursorPosition + InternalRectangle.X, InternalRectangle.Y,
                    1, InternalRectangle.Height), ColorText);

            spriteBatch.DrawString(Font, _displayText, new Vector2(InternalRectangle.X,InternalRectangle.Y), ColorText);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            base.UpdateItem(gameTime);
            if (!IsDrawable)
                return;

            if (GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                _isSelect = RealRectangle.Intersects(new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1));
            if (!_isSelect || gameTime.TotalGameTime.Ticks % 3 != 0)
                return;
            KeyboardState ks = GameEngine.KeyboardState;
            foreach (Keys key in ks.GetPressedKeys())
            {
                var delBack = false;
                char c = GetChar(key);
                if (((_actualPos > 0 && char.ToLower(Text[_actualPos - 1]) == char.ToLower(c)) || (key == _previousKey))
                    && (gameTime.TotalGameTime.TotalMilliseconds - _previousTime) < 150)
                    break;
                if (c != '\0')
                    Text = Text.Substring(0, _actualPos) +
                           ((ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift)) ? GetInverseChar(c) : c) +
                           Text.Substring(_actualPos, Text.Length - _actualPos++);
                else if (key == Keys.Back && Text.Length > 0 && _actualPos > 0)
                {
                    delBack = true;
                    Text = Text.Substring(0, _actualPos - 1) + Text.Substring(_actualPos, Text.Length - _actualPos--);
                }
                else if (key == Keys.Delete && _actualPos < Text.Length)
                    Text = Text.Substring(0, _actualPos) + Text.Substring(_actualPos + 1, Text.Length - _actualPos - 1);
                else if (key == Keys.Left && _actualPos > 0)
                    _actualPos--;
                else if (key == Keys.Right && _actualPos < Text.Length)
                    _actualPos++;
                else if (key == Keys.End)
                    _actualPos = Text.Length;
                else if (key == Keys.Pa1 || key == Keys.Home)
                    _actualPos = 0;
                else if (key == Keys.Enter)
                    _isSelect = false;
                UpdateDisplayText(delBack);
                _previousKey = key;
                _previousTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        private void UpdateDisplayText(bool delBack)
        {
            bool isGood = false;
            _displayPos = (delBack) ? Text.Length : _displayPos;
            do
            {
                _displayText = "";
                _cursorPosition = 0;
                if (_displayPos >= Text.Length)
                    _displayPos = 0;
                for (int i = _displayPos; i < Text.Length; i++)
                {
                    float measure = Font.MeasureString(_displayText + Text[i]).X;
                    if (measure >= InternalRectangle.Width) break;
                    _displayText += Text[i];
                    if (i != _actualPos - 1 && i != _actualPos) continue;
                    isGood = true;
                    _cursorPosition = (i == _actualPos) ? _cursorPosition : measure;
                }
                if (!isGood)
                    _displayPos += (_displayPos <= _actualPos) ? 1 : -1;
            } while (!isGood && _displayText != "");
        }

        private char GetChar(Keys key)
        {
            string str = key.ToString();
            if (str.Length == 1)
                return _CapsLock ? str[0] : char.ToLower(str[0]);
            if (str.StartsWith("NumPad") && _NumLock)
                return str[6];
            if (str[0] == 'D' && (str[1] >= '0' && str[1] <= '9'))
                return str[1];
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

        private static char GetInverseChar(char c)
        {
            return char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c);
        }
    }
}
