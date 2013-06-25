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
        private bool EscPop { get; set; }
        public delegate void UpdateDelegate(GameTime gameTime);
        public UpdateDelegate UpdateFunc { get; set; }

        public Menu(Texture2D background, bool escPop = true)
        {
            Background = background;
            EscPop = escPop;
            Items = new Dictionary<string, Item>();
            Mouse = RessourceProvider.Cursors["thunder"];
            UpdateFunc = time => {};
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
            for (int i = 0; i < Items.Values.Count; i++)
                Items.Values.ElementAt(i).DrawItem(gameTime,spriteBatch);

            DrawMouse(spriteBatch);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            if (EscPop && GameEngine.KeyboardState.IsKeyDown(Keys.Escape))
            {
                System.Threading.Thread.Sleep(200);
                if (GameEngine.DisplayStack.Count > 1)
                    GameEngine.DisplayStack.Pop();
            }
            for (int i = 0; i < Items.Values.Count; i++)
                Items.Values.ElementAt(i).UpdateItem(gameTime);
            UpdateFunc(gameTime);
        }

        public void DrawMouse(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Mouse,new Vector2(GameEngine.MouseState.X,GameEngine.MouseState.Y),Color.White);
        }
    }
}
