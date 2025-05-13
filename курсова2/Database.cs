using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace курсова2
{
    internal static class Database
    {
        private static string connectionString = "Server=localhost;Database=kursova;User=root;Password=15011989;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}