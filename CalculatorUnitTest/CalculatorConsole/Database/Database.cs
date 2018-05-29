using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorConsole.Database
{
    public abstract class Database
    {
        public virtual SqlConnection Connection { get; set; }
        public virtual SqlCommand Command { get; set; }
    }
}
