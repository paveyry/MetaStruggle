using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Renderable.GUI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI
{
    public class Menu1 : Layout.IBasicLayout
    {
        private readonly List<Element> _elements;

        public Menu1()
        {
            _elements=new List<Element>();
        }

        public void Add(Element e)
        {
            _elements.Add(e);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var element in _elements)
                element.DrawElement(gameTime, spriteBatch);
            spriteBatch.End();
        }
        public void Update(GameTime gameTime)
        {
            foreach (var element in _elements)
                element.UpdateElement(gameTime);
        }
    }
}
