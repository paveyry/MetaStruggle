using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items.ListItems.Lines.Cells1
{
    internal class ClassicCell : Cell
    {
        internal ClassicCell(NameFunc text, Point position, PosOnScreen pos, SpriteFont font, Color colorNormal, Color colorSelected)
            : base(text, position, pos, font, colorNormal, colorSelected) { }
        internal ClassicCell(string text, Point position, PosOnScreen pos, SpriteFont font, Color colorNormal,Color colorSelected)
            : this(() => text, position, pos, font, colorNormal, colorSelected) { }
    }
}

