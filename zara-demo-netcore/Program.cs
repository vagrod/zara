using System;

namespace ZaraEngine.NetCore.Demo
{
    class Program
    {
        private static GameController _gc;

        static void Main(string[] args)
        {
            _gc = new GameController();
            _gc.Initialize();

            Console.WriteLine("Zara is running");
        }
    }
}
