using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Elements
{
    public class Element
    {
        public delegate void Event();

        public Rectangle ElementRectangle;
        public Vector2 Position { get { return new Vector2(ElementRectangle.Location.X, ElementRectangle.Y); } }

        public Element(Rectangle rectangle)
        {
            ElementRectangle = rectangle;
        }

        public virtual void DrawElement(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public virtual void UpdateElement(GameTime gameTime)
        {
        }
    }
}
