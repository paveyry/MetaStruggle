using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Renderable.GUI.Items.ListItems.Lines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items.ListItems
{
    class ClassicList : ListLines
    {
        public ClassicList(Rectangle abstractRectangle, IEnumerable<string[]> element, IEnumerable<string> field, int[] fieldsWidth,
            SpriteFont font, Color colorNormal, Color colorSelected, string theme, bool isDrawable = true) 
            : base(abstractRectangle,CreateLines(element,fieldsWidth, font, colorNormal,colorSelected),
            CreateLineOrField(field, fieldsWidth, font, colorNormal, colorNormal),GetLineHeight(font),theme,"ListLines", isDrawable)
        {
            
        }

        static List<Line> CreateLines(IEnumerable<string[]> element, int[] fieldsWidth, SpriteFont font, Color colorNormal, Color colorSelected)
        {
            return element.Select(stringse => CreateLineOrField(stringse, fieldsWidth, font, colorNormal, colorSelected)).ToList();
        }

        static Line CreateLineOrField(IEnumerable<string> line, int[] fieldsWidth, SpriteFont font, Color colorNormal, Color colorSelected)
        {
            return new ClassicLine(line,fieldsWidth,font, colorNormal, colorSelected);
        }
    }
}
