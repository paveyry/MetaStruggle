using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GameClient.Lang;
using GameClient.Menus;
using GameClient.Renderable.GUI;
using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameClient.Global;

namespace GameClient
{
    public class Game1 : Game
    {
        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => { GameEngine.SaveDatas(); GameEngine.SoundCenter.StopAll(); };
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            GameEngine.InitializeEngine(Content, _graphics);
            GameEngine.Config.ApplyConfig(_graphics);
            Window.Title = GameEngine.Config.GameName;
            GameEngine.DisplayStack.Push(new MainMenu(_spriteBatch, _graphics).CreateMainMenu());
            GameEngine.DisplayStack.Push(new Cinematic(RessourceProvider.Videos["Intro"]));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            GameEngine.UpdateEngine();
            GameEngine.DisplayStack.Peek().Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GameEngine.DisplayStack.Peek().Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}
