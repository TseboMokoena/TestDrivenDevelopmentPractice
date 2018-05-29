using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CalculatorLibrary
{
    [System.Runtime.InteropServices.Guid("99EBF6A8-1528-453B-8CF1-5E6360D44333")]
    public class Calculator
    {
        private double _finalNum;
        private IList<double> _tokens = new List<double>();
        private ILogger _logger;
        private IWebService _exceptionService;
        private IWriter _writer;

        public Calculator(ILogger logger, IWebService exceptionService, IWriter writer)
        {
            _logger = logger;
            _exceptionService = exceptionService;
            _writer = writer;
        }

        public double Addition(string mixedString)
        {
            MatchCollection matches = GetDelimiters(mixedString);

            foreach (Match m in matches)
            {
                string del = m.ToString();
                mixedString = PopulateTokens(mixedString, del);
            }

            try
            {
                _logger.LogResults(_tokens.Sum());
            }
            catch (Exception ex)
            {
                _exceptionService.ExceptionHandler(ex);
                throw; 
            }
            _writer.WriteResults(_tokens.Sum());
            return _tokens.Sum();
        }

        internal MatchCollection GetDelimiters(string mixedString)
        {
            var pattern = @"\[(.*?)\]";
            var query = mixedString;
            MatchCollection matches = Regex.Matches(query, pattern);
            return matches;
        }

        private string PopulateTokens(string numbers, string del)
        {
            string invalidTokens = string.Empty;
            var newTokens = numbers.Split(del.ToCharArray());

            foreach (var newToken in newTokens)
            {
                if (double.TryParse(newToken, out var value))
                {
                    VerifyNoNegativeNumbers(value);
                    _finalNum = CheckIfGreaterThanThousand(value);
                    _tokens.Add(value);
                }
                else
                {
                    invalidTokens += newToken;
                }
            }
            return invalidTokens;
        }

        public void VerifyNoNegativeNumbers(double number)
        {
            var negativeNumbers = "";

            try
            {
                if (number < 0)
                {
                    negativeNumbers += number + " , ";
                    throw new Exception("Negatives not allowed, the following operands are negative: " + negativeNumbers);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public double CheckIfGreaterThanThousand(double number)
        {
            if (number >= 1000)
            {
                number = 0;
            }
            return number;
        }
    }
}


