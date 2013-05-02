using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Renderable.GUI.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI
{
    public class Menu1 : Layout.IBasicLayout
    {
        private readonly List<Item> _items;
        private Texture2D Background;

        public Menu1(Texture2D background)
        {
            Background = background;
            _items=new List<Item>();
        }

        public void Add(Item item)
        {
            _items.Add(item);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            DrawBackground(spriteBatch);
            foreach (var item in _items)
                item.DrawItem(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var item in _items)
                item.UpdateItem(gameTime);
        }

        void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background,
                 new Rectangle(0, 0, Global.GameEngine.Config.ResolutionWidth,
                               Global.GameEngine.Config.ResolutionHeight), Color.White);
        }
    }
}
