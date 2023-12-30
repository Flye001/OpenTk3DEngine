using OpenTK.Windowing.Common;

namespace OpenTkEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            using (var game = new Game(600, 600, "My Game"))
            {
                //game.VSync = VSyncMode.Off;
                game.Run();
            }
        }
    }
}