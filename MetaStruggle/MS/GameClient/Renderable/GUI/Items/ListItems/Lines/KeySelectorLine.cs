using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using GameClient.Global.InputManager;
using GameClient.Renderable.GUI.Items.ListItems.Lines.Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Items.ListItems.Lines
{
    class KeySelectorLine : Line
    {
        public KeySelectorLine(List<string> elements, int indexPlayer, int[] fields,
            SpriteFont font, Color colorNormal,Color colorSelected, bool isDrawable = true)
            : base(CreateCells(elements, indexPlayer, font, colorNormal, colorSelected),fields, isDrawable)
        {
        }

        static List<Cell> CreateCells(List<string> elements, int indexPlayer, SpriteFont font, Color colorNormal, Color colorSelected)
        {
            if (elements.Count != 2)
                throw new Exception("Invalid Line");
            return new List<Cell>()
                {
                    new Cell(() => GameEngine.LangCenter.GetString(elements[0]), null, font, colorNormal, colorSelected),
                    new Cell(() => RessourceProvider.InputKeys[elements[1]].ToString(),
                             (r, time, oldTime) => ModifyKey(r, time, oldTime, elements[1],indexPlayer) , font, colorNormal, colorSelected)
                };
        }

        static void ModifyKey(Rectangle internalRectangle, GameTime gameTime, double oldTime, string key, int indexPlayer)
        {
            var keys = GameEngine.InputDevice.GetPressedKeys(indexPlayer);
            if (keys.Count >= 1 && (GameEngine.MouseState.LeftButton != ButtonState.Pressed
                || (internalRectangle.Intersects(new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1)) 
                && gameTime.TotalGameTime.TotalMilliseconds - oldTime > 200)))
                RessourceProvider.InputKeys[key] = keys.First();
        }
    }
}
