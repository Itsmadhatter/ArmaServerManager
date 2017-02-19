using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using WinForms = System.Windows.Forms;
using ArmaServerManager.Properties;

namespace ArmaServerManager
{
    /// <summary>
    /// Interaktionslogik für FirstStart.xaml
    /// </summary>
    public partial class FirstStart : Window
    {
        public FirstStart()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog()
            {
                Filter = "Arma Server File (*server.exe)|*server.exe"
            };
            Nullable<bool> result = diag.ShowDialog();
            if (result == true)
            {
                ServerExe.Text = diag.FileName;
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog()
            {
                Filter = "Arma Config Cfg (*.cfg)|*.cfg"
            };
            Nullable<bool> result = diag.ShowDialog();
            if (result == true)
            {
                ServerConfig.Text = diag.FileName;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog diag = new WinForms.FolderBrowserDialog();
            WinForms.DialogResult result = diag.ShowDialog();
            if (result == WinForms.DialogResult.OK)
            {
                ServerProfile.Text = diag.SelectedPath;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog()
            {
                Filter = "Arma Basic Cfg (*.cfg)|*.cfg"
            };
            Nullable<bool> result = diag.ShowDialog();
            if (result == true)
            {
                ServerBasic.Text = diag.FileName;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (File.Exists(ServerConfig.Text) && File.Exists(ServerBasic.Text) && File.Exists(ServerExe.Text) && Directory.Exists(ServerProfile.Text))
            {
                string data = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                string path = Path.Combine(data, name);

                Settings.Default.ServerCfgPath = ServerConfig.Text;
                Settings.Default.ServerExePath = ServerExe.Text;
                Settings.Default.ProfilePath = ServerProfile.Text;
                Settings.Default.BasicCfg = ServerBasic.Text;
                Settings.Default.Save();

                DirectoryInfo di = Directory.CreateDirectory(path);
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please fill out all Fields", "All Paths required", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }
    }
}
