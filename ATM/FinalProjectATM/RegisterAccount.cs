using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;


namespace FinalProjectATM
{
    public class ClientDetails
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int pinCode { get; set; }
        public CardDetails cardDetails { get; set; }
        public TransactionHistory transactionHistory { get; set; }
    }
    public class CardDetails
    {
        public string cardNumber { get; set; }
        public string expirationDate { get; set; }
        public int CVC { get; set; }
    }
    public class TransactionHistory
    {
        public string transactionDate { get; set; }
        public string transactionType { get; set; }
        public decimal amountGEL { get; set; }
        public decimal amountUSD { get; set; }
        public decimal amountEUR { get; set; }
    }
    public class RegisterNewClient
    {
        public void Register()
        {
            var check = new Check();
            Console.Write("Please type your first name: ");
            var firstName = check.CheckStringInput("Invalid input. Please enter only letters.\nPlease type your first name: ");

            Console.Write("Please type your last name: ");
            var lastName = check.CheckStringInput("Invalid input. Please enter only letters.\nPlease type your last name: ");

            Console.Write("Please type your pin code: ");
            var pinCode = check.CheckPinCode("Invalid input.Pin must contain 4 digits and numbers only.\nPlease type your pin code: ");

            Console.Write("Please type your card number (format: 1234-5678-9012-3456): ");
            var cardNumber = check.CheckCardNumber("Invalid input.\nCard number must contain 16 digits and numbers only." +
                "\nPlease enter the card number in the format 1234-5678-9012-3456: ");

            Console.Write("Please type your expiration date MM/YY: ");
            var expirationDateInput = check.CheckExpDate("Invalid input." +
                "\nExpiration Date format must contain numbers only." +
                "\nPlease enter the expiration date in the format MM/YY with 2 digits on each side: ");

            Console.Write("Please type your CVC: ");
            var cvc = check.CheckCVC("Invalid input.CVC must contain 3 digits and numbers only.\nPlease type your CVC: ");

            CardDetails cardInfo = new CardDetails
            {
                cardNumber = cardNumber,
                expirationDate = expirationDateInput,
                CVC = cvc
            };

            TransactionHistory history = new TransactionHistory
            {
                transactionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                transactionType = "Registration",
                amountGEL = 0,
                amountUSD = 0,
                amountEUR = 0
            };

            JObject o = new JObject(
                new JProperty("firstName", firstName),
                new JProperty("lastName", lastName),
                new JProperty("cardDetails", new JObject(
                    new JProperty("cardNumber", cardInfo.cardNumber),
                    new JProperty("expirationDate", cardInfo.expirationDate),
                    new JProperty("CVC", cardInfo.CVC)
                )),
                new JProperty("pinCode", pinCode),
                new JProperty("transactionHistory", new JArray(
                    new JObject(
                        new JProperty("transactionDate", history.transactionDate),
                        new JProperty("transactionType", history.transactionType),
                        new JProperty("amountGEL", history.amountGEL),
                        new JProperty("amountUSD", history.amountUSD),
                        new JProperty("amountEUR", history.amountEUR)
                    )
                ))
            );

            var path = Path.GetFullPath(@"C:\Users\User\Desktop\Programming\COMM School\FinalProjectATM\FinalProjectATM\");
            string fileName = "Data.json";
            string filePath = Path.Combine(path, fileName);

            try
            {
                Directory.CreateDirectory(path);
                string jsonContent = JsonConvert.SerializeObject(o, Formatting.Indented);
                File.WriteAllText(filePath, jsonContent);
                Console.WriteLine($"JSON file '{filePath}' created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
