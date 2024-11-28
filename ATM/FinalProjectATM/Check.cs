using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinalProjectATM
{
    public class Check
    {
        #region Check String inputs
        public string CheckStringInput(string errorMessage)
        {
            string input;
            while (true)
            {
                input = Console.ReadLine();
                if (IsAlpha(input))
                {
                    input = char.ToUpper(input[0]) + input.Substring(1).ToLower();
                    break;
                }
                Console.Write(errorMessage);
            }
            return input;
        }
        #endregion


        #region Check Pin Code
        public int CheckPinCode(string errorMessage)
        {
            int input;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out input) && input >= 1000 && input <= 9999)
                {
                    break;
                }
                Console.Write(errorMessage);
            }

            return input;
        }
        #endregion


        #region Check CVC
        public int CheckCVC(string errorMessage)
        {
            int input;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out input) && input >= 100 && input <= 999)
                {
                    break;
                }
                Console.Write(errorMessage);
            }

            return input;
        }
        #endregion


        #region Check Expiration Date
        public string CheckExpDate(string errorMessage)
        {
            string input;
            while (true)
            {
                input = Console.ReadLine();
                if (IsValidExpirationDateFormat(input) && IsValidExpirationMonth(input))
                {
                    break;
                }
                Console.Write(errorMessage);
            }

            return input;
        }

        static bool IsValidExpirationDateFormat(string expirationDate)
        {
            return Regex.IsMatch(expirationDate, @"^\d{2}/\d{2}$");
        }

        static bool IsValidExpirationMonth(string expirationDate)
        {
            string monthString = expirationDate.Split('/')[0];
            if (int.TryParse(monthString, out int month))
            {
                return month >= 1 && month <= 12;
            }

            return false;
        }
        #endregion


        #region Check Letter

        public bool IsAlpha(string input)
        {
            return !string.IsNullOrEmpty(input) && input.All(char.IsLetter);
        }
        #endregion


        #region Check Card Number
        public string CheckCardNumber(string errorMessage)
        {
            string input;
            while (true)
            {
                input = Console.ReadLine();
                if (IsValidCardNumberFormat(input))
                {
                    break;
                }
                Console.Write(errorMessage);
            }

            return input;
        }
        public bool IsValidCardNumberFormat(string cardNumber)
        {
            return Regex.IsMatch(cardNumber, @"^\d{4}-\d{4}-\d{4}-\d{4}$");
        }
        #endregion
    }
}
