using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Global.InputManager
{
    class InputDevice
    {
        static public List<UniversalKeys> GetPressedKeys()
        {
            var ks = GameEngine.KeyboardState;
            var gs = GameEngine.GamePadState;

            var value = ks.GetPressedKeys().Select(key => new UniversalKeys("Keyboard." + key.ToString())).ToList();
            value.AddRange(from Buttons button in Enum.GetValues(typeof (Buttons)) where gs.IsButtonDown(button) 
                           select new UniversalKeys("GamePad." + button.ToString()));
            value.AddRange(from MouseButton mouseButton in Enum.GetValues(typeof (MouseButton)) where
                               (ButtonState) (typeof (MouseState).GetProperty(mouseButton.ToString()).GetValue(GameEngine.MouseState, null)) == ButtonState.Pressed 
                           select new UniversalKeys("Mouse." + mouseButton.ToString()));
            return value;
        }

        public static bool UniversalKeysIsDown(string key)
        {
            return new UniversalKeys(key).IsPressed();
        }

    }

    public class UniversalKeys
    {
        public enum Device
        {
            Keyboard,
            Mouse,
            GamePad
        }

        public Device KeyDevice;

        private readonly Keys _keyboardKey;
        private readonly MouseButton _mouseKey;
        private readonly Buttons _gamePadKey;

        public UniversalKeys(string key)
        {
            var keysElement = key.Split('.');
            if (!Enum.TryParse(keysElement[0], out KeyDevice))
                throw new Exception("Invalid entry");
            switch (KeyDevice)
            {
                case Device.Keyboard:
                    if (!Enum.TryParse(keysElement[1], out _keyboardKey))
                        throw new Exception("Invalid keyboard entry");
                    break;
                case Device.Mouse:
                    if (!Enum.TryParse(keysElement[1], out _mouseKey))
                        throw new Exception("Invalid mouse entry");
                    break;
                case Device.GamePad:
                    if (!Enum.TryParse(keysElement[1], out _gamePadKey))
                        throw new Exception("Invalid gamepad entry");
                    break;
            }
        }

        public bool IsPressed()
        {
            switch (KeyDevice)
            {
                case Device.Keyboard:
                    return GameEngine.KeyboardState.IsKeyDown(_keyboardKey);
                case Device.Mouse:
                    return (ButtonState)(typeof(MouseState).GetProperty(_mouseKey.ToString()).GetValue(GameEngine.MouseState, null)) == ButtonState.Pressed;
                default:
                    return GameEngine.GamePadState.IsButtonDown(_gamePadKey);
            }
        }

        public override string ToString()
        {
            string key;
            switch (KeyDevice)
            {
                case Device.Keyboard:
                    key = _keyboardKey.ToString();
                    break;
                case Device.Mouse:
                    key = _mouseKey.ToString();
                    break;
                default:
                    key = _gamePadKey.ToString();
                    break;
            }
            return KeyDevice.ToString() + "." + key;
        }
    }
}
    

    enum MouseButton
    {
        LeftButton,
        RightButton,
        MiddleButton,
        XButton1,
        XButton2
    }

