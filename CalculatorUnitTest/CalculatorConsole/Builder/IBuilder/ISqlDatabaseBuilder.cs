using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using CalculatorConsole.Database;

namespace CalculatorConsole.Builder.IBuilder
{
    public interface ISqlDatabaseBuilder
    {
        ISqlDatabaseBuilder ForConnection(string connection);
        SqlDatabase Build();
    }
}