using CalculatorConsole;
using CalculatorLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace CalculatorIntergrationTests
{
    [TestClass]
    public class CalculatprogramIntergrationTest
    {
        private static readonly UnityContainer Container = new UnityContainer();

        public void ContainerConstuctor()
        {
            Container.RegisterType<IGetUserInput, GetUserInput>();
            Container.RegisterType<ILogger, Logger>();
            Container.RegisterType<IWebService, WebService>();
            Container.RegisterType<IWriter, Writer>();
            Container.RegisterType<CalculatorProgram>();
        }

        [TestMethod]
        public void
            DoAddition_ChechkToSeeIfTheDoAdditionFunctionMakesUseOfTheAdditionFunctionInCalculatorCorretly_ReturnsResilt()
        {
            //Arrange
            ContainerConstuctor();

            var calc = Container.Resolve<CalculatorProgram>();
            ;

            //Act
            var result = calc.DoAdition("scalc '1,2,3'");

            //Assert
            Assert.AreEqual(6, result);
        }
    }
}