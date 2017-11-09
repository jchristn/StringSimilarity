using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Similarity;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.CancelKeyPress += (sender, eventArgs) => { Environment.Exit(0); };

                Console.Write("String 1: ");
                string str1 = Console.ReadLine();
                Console.Write("String 2: ");
                string str2 = Console.ReadLine();
                Console.WriteLine("Score   : " + StringSimilarity.Calculate(str1, str2));
            }
        }
    }
}
