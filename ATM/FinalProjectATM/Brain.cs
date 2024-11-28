using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FinalProjectATM
{
    internal class Brain
    {
        public string Path { get; private set; }
        public ClientDetails ClientDetails { get; private set; }

        public Brain(string path)
        {
            Path = path;
        }


        #region Load Json Data

        public JObject LoadData()
        {
            string jsonString = null;
            JObject jsonData = null;

            try
            {
                jsonString = File.ReadAllText(Path);
                jsonData = JObject.Parse(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading client details from file: {ex.Message}");
            }

            return jsonData;
        }

        #endregion


        #region Verify Pin

        public bool VerifyPin()
        {
            var check = new Check();
            Console.Write("Enter your PIN: ");

            var enteredPin = check.CheckPinCode("Invalid input. Pin must contain 4 digits and numbers only.\nEnter your PIN: ");
            var pinDetail = LoadData()?["pinCode"]?.Value<int>();
            if (pinDetail == enteredPin)
            {
                Console.Clear();

                return true;
            }
            else
            {
                AskForDetails();
            }
            return false;
        }
        #endregion


        #region Display Menu

        public void DisplayMenu()
        {
            var person = LoadData();
            while (true)
            {
                Console.WriteLine($"Welcome, {person["firstName"]} {person["lastName"]}!");
                Console.WriteLine("Select an option:");
                Console.WriteLine($"{(int)MenuOption.CheckBalance}. Check Balance");
                Console.WriteLine($"{(int)MenuOption.Withdraw}. Withdraw");
                Console.WriteLine($"{(int)MenuOption.GetLastTransactions}. Get Last 5 Transactions");
                Console.WriteLine($"{(int)MenuOption.AddAmount}. Add Amount");
                Console.WriteLine($"{(int)MenuOption.ChangePIN}. Change PIN");
                Console.WriteLine($"{(int)MenuOption.ChangeAmount}. Change Amount");
                Console.WriteLine($"{(int)MenuOption.Exit}. Exit");

                MenuOption choice = SelectOption();

                switch (choice)
                {
                    case MenuOption.CheckBalance:
                        CheckDeposit();
                        AskForDetails();
                        VerifyPin();
                        break;
                    case MenuOption.Withdraw:
                        new WithdrawMoney().Withdraw();

                        break;
                    case MenuOption.GetLastTransactions:
                        GetLastTransactions();
                        break;
                    case MenuOption.AddAmount:
                        new DepositMoney().AddAmount();

                        break;
                    case MenuOption.ChangePIN:
                        ChangePin();

                        break;
                    case MenuOption.ChangeAmount:
                        new Conversion().ChangeMoney();

                        break;
                    case MenuOption.Exit:
                        Console.WriteLine("Exiting the application. Goodbye!");
                        ;
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private MenuOption SelectOption()
        {
            Console.Write("Enter your choice (1-7): ");
            MenuOption choice;
            while (!Enum.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(MenuOption), choice))
            {
                Console.Write("Invalid input. Enter your choice (1-7): ");
            }
            return choice;
        }
        #endregion



        #region Veirify Card Details

        public (string CardNumber, int CVC, string ExpirationDate) AskForDetails()
        {
            Console.Clear();
            var check = new Check();
            Console.Write("Please type your card number (format: 1234-1234-1234-1234): ");
            var cardNumber = check.CheckCardNumber("Invalid input.\nCard number must contain 16 digits and numbers only." +
                "\nPlease enter the card number in the format 1234-1234-1234-1234: "); 

            Console.Write("Please type your CVC: ");
            var cvc = check.CheckCVC("Invalid input.CVC must contain 3 digits and numbers only.\nPlease type your CVC: ");

            Console.Write("Please type your expiration date MM/YY: ");
            var expirationDateInput = check.CheckExpDate("Invalid input." +
                "\nExpiration Date format must contain numbers only." +
                "\nPlease enter the expiration date in the format MM/YY with 2 digits on each side: ");
            return (cardNumber, cvc, expirationDateInput);
        }

        public bool CheckData()
        {
            do
            {
                (string returnedCardNumber, int returnedCVC, string returnedExpirationDate) = AskForDetails();

                var cardDetails = LoadData()?["cardDetails"];
                if (cardDetails != null)
                {
                    if ((string)cardDetails["cardNumber"] == returnedCardNumber &&
                        (int)cardDetails["CVC"] == returnedCVC &&
                        (string)cardDetails["expirationDate"] == returnedExpirationDate)
                    {
                        return true;
                    }
                    Console.WriteLine("Please provide the correct data.");
                }
 
            } while (true);
        }
        #endregion


        #region Get Hold to Balances

        public (double amountGEL, double amountUSD, double amountEUR) GetBalance()
        {
            var jsonData = LoadData();

            if (jsonData != null)
            {
                var transactionHistory = jsonData["transactionHistory"] as JArray;

                if (transactionHistory != null && transactionHistory.Any())
                {
                    var lastTransaction = transactionHistory.Last;

                    var amountGEL = (double)lastTransaction["amountGEL"];
                    var amountUSD = (double)lastTransaction["amountUSD"];
                    var amountEUR = (double)lastTransaction["amountEUR"];

                    return (amountGEL, amountUSD, amountEUR);
                }
                else
                {
                    return (0, 0, 0);
                }
            }
            else
            {
                throw new InvalidOperationException("Error reading transaction history from file. Please try again.");
            }
        }

        #endregion


        #region Check Balance

        private void CheckDeposit()
        {
            Console.Clear();
            Console.WriteLine($"Amount GEL: {GetBalance().amountGEL}");
            Console.WriteLine($"Amount USD: {GetBalance().amountUSD}");
            Console.WriteLine($"Amount EUR: {GetBalance().amountEUR}");
            Console.WriteLine("\n\n\n>>>Press Enter to go Back<<<");
            Console.ReadLine();
            Console.Clear();
            UpdateTransactionHistoryFromMenu(MenuOption.CheckBalance);
        }
        #endregion


        #region Update Transaction History Withdraw/Deposit
        public string UpdateTransactionHistory(TransactionType transactionType, double amount, Currency currency)
        {
            var jsonData = LoadData();
            if (jsonData != null)
            {
                var transactionHistory = jsonData["transactionHistory"] as JArray;

                if (transactionHistory != null)
                {
                    var newTransaction = new JObject
                    {
                        ["transactionDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        ["transactionType"] = transactionType.ToString(),
                        ["amountGEL"] = (decimal)transactionHistory.Last?["amountGEL"],
                        ["amountUSD"] = (decimal)transactionHistory.Last?["amountUSD"],
                        ["amountEUR"] = (decimal)transactionHistory.Last?["amountEUR"],
                    };

                    switch (transactionType)
                    {
                        case TransactionType.Withdrawal:
                            newTransaction[$"amount{currency}"] = (double)transactionHistory.Last?[$"amount{currency}"] - amount;
                            break;
                        case TransactionType.Deposit:
                            newTransaction[$"amount{currency}"] = (double)transactionHistory.Last?[$"amount{currency}"] + amount;
                            break;
                        default:
                            throw new InvalidOperationException("Invalid transaction type.");
                    }

                    transactionHistory.Add(newTransaction);

                    while (transactionHistory.Count > 5)
                    {
                        transactionHistory.RemoveAt(0);
                    }

                    File.WriteAllText(Path, jsonData.ToString());

                    return "Transaction history updated successfully.";
                }
                else
                {
                    throw new InvalidOperationException("Transaction history is not a valid array. Please check your JSON file structure.");
                }
            }
            else
            {
                throw new InvalidOperationException("Error loading data from file. Please try again.");
            }
        }

        #endregion


        #region Update Transaction Hist for CehckBalance/ChangePing

        public string UpdateTransactionHistoryFromMenu(MenuOption menuOption)
        {
            var jsonData = LoadData();
            if (jsonData != null)
            {
                var transactionHistory = jsonData["transactionHistory"] as JArray;

                if (transactionHistory != null)
                {
                    var newTransaction = new JObject
                    {
                        ["transactionDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        ["transactionType"] = menuOption == MenuOption.CheckBalance ? "Checked Balance" :
                                              menuOption == MenuOption.ChangePIN ? "Changed Pin" :
                                              ToString(),
                        ["amountGEL"] = (decimal)transactionHistory.Last?["amountGEL"],
                        ["amountUSD"] = (decimal)transactionHistory.Last?["amountUSD"],
                        ["amountEUR"] = (decimal)transactionHistory.Last?["amountEUR"],
                    };
                    transactionHistory.Add(newTransaction);

                    while (transactionHistory.Count > 5)
                    {
                        transactionHistory.RemoveAt(0);
                    }

                    File.WriteAllText(Path, jsonData.ToString());

                    return "Transaction history updated successfully.";
                }
                else
                {
                    throw new InvalidOperationException("Transaction history is not a valid array. Please check your JSON file structure.");
                }
            }
            else
            {
                throw new InvalidOperationException("Error loading data from file. Please try again.");
            }
        }
        #endregion


        #region Get Last Transactions

        private void GetLastTransactions()
        {
            var brain = new Brain(new filePath().GetPath());
            var jsonData = brain.LoadData();

            if (jsonData != null)
            {
                var transactionHistory = jsonData["transactionHistory"] as JArray;

                if (transactionHistory != null && transactionHistory.Any())
                {
                    var last5Transactions = GetLastNTransactions(transactionHistory, 5);

                    var newTransactionHistory = new JObject
                    {
                        ["transactionHistory"] = new JArray(last5Transactions)
                    };

                    var json = newTransactionHistory.ToString(Formatting.Indented);

                    Console.WriteLine(json);
                }
                else
                {
                    Console.WriteLine("No transactions available.");
                }
            }
            else
            {
                Console.WriteLine("Error loading data from file. Please try again.");
            }

            Console.WriteLine("\n\n\n>>>Press Enter to go Back<<<");
            Console.ReadLine();
            Console.Clear();
            brain.UpdateTransactionHistoryFromMenu(MenuOption.GetLastTransactions);
        }

        private List<JToken> GetLastNTransactions(JArray transactions, int n)
        {
            int count = Math.Min(n, transactions.Count);
            return transactions.Reverse().Take(count).ToList();
        }

        #endregion


        #region Change Pin

        private void ChangePin()
        {
            var jsonData = LoadData();
            var pinCode = jsonData?["pinCode"];

            if (pinCode != null)
            {
                AskQuestion("Enter your current PIN: ",
                    answer => int.TryParse(answer, out int _),
                    answer =>
                    {
                        int currentPin = int.Parse(answer);
                        if ((int)pinCode == currentPin)
                        {
                            AskQuestion("Enter your new PIN: ",
                                newPin => int.TryParse(newPin, out int _),
                                newPin =>
                                {
                                    jsonData["pinCode"] = int.Parse(newPin);
                                    File.WriteAllText(Path, jsonData.ToString());
                                    Console.WriteLine("PIN changed successfully.");
                                    UpdateTransactionHistoryFromMenu(MenuOption.ChangePIN);
                                });
                        }
                        else
                        {
                            Console.WriteLine("Incorrect current PIN. PIN not changed.");
                        }
                    });
            }
            else
            {
                Console.WriteLine("Error reading PIN from file. Please try again.");
            }
        }

        #endregion


        public void AskQuestion(string question, Func<string, bool> validator, Action<string> action)
        {
            Console.Write(question);
            string answer;
            do
            {
                answer = Console.ReadLine();
                if (validator(answer))
                {
                    action(answer);
                    break;
                }
                Console.Write("Invalid input. " + question);
            } while (true);
        }
    }

    public enum MenuOption
    {
        CheckBalance = 1,
        Withdraw,
        GetLastTransactions,
        AddAmount,
        ChangePIN,
        ChangeAmount,
        Exit
    }
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Converted
    }
    public enum Currency
    {
        GEL = 1,
        USD,
        EUR,
        Exit
    }
}
