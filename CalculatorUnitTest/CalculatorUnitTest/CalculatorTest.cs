using System;
using System.Text.RegularExpressions;
using CalculatorConsole;
using CalculatorLibrary;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CalculatorUnitTest
{
    [TestClass]
    public class CalculatorUnitTest
    {
        [TestMethod]
        public void Addition_WithEmptyString_ShouldReturnZero()
        {
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();

            Calculator calc = new Calculator(mockLogger.Object, mockWebService.Object, mockWriter.Object);

            //Act
            double result = calc.Addition("");

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Addition_StringSeparatedBySingleDelimiter_ShouldAddStingNumbers()
        {
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();

            Calculator calc = new Calculator(mockLogger.Object, mockWebService.Object, mockWriter.Object);

            //Act
            double result = calc.Addition("//[,]\n1,2,3");

            //Assert
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void Addition_StringSeparatedBySinglDelimiterRepeated_ShouldAddStingNumbers()
        {
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();

            Calculator calc = new Calculator(mockLogger.Object, mockWebService.Object, mockWriter.Object);

            //Act
            double result = calc.Addition("//[***]\n1***2***3");

            //Assert
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void Addition_StringSeparatedByMultipleDelimiters_ShouldAddStingNumbers()
        {
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();

            Calculator calc = new Calculator(mockLogger.Object, mockWebService.Object, mockWriter.Object);

            //Act
            double result = calc.Addition("//[*][%]\n1*2%3");

            //Assert
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void Addition_StringSeparatedByMultipleDelimitersIncludingSquareBracket_ShouldAddStingNumbers()
        {
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();

            Calculator calc = new Calculator(mockLogger.Object, mockWebService.Object, mockWriter.Object);

            //Act
            double result = calc.Addition("//[*][%][[]]\n1*2%3[]4");

            //Assert
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void GetDelimiters_VerifyDelimiters_ChecksTheDelimitersToUse()
        {
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();

            Calculator calc = new Calculator(mockLogger.Object, mockWebService.Object, mockWriter.Object);

            //Act
            MatchCollection result = calc.GetDelimiters("//[*][%][.]\n1*2%3.4");

            //Assert
            Assert.AreEqual("[*]", result[0].ToString());
            Assert.AreEqual("[%]", result[1].ToString());
            Assert.AreEqual("[.]", result[2].ToString());
        }

        [TestMethod]
        public void VerifyNoNegativeNumbers_WithNegativeNumber_ThrowsException()
        {
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();

            Calculator calc = new Calculator(mockLogger.Object, mockWebService.Object, mockWriter.Object);

            //Act
            Action act = new Action(() => calc.VerifyNoNegativeNumbers(-1));

            //Assert
            Assert.ThrowsException<Exception>(act);
        }

        [TestMethod]
        public void CheckIfGreaterThanThousand_LessThanThousand_ShouldAddStingNumbers()
        {
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();

            Calculator calc = new Calculator(mockLogger.Object, mockWebService.Object, mockWriter.Object);

            //Act
            double result = calc.CheckIfGreaterThanThousand(1000);

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Addition_VerifyTheDependency_CallsAdditionFunction()
        {
            //Arrange
            var mockLogger = new Mock<ILogger>();                                                            //faking logger
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();

            Calculator calc = new Calculator(mockLogger.Object, mockWebService.Object, mockWriter.Object);   //The calculator class depends in the ILogger forka specific function
            mockLogger
                .Setup(logger => logger.LogResults(It.IsAny<double>()))
                    .Verifiable();                                                                            //Tesing to see if the add fucntion executed and used the desired fucntion in ILogger

            //Act
            calc.Addition("//[*][%][[]]\n1*2%3[]4");

            //Assert
            mockLogger.Verify();
        }

        [TestMethod]
        public void Addition_CompareNewResultTheLoggedResult_CallsAdditionFunctionAndReturnsResult()
        {
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();

            Calculator calc = new Calculator(mockLogger.Object, mockWebService.Object, mockWriter.Object);
            mockLogger.Setup(logger => logger.LogResults(It.IsAny<double>()))
                .Callback(delegate (double result)
                {
                    mockLogger.Setup(l => l.LastResult).Returns(result);
                });

            //Act
            double newResult = calc.Addition("//[*][%][[]]\n1*2%3[]4");

            //Assert                                                       
            Assert.AreEqual(mockLogger.Object.LastResult, newResult);
        }

        [TestMethod]
        public void Addition_WaitForloggerToThrowException_NotifiesIWebServiceOfException()
        {
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();

            Calculator calc = new Calculator(mockLogger.Object, mockWebService.Object, mockWriter.Object);

            mockLogger.Setup(logger => logger.LogResults(It.IsAny<double>())).Throws(new Exception("An exception has been thrown"));
            mockWebService.Setup(service => service.ExceptionHandler(It.IsAny<Exception>())).Verifiable();

            //Act
            Assert.ThrowsException<Exception>(() => calc.Addition("//[*][%][[]]\n1*2%3[]4"));

            //Assert
            mockWebService.Verify();
        }

        [TestMethod]
        public void Addition_DisplayAdditonResultToConsole_CheckIfWrittingToConsole()
        {
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();

            var calc = new Calculator(mockLogger.Object, mockWebService.Object, mockWriter.Object);
            mockWriter.Setup(writer => writer.WriteResults(It.IsAny<double>()))
                .Callback(delegate (double result)
                {
                    Console.Write(result);

                }).Verifiable();

            //Act
            calc.Addition("//[*][%][[]]\n1*2%3[]4");

            //Assert                                                       
            mockWriter.Verify();
        }

        [TestMethod]
        public void GetCommandLineArgs_CheckingIfCommandLineArgumentsAreNotNull_VerifyCommandLineArgumentsArePresent()
        {
            // Arrange
            string[] expectedArgs = new[] { "Hello", "World", "Fake", "Args" };
            var mockCli = new Mock<ICommandLineInterface>();
            mockCli.Setup(m => m.GetCommandLineArgs()).Returns(expectedArgs);
            var cliObject = mockCli.Object;

            // Act
            var args = cliObject.GetCommandLineArgs();

            // Assert
            Assert.IsNotNull(args);
            args.Should().ContainInOrder(expectedArgs);  //.should() is from the fluentAssertions Librabry         
        }

        [TestMethod]
        public void DoAdition_IsStartCalculatorAddingTheValuesAndReturningTheResult_ShouldReturnResult()
        {
            //Arrange
            var mockUser = new Mock<IGetUserInput>();
            var mockLogger = new Mock<ILogger>();
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();
            
            CalculatorProgram calc = new CalculatorProgram(mockUser.Object, mockLogger.Object, mockWebService.Object, mockWriter.Object);

            //Act
            var result = calc.DoAdition("scalc '1,2,3'");

            //Assert
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void StartCalculator_SetNextUserInput_ShouldReturnResultTwice()
        {
            //arrange
            var mockUser = new Mock<IGetUserInput>();
            var mockLogger = new Mock<ILogger>();
            var mockWebService = new Mock<IWebService>();
            var mockWriter = new Mock<IWriter>();                        
            string newUserInput = "scalc '1,2,3'";
            int uCount = 0; 
           
            CalculatorProgram calc = new CalculatorProgram(mockUser.Object, mockLogger.Object, mockWebService.Object, mockWriter.Object);

            mockUser.Setup(i => i.GetNextUserInput()).Returns(newUserInput).Callback(() => {
                ++uCount;
                if (uCount < 2)
                { 
                    return; 
                }
                mockUser.Setup(i => i.GetNextUserInput()).Returns(string.Empty);
            }); 

            mockWriter.Setup(w => w.WriteMessage(It.IsAny<string>()))
             .Callback(delegate (string outout)
             { Console.WriteLine(outout); });    
            
            //Act
            calc.StartCalculator();           

            //Assert                                                       
            Assert.AreEqual(2, uCount);
        }
    }
}
 