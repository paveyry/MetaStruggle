using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Global.InputManager
{

    public class UniversalKeys
    {
        public enum Device
        {
            Keyboard,
            Mouse,
            GamePad
        }

        #region Fields
        public Device KeyDevice;

        private readonly Keys _keyboardKey;
        private readonly MouseButton _mouseKey;
        private readonly Buttons _gamePadKey;
        private readonly int _gamePadNb;
        #endregion

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
                    if ((!int.TryParse(keysElement[1], out _gamePadNb) && --_gamePadNb >= 0 && _gamePadNb < 4)
                        || (!Enum.TryParse(keysElement[2], out _gamePadKey)))
                        throw new Exception("Invalid gamepad entry");
                    return;
            }
            _gamePadNb = -1;
        }

        public bool IsPressed()
        {
            switch (KeyDevice)
            {
                case Device.Keyboard:
                    return GameEngine.KeyboardState.IsKeyDown(_keyboardKey);
                case Device.Mouse:
                    return (ButtonState)(typeof(MouseState).GetProperty(_mouseKey.ToString())
                        .GetValue(GameEngine.MouseState, null)) == ButtonState.Pressed;
                default:
                    return GameEngine.GamePadState[_gamePadNb].IsButtonDown(_gamePadKey);
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
                    key = _gamePadNb + "." + _gamePadKey.ToString();
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