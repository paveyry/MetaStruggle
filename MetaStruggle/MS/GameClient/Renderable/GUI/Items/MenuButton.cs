using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items
{
    class MenuButton : Item
    {
        public delegate Texture2D TextureFunc(bool isSelect);

        public string Id { get; set; }
        public NameFunc Text { get; set; }
        public bool IsSelect { get; set; }
        private TextureFunc Image { get; set; }
        private SpriteFont Font { get; set; }
        private Color ColorNormal { get; set; }
        private Color ColorSelected { get; set; }
        private Event OnClick { get; set; }
        private Vector2 MiddlePos { get; set; }
        private Rectangle SelectedRectangle
        {
            get
            {
                var select = Image.Invoke(true);
                return GetRectangle(select.Width, select.Height);
            }
        }
        private Rectangle NormalRectangle
        {
            get
            {
                var select = Image.Invoke(false);
                return GetRectangle(select.Width, select.Height);
            }
        }

        public MenuButton(string id, Vector2 pos, NameFunc text, TextureFunc image, SpriteFont font, Color normal, Color selected, Event onClick)
            : base(new Rectangle((int)pos.X, (int)pos.Y, 0, 0))
        {
            Id = id;
            MiddlePos = pos;
            Text = text;
            Image = image;
            Font = font;
            ColorNormal = normal;
            ColorSelected = selected;
            OnClick = onClick;

        }
        public MenuButton(string id, Vector2 pos, SpriteFont font, Color normal, Color selected, Event onClick)
            : this(id, pos, () => GameEngine.LangCenter.GetString(id), (isSelect) => GameEngine.LangCenter.GetImage(id, isSelect), font, normal, selected, onClick) { }
        public MenuButton(string id, Vector2 pos, SpriteFont font, Event onClick) : this(id, pos, font, Color.White, Color.White, onClick) { }

        public Rectangle GetRectangle(int width, int height)
        {
            if (Image != null)
                return new Rectangle((int)(MiddlePos.X - width / 2f), (int)(MiddlePos.Y - height / 2f), width, height);
            var selectText = Font.MeasureString(Text.Invoke());
            return GetRectangle((int)selectText.X, (int)selectText.Y);
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Image != null)
                if (IsSelect)
                    spriteBatch.Draw(Image.Invoke(IsSelect), NormalRectangle, ColorNormal);
                else
                    spriteBatch.Draw(Image.Invoke(IsSelect), SelectedRectangle, ColorSelected);
            else
            {
                var temp = SelectedRectangle.Location;
                spriteBatch.DrawString(Font, Text.Invoke(), new Vector2(temp.X, temp.Y), (IsSelect) ? ColorSelected : ColorNormal);
            }
        }

        public override void UpdateItem(GameTime gameTime)
        {
            base.UpdateItem(gameTime);
        }
    }
}
