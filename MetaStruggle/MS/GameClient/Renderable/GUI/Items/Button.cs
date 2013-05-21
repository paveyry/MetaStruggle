using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Items
{
    public class Button : Item
    {
        #region Fields
        public bool IsSelect { get; set; }
        public string Name { get { return (_nameFunc != null) ? _nameFunc.Invoke() : _name; } set { _name = value; } }
        private string _name;

        private SpriteFont Font { get; set; }
        private Color ButtonColor { get; set; }
        private Color ButonColorSelected { get; set; }
        private Texture2D Image { get; set; }
        private Texture2D ImageSelected { get; set; }
        private readonly bool _drawImageSelected;
        private readonly Event _onClick;
        private NameFunc _nameFunc;
        #endregion

        #region Constructors

        internal Button(string text, NameFunc func, PosOnScreen pos, Rectangle rectangle, Texture2D image, Texture2D imageSelected, bool drawImageSelected,
                      SpriteFont font, Color buttonColor, Color buttonSelected, Event onClick)
            : base(rectangle, pos)
        {
            Image = image;
            ImageSelected = imageSelected;
            Name = text;
            Font = font;
            ButtonColor = buttonColor;
            ButonColorSelected = buttonSelected;
            _onClick = onClick;
            _nameFunc = func;
            _drawImageSelected = drawImageSelected;
            IsSelect = false;
        }
        public Button(string text, PosOnScreen pos, Rectangle rectangle, SpriteFont font, Color buttonColor, Color buttonSelected,
                      Event onClick)
            : this(text, null, pos, rectangle, null, null, false, font, buttonColor, buttonSelected, onClick) { }
        public Button(string text, PosOnScreen pos, Rectangle rectangle, Event onClick)
            : this(text, pos, rectangle, RessourceProvider.Fonts["Menu"], Color.White, Color.DarkOrange, onClick) { }
        public Button(NameFunc func, PosOnScreen pos, Rectangle rectangle, SpriteFont font, Color buttonColor, Color buttonSelected, Event onClick)
            : this("", func, pos, rectangle, null, null, false, font, buttonColor, buttonSelected, onClick) { }
        #endregion

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsSelect)
            {
                if (_drawImageSelected)
                    spriteBatch.Draw(ImageSelected, RealRectangle, Color.White);
                else if (Image != null)
                    spriteBatch.Draw(Image, RealRectangle, ButonColorSelected);
                spriteBatch.DrawString(Font, Name, Position, ButonColorSelected);
            }
            else
            {
                if (Image != null)
                    spriteBatch.Draw(Image, RealRectangle, Color.White);
                spriteBatch.DrawString(Font, Name, Position, ButtonColor);
            }
        }

        public override void UpdateItem(GameTime gameTime)
        {
            if (RealRectangle.Intersects(new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1)))
            {
                if (GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                    _onClick.Invoke();
                IsSelect = true;
            }
            else
                IsSelect = false;
            base.UpdateItem(gameTime);
        }
    }
}
