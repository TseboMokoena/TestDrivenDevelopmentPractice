using System;
using System.IO;
using CalculatorLibrary;
using System.Linq;
using System.Text;

namespace CalculatorConsole
{
    public class Logger : ILogger
    {
        private const string FileName = "log.txt";
        public double LastResult {get; set;}

        public void LogResults(double result)
       {
           using (FileStream file = GetFile())
           {
               PerformLog(result,file);
           }
       }

        private FileStream GetFile()
        {
            if (!File.Exists(FileName) || File.ReadLines(FileName).Count() > 50)
            {
                return CreateNewFile(FileName);
            }
            else
            {
                return File.Open(FileName, FileMode.Append);
            }
        }

        private FileStream CreateNewFile(string fileName)
        {
            var filename = string.Format(FileName,System.DateTime.Now.ToString());
            return File.Open(fileName, FileMode.OpenOrCreate); 

        }
     
        private void PerformLog (double result, FileStream file)
        {
            string data = "\r\nLog Entry :\nTime: " 
                          + DateTime.Now.ToLongTimeString() 
                          + "\nDate: " 
                          +DateTime.Now.ToLongDateString() 
                          + "\nThe result is: " 
                          + result 
                          +"\n-------------------------------"; 
            byte[] info = new UTF8Encoding(true).GetBytes(data);
            file.Write(info, 0, info.Length);
            file.Close();
        }
    }
}