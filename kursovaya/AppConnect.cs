using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovaya
{
    internal class AppConnect
    {
      // Строка подключения к вашей базе данных
            private static string connectionString = "Data Source=DESKTOP-3J2SH1U\\SQLEXPRESS;Initial Catalog=WorkloadBase;Integrated Security=True";

            public static SqlConnection GetConnection()
            {
                SqlConnection connection = new SqlConnection(connectionString);
                return connection;
            }
    }
}
