using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Items
{
    public class CheckBox : Item
    {
        public bool IsSelect { get; set; }
        private Dictionary<string, Texture2D> Theme { get; set; }
        private Texture2D[] CheckBoxImages { get; set; }
        Event OnChange { get; set; }
        private double _oldTime;

        public CheckBox(Vector2 pos, string theme, bool isSelect, Event onChange = null,bool isDrawable = true)
            : base(CreateRectangles(pos, theme), isDrawable)
        {
            IsSelect = isSelect;
            OnChange = onChange ?? (() => { });
            Theme = RessourceProvider.Themes[theme];
            CheckBoxImages = new[] { Theme["CheckBox.Normal"], Theme["CheckBox.Selected"] };
            _oldTime = -1;
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsDrawable)
                return;
            spriteBatch.Draw(CheckBoxImages[(IsSelect) ? 1 : 0], RealRectangle, Color.White);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            base.UpdateItem(gameTime);
            if (!IsDrawable)
                return;

            if (RealRectangle.Intersects(new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1))
                && GameEngine.MouseState.LeftButton == ButtonState.Pressed && (gameTime.TotalGameTime.TotalMilliseconds -_oldTime) > 200)
            {  
                IsSelect = !IsSelect;
                OnChange.Invoke();
                _oldTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

        }

        private static Rectangle CreateRectangles(Vector2 pos, string themeName)
        {
            var theme = RessourceProvider.Themes[themeName];
            int width = Math.Max(theme["CheckBox.Normal"].Width, theme["CheckBox.Selected"].Width);
            int height = Math.Max(theme["CheckBox.Normal"].Height, theme["CheckBox.Selected"].Height);
            return new Rectangle((int)pos.X, (int)pos.Y, width, height);
        }
    }
}
