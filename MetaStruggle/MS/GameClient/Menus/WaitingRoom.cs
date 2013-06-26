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
    class WaitingRoom
    {
        Menu Menu { get; set; }
        string[] Server { get; set; }
        private Client Client;
        private string NumberOfPlayer { get; set; }
        private double _oldTime;


        public WaitingRoom(string[] server)
        {
            NumberOfPlayer = server[2];
            Server = server;
            GameEngine.EventManager.Register("Network.Master.ServerList", UpdateNumberOfPlayer);
            Client = new Client("metastruggle.eu", 5555, GameEngine.EventManager, new Parser().Parse);
            AskList();
            _oldTime = -1;
        }

        public Menu Create()
        {
            Menu = new Menu(RessourceProvider.MenuBackgrounds["SimpleMenu"],false);
            Menu.Add("NumberOfPlayerText", new SimpleText(() => GameEngine.LangCenter.GetString("Menu.WaitingRoom"),
                new Vector2(30, 50), Item.PosOnScreen.DownLeft, RessourceProvider.Fonts["MenuLittle"], Color.White));
            //Menu.UpdateFunc = Update;
            return Menu;
        }

        void AskList()
        {
            new MasterServerListRequest().Pack(Client.Writer);
        }

        void Update(GameTime gameTime)
        {
            if (_oldTime != -1 || gameTime.TotalGameTime.TotalMilliseconds - _oldTime > 2000)
                AskList();
            else
                _oldTime = gameTime.TotalGameTime.TotalMilliseconds;
        }

        void UpdateNumberOfPlayer(object datas)
        {
            Client = new Client("metastruggle.eu", 5555, GameEngine.EventManager, new Parser().Parse);
            var serverUpdate = ((List<MasterServerDatas>)datas).First(ms => ms.IP + ":" + ms.Port == Server[1]);
            Client.Disconnect();
            NumberOfPlayer = serverUpdate.ConnectedPlayer + "/" + serverUpdate.MaxPlayer;
        }
    }
}
