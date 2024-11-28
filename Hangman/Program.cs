namespace Hangman
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Hangman!");
            string wordToGuess = Data.GetRandomWord();

            LaunchProject game = new LaunchProject(wordToGuess);
            game.RunGameLoop();
        }
    }
}
