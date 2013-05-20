using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Renderable.Layout;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameClient.Renderable.GUI
{
    class Cinematic : IBasicLayout
    {
        private Video _video;
        private VideoPlayer _player;
        private Texture2D _videoTexture;
        private bool played = false;

        public Cinematic(Video video)
        {
            _video = video;
            _player = new VideoPlayer();
        }

        public void Update(GameTime gameTime)
        {
            if (Global.GameEngine.KeyboardState.IsKeyDown(Keys.Escape))
            {
                Global.GameEngine.DisplayStack.Pop();
                _player.Stop();
                System.Threading.Thread.Sleep(200);
                return;
            }

            if (_player.State == MediaState.Stopped && played)
            {
                Global.GameEngine.DisplayStack.Pop();
                return;
            }

            if (_player.State != MediaState.Stopped) return;

            played = true;
            _player.Play(_video);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_player.State != MediaState.Stopped)
                _videoTexture = _player.GetTexture();

            var screen = new Rectangle(spriteBatch.GraphicsDevice.Viewport.X,
                                       spriteBatch.GraphicsDevice.Viewport.Y,
                                       spriteBatch.GraphicsDevice.Viewport.Width,
                                       spriteBatch.GraphicsDevice.Viewport.Height);

            if (_videoTexture == null) return;

            spriteBatch.Begin();
            spriteBatch.Draw(_videoTexture, screen, Color.White);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
