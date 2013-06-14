using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable._3D
{
    public interface I3DElement
    {
        float XRotation { get; set; }
        float YRotation { get; set; }
        float ZRotation { get; set; }
        float Yaw { get; set; }
        float Pitch { get; set; }
        float Roll { get; set; }
        Vector3 Position { get; set; }
        Vector3 Scale { get; set; }
        string ModelName { get; set; }
        SceneManager Scene { get; set; }
        float Gravity { get; set; }
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
