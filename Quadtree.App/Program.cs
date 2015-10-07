using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadtree.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var quadtree = new Quadtree(500, 500);

            try
            {
                for (int i = 0; i < 20; i++)
                {
                    quadtree.Push(RandomItem());
                }

                Console.WriteLine(quadtree.ToString());
            }
            catch (StackOverflowException error)
            {
                Console.WriteLine("Stackoverflow error");
                Console.WriteLine(error.StackTrace);
            }

            Console.Read();
        }

        static Random xyRandom = new Random(10);
        static Random stringRandom = new Random();

        public static Item RandomItem()
        {
            return new Item()
            {
                Key = RandomString(),
                X = xyRandom.Next(250) - 1,
                Y = xyRandom.Next(250) - 1
            };
        }

        public static string RandomString(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[stringRandom.Next(s.Length)]).ToArray()).ToLower();
        }
    }
}
