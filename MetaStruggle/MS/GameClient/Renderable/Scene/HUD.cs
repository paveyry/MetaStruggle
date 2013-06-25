using System;
using System.Collections.Generic;
using GameClient.Characters;
using GameClient.Global;
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
            _intervalSize.X = (int)Math.Floor(RessourceProvider.Fonts["HUD"].MeasureString(999 + "%" + " (99)").X) + 1;
            _intervalSize.Y = (int)Math.Floor(RessourceProvider.Fonts["HUD"].MeasureString(999 + "%" + " (99)").Y) + 1;
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
                int width = character.Face.Width / divCoef, height = character.Face.Height / divCoef;

                spriteBatch.Draw(character.Face, new Rectangle(x, addCoordonate, width, height), Color.White);

                if (character.IsDead)
                    spriteBatch.DrawString(RessourceProvider.Fonts["HUDlittle"],
                        character.IsPermanentlyDead ? GameEngine.LangCenter.GetString("Text.Dead") : GameEngine.LangCenter.GetString("Text.Respawn") +
                                           ((int) (5 - (DateTime.Now - character.DeathDate).TotalSeconds)),
                                           new Vector2(x + addCoordonate, height + addCoordonate), Color.DarkOrange);
                else
                    spriteBatch.DrawString(RessourceProvider.Fonts["HUDlittle"],
                                           character.PlayerName,
                                           new Vector2(x + addCoordonate, height + addCoordonate), Color.White);
                x += width;
                spriteBatch.DrawString(RessourceProvider.Fonts["HUD"], (int)character.Damages + "%" + " (" + (character.NumberMaxOfLives - character.NumberOfDeath) + ")",
                                       new Vector2(x, height - (int) _intervalSize.Y), Color.White);
                x += (int)_intervalSize.X;
            }
            spriteBatch.End();
            spriteBatch.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
