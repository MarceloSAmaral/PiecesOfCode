using System;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Task initializing...");
            CodeV01.RunTest();
            GC.Collect();
            await CodeV02.RunTest();
            Console.WriteLine("Press ENTER to end...");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
