// See https://aka.ms/new-console-template for more information

using System;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace IDValidation
{
    class Program
    {
        const bool devDebugMode = false;
        static void Main(string[] args)
        {
            while (1 == 1)
            {
                var input = Initialize();
                if (devDebugMode) { Console.WriteLine("validateInput called"); }
                if (!ValidateInput(input))
                {
                    break;
                }
            }
        }

        static string Initialize()
        {
            Console.WriteLine("");
            Console.WriteLine("Please input a valid ID code or link. Write 'Exit' to close the application.");
            if (devDebugMode) { Console.WriteLine("Debug mode is ON"); }

            var input = Console.ReadLine();
            var urlInput= "";
            if (input.Contains("https"))
            {
                using (WebClient client = new WebClient())
                {
                    urlInput = client.DownloadString(input);
                }
                if (devDebugMode) { Console.WriteLine("urlString: " + urlInput); }
                return urlInput;
            }
            else
            {
                return input;
            }
            Console.WriteLine("");
        }

        static bool ValidateInput(string input)
        {

            if (input.ToLower() == "exit")
            {
                return false;
            }
            else if (input.Contains("["))
            {
                if (devDebugMode) { Console.WriteLine("urlPreCalc called"); }
                UrlPreCalc(input);
                if (devDebugMode) { Console.WriteLine("after urlPreCalc called"); }
            }
            else if (input.Length == 10)
            {
                if (IsDigitsOnly(input))
                {
                    if (devDebugMode) { Console.WriteLine("Calc called"); }
                    int calcVal = Calc(input);
                    if (calcVal == 0) { Console.WriteLine("This is a valid ID"); }
                    else if (calcVal == 1) { Console.WriteLine("This is an invalid ID"); }
                }
                else
                {
                    Console.WriteLine("This is an invalid ID");
                }
            }
            else
            {
                Console.WriteLine("This ID is invalid");
            }
            return true;
        }

        static void UrlPreCalc(string input)
        {
            int count = 0;
            int validCount = 0;
            int inValidCount = 0;
            int inValidOtherCount = 0;
            char delim = '"';
            string trimInput = input.Replace(",", "");
            trimInput = trimInput.Replace("[", "");
            trimInput = trimInput.Replace("]", "");
            string[] idArray = trimInput.Split(delim);
            idArray = idArray.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            foreach (string i in idArray)
            {
                Console.WriteLine("ID: " + i);

                int calcVal = Calc(i);
                if (calcVal == 0)
                {
                    Console.WriteLine("This is a valid ID");
                    validCount++;
                }
                else if (calcVal == 1)
                {
                    Console.WriteLine("This is a invalid ID due to an invalid check digit");
                    inValidCount++;
                }
                if (calcVal == 2)
                {
                    Console.WriteLine("This is a invalid ID for other reasons");
                    inValidOtherCount++;
                }

                count++;
            }
            Console.WriteLine("ID Count: " + count);
            Console.WriteLine("Valid ID Count: " + validCount);
            Console.WriteLine("Invalid ID Count: " + inValidCount);
            Console.WriteLine("invalid Other ID Count: " + inValidOtherCount);
        }

        static int Calc(string input)
        {
            if (devDebugMode) { Console.WriteLine("Calc Begin, input: " + input); }
            var inputArray = input.ToCharArray();
            int count = 1;
            int multipyValue = 11;
            int valueSum = 0;
            int remainder = 0;
            int result = 0;

            if (input.Length != 10)
            {
                return 2;
            }
            else
            {
                var checkDigit = inputArray[9];
                while (count < 10)
                {
                    char i = inputArray[count - 1];
                    if (devDebugMode) { Console.WriteLine("----------Multiply value: " + multipyValue); }
                    if (devDebugMode) { Console.WriteLine("Current value: " + inputArray[count - 1]); }

                    valueSum += Convert.ToInt32(char.GetNumericValue(i)) * multipyValue; //step 2

                    if (devDebugMode) { Console.WriteLine("Value Sum: " + valueSum); }

                    multipyValue--;
                    count++;
                }


                //remainder = valueSum / 12; //step 3
                Math.DivRem(valueSum, 12, out remainder);
                if (devDebugMode) { Console.WriteLine("remainder: " + remainder); }
                result = 12 - remainder; //step 4
                if (devDebugMode) { Console.WriteLine("result: " + result); }

                switch (result)
                {
                    case 10:
                        if (devDebugMode) { Console.WriteLine("case 10"); }
                        return 1;

                    case 12:
                        if (devDebugMode) { Console.WriteLine("case 12"); }
                        return 1;

                    case 11:
                        if (devDebugMode) { Console.WriteLine("case 11"); }
                        if (Convert.ToInt32(char.GetNumericValue(checkDigit)) == 0) { return 0; }
                        else { return 1; }

                    default:
                        if (devDebugMode) { Console.WriteLine("case default"); }
                        if (result == Convert.ToInt32(char.GetNumericValue(checkDigit))) { return 0; }
                        else { return 1; }
                }
            }
            if (devDebugMode) { Console.WriteLine("Calc End"); }
        }

        static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
    }
}