using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using GameClient.Renderable.GUI.Items.ListItems.Lines.Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Items.ListItems.Lines
{
    internal abstract class Line : Item
    {
        public Rectangle InternalRectangle
        {
            get { return RealRectangle; }
            set
            {
                RealRectangle = value;
                UpdateCells();
            }
        }
        public int PosYInternalRectangle
        {
            get { return InternalRectangle.Y; }
            set { InternalRectangle = new Rectangle(InternalRectangle.X, value, InternalRectangle.Width, InternalRectangle.Height); }
        }
        internal List<Cell> Cells { get; set; }
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
            if (!IsDrawable)
                return;

            foreach (var cell in Cells)
                cell.DrawCell(spriteBatch);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            if (!IsDrawable)
                return;

            IsSelect = (InternalRectangle.Intersects(new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1))
                && GameEngine.MouseState.LeftButton == ButtonState.Pressed) || IsSelect;
        }

        public virtual void UpdateCells()
        {
        }
    }
}
