using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI.Items.ListItems.Lines
{
    interface ILine
    {
        Rectangle InternalRectangle { get; set; }
        bool IsDrawable { get; set; }
        int PosYInternalRectangle { get; set; }
        void DrawItem(GameTime gameTime, SpriteBatch spriteBatch);
        void UpdateItem(GameTime gameTime);
    }
}
