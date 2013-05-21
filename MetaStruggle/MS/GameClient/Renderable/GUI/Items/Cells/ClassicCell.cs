using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Items.Cells
{
    class ClassicCell : Cell
    {
        public ClassicCell(NameFunc text, Point position, PosOnScreen pos, SpriteFont font, Color colorNormal, Color colorSelected)
            : base(text, position, pos, font, colorNormal, colorSelected) { }
        public ClassicCell(string text, Point position, PosOnScreen pos, SpriteFont font, Color colorNormal,Color colorSelected)
            : this(() => text, position, pos, font, colorNormal, colorSelected) { }
    }
}

