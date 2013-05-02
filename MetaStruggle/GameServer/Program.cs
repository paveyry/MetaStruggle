using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MetaStruggle Game Server\n");

            short port = GetPort();
            string map = GetMap();
            byte max = GetMaxPlayer();

            var gh = new GameHost(port, map, max);

            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "quit":
                        if(gh.State == State.Lobby)
                            gh.MasterOperation(false);
                        break;
                }
            }
        }

        static short GetPort()
        {
            short port;
            string sport;

            do
            {
                Console.Write("Port: ");
                sport = Console.ReadLine();
            } while (!short.TryParse(sport, out port) || port < 0 || port > 65535);

            return port;
        }

        static string GetMap()
        {
            string[] availableMaps = new [] {"MapDesert", "tardis map", "tree map"};
            string schoice;
            int choice;

            do
            {
                Console.WriteLine("Maps disponibles: ");

                for (int i = 0; i < availableMaps.Length; i++)
                    Console.WriteLine("\t" + i + ": " + availableMaps[i]);

                Console.Write("Numero de la map: ");
                schoice = Console.ReadLine();
            } while (!int.TryParse(schoice, out choice) || choice < 0 || choice >= availableMaps.Length);

            return availableMaps[choice];
        }

        static byte GetMaxPlayer()
        {
            byte max;
            string smax;

            do
            {
                Console.Write("Nombre de joueurs (2 à 4): ");
                smax = Console.ReadLine();
            } while (!byte.TryParse(smax, out max) || max < 2 || max > 4);

            return max;
        }
    }
}
