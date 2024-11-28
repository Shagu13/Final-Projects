using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Calculator
    {
        public void StartCalculator()
        {
            while (true)
            {
                try
                {
                    double firstNumber = GetNumber("Enter the first number:");
                    string operation = GetOperator();
                    double secondNumber = GetNumber("Enter the second number:");

                    if (operation == "/" && secondNumber == 0)
                        throw new DivideByZeroException("Division by zero is not allowed.");

                    double result = PerformCalculation(firstNumber, secondNumber, operation);
                    Console.WriteLine($"Result: {firstNumber} {operation} {secondNumber} = {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    continue;
                }

                Console.WriteLine("\nWould you like to perform another calculation? (yes/no):");
                string response = Console.ReadLine()?.Trim().ToLower();
                if (response == "yes")
                {
                    Console.Clear();
                    continue;
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("Thank you for using the calculator. Goodbye!");
        }

        private double GetNumber(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                if (double.TryParse(Console.ReadLine(), out double number))
                    return number;

                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }

        private string GetOperator()
        {
            while (true)
            {
                Console.WriteLine("Enter an operator (+, -, *, /, %):");
                string operation = Console.ReadLine();
                if (IsValidOperator(operation))
                    return operation;

                Console.WriteLine("Invalid operator. Please enter a valid operator (+, -, *, /, %).");
            }
        }

        private bool IsValidOperator(string operation)
        {
            return operation switch
            {
                "+" or "-" or "*" or "/" or "%" => true,
                _ => false
            };
        }

        private double PerformCalculation(double firstNumber, double secondNumber, string operation)
        {
            return operation switch
            {
                "+" => firstNumber + secondNumber,
                "-" => firstNumber - secondNumber,
                "*" => firstNumber * secondNumber,
                "/" => firstNumber / secondNumber,
                "%" => firstNumber % secondNumber,
                _ => throw new InvalidOperationException("Invalid operation.")
            };
        }
    }
}
