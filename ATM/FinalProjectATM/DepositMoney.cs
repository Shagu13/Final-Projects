using System;


namespace FinalProjectATM
{
    internal class DepositMoney
    {

        public void AddAmount()
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
                        DepositToBalance(Currency.GEL);
                        break;
                    case Currency.USD:
                        DepositToBalance(Currency.USD);
                        break;
                    case Currency.EUR:
                        DepositToBalance(Currency.EUR);
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

        private void DepositToBalance(Currency currency)
        {
            Console.Write($"How much {currency} would you like to Deposit? ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount >= 0)
            {
                var path = new filePath().GetPath();
                var brain = new Brain(path);
                var balance = brain.GetBalance();
                var transaction = new Brain(path);
                switch (currency)
                {
                    case Currency.GEL:
                        if (amount >= 0)
                        {
                            balance.amountGEL += amount;
                            transaction.UpdateTransactionHistory(TransactionType.Deposit, amount, currency);
                            Console.WriteLine($"Deposit of {amount} {currency} successful.");
                        }
                        else
                        {
                            Console.WriteLine("Please enter numbers only.");
                        }
                        break;
                    case Currency.USD:
                        if (amount >= 0)
                        {
                            balance.amountUSD += amount;
                            transaction.UpdateTransactionHistory(TransactionType.Deposit, amount, currency);
                            Console.WriteLine($"Deposit of {amount} {currency} successful.");
                        }
                        else
                        {
                            Console.WriteLine("Please enter numbers only.");
                        }
                        break;
                    case Currency.EUR:
                        if (amount >= 0)
                        {
                            balance.amountEUR += amount;
                            transaction.UpdateTransactionHistory(TransactionType.Deposit, amount, currency);
                            Console.WriteLine($"Deposit of {amount} {currency} successful.");
                        }
                        else
                        {
                            Console.WriteLine("Please enter numbers only.");
                        }
                        break;
                }
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
