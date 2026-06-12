using System;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp01
{
    public static class DbContext
    {
        
        private static string connectionString = @"Data Source=CB05\SQLEXPRESS;Initial Catalog=QLSinhVienC#;User ID=sa;Password=1234;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}