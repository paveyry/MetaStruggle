using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GameClient.Renderable._3D
{
    public class Skybox
    {
        public Texture2D Image { get; set; }
        public Video Video { get; set; }
        private VideoPlayer _player;

        public Skybox(Texture2D image)
        {
            Image = image;
        }

        public Skybox(Video video)
        {
            Video = video;
            _player = new VideoPlayer();
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Video != null && _player.State != MediaState.Playing)
            {
                _player.Play(Video);
                _player.IsLooped = true;
                _player.IsMuted = true;
            }

            spriteBatch.Begin();
                spriteBatch.Draw(Video == null ? Image : _player.GetTexture(),
                                 new Rectangle(0, 0, Global.GameEngine.Config.ResolutionWidth,
                                               Global.GameEngine.Config.ResolutionHeight), Color.White);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        public void Pause()
        {
            if (Video != null)
                _player.Pause();
        }
    }
}
