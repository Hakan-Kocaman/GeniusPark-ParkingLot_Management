using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingLot.API;
using ParkingLot.API.Controllers;
using ParkingLot.API.Services;
using ParkingLot.Core;
using ParkingLot.Data;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace ParkingLot.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private HttpClient httpClient;
        public User User { get; set; }

        private Company _company;
  
        private ObservableCollection<Bill> _bills;

        private ApproveWindow _approveWindow;



        public MainWindow()
        {
            InitializeComponent();

             httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };

            LoginMenu_Appear(); 
            _approveWindow=new ApproveWindow();



        }

        private void LoginMenu_Appear(){
        var loginMenu = new LoginWindow(this,httpClient);
            loginMenu.Show();
        }

        

        public async void Preload_Data(int company_id){

                try
                {
                    var preloadData = await httpClient.GetFromJsonAsync<PreloadResponse>("api/preload/"+company_id);
    
                    if (preloadData != null)
                    {
                        _company = preloadData.Company;

                    _bills.Clear(); 
                        foreach (var bill in preloadData.Bill)
                        {
                            _bills.Add(bill);
                        }

                    MessageBox.Show("Preload data loaded successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to load preload data.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    if (ex.InnerException != null)
                        MessageBox.Show(ex.InnerException.Message);
            }

        }



        private async void Load_Image(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
            if (openFileDialog.ShowDialog() != true)
            { return; }


                var filePath = openFileDialog.FileName;
                var file = CreateFormFile(filePath);

            try
            {
                using var content = new MultipartFormDataContent();
                using var stream = File.OpenRead(filePath);

                content.Add(new StreamContent(stream), "file", file.FileName);

                var response = await httpClient.PostAsync("api/plate/detect", content);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<PlateResponse>();

                if (result.Confidence >= 0.9) {  
                    PostOrUpdateBill(result.Plate);
                    
                    MessageBox.Show("Plate:" + result.Plate + " detected"); }
                else if (result.Confidence >= 0.6)
                {
                    _approveWindow.DetectedPlate = result.Plate;
                    _approveWindow.Confidence = result.Confidence;
                    _approveWindow.Reload();
                    _approveWindow.Show();

                    

                    if (_approveWindow.ApproveStatus)
                    {
                       PostOrUpdateBill(result.Plate);
                    }
                    else
                    {
                        return;
                    }


                }
                else { MessageBox.Show("Plate Could Not Detect"); }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    if (ex.InnerException != null)
                        MessageBox.Show(ex.InnerException.Message);
                }

        }

        private async void PostOrUpdateBill(string detectedPlate) {
            Bill bill = new Bill
            {
                LicensePlate = detectedPlate,
                Company_id = _company.Id,
                User_id= User.Id,
            };

            var response = await httpClient.PostAsJsonAsync("api/bill/postorupdate", bill);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<Bill>();
                if (result != null)
                {
                    var existingBill = _bills.FirstOrDefault(b => b.Id == result.Id);
                    if (existingBill != null)
                    {
                        int index = _bills.IndexOf(existingBill);
                        _bills[index] = result; 
                    }
                    else
                    {
                        _bills.Add(result); 
                    }
    
                    MessageBox.Show("Bill updated successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to update bill.");
            }

        }
        private IFormFile CreateFormFile(string filePath)
        {
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return new FormFile(stream, 0, stream.Length, "image", System.IO.Path.GetFileName(filePath));
        }


    }
}