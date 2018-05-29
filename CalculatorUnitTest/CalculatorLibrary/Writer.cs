using System;
using CalculatorLibrary;

namespace CalculatorConsole
{
    public class Writer : IWriter
    {
        public void WriteMessage(string userMessage)
        {
            Console.WriteLine(userMessage);
        }

        public void WriteResults(double result)
        {
            Console.WriteLine("The result is : " + result);
        }
    }
}