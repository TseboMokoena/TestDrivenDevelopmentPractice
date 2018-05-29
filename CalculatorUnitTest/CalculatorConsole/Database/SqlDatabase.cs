using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorConsole.Database
{
    public class SqlDatabase 
    {
        private SqlConnection _connection = null;
        private SqlCommand _command = null;

        public SqlConnection Connection
        {
            get => _connection;
            set => _connection = value; 
        }

        public SqlCommand Command
        {
            get => _command;
            set => _command = value;
        }
    }
}
