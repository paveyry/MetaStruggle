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
    public class SimpleText : Item
    {
        #region Fields
        public bool IsSelect { get; set; }
        public string Text { get { return _nameFunc.Invoke(); } }

        private string _text;
        private readonly NameFunc _nameFunc;
        
        private SpriteFont Font { get; set; }
        private Color ColorNormal { get; set; }
        private Color ColorSelected { get; set; }
        #endregion

        #region Constructors
        internal SimpleText(NameFunc nameFunc,Point position, PosOnScreen pos, SpriteFont font, Color colorNormal, Color colorSelected, bool isDrawable = true)
            : base(CreateRectangle(position, font, nameFunc.Invoke()), pos, isDrawable)
        {
            Font = font;
            _nameFunc = nameFunc; 
            ColorNormal = colorNormal;
            ColorSelected = colorSelected;
        }
        public SimpleText(string text, Point position, PosOnScreen pos, SpriteFont font, Color colorNormal)
            : this(() => GameEngine.LangCenter.GetString(text), position, pos, font, colorNormal, colorNormal) { }
        public SimpleText(NameFunc text, Point position, PosOnScreen pos, SpriteFont font, Color colorNormal) 
            : this(text,position, pos, font, colorNormal, colorNormal) { }
        #endregion

        private static Rectangle CreateRectangle(Point position, SpriteFont font, string text)
        {
            var value = font.MeasureString(text);
            return new Rectangle(position.X, position.Y, (int)value.X + 1, (int)value.Y + 1);
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsDrawable)
                return;
            spriteBatch.DrawString(Font, Text, Position, (IsSelect) ? ColorSelected : ColorNormal);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            base.UpdateItem(gameTime);
            if (!IsDrawable)
                return;
            var mouse = new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1);
            if (IsSelect)
                return;
            if (GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                IsSelect = ItemRectangle.Intersects(mouse);
        }
    }
}
