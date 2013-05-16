using System;
using System.Collections.Generic;
using System.Linq;
using GameClient.Global;
using GameClient.Renderable.GUI.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI
{
    public class Menu1 : Layout.IBasicLayout
    {
        public Dictionary<string, Item> Items;
        private Texture2D Background;

        public Menu1(Texture2D background)
        {
            Background = background;
            Items = new Dictionary<string, Item>();
        }

        public void Add(string key, Item item)
        {
            if (Items.ContainsKey(key))
                Items["key"] = item;
            else
                Items.Add(key, item);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            DrawBackground(spriteBatch);
            foreach (var item in Items)
                item.Value.DrawItem(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Items.Values.Count; i++)
                Items.Values.ElementAt(i).UpdateItem(gameTime);

            if (!GameEngine.KeyboardState.IsKeyDown(Keys.Escape)) return;
            System.Threading.Thread.Sleep(200);
            if (GameEngine.DisplayStack.Count > 1)
                GameEngine.DisplayStack.Pop();
            else
                Environment.Exit(0);
        }

        void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background,
                 new Rectangle(0, 0, Global.GameEngine.Config.ResolutionWidth,
                               Global.GameEngine.Config.ResolutionHeight), Color.White);
        }
    }
}
