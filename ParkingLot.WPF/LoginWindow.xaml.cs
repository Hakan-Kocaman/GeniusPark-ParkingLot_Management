using ParkingLot.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ParkingLot.WPF
{
    /// <summary>
    /// LoginWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class LoginWindow : Window
    {
        private MainWindow _mainWindow;
        private HttpClient httpClient;
        public LoginWindow(MainWindow mainWindow, HttpClient httpClient)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            this.httpClient = httpClient;
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            var username=Username.Text;
            var password=Password.Text;
            try
            {
                var response = await this.httpClient.GetFromJsonAsync<User>("api/user/" + username + "/" + password);
                if (response != null)
                {

                    _mainWindow.User = response;
                    _mainWindow.Preload_Data(response.Company_id);
                    MessageBox.Show("Login successful!");
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Login failed. Please check your credentials.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (ex.InnerException != null)
                    MessageBox.Show(ex.InnerException.Message);
            }
        }

        
    }
}
