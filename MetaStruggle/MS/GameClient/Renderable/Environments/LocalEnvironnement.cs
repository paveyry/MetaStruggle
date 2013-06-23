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
        public SceneManager SceneManager { get; private set; }

        public LocalEnvironnement(List<PartialAICharacter> characters, SpriteBatch spriteBatch, string mapName)
        {
            SceneManager = SceneManager.CreateScene( new Vector3(-5, 5, -30), new Vector3(0, 0, 0), spriteBatch,mapName);
            CreateCharacters(characters,mapName);

            SceneManager.Camera.FollowsCharacters(SceneManager.Camera, SceneManager.Items.FindAll(e => e is Character));
        }

        void CreateCharacters(List<PartialAICharacter> characters, string mapName)
        {
            foreach (var partialCharacter in characters)
            {
                var player = RessourceProvider.Characters[partialCharacter.ModelName].ConvertToCharacter();
                if (partialCharacter.IsAI)
                    player.SetEnvironnementDatas(partialCharacter.PlayerName,mapName, SceneManager, false,
                                                 new ComputerCharacter(SceneManager, partialCharacter.Handicap,partialCharacter.Level));
                else
                    player.SetEnvironnementDatas(partialCharacter.PlayerName,mapName, SceneManager, true,
                                                 partialCharacter.PlayerNb);
                SceneManager.AddElement(player);
            }
        }
    }
}
