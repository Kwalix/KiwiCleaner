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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Path = System.IO.Path;

namespace KiwiCleaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FetchLastScan();
        }

        private void ScanBtn_Click(object sender, RoutedEventArgs e)
        {
            // Generate new date scan and save it to lastscan file
            string lastScan = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            File.WriteAllText(@".\lastscan.txt", lastScan);
            string[] splitedDate = lastScan.Split(" ");
            LastScanLabel.Content = $"Le {splitedDate[0]} à {splitedDate[1]}";

            StartScan();
        }

        private void CleanBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AboutBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FetchLastScan()
        {
            if(File.Exists(@".\lastscan.txt"))
            {
               string lastScanDate = File.ReadAllText(@".\lastscan.txt");
                string[] splitedDate = lastScanDate.Split();
                LastScanLabel.Content = $"Le {splitedDate[0]} à {splitedDate[1]}";
            }
        }

        private void StartScan()
        {
            string tmpPath = Path.GetTempPath();
            string[] tmpFiles = Directory.GetFiles(tmpPath);
            TaskProgressBar.Maximum = tmpFiles.Length;

            LogBox.Text = "";
            LogBox.Text += $"Getting : {tmpPath}" + Environment.NewLine;

            foreach (string file in tmpFiles)
            {
                TaskProgressBar.Value++;
                LogBox.Text += $"Get : {file}" + Environment.NewLine;
            }

            DirectoryInfo dir = new DirectoryInfo(tmpPath);
            long size = dir.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
            SpaceToCleanLabel.Content = $"Espace à nettoyer : {size} Mb";

            CleanBtn.IsEnabled = true;
        }
    }
}
