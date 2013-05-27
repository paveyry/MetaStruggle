using System;
using System.Collections.Generic;
using System.Linq;
using GameClient.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.GUI
{
    public class MenuOld : Layout.IBasicLayout
    {
        public string Id { get; set; }
        public Texture2D Background { get; set; }
        public Point ButtonsStart { get; set; }
        public Color TextButtonColor { get; set; }
        public Color ImageButtonColor { get; set; }
        public Color OnHoverTextButtonColor { get; set; }
        public Color OnHoverImageButtonColor { get; set; }
        public int ButtonsSpacing { get; set; }
        public Dictionary<Rectangle, MenuButton1> ButtonsRectangles { get; set; }
        readonly SpriteFont _font;

        public MenuOld(string id, IEnumerable<MenuButton1> buttons, Texture2D background, Point buttonsStart)
        {
            Id = id;
            Background = background;
            ButtonsStart = buttonsStart;
            ButtonsSpacing = 20;
            TextButtonColor = Color.White;
            ImageButtonColor = Color.White;
            OnHoverTextButtonColor = Color.CornflowerBlue;
            OnHoverImageButtonColor = Color.Wheat;
            _font = RessourceProvider.Fonts["Menu"];

            CreateRectangles(buttons);
        }

        public void CreateRectangles(IEnumerable<MenuButton1> buttons)
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
                    rec = new Rectangle(ButtonsStart.X, currentY, width, height);
                }
                else
                {
                    width = button.Image.Width;
                    height = button.Image.Height;
                    rec = new Rectangle(ButtonsStart.X, currentY, (int)(width * button.Scale), (int)(height * button.Scale));
                }

                ButtonsRectangles.Add(rec, button);
                currentY += height + ButtonsSpacing;
            }
        }

        public void Update(GameTime gameTime)
        {
            var mouse = new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1);

            foreach (var buttonsRectangle in ButtonsRectangles.Where(buttonsRectangle => buttonsRectangle.Key.Intersects(mouse)
                && GameEngine.MouseState.LeftButton == ButtonState.Pressed).Where(buttonsRectangle => buttonsRectangle.Value.OnClick != null))
            {
                buttonsRectangle.Value.OnClick.Invoke();
                break;
            }

            if (!GameEngine.KeyboardState.IsKeyDown(Keys.Escape)) return;
            System.Threading.Thread.Sleep(200);
            if (GameEngine.DisplayStack.Count > 1)
                GameEngine.DisplayStack.Pop();
            else
                Environment.Exit(0);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            DrawBackground(spriteBatch);
            DrawButtons(spriteBatch);

            spriteBatch.End();
            spriteBatch.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        private void DrawButtons(SpriteBatch spriteBatch)
        {
            foreach (var buttonsRectangle in ButtonsRectangles)
                if (buttonsRectangle.Value.DisplayType == MenuButtonDisplayType.Text)
                    spriteBatch.DrawString(_font, buttonsRectangle.Value.DisplayedName,
                                           new Vector2(buttonsRectangle.Key.Location.X, buttonsRectangle.Key.Location.Y),
                                           GetButtonColor(buttonsRectangle.Key, buttonsRectangle.Value));
                else
                {
                    //spriteBatch.Draw(buttonsRectangle.Value.Image, buttonsRectangle.Key,
                    //                 GetButtonColor(buttonsRectangle.Key, buttonsRectangle.Value));
                    spriteBatch.Draw(GetImage(buttonsRectangle.Key, buttonsRectangle.Value), buttonsRectangle.Key, ImageButtonColor);
                }
        }

        private Color GetButtonColor(Rectangle rec, MenuButton1 button)
        {
            var mouse = new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1);

            if (button.DisplayType == MenuButtonDisplayType.Text)
                return rec.Intersects(mouse) ? OnHoverTextButtonColor : TextButtonColor;

            return rec.Intersects(mouse) ? OnHoverImageButtonColor : ImageButtonColor;
        }

        private Texture2D GetImage(Rectangle rec, MenuButton1 button)
        {
            var mouse = new Rectangle(GameEngine.MouseState.X, GameEngine.MouseState.Y, 1, 1);

            return rec.Intersects(mouse) ? button.ImageOnClick : button.Image;
        }

        void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background,
                 new Rectangle(0, 0, GameEngine.Config.ResolutionWidth,
                               GameEngine.Config.ResolutionHeight), Color.White);
        }
    }
}
