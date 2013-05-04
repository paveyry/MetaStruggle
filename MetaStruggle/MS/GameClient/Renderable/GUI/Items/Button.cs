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
        #region Fields

        private SpriteFont ButtonFont;
        public string Name;
        private Color ButtonColor;
        private Color ButonColorSelected;
        private Texture2D Image;
        private Texture2D ImageSelected;
        private bool _drawImageSelected;
        private bool _isSelect { get; set; }
        public bool IsSelect { get; set; }
        private Event OnClick;

        #endregion

        #region Constructors

        public Button(string text, PosOnScreen pos,Rectangle rectangle, Texture2D image, Texture2D imageSelected, bool drawImageSelected, 
                      SpriteFont font, Color buttonColor, Color buttonSelected, Event onClick)
            : base(rectangle, pos)
        {
            Image = image;
            ImageSelected = imageSelected;
            Name = text;
            ButtonFont = font;
            ButtonColor = buttonColor;
            ButonColorSelected = buttonSelected;
            OnClick = onClick;
            _isSelect = false;
        }
        public Button(string text, PosOnScreen pos, Rectangle rectangle, SpriteFont font, Color buttonColor, Color buttonSelected,
                      Event onClick)
            : this(text, pos, rectangle, null, null, false ,font, buttonColor, buttonSelected, onClick) { }
        public Button(string text, PosOnScreen pos,Rectangle rectangle, Event onClick)
            : this(text, pos,rectangle, RessourceProvider.Fonts["Menu"], Color.White, Color.DarkOrange, onClick) { }

        #endregion

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_isSelect)
            {
                if (_drawImageSelected)
                    spriteBatch.Draw(ImageSelected,RealRectangle, Color.White);
                else if (Image != null)
                    spriteBatch.Draw(Image, RealRectangle, ButonColorSelected);
                spriteBatch.DrawString(ButtonFont, Name, Position,ButonColorSelected);
            }
            else
            {
                if (Image != null)
                    spriteBatch.Draw(Image, RealRectangle, Color.White);
                spriteBatch.DrawString(ButtonFont, Name, Position, ButtonColor);
            }
        }

        public override void UpdateItem(GameTime gameTime)
        {
            var mouse = new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1);
            if (RealRectangle.Intersects(mouse) && GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                OnClick.Invoke();
            if (GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                _isSelect = RealRectangle.Intersects(mouse);
            base.UpdateItem(gameTime);
        }
    }
}
