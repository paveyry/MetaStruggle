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
    class Slider : Item
    {
        #region Fields
        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                if (value > Max)
                    _value = Max;
                else if (value < Min)
                    _value = Min;
                else
                    _value = value;
            }
        }
        public int Min { get; private set; }
        public int Max { get; private set; }
        private int ValuePos
        {
            get { return (int)(((Value - Min) / (float)(Max - Min)) * InternalRectangle.Width); }
            set { Value = (int)((value / (float)InternalRectangle.Width) * (Max - Min)) + Min; }
        }

        private int _oldWheelValue;

        private Dictionary<string, Texture2D> Theme { get; set; }
        private Rectangle InternalRectangle { get; set; }
        private SpriteFont Font { get; set; }
        #endregion

        public Slider(Rectangle rectangle, int value, int min, int max, string theme,SpriteFont font, bool isDrawable = true)
            : base(rectangle, isDrawable)
        {
            Min = Math.Min(min,max);
            Max = Math.Max(min,max);
            Theme = RessourceProvider.Themes[theme];
            Font = font;

            InternalRectangle = new Rectangle(RealRectangle.X + Theme["Slider.LeftSide"].Width, RealRectangle.Y
                + Theme["Slider.Top"].Height, RealRectangle.Width - (Theme["Slider.LeftSide"].Width + Theme["Slider.RightSide"].Width),
                RealRectangle.Height - (Theme["Slider.Top"].Height + Theme["Slider.Down"].Height));
            Value = (value >= min && value <= max) ? value : min;

            _oldWheelValue = GameEngine.MouseState.ScrollWheelValue;
        }
        public Slider(Rectangle rectangle, int value, string theme,SpriteFont font, bool isDrawable = true)
            : this(rectangle, value, 0, 100, theme, font,isDrawable)
        {
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsDrawable)
                return;
            spriteBatch.DrawString(Font, Value.ToString(), new Vector2(RealRectangle.X+RealRectangle.Width, RealRectangle.Y), Color.White);

            spriteBatch.Draw(Theme["Slider.Top"], new Rectangle(RealRectangle.X, RealRectangle.Y, RealRectangle.Width,
                Theme["Slider.Top"].Height), Color.White);
            spriteBatch.Draw(Theme["Slider.Down"], new Rectangle(RealRectangle.X, InternalRectangle.Y
                + InternalRectangle.Height, RealRectangle.Width, Theme["Slider.Down"].Height), Color.White);
            spriteBatch.Draw(Theme["Slider.LeftSide"], new Rectangle(RealRectangle.X, InternalRectangle.Y,
                Theme["Slider.LeftSide"].Width, InternalRectangle.Height), Color.White);
            spriteBatch.Draw(Theme["Slider.RightSide"], new Rectangle(InternalRectangle.X + InternalRectangle.Width,
                InternalRectangle.Y, Theme["Slider.RightSide"].Width, InternalRectangle.Height), Color.White);
            spriteBatch.Draw(Theme["Slider.BackgroundSelected"], new Rectangle(InternalRectangle.X, InternalRectangle.Y,
                ValuePos, InternalRectangle.Height), Color.White);
            spriteBatch.Draw(Theme["Slider.BackgroundNormal"], new Rectangle(InternalRectangle.X + ValuePos, InternalRectangle.Y,
                InternalRectangle.Width - ValuePos, InternalRectangle.Height), Color.White);
            spriteBatch.Draw(Theme["Slider.Cursor"], new Rectangle(InternalRectangle.X + ValuePos - Theme["Slider.Cursor"].Width / 2,
                RealRectangle.Y, Theme["Slider.Cursor"].Width, RealRectangle.Height), Color.White);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            base.UpdateItem(gameTime);
            if (!IsDrawable)
                return;

            if (InternalRectangle.Intersects(new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1)))
            {
                if (GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                    ValuePos = GameEngine.MouseState.X - InternalRectangle.X;
                int abstractValue = Value - Min, tenOfTotal = (Max - Min) / 10;
                if (GameEngine.MouseState.ScrollWheelValue > _oldWheelValue && Value < Max)
                    Value = (abstractValue - abstractValue%tenOfTotal) + tenOfTotal + Min;
                else if (GameEngine.MouseState.ScrollWheelValue < _oldWheelValue && Value > Min)
                    Value = (abstractValue + ((tenOfTotal - abstractValue%tenOfTotal)%tenOfTotal)) - tenOfTotal + Min;
            }
            _oldWheelValue = GameEngine.MouseState.ScrollWheelValue;
        }

    }
}
