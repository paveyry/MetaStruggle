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

        public LocalEnvironnement(IEnumerable<PartialAICharacter> characters, SpriteBatch spriteBatch, string mapName, int numberOfLives = 5)
        {
            SceneManager = SceneManager.CreateScene( new Vector3(-5, 5, -30), new Vector3(0, 0, 0), spriteBatch, mapName, true, numberOfLives);
            CreateCharacters(characters,mapName);

            SceneManager.Camera.FollowsCharacters(SceneManager.Camera, SceneManager.Items.FindAll(e => e is Character));
        }

        void CreateCharacters(IEnumerable<PartialAICharacter> characters, string mapName)
        {
            foreach (var pChar in characters)
            {
                var model = RessourceProvider.Characters[pChar.ModelName];
                SceneManager.AddElement(pChar.IsAI 
                    ? model.ConvertToComputerCharacter(pChar.PlayerName, mapName, SceneManager, pChar.Handicap,pChar.Level) 
                    : model.ConvertToCharacter(pChar.PlayerName, mapName, pChar.PlayerNb, SceneManager));
            }
            foreach (var character in SceneManager.Items.OfType<ComputerCharacter>())
                character.Initialize();
        }
    }
}
