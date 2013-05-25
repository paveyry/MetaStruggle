using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items
{
    public class ListMenuButton : Item
    {
        public enum PosOnScreenButton
        {
            OnLeft, OnRight, OnMidle
        }

        private PosOnScreenButton Pos { get; set; }
        private List<MenuButton> Buttons { get; set; }
        private Color ColorNormal { get; set; }
        private Color ColorSelected { get; set; }

        public ListMenuButton(Point buttonStart)
            : base(new Rectangle(buttonStart.X, buttonStart.Y, 0, 0))
        {

        }

    }
    public class PartialButton
    {
        public Texture2D buttonImage { get; set; }
        public string ID { get; set; }
        public PartialButton(string ID, Item.Event func)
        {

        }
    }
}
