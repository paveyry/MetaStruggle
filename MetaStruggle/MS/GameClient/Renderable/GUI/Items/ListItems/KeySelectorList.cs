using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global.InputManager;
using GameClient.Renderable.GUI.Items.ListItems.Lines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items.ListItems
{
    class KeySelectorList : NormalList
    {
        public KeySelectorList(Rectangle abstractRectangle, IEnumerable<string[]> elements,Dictionary<string,UniversalKeys> keys,
            int indexKey, Dictionary<string,int> field ,SpriteFont font, Color colorNormal, Color colorSelected, string theme, bool isDrawable = true)
            :base(abstractRectangle,CreateKeySelectorLines(elements,keys,field.Values.ToArray(),indexKey,font,colorNormal,colorSelected),
            field,font,colorNormal,theme,isDrawable)
        {
        }

        static List<ILine> CreateKeySelectorLines(IEnumerable<string[]> elements,Dictionary<string,UniversalKeys> keys,int[] fieldsWidth,
            int indexKey, SpriteFont font, Color colorNormal, Color colorSelected)
        {
            //return elements.Select(stringse => new KeySelectorLine(stringse.ToList(),ref keys, indexKey, fieldsWidth, font, colorNormal, colorSelected)).Cast<ILine>().ToList();
            var value = new List<ILine>();
            foreach (var stringse in elements)
            {
                value.Add(new KeySelectorLine(stringse.ToList(),keys, indexKey, fieldsWidth, font, colorNormal, colorSelected));
            }
            return value;
        }
    }
}
