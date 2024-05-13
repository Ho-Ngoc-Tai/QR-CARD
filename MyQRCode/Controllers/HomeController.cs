using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;
using MySql.Data.MySqlClient;


namespace MyQRCode.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult About()
        {
            string connectionString = "server=localhost;port=3306;database=scanqr;uid=root;password=";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    ViewBag.Message = "Kết nối cơ sở dữ liệu thành công!";
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Lỗi kết nối: " + ex.Message;
            }

            return View();
        }

        [HttpPost]
        public ActionResult GenerateQRFromDatabase()
        {
            // Lấy thông tin cá nhân từ cơ sở dữ liệu
            string personalInfo = GetPersonalInfoFromDatabase();

            // Tạo mã QR từ dữ liệu cá nhân
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(personalInfo, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(10);

            // Chuyển đổi Bitmap thành mảng byte
            byte[] byteImage;
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                byteImage = stream.ToArray();
            }

            // Chuyển đổi mảng byte thành chuỗi base64 để hiển thị trên trang
            string base64Image = Convert.ToBase64String(byteImage);
            string qrCodeImageUrl = $"data:image/png;base64,{base64Image}";

            // Trả về đường dẫn của hình ảnh QR và dữ liệu cá nhân
            ViewBag.QRCodeImageUrl = qrCodeImageUrl;
            ViewBag.PersonalInfo = personalInfo;

            return View("About");
        }

        private string GetPersonalInfoFromDatabase()
        {
            string personalInfo = "";
            string connectionString = "server=localhost;port=3306;database=scanqr;uid=root;password=";

            try
            {
                // Sử dụng lại kết nối đã được thiết lập trong phương thức About
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Kiểm tra xem kết nối đã mở chưa
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();

                    string query = "SELECT * FROM PersonalInfo WHERE ID = 1"; // Thay đổi điều kiện tùy thuộc vào cơ sở dữ liệu của bạn
                    MySqlCommand command = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            personalInfo = "Full Name: " + reader["FullName"].ToString() + "\n";
                            personalInfo += "Email: " + reader["Email"].ToString() + "\n";
                            personalInfo += "Phone: " + reader["Phone"].ToString() + "\n";
                            personalInfo += "Website: " + reader["Website"].ToString() + "\n";
                        }
                    }
                    // Không cần đóng kết nối ở đây vì sẽ đóng ở phương thức About()
                }
            }
            catch (Exception ex)
            {
                personalInfo = "Không thể lấy thông tin cá nhân: " + ex.Message;
            }

            return personalInfo;
        }

    }
}