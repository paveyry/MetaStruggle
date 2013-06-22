using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Characters;
using GameClient.Characters.AI;
using GameClient.Global;
using GameClient.Renderable.Scene;
using GameClient.Renderable._3D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.Environments
{
    class LocalEnvironnement
    {
        private SceneManager SceneManager { get; set; }

        public LocalEnvironnement(List<PartialCharacter> characters, SpriteBatch spriteBatch, string mapName)
        {
            SceneManager = SceneManager.CreateScene( new Vector3(-5, 5, -30), new Vector3(0, 0, 0), spriteBatch,mapName);
            CreateCharacters(characters,mapName);

            SceneManager.Camera.FollowsCharacters(SceneManager.Camera, SceneManager.Items.FindAll(e => e is Character));
        }

        void CreateCharacters(List<PartialCharacter> characters, string mapName)
        {
            foreach (var partialCharacter in characters)
            {
                var player = RessourceProvider.Characters[partialCharacter.ModelName];
                if (partialCharacter.IsAI)
                    player.SetEnvironnementDatas(partialCharacter.PlayerName, SceneManager, false,
                                                 new ComputerCharacter(SceneManager, partialCharacter.Handicap,partialCharacter.Level));
                else
                    player.SetEnvironnementDatas(partialCharacter.PlayerName, SceneManager, true,
                                                 partialCharacter.PlayerNb);
                SceneManager.Items.Add(player);
            }
        }
    }
}
