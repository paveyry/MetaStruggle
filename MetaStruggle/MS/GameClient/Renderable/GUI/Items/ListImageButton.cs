﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items
{
    public class ListImageButton : Item
    {
        private List<ImageButton> imageButtons;
        private int _actualResolutionWidth;
        private readonly int HeightFont;
        private ImageButton Selected;
        public string NameSelected { get { return Selected.Text; } }

        public ListImageButton(Rectangle rectangle, Dictionary<string, Texture2D> items, int divCoef,
            SpriteFont font, Color colorNormal, Color colorSelected)
            : base(rectangle,true )
        {
            imageButtons = new List<ImageButton>();
            _actualResolutionWidth = GameEngine.Config.ResolutionWidth;
            
            int x = (int)Position.X, y = (int)Position.Y;
            HeightFont = (int)font.MeasureString("A").Y + 3;
            foreach (var item in items)
            {
                int width = item.Value.Width / divCoef;
                int height = item.Value.Height / divCoef;
                imageButtons.Add(new ImageButton(item.Key,
                    new Rectangle(x, y, width, height) , item.Value, font, colorNormal, colorSelected));
                x += width + 5;
                if (x + width <= _actualResolutionWidth && x + width <= RealRectangle.X + RealRectangle.Width) 
                    continue;
                x = (int) Position.X;
                y += height + HeightFont;
            }
        }

        private void UpdateResolution()
        {
            _actualResolutionWidth = GameEngine.Config.ResolutionWidth;
            int x = ItemRectangle.X, y = ItemRectangle.Y;
            foreach (var item in imageButtons)
            {
                item.ItemRectangle.Location = new Point(x,y);
                x += item.ItemRectangle.Width + 5;
                if (x + item.ItemRectangle.Width <= _actualResolutionWidth && x + item.ItemRectangle.Width <= RealRectangle.Width)
                    continue;
                x = ItemRectangle.X;
                y += item.ItemRectangle.Height + HeightFont;
            }
        }

        public override void DrawItem(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var imageButton in imageButtons)
                imageButton.DrawItem(gameTime, spriteBatch);
        }

        public override void UpdateItem(GameTime gameTime)
        {
            if (_actualResolutionWidth != GameEngine.Config.ResolutionWidth)
                UpdateResolution();
            foreach (var imageButton in imageButtons)
            {
                imageButton.UpdateItem(gameTime);
                if (!imageButton.IsSelect) 
                    continue;
                if (Selected != null && !Selected.Equals(imageButton))
                    Selected.IsSelect = false;
                Selected = imageButton;
            }
            base.UpdateItem(gameTime);
        }
    }
}