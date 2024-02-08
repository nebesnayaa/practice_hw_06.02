using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.Json;
using System.Data.SqlClient;

namespace practice_06._02
{
    public partial class IntroWindow : Window
    {
        private readonly String _msconnectionString;
        private bool isConnectionOpen = false; //

        public IntroWindow()
        {
            InitializeComponent();
            var config = JsonSerializer.Deserialize<JsonElement>(
                System.IO.File.ReadAllText("appsettings.json"));
            var connectionStrings = config.GetProperty("ConnectionStrings");
            _msconnectionString = connectionStrings
                .GetProperty("LocalMS")
                .GetString()!;
        }

        private void ConnectMsButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection msConnection = new(_msconnectionString);
            if(!isConnectionOpen)
            {
                try
                {
                    msConnection.Open();
                    MsConnectionStatusLabel.Content = "Connected";
                    isConnectionOpen = true; //
                    ConnectMsButton.IsEnabled = false;
                    DisconnectMsButton.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MsConnectionStatusLabel.Content = ex.Message;
                }
            }
            else
            {
                MessageBox.Show("The connection is already opened");
            }
        }

        private void DisconnectMsButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection msConnection = new(_msconnectionString);
            if (isConnectionOpen)
            {
                try
                {
                    msConnection.Close();
                    MsConnectionStatusLabel.Content = "Disconnected";
                    isConnectionOpen = false;
                    ConnectMsButton.IsEnabled = true;
                    DisconnectMsButton.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MsConnectionStatusLabel.Content = ex.Message;
                }
            }
            else
            {
                MessageBox.Show("The connection is already closed");
            }
        }
    }
}
/*
 * ADO.NET - технологія доступу до даних, яка вводить єдиний інтерфейс 
 * для роботи з різними джерелами даних (з різними СУБД)
 * 
 * - Сама БД: ПКМ (Project) - Add new item - Service Based DataBase - OK
 * - Параметри підключення. Зазвичай, їх закладають в окремий файл
 *    з конфігурацією (appsettings.json)
 * - Драйвер підключення (конектори) - додаткові бібліотеки (NuGet),
 *    які містять інструмент для з'єднання з відповідною СУБД
 *    SQl Server -- System.Data.SqlClient;
 *    MySQL/MariaDB -- MySQL.Data.MySqlClient;
 */
