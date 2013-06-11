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
        public string[] Elements { get; set; }
        public ClassicLine(IEnumerable<string> elements, int[] fields,
            SpriteFont font, Color colorNormal, Color colorSelected, bool isDrawable = true)
            : base(CreateCells(elements, font, colorNormal, colorSelected),fields, isDrawable)
        {
            Elements = elements.ToArray();
        }

        static List<Cell> CreateCells(IEnumerable<string> elements, SpriteFont font, Color colorNormal, Color colorSelected)
        {
            return elements.Select(element => new Cell(() => GameEngine.LangCenter.GetString(element), null, font, colorNormal, colorSelected)).ToList();
        }
    }
}
