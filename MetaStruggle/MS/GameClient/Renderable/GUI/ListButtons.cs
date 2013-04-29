using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI
{
    class ListButtons
    {
        public Point ButtonsStart { get; set; }
        public int ButtonsSpacing { get; set; }
        private Dictionary<Rectangle, MenuButton> ButtonsRectangles { get; set; }
        private readonly SpriteFont _font;

        public ListButtons(IEnumerable<MenuButton> buttons, Point buttonsStart)
        {
            ButtonsStart = buttonsStart;
            ButtonsSpacing = 20;
            _font = Global.RessourceProvider.Fonts["Menu"];
            CreateRectangles(buttons);
        }

        void CreateRectangles(IEnumerable<MenuButton> buttons)
        {
            int currentY = ButtonsStart.Y;
            ButtonsRectangles = new Dictionary<Rectangle, MenuButton>();
            foreach (MenuButton button in buttons)
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