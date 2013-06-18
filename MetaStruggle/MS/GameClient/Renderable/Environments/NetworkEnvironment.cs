using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Characters;
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
        private SceneManager sm;
        public string CurrentCharacterName { get; set; }
        public Client Client { get; set; }

        public NetworkEnvironment(SpriteBatch spriteBatch, GameStartDatas gs, Client client, string currentchar)
        {
            RegisterEvents();
            CurrentCharacterName = currentchar;

            sm = SceneManager.CreateScene(
                new Vector3(-5, 5, -30), //Position initiale de la caméra
                new Vector3(0, 0, 0), //Point visé par la caméra
                spriteBatch); //SpriteBatch

            Client = client;

            GameStart(gs);
            //CreateItems(gs);

            sm.Camera.FollowsCharacters(sm.Camera, sm.Items.FindAll(e => e is Character));
        }

        void CreateItems(GameStartDatas gs)
        {
            foreach (var c in gs.Players.Select(p => new Character(p.Name, p.ModelType, 0,sm, new Vector3(0,0,-17), Vector3.One)
                {
                    ID = p.ID,
                    Client = p.Client
                }))
            {
                sm.AddElement(c);
            }

            sm.AddElement(new Model3D(sm, Global.RessourceProvider.StaticModels[gs.MapName], new Vector3(10, 0, 0),
                          new Vector3(1f, 1f, 0.8f)));
        }

        public SceneManager GetScene(SpriteBatch spriteBatch)
        {
            return sm;
        }

        void RegisterEvents()
        {
            Global.GameEngine.EventManager.Register("Network.Game.GameStart", GameStart);
            Global.GameEngine.EventManager.Register("Network.Game.SetCharacterPosition", SetCharacterPosition);
        }

        void SetCharacterPosition(object data)
        {
            var cp = (CharacterPositionDatas) data;

            var c = (Character) sm.Items.Where(e => e is Character).First(e => (e as Character).ID == cp.ID);
            if (!c.Playing)
            {
                c.F1 = c.F2;
                c.F2 = new Vector3(cp.X, cp.Y, -17);

                if (c.F1.HasValue)
                    c.Position = c.F1.Value;

                if (c.F1 != null && c.F2 != null)
                    c.dI = new Vector3((c.F2.Value.X - c.F1.Value.X)/(c.SyncRate + 1), (c.F2.Value.Y - c.F1.Value.Y)/(c.SyncRate), 0);
            }
            else
                c.Position = new Vector3(cp.X, cp.Y, -17);
        }

        void GameStart(object data)
        {
            var gs = (GameStartDatas) data;

            foreach (var p in gs.Players)
                sm.AddElement(new Character(p.Name, p.ModelType, 0, sm, new Vector3(0, 0, -17), new Vector3(1),
                                            (p.ModelType == "Spiderman" || p.ModelType == "Alex") ? 1.6f : 1)
                    {
                        ID = p.ID,
                        Playing = p.Name == CurrentCharacterName,
                        Client = p.Name == CurrentCharacterName ? Client : null
                    });

            sm.AddElement(new Model3D(sm, Global.RessourceProvider.StaticModels[gs.MapName], new Vector3(10, 0, 0),
                                      new Vector3(1f, 1f, 0.8f)));
        }
    }
}
