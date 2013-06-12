using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Network.Packet.Packets;
using Network.Packet.Packets.DatasTypes;

namespace GameServer
{
    public class GameManager
    {
        public List<NetworkCharacter> Characters { get; set; }
        public string Map { get; set; }
        private EventManager _em;

        public GameManager(IEnumerable<Player> players, string map, EventManager em)
        {
            Map = map;
            _em = em;

            RegisterEvents();

            foreach (var p in players)
                p.Client.OnDisconnect += c => PlayerDisconnect(p);

            CreateNetworkCharacters(players);

            foreach (var networkCharacter in Characters)
            {
                new GameStart().Pack(networkCharacter.Client.Writer,
                    Map, 
                    players);

                new SetCharacterPosition().Pack(networkCharacter.Client.Writer,
                                                new CharacterPositionDatas
                                                    {
                                                        ID = networkCharacter.ID,
                                                        X = networkCharacter.X,
                                                        Y = networkCharacter.Y,
                                                        Yaw = networkCharacter.Yaw
                                                    });
            }
        }

        void RegisterEvents()
        {
            _em.Register("Network.Game.SetCharacterPosition", SetCharacterPos);
        }

        void PlayerDisconnect(Player player)
        {
            Characters.RemoveAll(c => c.ID == player.ID);

            foreach (var p in Characters)
            {
                new ServerMessage().Pack(p.Client.Writer, player.Name + " s'est déconnecté");
                new RemovePlayer().Pack(p.Client.Writer, player.ID);
            }
        }

        void CreateNetworkCharacters(IEnumerable<Player> players)
        {
            Characters = new List<NetworkCharacter>();
            
            var spawnPositions = new[]
                {
                    new Tuple<float, float>(-5, 10), 
                    new Tuple<float, float>(-12, 10), 
                    new Tuple<float, float>(-8, 10),
                    new Tuple<float, float>(-3, 10)
                };

            foreach (var netchar in players.Select(player => new NetworkCharacter(player.ID, player.Client, player.Name, player.ModelType,
                                                                                  spawnPositions[player.ID].Item1, spawnPositions[player.ID].Item2)))
            {
                Characters.Add(netchar);
            }
        }

        void SetCharacterPos(object data)
        {
            var c = (CharacterPositionDatas) data;
            foreach (var networkCharacter in Characters/*.Where(e => e.ID != c.ID)*/)
                new SetCharacterPosition().Pack(networkCharacter.Client.Writer, c);
        }
    }
}
