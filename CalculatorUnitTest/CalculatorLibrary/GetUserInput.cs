using System;

namespace CalculatorConsole
{
    public class GetUserInput : IGetUserInput
    {
        public string GetNextUserInput()
        {
           var userInput =  Console.ReadLine();

            return userInput; 
        }
    }
}