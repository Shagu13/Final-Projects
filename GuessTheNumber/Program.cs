namespace GuessTheNumber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the 'Guess the Number' game!");

            var game = new GuessTheNumber();
            game.StartGame();
        }
    }    
}
