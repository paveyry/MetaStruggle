using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Items
{
    public class Button : Item
    {
        private SpriteFont ButtonFont;
        public string Name;
        private Color ButtonColor;
        private Color ButonColorSelected;
        private Texture2D Image;
        private Texture2D ImageSelected;
        bool _isSelect { get; set; }
        public bool IsSelect { get; set; }
        private Event OnClick;

        public Button(Rectangle rectangle, string text, SpriteFont font, Color buttonColor, Color buttonSelected, Event onClick)
            : base(rectangle)
        {
            Name = text;
            ButtonFont = font;
            ButtonColor = buttonColor;
            ButonColorSelected = buttonSelected;
            OnClick = onClick;
            _isSelect = false;
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ButtonFont, Name, Position,_isSelect ? ButonColorSelected : ButtonColor);
            base.DrawItem(gameTime, spriteBatch);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            var mouse = new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1);
            if (ElementRectangle.Intersects(mouse) && GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                OnClick.Invoke();
            if (!_isSelect)
                IsSelect = _isSelect;
            
            if (GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                _isSelect = ElementRectangle.Intersects(mouse);
            if (_isSelect)
                IsSelect = _isSelect;
        }
    }
}
