using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Characters;
using GameClient.Global;
using GameClient.Renderable._3D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.Scene
{
    public class HUD
    {
        private readonly List<Character> _players;
        private Vector2 _intervalSize;

        public HUD()
        {
            _players = new List<Character>();
            _intervalSize.X = (int)Math.Floor(RessourceProvider.Fonts["HUD"].MeasureString(300 + "%").X) + 1;
            _intervalSize.Y = (int)Math.Floor(RessourceProvider.Fonts["HUD"].MeasureString(300 + "%").Y) + 1;
        }

        public void AddCharacter(Character newChar)
        {
            _players.Add(newChar);
        }

        public void DrawHUD(SpriteBatch spriteBatch)
        {
            int x = 10;
            const int divCoef = 7, addCoordonate = 10;

            spriteBatch.Begin();
            foreach (var character in _players)
            {
                spriteBatch.Draw(character.Face,
                    new Rectangle(x, addCoordonate, character.Face.Width / divCoef, character.Face.Height / divCoef), Color.White);

                if (character.IsDead)
                    spriteBatch.DrawString(RessourceProvider.Fonts["HUDlittle"],
                                           Lang.Language.GetString("respawn") +
                                           ((int)(5 - (DateTime.Now - character.DeathDate).TotalSeconds)).ToString(),
                                           new Vector2(x + addCoordonate, character.Face.Height / divCoef + addCoordonate), Color.DarkOrange);
                else
                    spriteBatch.DrawString(RessourceProvider.Fonts["HUDlittle"],
                                           character.PlayerName,
                                           new Vector2(x + addCoordonate, character.Face.Height / divCoef + addCoordonate), Color.White);

                x += character.Face.Width / divCoef;

                spriteBatch.DrawString(RessourceProvider.Fonts["HUD"], character.Damages + "%",
                    new Vector2(x, character.Face.Height / divCoef - (int)_intervalSize.Y), Color.White);

                x += (int)_intervalSize.X;

            }
            spriteBatch.End();
            spriteBatch.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
