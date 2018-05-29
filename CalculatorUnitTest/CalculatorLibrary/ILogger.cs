using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary
{
    public interface ILogger
    {
        double LastResult { get; set; }

        void LogResults(double result); 


        
    }
}
