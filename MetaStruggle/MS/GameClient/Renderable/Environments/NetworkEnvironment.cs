using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Characters;
using GameClient.Global;
using GameClient.Renderable._3D;
using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Network;
using Network.Packet.Packets.DatasTypes;

namespace GameClient.Renderable.Environments
{
    public class NetworkEnvironment
    {
        private SceneManager SceneManager { get; set; }
        public string CurrentCharacterName { get; set; }
        public Client Client { get; set; }

        public NetworkEnvironment(SpriteBatch spriteBatch, GameStartDatas gs, Client client, string currentchar)
        {
            RegisterEvents();
            CurrentCharacterName = currentchar;

            SceneManager = SceneManager.CreateScene(
                new Vector3(-5, 5, -30), //Position initiale de la caméra
                new Vector3(0, 0, 0), //Point visé par la caméra
                spriteBatch); //SpriteBatch

            Client = client;

            GameStart(gs);
            //CreateItems(gs);

            SceneManager.Camera.FollowsCharacters(SceneManager.Camera, SceneManager.Items.FindAll(e => e is Character));
        }

        void CreateItems(GameStartDatas gs)
        {
            foreach (var c in gs.Players.Select(p => new Character(p.Name, p.ModelType, 0, SceneManager, new Vector3(0, 0, -17), Vector3.One)
                {
                    ID = p.ID,
                    Client = p.Client
                }))
            {
                SceneManager.AddElement(c);
            }

            SceneManager.AddElement(new Model3D(SceneManager, RessourceProvider.StaticModels[gs.MapName], new Vector3(10, 0, 0),
                          new Vector3(1f, 1f, 0.8f)));
        }

        public SceneManager GetScene(SpriteBatch spriteBatch)
        {
            return SceneManager;
        }

        void RegisterEvents()
        {
            GameEngine.EventManager.Register("Network.Game.GameStart", GameStart);
            GameEngine.EventManager.Register("Network.Game.SetCharacterPosition", SetCharacterPosition);
        }

        void SetCharacterPosition(object data)
        {
            if (SceneManager == null)
                return;

            var cp = (CharacterPositionDatas)data;

            var c = (Character)SceneManager.Items.Where(e => e is Character).First(e => (e as Character).ID == cp.ID);

            if (!c.Playing)
            {
                c.F1 = c.F2;
                c.F2 = new Vector3(cp.X, cp.Y, -17);

                if (c.F1.HasValue)
                    c.Position = c.F1.Value;

                if (c.F1 != null && c.F2 != null)
                    c.dI = new Vector3((c.F2.Value.X - c.F1.Value.X) / (c.SyncRate + 1), (c.F2.Value.Y - c.F1.Value.Y) / (c.SyncRate), 0);

                c.Yaw = cp.Yaw;
                c.SetAnimation((Animation)cp.Anim);
            }
        }

        void GameStart(object data)
        {
            var gs = (GameStartDatas)data;

            foreach (var p in gs.Players)
            {
                Character c = RessourceProvider.Characters[p.ModelType];
                c.SetEnvironnementDatas(p.Name, p.ID, SceneManager, p.Name == CurrentCharacterName, p.Name == CurrentCharacterName ? Client : null);
                SceneManager.AddElement(c);
            }

            //SceneManager.AddElement(new Model3D(SceneManager, RessourceProvider.StaticModels[gs.MapName], new Vector3(10, 0, 0),
            //                          new Vector3(1f, 1f, 0.8f)));
            SceneManager.AddMap(gs.MapName);
        }
    }
}
