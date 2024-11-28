namespace Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome To the Calculator!");
            Console.WriteLine("You can perform operations: Addition (+), Subtraction (-), Multiplication (*), Division (/), Modulus (%)");

            var calculator = new Calculator();

            try
            {
                calculator.StartCalculator();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}