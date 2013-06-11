using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global.InputManager;
using GameClient.Renderable.Layout;
using GameClient.SoundEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework.Input;
using Network;

namespace GameClient.Global
{
    public static class GameEngine
    {
        public static Config Config { get; set; }
        public static LayoutStack<IBasicLayout> DisplayStack { get; set; }
        public static SceneManager SceneManager { get; set; }
        public static SoundCenter SoundCenter { get; set; }
        public static LanguageLoader LangCenter { get; set; }
        public static EventManager EventManager { get; set; }
        public static KeyboardState KeyboardState { get; set; }
        public static MouseState MouseState { get; set; }
        public static GamePadState[] GamePadState { get; set; }
        public static InputDevice InputDevice { get; set; }


        public static void InitializeEngine(ContentManager content, GraphicsDeviceManager graphics)
        {
            EventManager = new EventManager();
            Config = IO.ConfigSerialization.LoadFile("config.xml");
            LangCenter = new LanguageLoader(graphics.GraphicsDevice);
            RessourceProvider.Fill(content);
            SoundCenter = SoundCenter.Instance;
            DisplayStack = new LayoutStack<IBasicLayout>();
            InputDevice = new InputDevice();
        }

        public static void UpdateEngine()
        {
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            GamePadState = new GamePadState[4];
            GamePadState[0] = GamePad.GetState(PlayerIndex.One);
            GamePadState[1] = GamePad.GetState(PlayerIndex.Two);
            GamePadState[2] = GamePad.GetState(PlayerIndex.Three);
            GamePadState[3] = GamePad.GetState(PlayerIndex.Four);
        }

        public static void SaveDatas()
        {
            IO.ConfigSerialization.SaveFile("config.xml", Config);
        }
    }
}
