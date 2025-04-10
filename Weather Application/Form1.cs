using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;
using static WeatherApp.WeatherInfo;

namespace WeatherApp
{
    public partial class Form1 : Form
    {
        private Size originalSize;
        private string APIKey = "29e8e546e5ac4f413e3c9ee688325a59";

        public Form1()
        {
            InitializeComponent();
            originalSize = this.Size; // Save the original size of the form
            HideControls();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the current form
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            getWeather();
            if (string.IsNullOrEmpty(tbCity.Text))
            {
                MessageBox.Show("Vui lòng nhập tên thành phố!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string city = tbCity.Text.Trim();
            if (string.IsNullOrEmpty(city))
            {
                MessageBox.Show("Vui lòng nhập tên thành phố để tìm dịch vụ du lịch!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Clear old items in ListBox
            listBox1.Items.Clear();

            // Hiển thị thông tin dịch vụ du lịch theo thành phố
            if (city.Equals("hanoi", StringComparison.OrdinalIgnoreCase))
            {
                listBox1.Items.Add("�� DỊCH VỤ DU LỊCH HÀ NỘI 🌟");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("✈️ VÉ MÁY BAY:");
                listBox1.Items.Add("• Vietnam Airlines: Giảm 15% chặng Hà Nội - Đà Nẵng");
                listBox1.Items.Add("• Vietjet Air: Ưu đãi 499.000đ/chiều HN-TP.HCM");
                listBox1.Items.Add("• Đặt vé: https://vemaybay.vn/hanoi");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("🏨 KHÁCH SẠN ĐỀ XUẤT:");
                listBox1.Items.Add("• Sofitel Legend Metropole: 5★ - 3.200.000đ/đêm");
                listBox1.Items.Add("• La Siesta Premium: 4★ - 1.500.000đ/đêm");
                listBox1.Items.Add("• Hanoi Emerald Waters: 3★ - 800.000đ/đêm");
                listBox1.Items.Add("• Đặt phòng: https://datphong.vn/hanoi");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("🏛️ ĐỊA ĐIỂM DU LỊCH NỔI TIẾNG:");
                listBox1.Items.Add("• Phố cổ Hà Nội: Chợ đêm T6-CN từ 19:00");
                listBox1.Items.Add("• Văn Miếu - Quốc Tử Giám: 8:00-18:00, 30.000đ/vé");
                listBox1.Items.Add("• Hoàng thành Thăng Long: 8:00-17:00, 40.000đ/vé");
                listBox1.Items.Add("• Tour city: https://tourhanoi.vn");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("📱 Liên hệ đặt tour: 0912.xxx.xxx");
                listBox1.Items.Add("🌐 Facebook: fb.com/hanoidulich");
            }
            else if (city.Equals("saigon", StringComparison.OrdinalIgnoreCase))
            {
                listBox1.Items.Add("🌟 DỊCH VỤ DU LỊCH TP. HỒ CHÍ MINH 🌟");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("✈️ VÉ MÁY BAY:");
                listBox1.Items.Add("• Bamboo Airways: Giảm 20% HCM-Phú Quốc");
                listBox1.Items.Add("• Pacific Airlines: Deal 599.000đ HCM-Đà Nẵng");
                listBox1.Items.Add("• Đặt vé: https://vemaybay.vn/tphcm");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("🏨 KHÁCH SẠN ĐỀ XUẤT:");
                listBox1.Items.Add("• Park Hyatt Saigon: 5★ - 4.500.000đ/đêm");
                listBox1.Items.Add("• Liberty Central: 4★ - 1.800.000đ/đêm");
                listBox1.Items.Add("• Silverland Yen: 3★ - 950.000đ/đêm");
                listBox1.Items.Add("• Đặt phòng: https://datphong.vn/saigon");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("🏛️ ĐỊA ĐIỂM DU LỊCH NỔI TIẾNG:");
                listBox1.Items.Add("• Chợ Bến Thành: 7:00-18:00, mua sắm đặc sản");
                listBox1.Items.Add("• Nhà thờ Đức Bà: 8:00-17:00, tham quan miễn phí");
                listBox1.Items.Add("• Bảo tàng chiến tranh: 7:30-18:00, 40.000đ/vé");
                listBox1.Items.Add("• Tour city: https://toursaigon.vn");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("📱 Liên hệ đặt tour: 0919.xxx.xxx");
                listBox1.Items.Add("🌐 Facebook: fb.com/saigontravel");
            }
            else if (city.Equals("quy nhơn", StringComparison.OrdinalIgnoreCase))
            {
                listBox1.Items.Add("🌟 DỊCH VỤ DU LỊCH QUY NHƠN 🌟");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("✈️ VÉ MÁY BAY:");
                listBox1.Items.Add("• Vietnam Airlines: Bay thẳng từ HN/HCM từ 999.000đ");
                listBox1.Items.Add("• Vietjet Air: Flash sale thứ 4 hàng tuần");
                listBox1.Items.Add("• Đặt vé: https://vemaybay.vn/quynhon");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("🏨 KHÁCH SẠN ĐỀ XUẤT:");
                listBox1.Items.Add("• FLC Quy Nhon Beach Resort: 5★ - 2.500.000đ/đêm");
                listBox1.Items.Add("• Avani Quy Nhon: 4★ - 1.600.000đ/đêm");
                listBox1.Items.Add("• Seagull Hotel: 3★ - 750.000đ/đêm");
                listBox1.Items.Add("• Đặt phòng: https://datphong.vn/quynhon");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("🏖️ ĐỊA ĐIỂM DU LỊCH NỔI TIẾNG:");
                listBox1.Items.Add("• Eo Gió: Ngắm bình minh tuyệt đẹp 5:00-6:00");
                listBox1.Items.Add("• Kỳ Co: Tour đảo 350.000đ/người gồm tàu và ăn trưa");
                listBox1.Items.Add("• Tháp Đôi: 7:00-17:30, 30.000đ/vé");
                listBox1.Items.Add("• Tour đảo: https://tourquynhon.vn");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("📱 Liên hệ đặt tour: 0905.xxx.xxx");
                listBox1.Items.Add("🌐 Facebook: fb.com/quynhonparadise");
            }
            else if (city.Equals("danang", StringComparison.OrdinalIgnoreCase))
            {
                listBox1.Items.Add("🌟 DỊCH VỤ DU LỊCH ĐÀ NẴNG 🌟");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("✈️ VÉ MÁY BAY:");
                listBox1.Items.Add("• Vietnam Airlines: Bay thẳng từ HN/HCM từ 799.000đ");
                listBox1.Items.Add("• Vietjet Air: Combo vé máy bay + khách sạn 4★ từ 2.399.000đ");
                listBox1.Items.Add("• Đặt vé: https://vemaybay.vn/danang");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("🏨 KHÁCH SẠN ĐỀ XUẤT:");
                listBox1.Items.Add("• InterContinental Danang: 5★ - 5.800.000đ/đêm");
                listBox1.Items.Add("• Danang Golden Bay: 4★ - 1.900.000đ/đêm");
                listBox1.Items.Add("• Fivitel Hotel: 3★ - 850.000đ/đêm");
                listBox1.Items.Add("• Đặt phòng: https://datphong.vn/danang");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("🏞️ ĐỊA ĐIỂM DU LỊCH NỔI TIẾNG:");
                listBox1.Items.Add("• Bà Nà Hills: 7:00-22:00, 850.000đ/vé (gồm cáp treo)");
                listBox1.Items.Add("• Bán đảo Sơn Trà: Tham quan miễn phí, thuê xe máy 150.000đ/ngày");
                listBox1.Items.Add("• Phố cổ Hội An: Tour nửa ngày 400.000đ/người");
                listBox1.Items.Add("• Tour trọn gói: https://tourdanang.vn");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("📱 Liên hệ đặt tour: 0906.xxx.xxx");
                listBox1.Items.Add("🌐 Facebook: fb.com/danangfantastic");
            }
            else if (city.Equals("london", StringComparison.OrdinalIgnoreCase))
            {
                listBox1.Items.Add("🌟 LONDON TRAVEL SERVICES 🌟");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("✈️ FLIGHT TICKETS:");
                listBox1.Items.Add("• British Airways: 20% off London-Paris routes");
                listBox1.Items.Add("• Vietnam Airlines: Direct flights from HN/HCM from $600");
                listBox1.Items.Add("• Book tickets: https://flytouk.com/london");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("🏨 RECOMMENDED HOTELS:");
                listBox1.Items.Add("• The Savoy: 5★ - £450/night");
                listBox1.Items.Add("• Park Plaza Westminster: 4★ - £220/night");
                listBox1.Items.Add("• Premier Inn County Hall: 3★ - £120/night");
                listBox1.Items.Add("• Book rooms: https://bookrooms.uk/london");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("🏛️ FAMOUS ATTRACTIONS:");
                listBox1.Items.Add("• Tower of London: 9:00-17:30, £28.90/ticket");
                listBox1.Items.Add("• London Eye: 11:00-18:00, £32.50/ticket");
                listBox1.Items.Add("• British Museum: 10:00-17:00, Free entry");
                listBox1.Items.Add("• City tour: https://londontours.uk");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("📱 Tour booking: +44.xxx.xxxx.xxxx");
                listBox1.Items.Add("🌐 Facebook: fb.com/londontravelguide");
            }
            else
            {
                // Display message for other cities
                listBox1.Items.Add($"Chưa có thông tin dịch vụ du lịch tại {city}");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("🔍 DỊCH VỤ CHỜ KÍCH HOẠT");
                listBox1.Items.Add("Thành phố của bạn sẽ sớm được cập nhật!");
                listBox1.Items.Add("-----------------------------------");
                listBox1.Items.Add("📱 Hotline hỗ trợ: 1900.xxx.xxx");
                listBox1.Items.Add("🌐 Website: https://dulich.vn");
                listBox1.Items.Add("💬 Facebook: fb.com/vietnamtravel");
            }

            // Show ListBox only if there are items to display
            listBox1.Visible = listBox1.Items.Count > 0;
        }

        private void lb03_Click(object sender, EventArgs e)
        {
            // Xử lý khi label "Tốc độ gió" được click
            // Bạn có thể để trống nếu không cần xử lý gì
        }

        private void getWeather()
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    string cityName = Uri.EscapeDataString(tbCity.Text);
                    string url = string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&appid={1}&units=metric", cityName, APIKey);

                    var json = web.DownloadString(url);

                    WeatherInfo.Root info = JsonConvert.DeserializeObject<WeatherInfo.Root>(json);

                    if (info != null && info.List != null && info.List.Count > 0)
                    {
                        // Assuming we want to display the first forecast entry
                        var forecast = info.List[0];

                        string iconUrl = "https://openweathermap.org/img/w" + "/" + forecast.Weather[0].Icon + ".png";

                        // Thay đổi hình nền dựa trên tên thành phố
                        ChangeBackgroundImage(tbCity.Text);

                        // Translate and display weather information
                        lab_tinhtrang.Text = WeatherTranslator.TranslateMain(forecast.Weather[0].Main);
                        lab_tinhtrang.ForeColor = Color.FromArgb(255, 0, 0);
                        lab_chitiet.Text = WeatherTranslator.TranslateDescription(forecast.Weather[0].Description);

                        ShowControls();

                        // Display temperature in Celsius
                        double tempCelsius = forecast.Main.Temp;
                        lab_nhietdo.Text = $"{tempCelsius.ToString("0.0")} °C";
                        lab_nhietdo.ForeColor = Color.Red;


                        // Display humidity
                        // Display humidity
                        lab_doam.Text = $"{forecast.Main.Humidity} %";
                        lab_doam.ForeColor = Color.Red;  // Set text color to blue
                                                         // Set background color to white


                        // Display pressure
                        lab_apsuat.Text = $"{forecast.Main.Pressure} hPa";
                        lab_apsuat.ForeColor = Color.Red;



                        // Display wind gust
                        lab_giogiat.Text = $"{forecast.Wind.Gust?.ToString("0.00") ?? "N/A"} m/s";
                        lab_giogiat.ForeColor = Color.Red;



                        // Display wind speed
                        lab_tdgio.Text = $"{forecast.Wind.Speed:0.00} m/s";
                        lab_tdgio.ForeColor = Color.Red;



                        // Display rainfall
                        lab_luongmua.Text = $"{forecast.Rain?.Rain1h?.ToString("0.0") ?? "0.0"} mm";
                        lab_luongmua.ForeColor = Color.Red;


                        // Adjust form size
                        this.Size = originalSize;
                    }
                    else
                    {
                        MessageBox.Show("Weather data is not available or incomplete.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving weather data: " + ex.Message);
            }
        }

        private void HideControls()
        {
            lab_nhietdo.Visible = false;
            lab_luongmua.Visible = false;
            lab_tdgio.Visible = false;
            lab_doam.Visible = false;
            lab_apsuat.Visible = false;
            lab_giogiat.Visible = false;
            label1.Visible = false;
            lb02.Visible = false;
            lab_chitiet.Visible = false;
            lb03.Visible = false;
            lb04.Visible = false;
            lb05.Visible = false;
            lb06.Visible = false;
            lb07.Visible = false;

            lab_tinhtrang.Visible = false;
            lab_giogiat.Visible = false;
            lab_luongmua.Visible = false;
            lab_apsuat.Visible = false;
            lab_doam.Visible = false;
            lab_tdgio.Visible = false;
            lab_nhietdo.Visible = false;

            btn_chitiet01.Visible = false;


        }

        private void ShowControls()
        {
            lab_nhietdo.Visible = true;
            lab_luongmua.Visible = true;
            lab_tdgio.Visible = true;
            lab_doam.Visible = true;
            lab_apsuat.Visible = true;
            lab_giogiat.Visible = true;

            lb02.Visible = true;
            lab_chitiet.Visible = false;
            lb03.Visible = true;
            lb04.Visible = true;
            lb05.Visible = true;
            lb06.Visible = true;
            lb07.Visible = true;

            lab_tinhtrang.Visible = true;
            lab_giogiat.Visible = true;
            lab_luongmua.Visible = true;
            lab_apsuat.Visible = true;
            lab_doam.Visible = true;
            lab_tdgio.Visible = true;
            lab_nhietdo.Visible = true;

            btn_chitiet01.Visible = true;

            // Ẩn khuyến cáo
            label1.Visible = false;
        }

        private void LoadAndResizeImage(string url)
        {
            try
            {
                using (WebClient webClient = new WebClient()) // Tạo một instance mới của WebClient
                {
                    byte[] imageBytes = webClient.DownloadData(url); // Tải dữ liệu hình ảnh từ URL đã chỉ định

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message); // Hiển thị thông báo lỗi nếu có sự cố
            }
        }

        void GiveHealthAdvice(double temperature, int humidity, double windSpeed)
        {
            // Để trống nội dung khuyến cáo
            label1.Text = "";

            // Hoặc có thể ẩn luôn label
            label1.Visible = false;
        }
        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }
        public static class WeatherTranslator
        {
            private static readonly Dictionary<string, string> WeatherMainDescriptions = new Dictionary<string, string>
            {
                { "Thunderstorm", "Dông bão" },
                { "Drizzle", "Mưa phùn" },
                { "Rain", "Mưa" },
                { "Snow", "Tuyết" },
                { "Mist", "Sương mù" },
                { "Smoke", "Khói" },
                { "Haze", "Sương mù" },
                { "Dust", "Bụi" },
                { "Fog", "Sương mù" },
                { "Sand", "Cát" },
                { "Ash", "Tro" },
                { "Squall", "Gió mạnh" },
                { "Tornado", "Lốc xoáy" },
                { "Clear", "Trời quang" },
                { "Clouds", "Mây" }
            };

            private static readonly Dictionary<string, string> WeatherDetailDescriptions = new Dictionary<string, string>
            {
                { "light rain", "Mưa nhẹ" },
                { "moderate rain", "Mưa vừa" },
                { "heavy intensity rain", "Mưa lớn" },
                { "very heavy rain", "Mưa rất lớn" },
                { "extreme rain", "Mưa cực lớn" },
                { "freezing rain", "Mưa đá" },
                { "light intensity shower rain", "Mưa rào nhẹ" },
                { "shower rain", "Mưa rào" },
                { "heavy intensity shower rain", "Mưa rào lớn" },
                { "ragged shower rain", "Mưa rào không đều" },
                { "light snow", "Tuyết nhẹ" },
                { "snow", "Tuyết" },
                { "heavy snow", "Tuyết lớn" },
                { "sleet", "Mưa tuyết" },
                { "light shower sleet", "Mưa tuyết nhẹ" },
                { "shower sleet", "Mưa tuyết" },
                { "light rain and snow", "Mưa và tuyết nhẹ" },
                { "rain and snow", "Mưa và tuyết" },
                { "light shower snow", "Tuyết rơi nhẹ" },
                { "shower snow", "Tuyết rơi" },
                { "heavy shower snow", "Tuyết rơi lớn" },
                { "mist", "Sương mù" },
                { "smoke", "Khói" },
                { "haze", "Sương mù" },
                { "sand/dust whirls", "Bụi cát" },
                { "fog", "Sương mù" },
                { "sand", "Cát" },
                { "dust", "Bụi" },
                { "volcanic ash", "Tro núi lửa" },
                { "squalls", "Gió mạnh" },
                { "tornado", "Lốc xoáy" },
                { "clear sky", "Bầu trời quang đãng" },
                { "few clouds", "Ít mây" },
                { "scattered clouds", "Mây rải rác" },
                { "broken clouds", "Mây đứt đoạn" },
                { "overcast clouds", "Mây bao phủ" }
            };

            public static string TranslateMain(string main)
            {
                if (WeatherMainDescriptions.TryGetValue(main, out string description))
                {
                    return description;
                }
                return "Không xác định";
            }

            public static string TranslateDescription(string description)
            {
                if (WeatherDetailDescriptions.TryGetValue(description, out string translatedDescription))
                {
                    return translatedDescription;
                }
                return "Không xác định";
            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            HideControls();

            this.Size = new Size(781, 500); // Set initial small size
            lab_nhietdo.Location = new Point(370, 130);
            lab_tdgio.Location = new Point(200, 190);     // Vị trí cho tốc độ gió
            lab_doam.Location = new Point(200, 230);      // Vị trí cho độ ẩm

            lab_apsuat.Location = new Point(270, 270);    // Vị trí cho áp suất
            lab_giogiat.Location = new Point(200, 360);   // Vị trí cho gió giật
            lab_luongmua.Location = new Point(200, 310);  // Vị trí cho lượng mưa

            lab_tinhtrang.Location = new Point(590, 140);   // Vị trí tình trạng

            label1.Visible = true;



        }

        private void tbCity_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add logic to handle text changes in the city name textbox
        }

        private void btn_chitiet01_Click(object sender, EventArgs e)
        {
            string city = tbCity.Text;
            Form2 form2 = new Form2(city);
            form2.Show();

        }



        private void lb02_Click(object sender, EventArgs e)
        {

        }

        private void lab_tieude_Click(object sender, EventArgs e)
        {

        }
        private void lab_HealthAdvice1_Click(object sender, EventArgs e)
        {

        }
        private void AdjustUI()
        {
            // Đưa Label lên trên cùng
            label1.BringToFront();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ChangeBackgroundImage(string cityName)
        {
            try
            {
                string imagePath = "";
                cityName = cityName.ToLower();

                // Đường dẫn đến thư mục chứa hình ảnh thành phố
                string baseImagePath = Path.Combine(Application.StartupPath, "CityImages");

                switch (cityName)
                {
                    case "hanoi":
                        imagePath = Path.Combine(baseImagePath, "hanoi.jpg");
                        break;
                    case "saigon":
                        imagePath = Path.Combine(baseImagePath, "saigon.jpg");
                        break;
                    case "danang":
                        imagePath = Path.Combine(baseImagePath, "danang.jpg");
                        break;
                    case "london":
                        imagePath = Path.Combine(baseImagePath, "london.jpg");
                        break;
                    case "quy nhơn":
                        imagePath = Path.Combine(baseImagePath, "quy nhơn.jpg");
                        break;
                    default:
                        imagePath = Path.Combine(baseImagePath, "default.jpg");
                        break;
                }

                if (File.Exists(imagePath))
                {
                    this.BackgroundImage = Image.FromFile(imagePath);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else
                {
                    MessageBox.Show($"Không tìm thấy hình ảnh cho thành phố {cityName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi hình nền: {ex.Message}");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}