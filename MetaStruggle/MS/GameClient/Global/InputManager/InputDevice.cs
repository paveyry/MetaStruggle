using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Global.InputManager
{
    public class InputDevice
    {
        public List<UniversalKeys> GetPressedKeys(int nbPlayer)
        {
            var ks = GameEngine.KeyboardState;
            var gs = GameEngine.GamePadState;

            var value = ks.GetPressedKeys().Select(key => new UniversalKeys("Keyboard." + key.ToString())).ToList();
            value.AddRange(from Buttons button in Enum.GetValues(typeof (Buttons))
                           where gs[nbPlayer].IsButtonDown(button)
                           select new UniversalKeys("GamePad." + button.ToString()));
            value.AddRange(from MouseButton mouseButton in Enum.GetValues(typeof (MouseButton))
                           where
                               (ButtonState)
                               (typeof (MouseState).GetProperty(mouseButton.ToString())
                                                   .GetValue(GameEngine.MouseState, null)) == ButtonState.Pressed
                           select new UniversalKeys("Mouse." + mouseButton.ToString()));
            return value;
        }

        public bool UniversalKeysIsDown(string key)
        {
            return new UniversalKeys(key).IsPressed();
        }

    }
}