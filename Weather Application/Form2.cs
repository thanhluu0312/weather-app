using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace WeatherApp
{
    public partial class Form2 : Form
    {
        private WeatherInfo.Root data;
        private string cityName;
        private const string APIKey = "4359ef1cd11b4c97b0da50cce76d01e7";

        public Form2(string City)
        {
            InitializeComponent();
            this.cityName = City;
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            ChangeBackgroundImage(cityName);
            
            await prepareForecastToDisplay(cityName);
            displayWeather();
            double temperature = 28.5; // Lấy giá trị nhiệt độ từ nguồn dữ liệu của bạn
            GiveHealthAdvice(temperature);
        }

        public async Task prepareForecastToDisplay(string City)
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    string url = string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&units=metric&appid={1}", Uri.EscapeDataString(City), APIKey);
                    var json = await web.DownloadStringTaskAsync(url);
                    data = JsonConvert.DeserializeObject<WeatherInfo.Root>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy dữ liệu thời tiết: {ex.Message}");
            }
        }

        public void displayWeather()
        {
            temperatureLabel1.Location = new Point(50, 266);
            temperatureLabel2.Location = new Point(240, 266);
            temperatureLabel31.Location = new Point(440, 266);



            label5.Location = new Point(609, 269);
            label6.Location = new Point(780, 269);





            temperatureLabel31.Font = new Font(temperatureLabel31.Font.FontFamily, 16, FontStyle.Bold);
            temperatureLabel31.BackColor = Color.Transparent;
            temperatureLabel31.ForeColor = Color.Red;

            if (data != null && data.List != null && data.List.Count > 0)
            {
                var forecasts = data.List
                    .GroupBy(x => DateTime.Parse(x.DtTxt).Date)
                    .Select(g => g.First())
                    .GroupBy(x => DateTime.Parse(x.DtTxt).Date)
                    .Select(g => g.First())
                    .Take(5) // Lấy 5 ngày đầu tiên

                    .ToList();

                DisplayForecast(forecasts[0], dateLabel1, temperatureLabel1, weatherIconBox1, 200, 150);
                if (forecasts.Count > 1) DisplayForecast(forecasts[1], dateLabel2, temperatureLabel2, weatherIconBox2, 200, 250);
                if (forecasts.Count > 2) DisplayForecast(forecasts[2], dateLabel3, temperatureLabel31, weatherIconBox3, 200, 350);
                if (forecasts.Count > 3) DisplayForecast(forecasts[3], dateLabel4, label5, weatherIconBox4, 200, 450);
                if (forecasts.Count > 4) DisplayForecast(forecasts[4], dateLabel5, label6, weatherIconBox5, 200, 550);

                // ✅ Lấy danh sách nhiệt độ để đưa vào lời khuyên
                List<double> temps = forecasts.Select(f => f.Main.Temp).ToList();
                string advice = GenerateAdvice(temps);

                // ✅ Gán lời khuyên vào labelAdvice
                labelAdvice.Text = advice;
                labelAdvice.Visible = true;
            }
            else
            {
                MessageBox.Show("Dữ liệu thời tiết không khả dụng.");
            }
        }

        private void DisplayForecast(WeatherInfo.Forecast forecast, Label dateLabel, Label tempLabel, PictureBox iconBox, int x, int y)
        {
            dateLabel.Text = DateTime.Parse(forecast.DtTxt).ToString("dd/MM/yyyy");
            tempLabel.Text = forecast.Main.Temp.ToString("F1") + " °C";
            tempLabel.ForeColor = Color.Red;
            tempLabel.Font = new Font(tempLabel.Font, FontStyle.Bold);

            string img = "http://openweathermap.org/img/w/" + forecast.Weather[0].Icon + ".png";
            iconBox.Size = new Size(150, 150);
            iconBox.SizeMode = PictureBoxSizeMode.StretchImage;
            iconBox.Load(img);

            dateLabel.Visible = tempLabel.Visible = iconBox.Visible = true;
        }

        // ✅ Hàm sinh lời khuyên
        private string GenerateAdvice(List<double> temps)
        {
            double avgTemp = temps.Average();

            if (avgTemp < 15)
                return "🌬️ Trời lạnh, bạn nên mặc áo ấm và giữ ấm cơ thể.";
            else if (avgTemp >= 15 && avgTemp < 25)
                return "🌤️ Thời tiết khá dễ chịu, bạn có thể ra ngoài thoải mái.";
            else
                return "🔥 Trời nóng, hãy uống nhiều nước và tránh ở ngoài quá lâu.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void temperatureL_Click(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dateLabel2_Click(object sender, EventArgs e)
        {

        }

        void GiveHealthAdvice(double temperature)
        {
            string advice = "";
            string clothingAdvice = "";
            string activityAdvice = "";
            string healthAdvice = "";

            // KHUYẾN CÁO CHỈ DỰA TRÊN NHIỆT ĐỘ
            if (temperature > 35)
            {
                advice = "⚠️ CẢNH BÁO NẮNG NÓNG: Nhiệt độ quá cao, nguy cơ sốc nhiệt và say nắng cao.\n";
                clothingAdvice = "- Mặc quần áo rộng rãi, nhẹ, thoáng khí và màu sáng.\n- Đội mũ rộng vành và kính râm.\n";
                activityAdvice = "- Hạn chế tối đa hoạt động ngoài trời từ 10h đến 16h.\n- Tránh các hoạt động thể chất nặng.\n";
                healthAdvice = "- Uống nhiều nước, ngay cả khi không khát.\n- Người già, trẻ em và người có bệnh nền nên ở trong nhà có điều hòa.\n";
            }
            else if (temperature > 30)
            {
                advice = "🔆 NẮNG NÓNG: Nhiệt độ cao có thể gây mất nước và kiệt sức.\n";
                clothingAdvice = "- Mặc quần áo nhẹ, thoáng khí và đội mũ rộng vành.\n- Sử dụng kem chống nắng.\n";
                activityAdvice = "- Giảm cường độ hoạt động ngoài trời, nghỉ ngơi thường xuyên trong bóng râm.\n";
                healthAdvice = "- Uống nhiều nước khoảng 2-3 lít/ngày.\n- Tránh đồ uống có cồn và caffeine.\n";
            }
            else if (temperature > 25)
            {
                advice = "☀️ THỜI TIẾT ẤM ÁP: Nhiệt độ dễ chịu nhưng vẫn cần chú ý khi ở ngoài trời lâu.\n";
                clothingAdvice = "- Mặc quần áo vừa phải, có thể cần áo khoác nhẹ vào buổi tối.\n";
                activityAdvice = "- Thích hợp cho các hoạt động ngoài trời và thể thao.\n";
                healthAdvice = "- Vẫn nên uống nước đầy đủ, đặc biệt khi vận động.\n- Sử dụng kem chống nắng nếu ra ngoài lâu.\n";
            }
            else if (temperature > 20)
            {
                advice = "🌤️ THỜI TIẾT DỄ CHỊU: Nhiệt độ lý tưởng cho hầu hết hoạt động.\n";
                clothingAdvice = "- Mặc áo dài tay hoặc áo khoác nhẹ.\n";
                activityAdvice = "- Rất thích hợp cho các hoạt động ngoài trời và du lịch.\n";
                healthAdvice = "- Thời tiết tốt cho sức khỏe, ít gây các vấn đề liên quan đến nhiệt độ.\n";
            }
            else if (temperature > 15)
            {
                advice = "🌥️ THỜI TIẾT MÁT MẺ: Nhiệt độ hơi mát, dễ chịu.\n";
                clothingAdvice = "- Mặc áo khoác nhẹ, có thể cần thêm áo ấm vào buổi tối.\n";
                activityAdvice = "- Thích hợp cho các hoạt động ngoài trời vừa phải.\n";
                healthAdvice = "- Thời tiết tốt cho sức khỏe, ít gây căng thẳng nhiệt.\n";
            }
            else if (temperature > 10)
            {
                advice = "🌥️ THỜI TIẾT HƠI LẠNH: Nhiệt độ thấp, cần giữ ấm.\n";
                clothingAdvice = "- Mặc áo khoác ấm, nên mang theo găng tay và mũ.\n";
                activityAdvice = "- Vẫn có thể hoạt động ngoài trời nhưng cần giữ ấm cơ thể.\n";
                healthAdvice = "- Uống đủ nước ấm, tránh đồ uống lạnh.\n- Người có bệnh hô hấp nên cẩn thận.\n";
            }
            else if (temperature > 5)
            {
                advice = "❄️ THỜI TIẾT LẠNH: Nhiệt độ thấp, nguy cơ hạ thân nhiệt nếu ở ngoài quá lâu.\n";
                clothingAdvice = "- Mặc nhiều lớp quần áo, áo khoác dày, găng tay, mũ và khăn quàng cổ.\n";
                activityAdvice = "- Hạn chế thời gian ở ngoài trời, đặc biệt vào buổi tối và sáng sớm.\n";
                healthAdvice = "- Cẩn thận với các bệnh hô hấp.\n- Người già và trẻ em nên hạn chế ra ngoài.\n";
            }
            else
            {
                advice = "❄️❄️ RẤT LẠNH: Nhiệt độ rất thấp, nguy cơ cao mắc các bệnh liên quan đến lạnh.\n";
                clothingAdvice = "- Mặc nhiều lớp quần áo, áo khoác chống lạnh dày, găng tay, mũ và khăn quàng cổ.\n";
                activityAdvice = "- Hạn chế tối đa thời gian ở ngoài trời, chuẩn bị các biện pháp sưởi ấm.\n";
                healthAdvice = "- Người già, trẻ em và người có bệnh nền cần đặc biệt chú ý giữ ấm và ở trong nhà.\n- Cẩn thận với nguy cơ hạ thân nhiệt và cước chân tay.\n";
            }

            // TỔNG HỢP KHUYẾN CÁO
            string finalAdvice = "📋 KHUYẾN CÁO SỨC KHỎE:\n" + advice;

            if (!string.IsNullOrEmpty(clothingAdvice))
                finalAdvice += "\n👕 TRANG PHỤC PHÙ HỢP:\n" + clothingAdvice;

            if (!string.IsNullOrEmpty(activityAdvice))
                finalAdvice += "\n🏃 HOẠT ĐỘNG:\n" + activityAdvice;

            if (!string.IsNullOrEmpty(healthAdvice))
                finalAdvice += "\n❤️ SỨC KHỎE:\n" + healthAdvice;

            // HIỂN THỊ KHUYẾN CÁO
            labelAdvice.Text = finalAdvice;
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
    }
}
