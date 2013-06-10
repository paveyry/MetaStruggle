using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using GameClient.Renderable.GUI.Items.ListItems.Lines.Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items.ListItems.Lines
{
    class ClassicLine : Line
    {
        int[] Fields { get; set; }
        public ClassicLine(IEnumerable<string> elements, int[] fields, SpriteFont font, Color colorNormal, Color colorSelected, bool isDrawable = true)
            : base(CreateCells(elements,font,colorNormal,colorSelected),isDrawable)
        {
            Fields = fields;
        }

        static List<Cell> CreateCells(IEnumerable<string> elements, SpriteFont font,Color colorNormal, Color colorSelected)
        {
            return elements.Select(element => new Cell(() => GameEngine.LangCenter.GetString(element), null, font, colorNormal, colorSelected)).ToList();
        }

        public override void UpdateCells()
        {
            for (int i = 0, posOnX = InternalRectangle.X,sum = Fields.Sum(); i < Cells.Count; i++)
            {
                int width = (int)((Fields[i]/(float)sum)*InternalRectangle.Width);
                Cells[i].UpdateRectangle(new Rectangle(posOnX,InternalRectangle.Y,width,InternalRectangle.Height));
                posOnX += width;
            }
        } 
    }
}
