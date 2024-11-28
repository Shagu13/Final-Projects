

namespace Hangman
{
    public class Data
    {
        private static readonly string[] WordList = { "apple", "banana", "cherry", "grape", "mango", "orange", "peach" };

        public static string GetRandomWord()
        {
            Random random = new Random();
            return WordList[random.Next(WordList.Length)];
        }

        public static void DisplayHangman(int incorrectGuesses)
        {
            string[] hangmanStages = 
                {
                                @"
                     ------
                     |    |
                     |
                     |
                     |
                     |
                    _|_",
                                @"
                     ------
                     |    |
                     |    O
                     |
                     |
                     |
                    _|_",
                                @"
                     ------
                     |    |
                     |    O
                     |    |
                     |
                     |
                    _|_",
                                @"
                     ------
                     |    |
                     |    O
                     |   /|
                     |
                     |
                    _|_",
                                @"
                     ------
                     |    |
                     |    O
                     |   /|\
                     |
                     |
                    _|_",
                                @"
                     ------
                     |    |
                     |    O
                     |   /|\
                     |   /
                     |
                    _|_",
                                @"
                     ------
                     |    |
                     |    O
                     |   /|\
                     |   / \
                     |
                    _|_"
                };

            Console.WriteLine(hangmanStages[incorrectGuesses]);
        }
    }
}
