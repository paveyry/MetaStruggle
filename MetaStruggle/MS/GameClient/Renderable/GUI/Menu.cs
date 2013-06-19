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
    public class Menu : Layout.IBasicLayout
    {
        public Dictionary<string, Item> Items { get; set; }
        private Texture2D Background { get; set; }
        private Texture2D Mouse { get; set; }

        public Menu(Texture2D background)
        {
            Background = background;
            Items = new Dictionary<string, Item>();
            Mouse = RessourceProvider.Cursors["thunder"];
        }

        public void Add(string key, Item item)
        {
            if (Items.ContainsKey(key))
                Items["key"] = item;
            else
                Items.Add(key, item);
        }

        public void UpdateResolution()
        {
            foreach (var item in Items)
                item.Value.UpdateResolution();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Background, new Rectangle(0, 0, GameEngine.Config.ResolutionWidth,
                               GameEngine.Config.ResolutionHeight), Color.White);
            foreach (var item in Items)
                item.Value.DrawItem(gameTime, spriteBatch);
            DrawMouse(spriteBatch);
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
        }

        public void DrawMouse(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Mouse,new Vector2(GameEngine.MouseState.X,GameEngine.MouseState.Y),Color.White);
        }
    }
}
