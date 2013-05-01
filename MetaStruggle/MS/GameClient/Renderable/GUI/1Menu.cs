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
        private Texture2D Background;

        public Menu1(Texture2D background)
        {
            Background = background;
            _elements=new List<Element>();
        }

        public void Add(Element e)
        {
            _elements.Add(e);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            DrawBackground(spriteBatch);
            foreach (var element in _elements)
                element.DrawElement(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var element in _elements)
                element.UpdateElement(gameTime);
        }

        void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background,
                 new Rectangle(0, 0, Global.GameEngine.Config.ResolutionWidth,
                               Global.GameEngine.Config.ResolutionHeight), Color.White);
        }
    }
}
