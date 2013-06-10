using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Network;
using Network.Packet.Packets.DatasTypes;

namespace Master
{
    class Program
    {
        static readonly List<MasterServerDatas> Servers = new List<MasterServerDatas>();
        private static Server _s;
        private static System.Threading.Timer _tmr;

        static void Main(string[] args)
        {
            PrintTitle();

            var em = new EventManager();
            em.Register("Network.Master.AddServer", AddServer);
            em.Register("Network.Master.RemoveServer", RemoveServer);
            em.Register("Network.Master.ServerListRequest", SendServerList);

            InitServer(em, 5555);

            _tmr = new Timer(Ping, null, 5000, 5000);

            while (true)
                switch (Console.ReadLine())
                {
                    case "quit":
                        _s.Stop();
                        Environment.Exit(0);
                        break;
                }
        }

        static void InitServer(EventManager ev, ushort port)
        {
            var p = new Parser();
            p.PacketNotFound += (pck) => Console.WriteLine("Packet inconnu [ID={0}]", pck.Header.ID);
            _s = new Server(ev, p.Parse);
            _s.ClientConnected += (c) => Console.WriteLine("Client connected [{0}]", c.ToString());
            _s.ClientDisconnected += (c) => Console.WriteLine("Client disconnected [{0}]", c.ToString());
            _s.Start(port);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Server UP !");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void PrintTitle()
        {
            Console.WriteLine("   _____    _________ ");
            Console.WriteLine("  /     \\  /   _____/                                            ");
            Console.WriteLine(" /  \\ /  \\ \\_____  \\                                             ");
            Console.WriteLine("/    Y    \\/        \\                                            ");
            Console.WriteLine("\\____|__  /_______  /                                            ");
            Console.WriteLine("        \\/        \\/                                             ");
            Console.WriteLine("   _____      _____    _________________________________________ ");
            Console.WriteLine("  /     \\    /  _  \\  /   _____/\\__    ___/\\_   _____/\\______   \\");
            Console.WriteLine(" /  \\ /  \\  /  /_\\  \\ \\_____  \\   |    |    |    __)_  |       _/");
            Console.WriteLine("/    Y    \\/    |    \\/        \\  |    |    |        \\ |    |   \\");
            Console.WriteLine("\\____|__  /\\____|__  /_______  /  |____|   /_______  / |____|_  /");
            Console.WriteLine("        \\/         \\/        \\/                    \\/         \\/ ");
            Console.WriteLine("  __________________________________   _________________________ ");
            Console.WriteLine(" /   _____/\\_   _____/\\______   \\   \\ /   /\\_   _____/\\______   \\");
            Console.WriteLine(" \\_____  \\  |    __)_  |       _/\\   Y   /  |    __)_  |       _/");
            Console.WriteLine(" /        \\ |        \\ |    |   \\ \\     /   |        \\ |    |   \\");
            Console.WriteLine("/_______  //_______  / |____|_  /  \\___/   /_______  / |____|_  /");
            Console.WriteLine("        \\/         \\/         \\/                   \\/         \\/ ");
        }

        static void AddServer(object data)
        {
            var serverDatas = (MasterServerDatas) data;

            lock (Servers)
                if (Servers.All(e => e.IP != serverDatas.IP || e.Port != serverDatas.Port))
                {
                    Servers.Add(serverDatas);
                    Console.WriteLine("[{0}:{1}] {2} ({3}/{4}) ajouté", serverDatas.IP, serverDatas.Port, serverDatas.Map, serverDatas.ConnectedPlayer, serverDatas.MaxPlayer);
                }
        }

        static void RemoveServer(object data)
        {
            var serverDatas = (MasterServerDatas)data;

            lock (Servers)
            {
                if (!Servers.Any(e => e.IP == serverDatas.IP && e.Port == serverDatas.Port)) return;

                Servers.RemoveAll(e => e.IP == serverDatas.IP && e.Port == serverDatas.Port);
                Console.WriteLine("[{0}:{1}] supprimé", serverDatas.IP, serverDatas.Port);
            }
        }

        static void SendServerList(object data)
        {
            new Network.Packet.Packets.MasterServerList().Pack(((Client) data).Writer, Servers);
        }

        static void Ping(object useless)
        {
            Console.WriteLine("=== ping des serveurs ===");

            foreach (var masterServerDatase in Servers.GetRange(0, Servers.Count))
            {
                Client c = null;

                try
                {
                    c = new Client(masterServerDatase.IP, masterServerDatase.Port, new EventManager(), new Parser().Parse);
                }
                catch (Exception e)
                {
                    RemoveServer(masterServerDatase);
                }
                finally
                {
                    if(c != null && c.Connected)
                        c.Disconnect();
                }
            }
        }
    }
}
