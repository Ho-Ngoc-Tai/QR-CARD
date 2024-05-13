using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace MyQRCode
{
    public class Utilities
    {
        static void Main()
        {
            string connectionString = "server=localhost;port=3306;database=your_database_name;uid=your_username;password=your_password";
        
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Kết nối thành công!");

                // Thực hiện truy vấn kiểm tra
                string query = "SELECT 1";
                MySqlCommand command = new MySqlCommand(query, connection);
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    Console.WriteLine("Kết nối cơ sở dữ liệu thành công!");
                }
                else
                {
                    Console.WriteLine("Kết nối cơ sở dữ liệu không thành công!");
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi kết nối: " + ex.Message);
            }
        }
        }
    }
}