using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Renderable.GUI.Items.ListItems.Lines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items.ListItems
{
    class NormalList : ListLines
    {

        public NormalList(Rectangle abstractRectangle, List<ILine> lines, Dictionary<string, int> field,
            SpriteFont font, Color colorNormal, string theme, bool isDrawable = true)
            : base(abstractRectangle, lines,
            CreateClassicLine(field.Keys.ToArray(), field.Values.ToArray(), font, colorNormal, colorNormal), GetLineHeight(font), theme, "ListLines", isDrawable)
        {
        }

        public static Line CreateClassicLine(IEnumerable<string> line, int[] fieldsWidth, SpriteFont font, Color colorNormal, Color colorSelected)
        {
            return new ClassicLine(line, fieldsWidth, font, colorNormal, colorSelected);
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawItem(gameTime, spriteBatch);
            for (int index = 1; index < Field.Cells.Count; index++)
                    spriteBatch.Draw(Theme["Separator"], new Rectangle(Field.Cells[index].InternalRectangle.X, InternalRectangle.Y,
                                                   Theme["Separator"].Width, InternalRectangle.Height), Color.White);
        }
    }
}
