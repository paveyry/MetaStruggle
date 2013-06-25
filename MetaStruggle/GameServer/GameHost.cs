using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Network.Packet.Packets;

namespace GameServer
{
    public class GameHost
    {
        public State State;
        public Lobby CurrentLobby;
        public GameManager GameManager;
        public Server Server;
        private readonly EventManager _em;
        private readonly Parser _parser;
        private readonly string _map;
        private readonly byte _maxPlayers;
        private readonly ushort _port;
        private const string MasterServerHost = "metastruggle.eu";
        private const ushort MasterServerPort = 5555;

        public GameHost(ushort port, string map, byte maxplayers)
        {
            _parser = new Parser();
            _em = new EventManager();
            _map = map;
            _maxPlayers = maxplayers;
            _port = port;
            OpenLobby();
        }

        public void OpenLobby()
        {
            CurrentLobby = new Lobby(_maxPlayers, _map, _em, this);
            Server = new Server(_em, _parser.Parse);

            Server.Start(_port);
            State = State.Lobby;

            MasterOperation(true);

            Console.WriteLine("Salle d'attente ouverte pour la map " + _map);
        }

        public void ChangeMode()
        {
            Console.WriteLine("===Salle d'attente complete===");
            State = State.InGame;
            MasterOperation(false);
            GameManager = new GameManager(CurrentLobby.Players, _map, _em, this);
            Server.Stop();
            Console.WriteLine("Debut du jeu"); 
        }

        public void MasterOperation(bool register)
        {
            var m = new EventManager();
            var p = new Parser();
            var c = new Client(MasterServerHost, MasterServerPort, m, p.Parse);

            if (register)
            {
                new MasterAddServer().Pack(c.Writer, Server.Port, CurrentLobby.MapName, CurrentLobby.MaxPlayers, CurrentLobby.PlayersConnected);
                Console.WriteLine("Serveur enregistré sur Master");
            }
            else
            {
                new MasterRemoveServer().Pack(c.Writer, Server.Port);
                Console.WriteLine("Serveur désenregistré sur Master");
            }

            c.Disconnect();
        }
    }

    public enum State
    {
        Lobby,
        InGame
    }
}
