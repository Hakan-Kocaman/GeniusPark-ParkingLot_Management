using System;
using System.Collections.Generic;
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
    /// ApproveWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class ApproveWindow : Window
    {

        public string DetectedPlate { get; set; }
        public double Confidence { get; set; }
        public bool ApproveStatus { get; set; }
        public ApproveWindow()
        {
            InitializeComponent();
        }

        public void Reload()
        {
            PlateText.Text = $"Detected Plate: {DetectedPlate}";
            ConfidenceText.Text = $"Confidence: {Confidence:P2}";
        }

        private void Yes(object sender, RoutedEventArgs e)
        {
            ApproveStatus = true;
            this.Hide();
        }

        private void No(object sender, RoutedEventArgs e)
        {
            ApproveStatus = false;
            this.Hide();
        }
    }
}
