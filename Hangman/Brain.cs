

namespace Hangman
{
    internal class Brain
    {
        public static void DisplayGameState(char[] hiddenWord, int remainingAttempts, HashSet<char> guessedLetters)
        {
            Console.WriteLine("Current Word: " + new string(hiddenWord));
            Console.WriteLine("Remaining Attempts: " + remainingAttempts);
            Console.WriteLine("Guessed Letters: " + string.Join(", ", guessedLetters));
        }

        public static void RevealGuessedLetters(string wordToGuess, char[] hiddenWord, char guessedChar)
        {
            for (int i = 0; i < wordToGuess.Length; i++)
            {
                if (wordToGuess[i] == guessedChar)
                {
                    hiddenWord[i] = guessedChar;
                }
            }
        }

        public static void DisplayFinalResult(string wordToGuess, char[] hiddenWord, int remainingAttempts)
        {
            if (new string(hiddenWord) == wordToGuess)
            {
                Console.WriteLine($"Congratulations! You guessed the word: {wordToGuess}");
            }
            else
            {
                Console.WriteLine($"Game Over! The word was: {wordToGuess}");
            }
        }

        public static void ContinuePrompt()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
