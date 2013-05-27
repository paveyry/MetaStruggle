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
    public class MenuButton : Item
    {
        public enum StatusMenuButton
        {
            None,
            Vertical,
            Horizontal
        }

        public delegate Texture2D TextureFunc(bool isSelect);

        public string Id { get; set; }
        public NameFunc Text { get; set; }
        public bool IsSelect { get; set; }
        private TextureFunc Image { get; set; }
        private SpriteFont Font { get; set; }
        private Color ColorNormal { get; set; }
        private Color ColorSelected { get; set; }
        private Event OnClick { get; set; }
        public Vector2 MiddlePos { get; set; }
        public Vector2 DimRectangles
        {
            get
            {
                return new Vector2(Math.Max(SelectedRectangle.Width, NormalRectangle.Width), Math.Max(SelectedRectangle.Height, NormalRectangle.Height));
            }
        }
        private Rectangle SelectedRectangle { get; set; }
        private Rectangle NormalRectangle { get; set; }
        private StatusMenuButton Status { get; set; }

        public MenuButton(string id, Vector2 pos, StatusMenuButton status, bool abstractPos, NameFunc text, TextureFunc image, SpriteFont font, Color normal, Color selected, Event onClick, bool isDrawable = true)
            : base(new Rectangle((int)pos.X, (int)pos.Y, 0, 0),isDrawable)
        {
            Id = id;
            MiddlePos = (abstractPos) ? Position : pos;
            Status = status;
            Text = text;
            Image = image;
            Font = font;
            ColorNormal = normal;
            ColorSelected = selected;
            OnClick = onClick;
            UpdateRectangles();
        }
        public MenuButton(string id, Vector2 pos, StatusMenuButton status, bool abstractPos, SpriteFont font, Color normal, Color selected, Event onClick, bool isDrawable = true)
            : this(id, pos, status,abstractPos, () => GameEngine.LangCenter.GetString(id), 
            (isSelect) => GameEngine.LangCenter.GetImage(id, !isSelect), font, normal, selected, onClick, isDrawable) { }
        public MenuButton(string id, Vector2 pos, SpriteFont font, Color normal, Color selected, Event onClick, bool isDrawable = true) 
            : this(id, pos, StatusMenuButton.None,true, font, normal, selected, onClick, isDrawable) { }

        private Rectangle GetRectangle(bool status)
        {
            var image = Image.Invoke(status);
            var text = Font.MeasureString(Text.Invoke());
            switch (Status)
            {
                case StatusMenuButton.Vertical:
                    return image != null
                        ? new Rectangle((int)(MiddlePos.X - image.Width / 2f), (int)(MiddlePos.Y), image.Width, image.Height)
                        : new Rectangle((int)(MiddlePos.X - text.X / 2f), (int)(MiddlePos.Y), (int)text.X, (int)text.Y);

                case StatusMenuButton.Horizontal:
                    return image != null
                        ? new Rectangle((int)(MiddlePos.X), (int)(MiddlePos.Y - image.Height / 2f), image.Width, image.Height)
                        : new Rectangle((int)(MiddlePos.X), (int)(MiddlePos.Y - text.Y / 2f), (int)text.X, (int)text.Y);
                default:
                    return image != null
                        ? new Rectangle((int)(MiddlePos.X), (int)(MiddlePos.Y), image.Width, image.Height)
                        : new Rectangle((int)(MiddlePos.X), (int)(MiddlePos.Y), (int)text.X, (int)text.Y);
            }
        }

        internal void UpdateRectangles()
        {
            NormalRectangle = GetRectangle(false);
            SelectedRectangle = GetRectangle(true);
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsDrawable)
                return;
            if (Image.Invoke(IsSelect) != null)
                spriteBatch.Draw(Image.Invoke(IsSelect), (IsSelect) ? SelectedRectangle : NormalRectangle, ColorNormal);
            else
                spriteBatch.DrawString(Font, Text.Invoke(), new Vector2(NormalRectangle.Location.X, NormalRectangle.Location.Y),
                                       (IsSelect) ? ColorSelected : ColorNormal);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            if (!IsDrawable)
                return;
            IsSelect = NormalRectangle.Intersects(new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1));
            if (IsSelect && GameEngine.MouseState.LeftButton == ButtonState.Pressed)
            {
                OnClick.Invoke();
                IsSelect = false;
            }
            base.UpdateItem(gameTime);
        }
    }
}
