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
        public delegate float RatioFunc();

        public string Id { get; set; }
        public NameFunc Text { get; set; }
        public bool IsSelect { get; set; }
        private TextureFunc Image { get; set; }
        private RatioFunc Ratio { get; set; }
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
        private bool AbstractPos { get; set; }

        public MenuButton(string id, Vector2 pos, StatusMenuButton status, bool abstractPos, NameFunc text, TextureFunc image,
            RatioFunc ratioFunc,SpriteFont font, Color normal, Color selected, Event onClick, bool isDrawable = true)
            : base(new Rectangle((int)pos.X, (int)pos.Y, 0, 0),isDrawable)
        {
            Id = id;
            MiddlePos = (abstractPos) ? Position : pos;
            Status = status;
            AbstractPos = abstractPos;
            Text = text;
            Image = image;
            Ratio = ratioFunc;
            Font = font;
            ColorNormal = normal;
            ColorSelected = selected;
            OnClick = onClick;
            UpdateRectangles();
        }
        public MenuButton(string id, Vector2 pos, StatusMenuButton status, bool abstractPos, SpriteFont font,
            Color normal, Color selected, Event onClick, bool isDrawable = true)
            : this(id, pos, status,abstractPos, () => GameEngine.LangCenter.GetString(id), 
            (isSelect) => GameEngine.LangCenter.GetImage(id, !isSelect),() => GameEngine.Config.ResolutionWidth/1920f
            , font, normal, selected, onClick, isDrawable) { }
        public MenuButton(string id, Vector2 pos, SpriteFont font, Color normal, Color selected, Event onClick, bool isDrawable = true) 
            : this(id, pos, StatusMenuButton.None,true, font, normal, selected, onClick, isDrawable) { }

        private Rectangle GetRectangle(bool isSelected)
        {
            var image = Image.Invoke(isSelected);
            var text = Font.MeasureString(Text.Invoke());
            var ratio = Ratio.Invoke();
            switch (Status)
            {
                case StatusMenuButton.Vertical:
                    return image != null
                        ? new Rectangle((int)(MiddlePos.X - (image.Width*ratio) / 2f), (int)(MiddlePos.Y), (int)(image.Width * ratio), (int)(image.Height * ratio))
                        : new Rectangle((int)(MiddlePos.X - text.X / 2f), (int)(MiddlePos.Y), (int)text.X, (int)text.Y);
                case StatusMenuButton.Horizontal:
                    return image != null
                        ? new Rectangle((int)(MiddlePos.X), (int)(MiddlePos.Y - (image.Height*ratio) / 2f), (int)(image.Width * ratio), (int)(image.Height * ratio))
                        : new Rectangle((int)(MiddlePos.X), (int)(MiddlePos.Y - text.Y / 2f), (int)text.X, (int)text.Y);
                default:
                    return image != null
                        ? new Rectangle((int)(MiddlePos.X), (int)(MiddlePos.Y), (int)(image.Width * ratio), (int)(image.Height * ratio))
                        : new Rectangle((int)(MiddlePos.X), (int)(MiddlePos.Y), (int)text.X, (int)text.Y);
            }
        }

        internal void UpdateRectangles()
        {
            if (AbstractPos)
                MiddlePos = Position;
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
            base.UpdateItem(gameTime);
            if (!IsDrawable)
                return;
            IsSelect = NormalRectangle.Intersects(new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1));
            if (IsSelect && GameEngine.MouseState.LeftButton == ButtonState.Pressed)
            {
                OnClick.Invoke();
                IsSelect = false;
            }
        }

        internal override void UpdateResolution()
        {
            base.UpdateResolution();
            UpdateRectangles();
        }
    }
}
