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
        public delegate void Event();
        public delegate string NameFunc();
        public Rectangle ItemRectangle;
        public Rectangle RealRectangle;
        public Vector2 Position { get { return new Vector2(RealRectangle.Location.X, RealRectangle.Location.Y); } }
        public Vector2 PositionItem {get {return new Vector2(ItemRectangle.Location.X,ItemRectangle.Location.Y);}}
        public PosOnScreen Pos;
        public bool IsExpandable;

        private Item(Rectangle rectangle, PosOnScreen pos, bool isExpandable)
        {
            IsExpandable = isExpandable;
            ItemRectangle = rectangle;
            Pos = pos;
            UpdateResolution();
        }

        protected Item(Rectangle rectangle, PosOnScreen pos) : this(rectangle, pos, false) { }
        protected Item(Rectangle rectangle, bool isExpandable) : this(rectangle, PosOnScreen.TopLeft, isExpandable) { }
        protected Item(Rectangle rectangle) : this(rectangle, PosOnScreen.TopLeft) { }

        public virtual void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public virtual void UpdateItem(GameTime gameTime)
        {
        }

        protected virtual void UpdateResolution()
        {
            switch (Pos)
            {
                case PosOnScreen.TopLeft:
                    RealRectangle = IsExpandable ? new Rectangle((int)((ItemRectangle.Location.X / 100f) * Width), (int)((ItemRectangle.Location.Y / 100f) * Height),
                        (int)((ItemRectangle.Width / 100f) * Width), (int)((ItemRectangle.Height / 100f) * Height))
                        : new Rectangle((int)((ItemRectangle.Location.X / 100f) * Width),(int) ((ItemRectangle.Location.Y / 100f) * Height),
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
