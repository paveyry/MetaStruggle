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
        public Rectangle ItemRectangle;
        public Rectangle RealRectangle;
        //private Rectangle _realRectangle
        //{
        //    get
        //    {
        //        return new Rectangle((ItemRectangle.Location.X / 100) * GameEngine.Config.ResolutionHeight,
        //            (ItemRectangle.Location.Y) / 100 * GameEngine.Config.ResolutionWidth,
        //            (ItemRectangle.Width / 100) * GameEngine.Config.ResolutionHeight,
        //            (ItemRectangle.Height / 100) * GameEngine.Config.ResolutionWidth);
        //    }
        //    set
        //    {
        //        ItemRectangle.Location = new Point((value.Location.X / GameEngine.Config.ResolutionHeight) * 100, (value.Location.Y / GameEngine.Config.ResolutionWidth) * 100);
        //        ItemRectangle.Height = (value.Height / GameEngine.Config.ResolutionHeight) * 100;
        //        ItemRectangle.Width = (value.Width / GameEngine.Config.ResolutionWidth) * 100;
        //    }
        //}
        public Vector2 Position { get { return new Vector2(RealRectangle.Location.X, RealRectangle.Location.Y); } }
        public Vector2 PositionItem {get {return new Vector2(ItemRectangle.Location.X,ItemRectangle.Location.Y);}}
        public PosOnScreen Pos;
        private bool IsExpandable;

        private Item(Rectangle rectangle, PosOnScreen pos, bool isExpandable)
        {
            IsExpandable = isExpandable;
            ItemRectangle = rectangle;
            Pos = pos;
            UpdateRectangle();
        }
        public Item(Rectangle rectangle, PosOnScreen pos) : this(rectangle, pos, false) { }
        public Item(Rectangle rectangle, bool isExpandable) : this(rectangle, PosOnScreen.TopLeft, isExpandable) { }
        public Item(Rectangle rectangle) : this(rectangle, PosOnScreen.TopLeft) { }

        public virtual void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public virtual void UpdateItem(GameTime gameTime)
        {
            UpdateRectangle();
        }

        internal virtual void test()
        {
            
        }

        void UpdateRectangle()
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
    }
}
