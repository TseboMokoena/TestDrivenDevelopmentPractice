using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculatorConsole.Builder.IBuilder;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Security;
using System.Xml.Serialization;
using CalculatorConsole.Database;

namespace CalculatorConsole.Builder.ConcreteBuilder
{
    public class SqlDatabaseBuilder : ISqlDatabaseBuilder
    {
        private string user;
        private SecureString password;
        private string connection;

        public ISqlDatabaseBuilder UseUser(string user)
        {
            this.user = user;
            return this;
        }

        public ISqlDatabaseBuilder WithPassword(SecureString password)
        {
            this.password = password;
            return this;
        }

        public ISqlDatabaseBuilder ForConnection(string connection)
        {
            this.connection = connection;
            return this;
        }

        public SqlDatabase Build()
        {
            return new SqlDatabase()
            {
                Connection = new SqlConnection(connection),
                Command = new SqlCommand()
            };
        }
    }
}
