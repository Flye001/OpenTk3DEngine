namespace OpenTkEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            using (var game = new Game(800, 600, "My Game"))
            {
                game.Run();
            }
        }
    }
}