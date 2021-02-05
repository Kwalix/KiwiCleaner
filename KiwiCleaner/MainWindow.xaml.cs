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
using System.Threading;
using System.Drawing;

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
            FetchPreferences();
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
            Clean();
        }

        private void AboutBtn_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void FetchLastScan()
        {
            if (File.Exists(@".\lastscan.txt"))
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
            DirectoryInfo dir = new DirectoryInfo(tmpPath);

            LogBox.Text = "";
            LogBox.Text += $"Getting : {tmpPath}" + Environment.NewLine;

            foreach(DirectoryInfo d in dir.GetDirectories())
            {
                LogBox.Text += $"Getting : {d}" + Environment.NewLine;
            }
            foreach (string file in tmpFiles)
            {
                LogBox.Text += $"Getting : {file}" + Environment.NewLine;
            }

            
            long size = dir.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
            size /= 1024;
            size /= 1024;
            SpaceToCleanLabel.Content = $"Espace à nettoyer : {size} Mo";

            CleanBtn.IsEnabled = true;
        }

        private void Clean()
        {
            CleanBtn.IsEnabled = false;
            string pathToDelFiles = Path.GetTempPath();
            string[] toDeleteFiles = Directory.GetFiles(pathToDelFiles);
            DirectoryInfo dir = new DirectoryInfo(pathToDelFiles);


            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                LogBox.Text += $"Deleting Directory : {d}" + Environment.NewLine;
                try
                {
                    d.Delete(true);
                } catch(Exception)
                {
                    LogBox.Text += $"Erreur dossier : {d} peux pas etre supprimé" + Environment.NewLine;
                }
            }
            foreach (string f in toDeleteFiles)
            {
                
                LogBox.Text += $"Deleting : {f}" + Environment.NewLine;
                try
                {    
                    File.Delete(f);
                }
                catch (Exception)
                {
                    LogBox.Text += $"Erreur fichier : {f} peux pas etre supprimé" + Environment.NewLine;
                }
            }
            SpaceToCleanLabel.Content = "Espace à nettoyer : 0 Mo";
            LogBox.Text += "Nettoyage réussi !";
        }

       private void FetchPreferences()
        {
            if (File.Exists(@".\preferences.txt"))
            {
                string[] preferences = File.ReadAllLines(@".\preferences.txt");
                foreach(string pref in preferences)
                {
                    if(pref.Contains("true") || pref.Contains("True"))
                    {
                        if(pref.Contains("TmpFileBox"))
                        {
                            TmpFileBox.IsChecked = true;
                        } else if(pref.Contains("MSUBox"))
                        {
                            MSUBox.IsChecked = true;
                        } else if (pref.Contains("MSSCacheBox"))
                        {
                            MSSCacheBox.IsChecked = true;
                        } else if (pref.Contains("DNSCacheBox"))
                        {
                            DNSCacheBox.IsChecked = true;
                        } else if (pref.Contains("PFBox"))
                        {
                            PFBox.IsChecked = true;
                        }
                    } else if (pref.Contains("false") || pref.Contains("False"))
                    {
                        if (pref.Contains("TmpFileBox"))
                        {
                            TmpFileBox.IsChecked = false;
                        }
                        else if (pref.Contains("MSUBox"))
                        {
                            MSUBox.IsChecked = false;
                        }
                        else if (pref.Contains("MSSCacheBox"))
                        {
                            MSSCacheBox.IsChecked = false;
                        }
                        else if (pref.Contains("DNSCacheBox"))
                        {
                            DNSCacheBox.IsChecked = false;
                        }
                        else if (pref.Contains("PFBox"))
                        {
                            PFBox.IsChecked = false;
                        }
                    }
                }
            }

        }

        private async void SavePrefBtn_Click(object sender, RoutedEventArgs e)
        {
            string getBoxState = $"TmpFileBox={TmpFileBox.IsChecked.ToString()}" + Environment.NewLine
                + $"MSUBox={MSUBox.IsChecked.ToString()}" + Environment.NewLine
                + $"MSSCacheBox={MSSCacheBox.IsChecked.ToString()}" + Environment.NewLine
                + $"DNSCacheBox={DNSCacheBox.IsChecked.ToString()}" + Environment.NewLine
                + $"PFBox={PFBox.IsChecked.ToString()}";
            File.WriteAllText(@".\preferences.txt", getBoxState , Encoding.UTF8);
            SaveSuccessLb.Opacity = 100;
            await Task.Delay(2500);
            SaveSuccessLb.Opacity = 0;
        }
    }
}
