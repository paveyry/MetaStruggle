using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Items.Cells
{
    class KeySelectorCell : Cell
    {      
        private readonly string _keyToModify;

        public KeySelectorCell(NameFunc text, string keyToModify, Point position, PosOnScreen pos, SpriteFont font,
                               Color colorNormal, Color colorSelected)
            : base(text, position, pos, font, colorNormal, colorSelected)
        {
            _keyToModify = keyToModify;
        }

        public override void UpdateItem(GameTime gameTime)
        {
            if (IsSelect)
            {
                var keys = GameEngine.KeyboardState.GetPressedKeys();
                if (keys.Length >= 1)
                {
                    foreach (var key in keys)
                        RessourceProvider.InputKeys[_keyToModify] = key;
                    IsSelect = false;
                }
                return;
            }
            base.UpdateItem(gameTime);
        }
    }
}
