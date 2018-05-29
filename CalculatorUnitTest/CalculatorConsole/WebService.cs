using System;
using System.Configuration;
using System.Data.Common;
using System.Security;
using CalculatorConsole.Builder.ConcreteBuilder;using CalculatorConsole.Builder.IBuilder;
using CalculatorConsole.Repository;
using CalculatorLibrary;
using Unity;

namespace CalculatorConsole
{
    public class WebService : IWebService
    {

        private static UnityContainer Container = new UnityContainer();

        public static void ContainerConstuctor()
        {
            Container.RegisterType<IRepository, Repository.Repository>();
            Container.RegisterType<ISqlDatabaseBuilder, SqlDatabaseBuilder>();
            
        }

        public void ExceptionHandler(Exception ex)
        {
            ContainerConstuctor();
            var repository = Container.Resolve<IRepository>();
            var builder = Container.Resolve<ISqlDatabaseBuilder>(); 
            string occuranceDate = DateTime.Now.ToString();
            string stackTrace = ex.StackTrace;
            string message = ex.Message;
            
            var database = builder.ForConnection(ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString).Build();
            
            //Passing the database instance to a repo that will make changes to the database
            repository.AddToDatabase(database, occuranceDate, stackTrace, message);
        }
    }
}