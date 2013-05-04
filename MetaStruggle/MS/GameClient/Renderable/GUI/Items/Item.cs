using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Items
{
    public class Item
    {
        public delegate void Event();

        public Rectangle ItemRectangle;
        public Vector2 Position { get { return new Vector2(ItemRectangle.Location.X, ItemRectangle.Y); } }

        public Item(Rectangle rectangle)
        {
            ItemRectangle = rectangle;
        }

        public virtual void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public virtual void UpdateItem(GameTime gameTime)
        {
        }
    }
}
