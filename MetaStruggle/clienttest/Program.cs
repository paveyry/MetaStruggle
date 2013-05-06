using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Network.Packet.Packets;

namespace clienttest
{
    class Program
    {
        static void Main(string[] args)
        {
            EventManager ev = new EventManager();
            ev.Register("Network.Game.GameStart", o => Console.WriteLine("le jeu débute")); 
            Parser p = new Parser();
            Client c = new Client("127.0.0.1", 1234, ev, p.Parse);
            new JoinLobby().Pack(c.Writer, Console.ReadLine(), "Zeus");
            Console.ReadLine();
        }
    }
}
