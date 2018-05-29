using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculatorConsole.Database;

namespace CalculatorConsole.Repository
{
    class Repository : IRepository
    {
        public void AddToDatabase(SqlDatabase database, string occuranceDate, string stackTrace, string message)
        {
            SqlCommand command = database.Command;
            SqlConnection connection = database.Connection;
            
            command.Connection = connection; 
            command.Connection.Open();
            command.CommandText =
                "INSERT INTO Exceptions (Occurance_Date, Stack_Trace, Message) VALUES (@Occurance_Date, @Stack_Trace , @Message)"; 
            command.Parameters.AddWithValue("@Occurance_Date", occuranceDate);
            command.Parameters.AddWithValue("@Stack_Trace", stackTrace);
            command.Parameters.AddWithValue("@Message", message);
            command.ExecuteNonQuery();
            command.Connection.Close();

        }
    }
}
