using MyQRCode.Models;
using MySqlConnector;
using System;

namespace MyQRCode.Services
{
    public class PersonalInfoService
    {
        private string connectionString = "server=localhost;port=3306;database=scanqr;uid=root;password=";

        public PersonalInfo LayThongTinCaNhan(int id)
        {
            PersonalInfo personalInfo = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM PersonalInfo WHERE ID = @ID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        personalInfo = new PersonalInfo
                        {
                            ID = reader.GetInt32("ID"),
                            Name = reader.GetString("Name"),
                            Email = reader.GetString("Email"),
                            PhoneNumber = reader.GetString("PhoneNumber"),
                            UniqueIdentifier = reader.GetString("UniqueIdentifier")
                        };
                    }
                }
            }

            return personalInfo;
        }

        public void CapNhatMaNhanDang(int id, string uniqueIdentifier)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE PersonalInfo SET UniqueIdentifier = @UniqueIdentifier WHERE ID = @ID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@UniqueIdentifier", uniqueIdentifier);
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();
            }
        }

        public PersonalInfo LayThongTinCaNhanTheoMa(string uniqueIdentifier)
        {
            PersonalInfo personalInfo = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM PersonalInfo WHERE UniqueIdentifier = @UniqueIdentifier";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@UniqueIdentifier", uniqueIdentifier);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        personalInfo = new PersonalInfo
                        {
                            ID = reader.GetInt32("ID"),
                            Name = reader.GetString("Name"),
                            Email = reader.GetString("Email"),
                            PhoneNumber = reader.GetString("PhoneNumber"),
                            UniqueIdentifier = reader.GetString("UniqueIdentifier")
                        };
                    }
                }
            }

            return personalInfo;
        }
    }
}
