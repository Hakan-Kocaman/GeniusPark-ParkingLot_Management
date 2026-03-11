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

        public HttpClient httpClient { get; }
        public User User { get; set; }

        private Company _company;

        private ObservableCollection<Bill> ActiveBills;

        private ObservableCollection<Bill> InactiveBills;

        private ApproveWindow _approveWindow;

        private string Uploaded_filePath;



        public MainWindow()
        {
            InitializeComponent();

            httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };

            
            _approveWindow = new ApproveWindow();
            ActiveBills = new ObservableCollection<Bill>();
            InactiveBills = new ObservableCollection<Bill>();

            UploadImage_Status.Text = "";
            CompanyTitle.Text = "";
            UserTitle.Text = "";

            this.Show();
            LoginMenu_Appear();
        }

        private void LoginMenu_Appear()
        {
            var loginMenu = new LoginWindow(this);
            loginMenu.Show();
        }



        public async void Preload_Data(int company_id)
        {

            try
            {
                var preloadData = await httpClient.GetFromJsonAsync<PreloadResponse>("api/preload/" + company_id);

                if (preloadData == null)
                {
                    MessageBox.Show("No preload data found for the specified company ID.");
                    return;
                }

                if (preloadData != null)
                {
                    _company = preloadData.Company;

                    ActiveBills.Clear();
                    InactiveBills.Clear();
                    foreach (var bill in preloadData.Bill)
                    {
                        if (bill.ExitDate == null)
                        {
                            ActiveBills.Insert(0, bill);
                        }
                        else
                        {
                            InactiveBills.Insert(0, bill);
                        }
                    }

                    UserTitle.Text = "Welcome, " + User.Name;
                    CompanyTitle.Text = _company.Name;
                    ImageSubmitButton.IsEnabled = true;
                    UploadImageButton.IsEnabled = true;
                    TextSubmitButton.IsEnabled = true;

                    ActiveBills_Dg.Columns.Add(new DataGridTextColumn { Header = "#", Binding = new Binding("Id") });
                    ActiveBills_Dg.Columns.Add(new DataGridTextColumn { Header = "License Plate", Binding = new Binding("LicensePlate") });
                    ActiveBills_Dg.Columns.Add(new DataGridTextColumn { Header = "Enter Date", Binding = new Binding("EnterDate") });
                    ActiveBills_Dg.ItemsSource = ActiveBills;

                    InActiveBills_Dg.Columns.Add(new DataGridTextColumn { Header = "#", Binding = new Binding("Id") });
                    InActiveBills_Dg.Columns.Add(new DataGridTextColumn { Header = "License Plate", Binding = new Binding("LicensePlate") });
                    InActiveBills_Dg.Columns.Add(new DataGridTextColumn { Header = "Price", Binding = new Binding("Price") });
                    InActiveBills_Dg.Columns.Add(new DataGridTextColumn { Header = "Exit Date", Binding = new Binding("ExitDate") });
                    InActiveBills_Dg.Columns.Add(new DataGridTextColumn { Header = "Enter Date", Binding = new Binding("EnterDate") });


                    InActiveBills_Dg.ItemsSource = InactiveBills;


                    LastChange.Text = ("Preload data loaded successfully!");
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

            Uploaded_filePath = openFileDialog.FileName;
            UploadImage_Status.Text = "File: " + System.IO.Path.GetFileName(openFileDialog.FileName) + " uploaded";

        }

        private async void PostOrUpdateBill(string detectedPlate)
        {
            Bill bill = new Bill
            {
                LicensePlate = detectedPlate,
                Company_id = _company.Id,
                User_id = User.Id,
            };

            var response = await httpClient.PostAsJsonAsync("api/bill/", bill);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<Bill>();
            if (result != null)
            {
                var existingBill = ActiveBills.FirstOrDefault(b => b.Id == result.Id);
                if (existingBill != null)
                {
                    InactiveBills.Insert(0, result);
                    ActiveBills.Remove(existingBill);
                    var timeSpent = (result.ExitDate - result.EnterDate)?.TotalMinutes;
                    var hour = (int)(timeSpent / 60);
                    var minute = (int)(timeSpent % 60);
                    LastChange.Text = ("Vehicle " + result.LicensePlate + " exited.");
                    MessageBox.Show("Vehicle  '" + result.LicensePlate + "'  exited.\nTime spent: " + hour + " hour, " + minute + " minutes \nPrice: " + result.Price + "");
                }
                else
                {
                    ActiveBills.Insert(0, result);
                    LastChange.Text = ("New vehicle " + result.LicensePlate + " entered.");
                }


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

        private async void Submit_Image(object sender, RoutedEventArgs e)
        {
            var filePath = Uploaded_filePath;
            var file = CreateFormFile(filePath);

            try
            {
                using var content = new MultipartFormDataContent();
                using var stream = File.OpenRead(filePath);

                content.Add(new StreamContent(stream), "file", file.FileName);

                var response = await httpClient.PostAsync("api/plate/detect", content);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<PlateResponse>();

                UploadImage_Status.Text = "";

                if (result.Confidence >= 0.9)
                {
                    PostOrUpdateBill(result.Plate);

                    MessageBox.Show("Plate:" + result.Plate + " detected");
                }
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

        private async void Submit_inputPlate(object sender, RoutedEventArgs e)
        {
            var InputPlate = inputPlate.Text;

            if (InputPlate == "")
                return;

            PostOrUpdateBill(InputPlate);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            httpClient.Dispose();
            Application.Current.Shutdown();
        }

    }
}