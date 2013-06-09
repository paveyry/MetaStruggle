using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Renderable.GUI.Items.ListItems.Lines.Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items.ListItems.Lines
{
    internal abstract class Line : Item
    {
        public Rectangle InternalRectangle { get { return RealRectangle; } set
        {
            RealRectangle = value;
//
        } }
        public int PosYInternalRectangle
        {
            get { return RealRectangle.Y; }
            set { RealRectangle = new Rectangle(RealRectangle.X, value, RealRectangle.Width, RealRectangle.Height); }
        }
        List<Cell> Cells { get; set; }
        private bool _isSelect;
        public bool IsSelect
        {
            get { return _isSelect; }
            set
            {
                _isSelect = value;
                foreach (var cell in Cells)
                    cell.IsSelect = value;
            }
        }

        protected Line(List<Cell> cells, bool isDrawable = true)
            : base(new Rectangle(), isDrawable)
        {
            Cells = cells;
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var cell in Cells)
                cell.DrawCell(spriteBatch);
        }

        public override void UpdateItem(GameTime gameTime)
        {
        }

    }
}
