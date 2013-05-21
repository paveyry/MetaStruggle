using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using GameClient.Renderable.GUI.Items.Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI.Items
{
    internal class Line : Item
    {
        int[] Fields { get; set; }
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
        public bool IsDrawable;
        public string[] Elements { get; set; }

        public Line(Rectangle rectangle, string[] elements, int[] fields, SpriteFont font, Color colorNormal,
            Color colorSelected, bool isDrawable, bool isNormal)
            : base(rectangle)
        {
            Fields = fields;
            Elements = elements;
            Cells = new List<Cell>();
            IsDrawable = isDrawable;

            int width = (int)PositionItem.X;
            if (isNormal)
                AddClassicCell(font, colorNormal, colorSelected, ref width, 0);
            else
            {
                AddClassicCell(font, colorNormal, colorSelected, ref width, 1);
                Cells.Add(new KeySelectorCell(() => RessourceProvider.InputKeys[elements[elements.Length - 1]].ToString(),
                                elements[elements.Length - 1],new Point(width, (int)PositionItem.Y),
                                PosOnScreen.TopLeft, font, colorNormal, colorSelected));
            }
        }

        void AddClassicCell(SpriteFont font, Color colorNormal, Color colorSelected,ref int width, int length)
        {

            for (int i = 0; i < Elements.Length - length; width += Fields[i], i++)
                if (Lang.Language.GetString(Elements[i]) == Elements[i])
                    Cells.Add(new ClassicCell(GetCorrectString(Elements[i], Fields[i], font),
                                              new Point(width, (int)PositionItem.Y),
                                              PosOnScreen.TopLeft, font, colorNormal, colorSelected));
                else
                    Cells.Add(
                        new ClassicCell(
                            () => GetCorrectString(Lang.Language.GetString(Elements[i]), Fields[i], font),
                            new Point(width, (int)PositionItem.Y),
                            PosOnScreen.TopLeft, font, colorNormal, colorSelected));
        }

        public void UpdatePosition(int positionInY)
        {
            foreach (var cell in Cells)
                cell.InternalRectangle.Y += positionInY;
            ItemRectangle.Y += positionInY;
        }

        string GetCorrectString(string str, int width, SpriteFont font)
        {
            string s = "";
            for (int i = 0; i < str.Length; s += str[i], i++)
                if (font.MeasureString(s).X > width)
                    return ((s.Length >= 4) ? s.Substring(0, s.Length - 4) : "") + "...";
            return s;
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var cell in Cells)
                cell.DrawItem(gameTime, spriteBatch);
            base.DrawItem(gameTime, spriteBatch);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            foreach (var cell in Cells)
            {  
                cell.UpdateItem(gameTime);
                if (!cell.IsSelect)
                    IsSelect = false;
            }
            if (IsSelect)
                return;
            if (GameEngine.MouseState.LeftButton == ButtonState.Pressed)
                IsSelect = ItemRectangle.Intersects(new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1));
        }
    }
}
