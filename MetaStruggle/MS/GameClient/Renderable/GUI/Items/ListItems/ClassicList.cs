using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Renderable.GUI.Items.ListItems.Lines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items.ListItems
{
    class ClassicList : NormalList
    {
        public string[] Selected { get { return (LineSelected as ClassicLine).Elements; } }
        public ClassicList(Rectangle abstractRectangle, IEnumerable<string[]> element,Dictionary<string,int> field,
            SpriteFont font, Color colorNormal, Color colorSelected, string theme, bool isDrawable = true)
            : base(abstractRectangle, CreateLines(element, field.Values.ToArray(),font,colorNormal,colorSelected),
            field,font,colorNormal,theme,isDrawable)
        {
        }

        static List<ILine> CreateLines(IEnumerable<string[]> element, int[] fieldsWidth, SpriteFont font, Color colorNormal, Color colorSelected)
        {
            return element.Select(stringse => CreateClassicLine(stringse, fieldsWidth, font, colorNormal, colorSelected)).Cast<ILine>().ToList();
        }
    }
}
