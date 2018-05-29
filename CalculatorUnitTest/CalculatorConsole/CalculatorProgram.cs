using CalculatorLibrary;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using Unity;

namespace CalculatorConsole
{
    public class CalculatorProgram : ICommandLineInterface
    {
        private static UnityContainer Container = new UnityContainer();
        private static IGetUserInput _getUserInput;
        private static ILogger _logger;
        private static IWebService _exceptionService;
        private static IWriter _writer;
        private const string UserMessage1 = "To start the calculator:\nType \"scalc\" followed by the numbers you want to add e.g. scalc '1, 2, 3' \nPress Enter to exit.";
        private const string UserMessage2 = "Another input please";
        private const string UserMessage3 = "Invalid Entry! Try again";
        private const string UserMessage4 = "Calculator will now exit.";

        public CalculatorProgram(IGetUserInput getUserInput, ILogger logger, IWebService exceptionService, IWriter writer)
        {
            _logger = logger;
            _exceptionService = exceptionService;
            _writer = writer;
            _getUserInput = getUserInput;
        }

        public static void ContainerConstuctor()
        {
            Container.RegisterType<IGetUserInput, GetUserInput>();
            Container.RegisterType<ILogger, Logger>();
            Container.RegisterType<IWebService, WebService>();
            Container.RegisterType<IWriter, Writer>();
            Container.RegisterType<CalculatorProgram>();
        }

        public static void Main(string[] args)
        {
            try
            {
                ContainerConstuctor();
                
                var calculatorProgram = Container.Resolve<CalculatorProgram>(); ;

                calculatorProgram.StartCalculator();
            }
            catch (Exception e)
            {
                _exceptionService.ExceptionHandler(e);
                throw;
            }
        }


        public string[] GetCommandLineArgs()
        {
            try
            {
                return Environment.GetCommandLineArgs();
            }
            catch (Exception e)
            {
               _exceptionService.ExceptionHandler(e);
                throw;
            }
            
        }

        public void StartCalculator()
        {
            var running = true;

            _writer.WriteMessage(UserMessage1);

            try
            {
                while (running)
                {
                    var startString = _getUserInput.GetNextUserInput();

                    if (!string.IsNullOrEmpty(startString) && startString.Contains("scalc"))
                    {
                        DoAdition(startString);
                        _writer.WriteMessage(UserMessage2);
                    }
                    else if (!string.IsNullOrEmpty(startString) && !startString.Contains("scalc"))
                    {
                        _writer.WriteMessage(UserMessage3);
                       
                    }
                    else
                    {
                        running = false;
                        _writer.WriteMessage(UserMessage4);
                        int milliseconds = 1000;
                        Thread.Sleep(milliseconds);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                _exceptionService.ExceptionHandler(e);
                throw;
            }
        }

        public double DoAdition(string startString)
        {       
            double result = 0;

            try
            {       var additionString = "//[,]\n";
                    const string pattern = @"\'(.*?)\'";
                    var query = startString;
                    var matches = Regex.Match(query, pattern);

                    additionString = additionString + matches.Groups[1].Value;

                    var calculator = new Calculator(_logger, _exceptionService, _writer);
                    result = calculator.Addition(additionString);
            }
            catch (Exception e)
            {
                _exceptionService.ExceptionHandler(e);
                throw;
            }
            return result;
        }
    }
}