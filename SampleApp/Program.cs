using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Deployment Activity 1: Pass Task Completed");
            Console.WriteLine("Running from: " + AppDomain.CurrentDomain.BaseDirectory);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
