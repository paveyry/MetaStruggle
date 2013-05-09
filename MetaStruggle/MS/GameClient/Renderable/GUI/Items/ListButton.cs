using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

        public ListMenuButton(Point buttonStart) : base(new Rectangle(buttonStart.X,buttonStart.Y,0,0))
        {
            
        }

        //private void CreateRectangle()
        //{
        //    foreach (var menuButton in Buttons)
        //    {
        //        menuButton
        //    }
        //}
        
    }
}
