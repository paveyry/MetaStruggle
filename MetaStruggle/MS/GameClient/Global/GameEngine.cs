using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Lang;
using GameClient.Renderable.Layout;
using GameClient.SoundEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using GameClient.Renderable.Scene;

namespace GameClient.Global
{
    public static class GameEngine
    {
        public static Config Config { get; set; }
        public static Stack<IBasicLayout> DisplayStack { get; set; }
        public static KeyboardState KeyboardState { get; set; }
        public static MouseState MouseState { get; set; }
        public static SceneManager SceneManager { get; set; }
        public static SoundCenter SoundCenter { get; set; }
        public static LanguageLoader LangCenter { get; set; }
        public static EventManager.EventManager EventManager { get; set; }

        public static void InitializeEngine(ContentManager content, GraphicsDeviceManager graphics)
        {
            EventManager = new EventManager.EventManager();
            Config = IO.ConfigSerialization.LoadFile("config.xml");
            LangCenter = new LanguageLoader(graphics.GraphicsDevice);
            RessourceProvider.Fill(content);
            SoundCenter = SoundCenter.Instance;
            DisplayStack = new Stack<IBasicLayout>();
        }

        public static void UpdateEngine()
        {
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
        }

        public static void SaveDatas()
        {
            IO.ConfigSerialization.SaveFile("config.xml", Config);
        }
    }
}
