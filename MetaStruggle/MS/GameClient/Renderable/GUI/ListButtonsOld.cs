using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI
{
    class ListButtonsOld
    {
        public Point ButtonsStart { get; set; }
        public int ButtonsSpacing { get; set; }
        private Dictionary<Rectangle, MenuButton1> ButtonsRectangles { get; set; }
        private readonly SpriteFont _font;

        public ListButtonsOld(IEnumerable<MenuButton1> buttons, Point buttonsStart)
        {
            ButtonsStart = buttonsStart;
            ButtonsSpacing = 20;
            _font = Global.RessourceProvider.Fonts["Menu"];
            CreateRectangles(buttons);
        }

        void CreateRectangles(IEnumerable<MenuButton1> buttons)
        {
            int currentY = ButtonsStart.Y;
            ButtonsRectangles = new Dictionary<Rectangle, MenuButton1>();
            foreach (MenuButton1 button in buttons)
            {
                int width, height;
                Rectangle rec;

                if (button.DisplayType == MenuButtonDisplayType.Text)
                {
                    width = (int)_font.MeasureString(button.DisplayedName).X;
                    height = (int)_font.MeasureString(button.DisplayedName).Y;
                }
                else
                {
                    width = button.Image.Width;
                    height = button.Image.Height;         
                }

                rec = new Rectangle(ButtonsStart.X, currentY, width, height);
                ButtonsRectangles.Add(rec, button);
                currentY += height + ButtonsSpacing;
            }
        }
    }
}