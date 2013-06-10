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
        private List<string[]> Servers;
        private Client Client;
        private SpriteBatch _spriteBatch;
        Menu Menu;

        public ServerSelector(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, string persoName, string playerName)
        {
            _spriteBatch = spriteBatch;
            PersoName = persoName;
            PlayerName = playerName;
            Servers = new List<string[]>();
            GameEngine.EventManager.Register("Network.Master.ServerList", ReceiveServers);
            GameEngine.EventManager.Register("Network.Game.GameStart", GameBegin);
            Client = new Client("metastruggle.eu", 5555, GameEngine.EventManager, new Parser().Parse);
            AskList();
        }

        public Menu Create()
        {
            Menu = new Menu(RessourceProvider.MenuBackgrounds["SimpleMenu"]);


            Menu.Add("NextButton.Item", new MenuButton("Menu.Next", new Vector2(70, 70), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange, NextButton));
            Menu.Add("ReturnButton.Item", new MenuButton("Menu.Back", new Vector2(15, 70), RessourceProvider.Fonts["Menu"], Color.White, 
                Color.DarkOrange, () => GameEngine.DisplayStack.Pop()));
            return Menu;
        }


        void NextButton()
        {

            System.Threading.Thread.Sleep(200);

            var listServer = Menu.Items["ListServer.Item"] as ListLines1;
            if (listServer == null || listServer.Selected == null)
                return;

            var serverItem = listServer.Selected[1].Split(':');

            Client = new Client(serverItem[0], int.Parse(serverItem[1]), GameEngine.EventManager, new Parser().Parse);
            ////menu.Add("text",new Button(new Rectangle(400, 400, 50, 50), "Player waiting...", RessourceProvider.Fonts["HUD"], Color.White, Color.DarkOrange, () => {}));
            new JoinLobby().Pack(Client.Writer, PlayerName, PersoName);

            //GameEngine.DisplayStack.Push(new ServerSelector(_spriteBatch, _graphics, perso).Create());
        }

        void AskList()
        {
            new MasterServerListRequest().Pack(Client.Writer);
        }

        void ReceiveServers(object data)
        {
            var listServers = (List<MasterServerDatas>) data;
            foreach (var s in listServers)
                Servers.Add(new[] {s.Map, s.IP + ":" + s.Port, s.ConnectedPlayer + "/" +s.MaxPlayer});
            Client.Disconnect();

            Menu.Add("ListServer.Item",new ListLines1(new Dictionary<string, float>
                {
                    {"Map",18}, {"IP:Port",11}, {"Player", 3}
                }, Servers,new Rectangle(10,10,80,50),"UglyTestTheme", RessourceProvider.Fonts["HUDlittle"],Color.White,Color.DarkOrange ));
        }
        
        void GameBegin(object data)
        {
            var gs = (GameStartDatas)data;
            GameEngine.SceneManager = new Renderable.Environments.NetworkEnvironment(_spriteBatch, gs).GetScene(_spriteBatch);
            GameEngine.SoundCenter.PlayWithStatus("music1");
            GameEngine.DisplayStack.Push(GameEngine.SceneManager);
        }
    }
}
