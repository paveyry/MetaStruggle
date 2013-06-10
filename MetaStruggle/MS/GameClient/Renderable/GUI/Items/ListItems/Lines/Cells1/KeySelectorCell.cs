using System.Linq;
using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items.ListItems.Lines.Cells1
{
    internal class KeySelectorCell : Cell
    {      
        private readonly string _keyToModify;
        private double _startMilliseconds;

        internal  KeySelectorCell(NameFunc text, string keyToModify, Point position, PosOnScreen pos, SpriteFont font,
                               Color colorNormal, Color colorSelected)
            : base(text, position, pos, font, colorNormal, colorSelected)
        {
            _keyToModify = keyToModify;
            _startMilliseconds = -1;
        }

        public override void UpdateItem(GameTime gameTime)
        {
            if (IsSelect)
            {
                if (_startMilliseconds < 0)
                    _startMilliseconds = gameTime.TotalGameTime.TotalMilliseconds;
                if (_startMilliseconds > 0 && gameTime.TotalGameTime.TotalMilliseconds - _startMilliseconds < 200)
                    return;
                var keys = GameEngine.InputDevice.GetPressedKeys(1);
                if (keys.Count >= 1)
                {
                    RessourceProvider.InputKeys[_keyToModify] = keys.First();
                    IsSelect = false;
                    _startMilliseconds = -1;
                }
                return;
            }
            base.UpdateItem(gameTime);
        }
    }
}
