using System;

namespace FinalProjectATM
{
    internal class WithdrawMoney
    {
        public void Withdraw()

        {
            while (true)
            {
                var path = new filePath().GetPath();
                var balance = new Brain(path).GetBalance();
                Console.WriteLine("Please Select Currency:");
                Console.WriteLine($"{(int)Currency.GEL}. {balance.amountGEL} GEL");
                Console.WriteLine($"{(int)Currency.USD}. {balance.amountUSD} USD");
                Console.WriteLine($"{(int)Currency.EUR}. {balance.amountEUR} EUR");
                Console.WriteLine($"{(int)Currency.Exit}. Exit");

                Currency choice = SelectCurrency();

                switch (choice)
                {
                    case Currency.GEL:
                        WithdrawFromBalance(Currency.GEL);
                        break;
                    case Currency.USD:
                        WithdrawFromBalance(Currency.USD);
                        break;
                    case Currency.EUR:
                        WithdrawFromBalance(Currency.EUR);
                        break;
                    case Currency.Exit:
                        Console.WriteLine("Exiting the application. Goodbye!");
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void WithdrawFromBalance(Currency currency)
        {
            Console.Write($"How much {currency} would you like to withdraw? ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
            {
                var path = new filePath().GetPath();
                var brain = new Brain(path);
                var balance = brain.GetBalance();
                var transaction = new Brain(path);

                switch (currency)
                {
                    case Currency.GEL:
                        if (amount <= balance.amountGEL)
                        {
                            balance.amountGEL -= amount;
                            transaction.UpdateTransactionHistory(TransactionType.Withdrawal, amount, currency);
                            Console.WriteLine($"Withdrawal of {amount} {currency} successful.");
                        }
                        else
                        {
                            Console.WriteLine("Insufficient balance. Please try again.");
                        }
                        break;
                    case Currency.USD:
                        if (amount <= balance.amountUSD)
                        {
                            balance.amountUSD -= amount;
                            transaction.UpdateTransactionHistory(TransactionType.Withdrawal, amount, currency);
                            Console.WriteLine($"Withdrawal of {amount} {currency} successful.");
                        }
                        else
                        {
                            Console.WriteLine("Insufficient balance. Please try again.");
                        }
                        break;
                    case Currency.EUR:
                        if (amount <= balance.amountEUR)
                        {
                            balance.amountEUR -= amount;
                            transaction.UpdateTransactionHistory(TransactionType.Withdrawal, amount, currency);
                            Console.WriteLine($"Withdrawal of {amount} {currency} successful.");
                        }
                        else
                        {
                            Console.WriteLine("Insufficient balance. Please try again.");
                        }
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid positive amount.");
            }
        }
        private Currency SelectCurrency()
        {
            Console.Write("Enter your choice (1-4): ");
            Currency choice;
            while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(Currency), choice))
            {
                Console.Write("Invalid input. Enter your choice (1-4): ");
            }
            return choice;
        }
    }
}
