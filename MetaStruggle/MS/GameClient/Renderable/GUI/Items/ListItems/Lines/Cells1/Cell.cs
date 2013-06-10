using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items.ListItems.Lines.Cells1
{
    abstract class Cell : Item
    {
        internal Rectangle InternalRectangle;
        public bool IsSelect { get; set; }
        public NameFunc Text { get; set; }
        
        private Color ColorNormal {get; set; }
        private Color ColorSelected {get; set; }
        private SpriteFont Font { get; set; }

        protected Cell(NameFunc text, Point position, PosOnScreen pos, SpriteFont font, Color colorNormal, Color colorSelected)
            : base(CreateRectangle(position, font, text), pos)
        {
            InternalRectangle = ItemRectangle;
            IsSelect = false;
            Text = text;
            Font = font;
            ColorNormal = colorNormal;
            ColorSelected = colorSelected;
        }
        
        private static Rectangle CreateRectangle(Point position, SpriteFont font, NameFunc text)
        {
            var value = font.MeasureString(text.Invoke());
            return new Rectangle(position.X, position.Y, (int)value.X + 1, (int)value.Y + 1);
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text.Invoke(), new Vector2(InternalRectangle.X, InternalRectangle.Y), (IsSelect) ? ColorSelected : ColorNormal);
        }

        public override void UpdateItem(GameTime gameTime)
        {
        }
    }
}