using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Elements
{
    public class Button : Element
    {
        private SpriteFont ButtonFont;
        private string Name;
        private Color ButtonColor;
        private Color ButonColorSelected;
        private Texture2D Image;
        private Texture2D ImageSelected;
        private bool isSelect;
        private Event OnClick;

        public Button(Rectangle rectangle,string text, SpriteFont font,Color buttonColor, Color buttonSelected, Event onClick) : base(rectangle)
        {
            Name = text;
            ButtonFont = font;
            ButtonColor = buttonColor;
            ButonColorSelected = buttonSelected;
            OnClick = onClick;
            isSelect = false;
        }

        public override void DrawElement(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ButtonFont, Name, Position, isSelect?ButonColorSelected:ButtonColor) ;
            base.DrawElement(gameTime,spriteBatch);
        }

        public override void UpdateElement(GameTime gameTime)
        {
            KeyboardState ks = GameEngine.KeyboardState;
            if (ks.IsKeyDown(Keys.A))
                Name += "a";
        }
    }
}
