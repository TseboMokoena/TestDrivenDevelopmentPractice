using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculatorConsole.Database;

namespace CalculatorConsole.Repository
{
    public interface IRepository
    {
        void AddToDatabase(SqlDatabase database, string occuranceDate, string stackTrace, string message); 
    }
}
