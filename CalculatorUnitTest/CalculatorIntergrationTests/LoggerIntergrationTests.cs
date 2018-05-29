using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using CalculatorConsole;
using CalculatorLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace CalculatorIntergrationTests
{
    [TestClass]
    public class LoggerIntergrationTests
    {
        private static UnityContainer Container = new UnityContainer();
        private static ILogger _logger;
        private static IWebService _exceptionService;
        private static IWriter _writer;
        private string filePath = "log.txt";

        public void ContainerConstuctor()
        {
            Container.RegisterType<IGetUserInput, GetUserInput>();
            Container.RegisterType<ILogger, Logger>();
            Container.RegisterType<IWebService, WebService>();
            Container.RegisterType<IWriter, Writer>();
            Container.RegisterType<Calculator>();
        }

        [TestInitialize]
        public void Setup()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            ContainerConstuctor();
        }

        [TestMethod]
        public void Addition_CheckToSeeIfANewFileIsCreated_CreatesFile()
        {
            //Arrange
            var calc = Container.Resolve<Calculator>();
            //Act
            calc.Addition("//[*][%][[]]\n1*2%3[]4");

            //Assert
            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod]
        public void Addition_CheckNumberOfLinesInFileDoesNotExeed50Lines_OpenFile()
        {
            //Arrange
            _logger = Container.Resolve<ILogger>();
            _exceptionService = Container.Resolve<IWebService>(); ;
            _writer = Container.Resolve<IWriter>(); ;
            Calculator calc = new Calculator(_logger, _exceptionService, _writer);

            //Act
            calc.Addition("//[*][%][[]]\n1*2%3[]4");

            //Assert
            Assert.AreNotEqual(File.Open(filePath, FileMode.Open).Length, 50);
        }

    }
}
