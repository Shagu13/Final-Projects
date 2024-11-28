using static Hangman.Data;  
using static Hangman.Brain;


namespace Hangman
{
    public class LaunchProject
    {
        private string wordToGuess;
        private char[] hiddenWord;
        private HashSet<char> guessedLetters;
        private int remainingAttempts;

        public LaunchProject(string wordToGuess)
        {
            this.wordToGuess = wordToGuess;
            hiddenWord = new string('_', wordToGuess.Length).ToCharArray();
            guessedLetters = new HashSet<char>();
            remainingAttempts = 6;
        }

        public void RunGameLoop()
        {
            while (remainingAttempts > 0 && new string(hiddenWord) != wordToGuess)
            {
                Console.Clear();
                DisplayHangman(6 - remainingAttempts); 
                DisplayGameState(hiddenWord, remainingAttempts, guessedLetters);

                Console.Write("\nEnter your guess (single letter): ");
                string input = Console.ReadLine()?.ToLower();

                if (string.IsNullOrWhiteSpace(input) || input.Length != 1)
                {
                    Console.WriteLine("Invalid input. Please enter a single letter.");
                    ContinuePrompt(); 
                    continue;
                }

                char guessedChar = input[0];

                if (guessedLetters.Contains(guessedChar))
                {
                    Console.WriteLine("You already guessed that letter. Try another one.");
                    ContinuePrompt();
                    continue;
                }

                guessedLetters.Add(guessedChar);

                if (wordToGuess.Contains(guessedChar))
                {
                    Console.WriteLine($"Good job! The letter '{guessedChar}' is in the word.");
                    RevealGuessedLetters(wordToGuess, hiddenWord, guessedChar); 
                }
                else
                {
                    Console.WriteLine($"Sorry! The letter '{guessedChar}' is not in the word.");
                    remainingAttempts--;
                }

                ContinuePrompt(); 
            }

            Console.Clear();
            DisplayHangman(6 - remainingAttempts); 
            DisplayFinalResult(wordToGuess, hiddenWord, remainingAttempts); 
        }
    }
}
