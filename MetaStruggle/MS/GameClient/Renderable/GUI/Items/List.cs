using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items
{
    class ListTexts : Item
    {
        public ListTexts(List<string[]> list, Rectangle abstractRectangle, Color normalColor, Color selectedColor) : base(abstractRectangle, true)
        {
            //flemme de continuer aujourd'hui
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void UpdateItem(GameTime gameTime)
        {
            base.UpdateItem(gameTime);
        }
    }
}
