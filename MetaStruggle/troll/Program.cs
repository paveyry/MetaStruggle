using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;

namespace troll
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                new Task(th).Start();
            }

            Console.ReadLine();
        }

        static void th()
        {
            while (true)
            {
                var c = new Client("10.3.38.18", 1337, new EventManager(), new Parser().Parse,
                                   a => Console.WriteLine("connected"));

                while (true)
                {
                    try
                    {
                        c.Writer.Write((byte) 0);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("network fail, reconnecting ...");
                        break;
                    }
                }
            }
        }
    }
}
