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
        private readonly Device _keyDevice;

        private readonly Keys _keyboardKey;
        private readonly MouseButton _mouseKey;
        private readonly Buttons _gamePadKey;
        private readonly int _gamePadNb;

        private delegate bool IsPressedDelegate();
        private IsPressedDelegate Pressed { get; set; }
        #endregion

        public UniversalKeys(string key)
        {
            var keysElement = key.Split('.');
            if (!Enum.TryParse(keysElement[0], out _keyDevice))
                throw new Exception("Invalid entry");
            switch (_keyDevice)
            {
                case Device.Keyboard:
                    if (!Enum.TryParse(keysElement[1], out _keyboardKey))
                        throw new Exception("Invalid keyboard entry");
                    Pressed = KeyboardPressed;
                    break;
                case Device.Mouse:
                    if (!Enum.TryParse(keysElement[1], out _mouseKey))
                        throw new Exception("Invalid mouse entry");
                    Pressed = MousePressed;
                    break;
                case Device.GamePad:
                    if ((!int.TryParse(keysElement[1], out _gamePadNb) && _gamePadNb > 0 && _gamePadNb <= 4)
                        || (!Enum.TryParse(keysElement[2], out _gamePadKey)))
                        throw new Exception("Invalid gamepad entry");
                    _gamePadNb--;
                    Pressed = GamePadPressed;
                    return;
            }
            _gamePadNb = -1;
        }

        public bool IsPressed()
        {
            return Pressed.Invoke();
        }

        private bool KeyboardPressed()
        {
            return GameEngine.KeyboardState.IsKeyDown(_keyboardKey);
        }

        private bool MousePressed()
        {
            return (ButtonState)(typeof(MouseState).GetProperty(_mouseKey.ToString())
                        .GetValue(GameEngine.MouseState, null)) == ButtonState.Pressed;
        }

        private bool GamePadPressed()
        {
            return GameEngine.GamePadState[_gamePadNb].IsButtonDown(_gamePadKey);
        }

        public override string ToString()
        {
            string key;
            switch (_keyDevice)
            {
                case Device.Keyboard:
                    key = _keyboardKey.ToString();
                    break;
                case Device.Mouse:
                    key = _mouseKey.ToString();
                    break;
                default:
                    key = (_gamePadNb+1) + "." + _gamePadKey.ToString();
                    break;
            }
            return _keyDevice.ToString() + "." + key;
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