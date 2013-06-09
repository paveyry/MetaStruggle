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
    public abstract class Item
    {
        public enum PosOnScreen
        {
            TopLeft, TopRight, DownRight, DownLeft
        }

        internal int Width { get { return GameEngine.Config.ResolutionWidth; } }
        internal int Height { get { return GameEngine.Config.ResolutionHeight; } }
        internal int ActualWidth { get; private set; }
        internal int ActualHeight { get; private set; }
        public delegate void Event();
        public delegate string NameFunc();
        public Rectangle ItemRectangle;
        public Rectangle RealRectangle;
        public Vector2 Position { get { return new Vector2(RealRectangle.Location.X, RealRectangle.Location.Y); } }
        public Vector2 PositionItem {get {return new Vector2(ItemRectangle.Location.X,ItemRectangle.Location.Y);}}
        public PosOnScreen Pos;
        public bool IsExpandable;
        public bool IsDrawable { get; set; }

        private Item(Rectangle rectangle, PosOnScreen pos, bool isExpandable, bool isDrawable = true)
        {
            IsDrawable = isDrawable;
            IsExpandable = isExpandable;
            ItemRectangle = rectangle;
            Pos = pos;
            ActualHeight = Height;
            ActualWidth = Width;
            SetRectangles();
        }

        protected Item(Rectangle rectangle, PosOnScreen pos, bool isDrawable = true) : this(rectangle, pos, false, isDrawable) { }
        protected Item(Rectangle rectangle, bool isExpandable, bool isDrawable = true) : this(rectangle, PosOnScreen.TopLeft, isExpandable, isDrawable) { }
        protected Item(Rectangle rectangle, bool isDrawable = true) : this(rectangle, PosOnScreen.TopLeft, isDrawable) { }

        public virtual void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public virtual void UpdateItem(GameTime gameTime)
        {
            if (ActualHeight == Height && ActualWidth == Width) return;

            UpdateResolution();
            ActualWidth = Width;
            ActualHeight = Height;
        }

        internal virtual void UpdateResolution()
        {
            SetRectangles();
        }

        private void SetRectangles()
        {
            switch (Pos)
            {
                case PosOnScreen.TopLeft:
                    RealRectangle = IsExpandable ? new Rectangle((int)((ItemRectangle.Location.X / 100f) * Width), (int)((ItemRectangle.Location.Y / 100f) * Height),
                        (int)((ItemRectangle.Width / 100f) * Width), (int)((ItemRectangle.Height / 100f) * Height))
                        : new Rectangle((int)((ItemRectangle.Location.X / 100f) * Width), (int)((ItemRectangle.Location.Y / 100f) * Height),
                            ItemRectangle.Width, ItemRectangle.Height);
                    break;
                case PosOnScreen.TopRight:
                    RealRectangle = new Rectangle(Width - (int)((ItemRectangle.Location.X / 100f) * Width), (int)((ItemRectangle.Location.Y / 100f) * Height),
                            ItemRectangle.Width, ItemRectangle.Height);
                    break;
                case PosOnScreen.DownLeft:
                    RealRectangle = new Rectangle((int)((ItemRectangle.Location.X / 100f) * Width), Height - (int)((ItemRectangle.Location.Y / 100f) * Height),
                            ItemRectangle.Width, ItemRectangle.Height);
                    break;
                case PosOnScreen.DownRight:
                    RealRectangle = new Rectangle(Width - (int)((ItemRectangle.Location.X / 100f) * Width), Height - (int)((ItemRectangle.Location.Y / 100f) * Height),
                            ItemRectangle.Width, ItemRectangle.Height);
                    break;
            }
        }

        protected static int GetLineHeight(SpriteFont font)
        {
            return (int)font.MeasureString("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789").Y + 1;
        }
    }
}
