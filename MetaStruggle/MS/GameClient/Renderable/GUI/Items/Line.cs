using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items
{
    internal class Line : Item
    {
        SpriteFont Font { get; set; }


        public Line(Rectangle rectangle) : base(rectangle)
        {
            
        }

        

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawItem(gameTime, spriteBatch);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            base.UpdateItem(gameTime);
        }
    }
}
