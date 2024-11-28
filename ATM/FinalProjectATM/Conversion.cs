using FinalProjectATM;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace FinalProjectATM
{
    internal class Conversion
    {

        private const double ExchangeRateBuyUSDwithGEL = 0.367;
        private const double exchangerateBuyGELwithEUR = 2.90;
        private const double exchangerateBuyGELwithUSD = 2.67;
        private const double ExchangeRateBuyEURwithGEL = 2.95;
        private const double ExchangeRateBuyEURwithUSD = 0.922;
        private const double ExchangeRateBuyUSDwithEUR = 1.084;

        public void ChangeMoney()
        {

            while (true)
            {
                var path = new filePath().GetPath();
                var balance = new Brain(path).GetBalance();
                Console.WriteLine("Please Select Currency 1:");
                Console.WriteLine($"{(int)Currency.GEL}. {balance.amountGEL} GEL");
                Console.WriteLine($"{(int)Currency.USD}. {balance.amountUSD} USD");
                Console.WriteLine($"{(int)Currency.EUR}. {balance.amountEUR} EUR");
                Console.WriteLine($"{(int)Currency.Exit}. Exit");

                Currency choice = SelectCurrency();

                switch (choice)
                {
                    case Currency.GEL:
                        ConvertMoney(Currency.GEL);
                        break;
                    case Currency.USD:
                        ConvertMoney(Currency.USD);
                        break;
                    case Currency.EUR:
                        ConvertMoney(Currency.EUR);
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

        public void ConvertMoney(Currency fromCurrency)
        {
            var path = new filePath().GetPath();
            var balance = new Brain(path).GetBalance();

            Console.Write($"How much {fromCurrency} would you like to convert? ");

            if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
            {
                double fromCurrencyBalance = 0;

                switch (fromCurrency)
                {
                    case Currency.GEL:
                        fromCurrencyBalance = balance.amountGEL;
                        break;
                    case Currency.USD:
                        fromCurrencyBalance = balance.amountUSD;
                        break;
                    case Currency.EUR:
                        fromCurrencyBalance = balance.amountEUR;
                        break;
                }

                if (amount <= fromCurrencyBalance)
                {
                    Console.WriteLine("Select the target currency:");
                    Console.WriteLine($"{(int)Currency.GEL}. GEL");
                    Console.WriteLine($"{(int)Currency.USD}. USD");
                    Console.WriteLine($"{(int)Currency.EUR}. EUR");

                    Currency toCurrency = SelectCurrency();

                    double convertedAmount = ConvertCurrency(amount, fromCurrency, toCurrency);

                    if (convertedAmount > 0)
                    {
                        Console.WriteLine($"Converted amount: {convertedAmount} {toCurrency}");
                        UpdateBalances(fromCurrency, toCurrency, amount, convertedAmount);
                    }
                    else
                    {
                        Console.WriteLine("Invalid target currency. Conversion failed.");
                    }
                }
                else
                {
                    Console.WriteLine($"Insufficient {fromCurrency} balance. Cannot convert.");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount. Please enter a valid number.");
            }
        }

        private double ConvertCurrency(double amount, Currency fromCurrency, Currency toCurrency)
        {
            double exchangeRate;

            switch (fromCurrency)
            {
                case Currency.GEL:
                    exchangeRate = (toCurrency == Currency.USD) ? ExchangeRateBuyUSDwithGEL :
                                   (toCurrency == Currency.EUR) ? ExchangeRateBuyEURwithGEL :
                                   1;
                    break;
                case Currency.USD:
                    exchangeRate = (toCurrency == Currency.GEL) ? exchangerateBuyGELwithUSD :
                                   (toCurrency == Currency.EUR) ? ExchangeRateBuyEURwithUSD :
                                   1;
                    break;
                case Currency.EUR:
                    exchangeRate = (toCurrency == Currency.GEL) ? exchangerateBuyGELwithEUR :
                                   (toCurrency == Currency.USD) ? ExchangeRateBuyUSDwithEUR :
                                   1;
                    Console.WriteLine(exchangeRate);
                    break;
                default:
                    return -1;
            }

            return amount * exchangeRate;
        }

        private void UpdateBalances(Currency fromCurrency, Currency toCurrency, double originalAmount, double convertedAmount)
        {
            var path = new filePath().GetPath();
            var brain = new Brain(path);

            var jsonData = brain.LoadData();
            if (jsonData != null)
            {
                var transactionHistory = jsonData["transactionHistory"] as JArray;

                if (transactionHistory != null)
                {
                    var newTransaction = new JObject
                    {
                        ["transactionDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        ["transactionType"] = "Converted",
                        ["amountGEL"] = (fromCurrency == Currency.GEL) ? (double)transactionHistory.Last?["amountGEL"] - (double)originalAmount : (double)transactionHistory.Last?["amountGEL"],
                        ["amountUSD"] = (fromCurrency == Currency.USD) ? (double)transactionHistory.Last?["amountUSD"] - (double)originalAmount : (double)transactionHistory.Last?["amountUSD"],
                        ["amountEUR"] = (fromCurrency == Currency.EUR) ? (double)transactionHistory.Last?["amountEUR"] - (double)originalAmount : (double)transactionHistory.Last?["amountEUR"],
                    };
                    transactionHistory.Add(newTransaction);

                    while (transactionHistory.Count > 5)
                    {
                        transactionHistory.RemoveAt(0);
                    }

                    switch (toCurrency)
                    {
                        case Currency.GEL:
                            transactionHistory.Last["amountGEL"] = (double)transactionHistory.Last?["amountGEL"] + (double)convertedAmount;
                            break;
                        case Currency.USD:
                            transactionHistory.Last["amountUSD"] = (double)transactionHistory.Last?["amountUSD"] + (double)convertedAmount;
                            break;
                        case Currency.EUR:
                            transactionHistory.Last["amountEUR"] = (double)transactionHistory.Last?["amountEUR"] + (double)convertedAmount;
                            break;
                    }

                    File.WriteAllText(path, jsonData.ToString());
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
