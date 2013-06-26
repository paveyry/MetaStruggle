using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Network.Packet.Packets.DatasTypes;
using Network.Packet.Packets;

namespace GameServer
{
    public class Lobby
    {
        public List<Player> Players = new List<Player>();
        public byte MaxPlayers { get; set; }
        public byte PlayersConnected { get { return (byte)Players.Count; } }
        public string MapName { get; set; }
        private readonly EventManager _em;
        private readonly GameHost _gameHost;

        public Lobby(byte maxPlayers, string mapName, EventManager em, GameHost gameHost)
        {
            MaxPlayers = maxPlayers;
            _em = em;
            MapName = mapName;
            _gameHost = gameHost;

            RegisterLobbyEvents();
        }

        public void AddPlayer(object data)
        {
            var playerBasicsDatas = (PlayerBasicsDatas) data;

            if (Players.Count == MaxPlayers)
            {
                new JoinLobbyRefused().Pack(playerBasicsDatas.Client.Writer, "Salle d'attente pleine");
                return;
            }

            if (Players.GetRange(0, Players.Count).Any(e => e.Name == playerBasicsDatas.Name))
            {
                new JoinLobbyRefused().Pack(playerBasicsDatas.Client.Writer, "Un joueur possède le même nom");
                return;
            }

            Players.Add(new Player(GetAvailableIDs().First(),
                                    playerBasicsDatas.Name,
                                    playerBasicsDatas.ModelType,
                                    playerBasicsDatas.Client));

            playerBasicsDatas.Client.OnDisconnect += RemovePlayer;

            Console.WriteLine("{0} connecté au salon avec {1}", playerBasicsDatas.Name, playerBasicsDatas.ModelType);

            UpdateMasterDatas();

            if(Players.Count == MaxPlayers)
                _gameHost.ChangeMode();
        }

        public void RemovePlayer(object data)
        {
            var client = (Client) data;
            Console.WriteLine(Players.First(e => e.Client.Equals(client)).Name + " quitte le salon");
            Players.RemoveAll(e => e.Client.Equals(client));

            UpdateMasterDatas();
        }

        IEnumerable<byte> GetAvailableIDs() 
        {
            var possibilities = new List<byte>();

            for (byte i = 0; i < MaxPlayers; i++)
                possibilities.Add(i);

            foreach (var player in Players)
                possibilities.Remove(player.ID);

            return possibilities;
        }

        void RegisterLobbyEvents() 
        {
            _em.Register("Network.Game.JoinLobby", data =>
            {
                if (_gameHost.State == State.Lobby)
                    AddPlayer(data);
            });

            _em.Register("Network.Game.LeaveLobby", data =>
            {
                if (_gameHost.State == State.Lobby)
                    RemovePlayer(data);
            });
        }

        void UpdateMasterDatas()
        {
            _gameHost.MasterOperation(false);
            _gameHost.MasterOperation(true);
        }
    }
}
