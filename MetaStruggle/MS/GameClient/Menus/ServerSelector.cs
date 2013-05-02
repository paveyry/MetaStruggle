using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using GameClient.Renderable.GUI;
using GameClient.Renderable.GUI.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Network.Packet.Packets.DatasTypes;
using Network.Packet.Packets;
using Network;

namespace GameClient.Menus
{
    public class ServerSelector
    {
        private string PersoName;
        private string PlayerName;
        private List<string> servers;
        private Client c;
        private SpriteBatch s;
        Menu1 menu;

        public ServerSelector(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, string persoName)
        {
            s = spriteBatch;
            PersoName = persoName;
            servers = new List<string>();
            Global.GameEngine.EventManager.Register("Network.Master.ServerList", ReceiveServers);
            Parser p = new Parser();
            c = new Client("metastruggle.eu", 5555, Global.GameEngine.EventManager, p.Parse);
            AskList();
        }

        void AskList()
        {
            new MasterServerListRequest().Pack(c.Writer);
        }

        void ReceiveServers(object data)
        {
            var list = (List<MasterServerDatas>) data;
            foreach (var s in list)
                servers.Add(s.ToString());
            
            c.Disconnect();

            int y = 100, x = 50;
            foreach (var server in servers)
            {
                menu.Add(new Button(new Rectangle(x, y, 500, 20), server, RessourceProvider.Fonts["Menu"], Color.White, Color.DarkOrange, () => { }));
                y += 35;
            }
        }

        public Menu1 Create()
        {
            menu = new Menu1(RessourceProvider.MenuBackgrounds["MainMenu"]);
            menu.Add( new Textbox("", new Rectangle(0, 0, 400, 50), RessourceProvider.Buttons["TextboxMulti"], RessourceProvider.Fonts["HUD"], Color.White));
            menu.Add(new Button(new Rectangle(400, 500, 50, 50), "OK", RessourceProvider.Fonts["HUD"], Color.White, Color.DarkOrange,() => Play()));
            return menu;
        }

        public void Play()
        {
            if (GameEngine.SceneManager == null)
                GameEngine.SceneManager = Renderable.Environments.Environment2.GetScene(s);

            GameEngine.SoundCenter.PlayWithStatus("music1");
            GameEngine.DisplayStack.Push(GameEngine.SceneManager);
        }

        void ButtonOk()
        {
            System.Threading.Thread.Sleep(200);
            string server = "";

            foreach (Textbox e in from Textbox e in menu.Items.FindAll(e => e is Textbox) where true select e)
                PlayerName = e.Text;
            foreach (Button e in from Button e in menu.Items.FindAll(e => e is Button) where e.IsSelect && e.Name != "OK" select e)
                server = e.Name;

            server = server.Substring(1, server.Length - 2);
            var t = server.Split(':');

            Parser p = new Parser();
            
            GameEngine.EventManager.Register("Network.Game.GameStart", GameBegin);

            Client c = new Client(t[0], int.Parse(t[1]), Global.GameEngine.EventManager, p.Parse);
            menu.Add(new Button(new Rectangle(400, 400, 50, 50), "Player waiting...", RessourceProvider.Fonts["HUD"], Color.White, Color.DarkOrange, () => {}));
            new JoinLobby().Pack(c.Writer, PlayerName, PersoName);

            //GameEngine.DisplayStack.Push(new ServerSelector(_spriteBatch, _graphics, perso).Create());
        }

        void GameBegin(object data)
        {
            var gs = (GameStartDatas) data;
            Global.GameEngine.SceneManager = new Renderable.Environments.NetworkEnvironment(s).GetScene(s);
            GameEngine.SoundCenter.PlayWithStatus("music1");
            GameEngine.DisplayStack.Push(GameEngine.SceneManager);
        }
    }
}
