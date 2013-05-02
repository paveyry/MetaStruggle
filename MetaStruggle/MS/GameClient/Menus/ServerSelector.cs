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
        Menu1 menu;

        public ServerSelector(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, string persoName)
        {
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
                menu.Add(new Button(new Rectangle(x, y, 60, 20), server, RessourceProvider.Fonts["Menu"], Color.White, Color.DarkOrange, () => { }));
                y += 22;
            }
        }

        public Menu1 Create()
        {
            menu = new Menu1(RessourceProvider.MenuBackgrounds["MainMenu"]);


            
            return menu;
        }
    }
}
