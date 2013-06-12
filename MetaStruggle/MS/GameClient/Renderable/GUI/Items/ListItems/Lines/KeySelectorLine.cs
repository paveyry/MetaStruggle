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
        public KeySelectorLine(List<string> elements, Dictionary<string,UniversalKeys> keys,int indexPlayer, int[] fields,
            SpriteFont font, Color colorNormal,Color colorSelected, bool isDrawable = true)
            : base(CreateCells(elements,keys, indexPlayer, font, colorNormal, colorSelected),fields, isDrawable)
        {
        }

        static List<Cell> CreateCells(List<string> elements,Dictionary<string,UniversalKeys> keys, int indexPlayer, SpriteFont font, Color colorNormal, Color colorSelected)
        {
            if (elements.Count != 2)
                throw new Exception("Invalid Line");
            return new List<Cell>()
                {
                    new Cell(() => GameEngine.LangCenter.GetString(elements[0]), null, font, colorNormal, colorSelected),
                    new Cell(() => keys[elements[1]].ToString(),
                             (rectangle, time, oldTime) => ModifyKey(rectangle, time, oldTime,keys, elements[1],indexPlayer) 
                             , font, colorNormal, colorSelected)
                };
        }

        static bool ModifyKey(Rectangle internalRectangle, GameTime gameTime, double oldTime,
            Dictionary<string, UniversalKeys> keysDic, string key, int indexPlayer)
        {
            var keys = GameEngine.InputDevice.GetPressedKeys(indexPlayer);
            if (keys.Count >= 1 && (GameEngine.MouseState.LeftButton != ButtonState.Pressed ||
                                    (internalRectangle.Intersects(new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1))
                                     && gameTime.TotalGameTime.TotalMilliseconds - oldTime > 200)))
            {
                keysDic[key] = keys.First();
                System.Threading.Thread.Sleep(200);
                return false;
            }
            return true;
        }
    }
}
