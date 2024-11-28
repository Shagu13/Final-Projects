using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheNumber
{
    public class GuessTheNumber
    {
        private readonly int _correctNumber;
        private const int MaxTries = 8;
        private const int MinValue = 0;
        private const int MaxValue = 1001;


        public GuessTheNumber()
        {
            var random = new Random();
            _correctNumber = random.Next(MinValue, MaxValue);
        }

        public void StartGame()
        {
            int triesLeft = MaxTries;

            while (triesLeft > 0)
            {
                Console.WriteLine($"You have {triesLeft} attempts left. Guess a number between {MinValue} and {MaxValue}:");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int guessedNumber))
                {
                    if (guessedNumber < 0 || guessedNumber > 1000)
                    {
                        Console.WriteLine("Please guess a number within the range (0-1000).");
                        continue;
                    }

                    if (guessedNumber == _correctNumber)
                    {
                        Console.WriteLine("Congratulations! You guessed the correct number!");
                        return;
                    }

                    GiveHint(guessedNumber);
                    triesLeft--;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

            Console.WriteLine($"Sorry, you've run out of tries. The correct number was {_correctNumber}.");
        }

        private void GiveHint(int guessedNumber)
        {
            if (guessedNumber < _correctNumber)
            {
                Console.WriteLine("Go up!");
            }
            else
            {
                Console.WriteLine("Go down!");
            }
        }
    }
}
