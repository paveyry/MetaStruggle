using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using GameClient.Renderable.GUI;
using GameClient.Renderable.GUI.Items;
using GameClient.Renderable.GUI.Items.ListItems;
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
        private Client Client;
        private SpriteBatch _spriteBatch;
        Menu Menu;

        public ServerSelector(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, string persoName, string playerName)
        {
            _spriteBatch = spriteBatch;
            PersoName = persoName;
            PlayerName = playerName;
            Client = new Client("metastruggle.eu", 5555, GameEngine.EventManager, new Parser().Parse);
            GameEngine.EventManager.Register("Network.Master.ServerList", ReceiveServers);
            GameEngine.EventManager.Register("Network.Game.GameStart", GameBegin);
            AskList();
        }

        public Menu Create()
        {
            Menu = new Menu(RessourceProvider.MenuBackgrounds["SimpleMenu"]);


            Menu.Add("WaitingRoom.Item", new SimpleText("Menu.WaitingRoom", new Vector2(15,80), Item.PosOnScreen.TopLeft, RessourceProvider.Fonts["MenuLittle"],Color.White,false));
            Menu.Add("NextButton.Item", new MenuButton("Menu.Next", new Vector2(70, 90), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange, NextButton));
            Menu.Add("ReturnButton.Item", new MenuButton("Menu.Back", new Vector2(15, 90), RessourceProvider.Fonts["Menu"], Color.White, 
                Color.DarkOrange, MainMenu.Back));
            return Menu;
        }

        void NextButton()
        {
            System.Threading.Thread.Sleep(200);

            var listServer = Menu.Items["ListServer.Item"] as ClassicList;
            if ((listServer == null || listServer.Selected == null))
                return;
            Menu.Items["WaitingRoom.Item"].IsDrawable = true;
            Menu.Items["NextButton.Item"].IsDrawable = false;

            var serverItem = listServer.Selected[1].Split(':');

            Client = new Client(serverItem[0], int.Parse(serverItem[1]), GameEngine.EventManager, new Parser().Parse);
            new JoinLobby().Pack(Client.Writer, PlayerName, PersoName);
        }

        void AskList()
        {
            new MasterServerListRequest().Pack(Client.Writer);
        }

        void ReceiveServers(object data)
        {
            Client = new Client("metastruggle.eu", 5555, GameEngine.EventManager, new Parser().Parse);
            var listServers = (List<MasterServerDatas>)data;
            var servers = new List<string[]>();
            foreach (var s in listServers)
                servers.Add(new[] { s.Map, s.IP + ":" + s.Port, s.ConnectedPlayer + "/" + s.MaxPlayer });
            Client.Disconnect();
            Menu.Add("ListServer.Item", new ClassicList(new Rectangle(10, 10, 80, 50), servers, new Dictionary<string, int>
                {
                    {"Map",18}, {"IP:Port",11}, {"Player", 3}
                }, RessourceProvider.Fonts["HUDlittle"], Color.White, Color.DarkOrange, "MSTheme"));
        }

        void GameBegin(object data)
        {
            var gs = (GameStartDatas)data;
            GameEngine.SceneManager = new Renderable.Environments.NetworkEnvironment(_spriteBatch, gs, Client, PlayerName).GetScene(_spriteBatch);
            GameEngine.DisplayStack.Push(GameEngine.SceneManager);
        }
    }
}
