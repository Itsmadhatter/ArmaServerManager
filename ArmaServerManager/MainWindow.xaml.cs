using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Media;
using ArmaServerManager.Properties;
using ArmaServerManager.Helper;

namespace ArmaServerManager
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            MouseDown += MainWindow_MouseDown;
            updateUI();
            updateServerInfo();
            loadGui();
            createChecklistBox();
        }

        private void loadGui()
        {
            Dictionary<string, string> cfg = GetCfg.getEntrys();
            if (cfg.Count > 0)
            {
                string name = cfg["hostname"];
                name = name.Remove(name.IndexOf('"'), 1);
                name = name.Remove(name.LastIndexOf('"'));
                ServerName.Text = name;
            }

            ServerIP.Text = Settings.Default.IP;
            ServerPort.Text = Settings.Default.Port;

            smodsbox.Text = Settings.Default.ServerMods;

            paramBox.Text = Settings.Default.Parameter;
            extraparam.Text = Settings.Default.ExtraParameter;

            port.Text = Settings.Default.Port;
            malloc.Text = Settings.Default.Malloc;
            memory.Text = Settings.Default.Memory;

            autoinit.IsChecked = Settings.Default.AutoInit;
            nosound.IsChecked = Settings.Default.NoSound;
            nologs.IsChecked = Settings.Default.NoLogs;
            netlogs.IsChecked = Settings.Default.NetLog;
            missiontomemory.IsChecked = Settings.Default.MissionToMemory;
            hyperthreading.IsChecked = Settings.Default.HT;
            filepatching.IsChecked = Settings.Default.FilePatching;

            port.Text = Settings.Default.Port;
            memory.Text = Settings.Default.Memory;
            malloc.Text = Settings.Default.Malloc;
        }

        private void createChecklistBox()
        {
            List<string> mods = LoadMods.GetMods(Path.GetDirectoryName(Settings.Default.ServerExePath));
            modlist.ItemsSource = mods;
            
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void updateParameter()
        {
            List<string> param = ParameterBuilder.createParam();
            StringBuilder sb = new StringBuilder();
            foreach (var para in param)
            {
                sb.Append(para);
                sb.Append(" ");
            }
            paramBox.Text = sb.ToString();
            Settings.Default.Parameter = sb.ToString();
            Settings.Default.Save();
        }

        #region Timer
        private void updateUI()
        {
            Timer aTimer = new Timer();
            aTimer.Elapsed += new ElapsedEventHandler(UITimer);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;
        }

        private void UITimer(object source, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() => {
                if(ServerProcess.IsRunning())
                {
                    ServerStatus.Foreground = Brushes.Green;
                    ServerStatus.Text = "Online";
                }
                else
                {
                    ServerStatus.Foreground = Brushes.Red;
                    ServerStatus.Text = "Offline";
                }
                CpuUsage.Text = ServerProcess.getCpuUsage();
                RamUsage.Text = ServerProcess.getMemUsage();
                uptime.Text = ServerProcess.getUptime();
            });
        }

        private void updateServerInfo()
        {
            Timer aTimer = new Timer();
            aTimer.Elapsed += new ElapsedEventHandler(ServerInfoTimer);
            aTimer.Interval = 30000;
            aTimer.Enabled = true;
        }

        private void ServerInfoTimer(object source, ElapsedEventArgs e)
        {
            if (ServerProcess.IsRunning())
            {
                try
                {
                    ServerQuery query = new ServerQuery("127.0.0.1", 2303);
                    Dispatcher.Invoke(() =>
                    {
                        PlayerCount.Text = Convert.ToString(query.getPlayer());
                    });
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);
                }
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    PlayerCount.Text = "0";
                });
            }    
        }

        #endregion
        #region Buttons
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var item in modlist.Items)
            {
                modlist.SelectedItems.Add(item);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            modlist.SelectedItems.Clear();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            createChecklistBox();
        }


        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ServerProcess.startServer();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            ServerProcess.stopServer();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MonitorHelper.GetRpt(Settings.Default.ProfilePath)))
            {
                MessageBox.Show("No Rpt File present", "ASM", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var path = Path.Combine(Settings.Default.ProfilePath, MonitorHelper.GetRpt(Settings.Default.ProfilePath));
                Monitor monitor = new Monitor(path);
                monitor.Show();
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {         
            if (string.IsNullOrEmpty(MonitorHelper.GetRpt(Settings.Default.ProfilePath)))
            {
                MessageBox.Show("No Rpt File present", "ASM", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var path = Path.Combine(Settings.Default.ProfilePath, MonitorHelper.GetRpt(Settings.Default.ProfilePath));
                Process.Start(path);
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MonitorHelper.GetRpt(Settings.Default.ProfilePath)))
            {
                MessageBox.Show("No Rpt File present", "ASM", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var path = Path.Combine(Settings.Default.ProfilePath, MonitorHelper.GetRpt(Settings.Default.ProfilePath));
                File.Delete(path);
            } 
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Work in progress", "ASM", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
        #region CheckBoxes
        private void AutoInit_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.AutoInit = true;
            Settings.Default.Save();
            updateParameter();
        }

        private void AutoInit_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.AutoInit = false;
            Settings.Default.Save();
            updateParameter();
        }

        private void NoSound_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.NoSound = true;
            Settings.Default.Save();
            updateParameter();
        }

        private void NoSound_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.NoSound = false;
            Settings.Default.Save();
            updateParameter();
        }

        private void NoLogs_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.NoLogs = true;
            Settings.Default.Save();
            updateParameter();
        }

        private void NoLogs_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.NoLogs = false;
            Settings.Default.Save();
            updateParameter();
        }

        private void NetLog_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.NetLog = true;
            Settings.Default.Save();
            updateParameter();
        }

        private void NetLog_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.NetLog = false;
            Settings.Default.Save();
            updateParameter();
        }

        private void HT_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.HT = true;
            Settings.Default.Save();
            updateParameter();
        }

        private void HT_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.HT = false;
            Settings.Default.Save();
            updateParameter();
        }

        private void FilePatching_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.FilePatching = true;
            Settings.Default.Save();
            updateParameter();
        }

        private void FilePatching_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.FilePatching = false;
            Settings.Default.Save();
            updateParameter();
        }

        private void MissionToMemory_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.MissionToMemory = true;
            Settings.Default.Save();
            updateParameter();
        }

        private void MissionToMemory_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.MissionToMemory = false;
            Settings.Default.Save();
            updateParameter();
        }

        #endregion
        #region TextChanged

        private void port_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.Default.Port = port.Text;
            Settings.Default.Save();
            updateParameter();
        }

        private void memory_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.Default.Memory = memory.Text;
            Settings.Default.Save();
            updateParameter();
        }

        private void malloc_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.Default.Malloc = malloc.Text;
            Settings.Default.Save();
            updateParameter();
        }


        private void extraparam_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(extraparam.Text))
            {
                Settings.Default.ExtraParameter = extraparam.Text;
                Settings.Default.Save();
            }
            else
            {
                Settings.Default.ExtraParameter = "";
                Settings.Default.Save();
            }
        }

        private void smodsbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(smodsbox.Text))
            {
                Settings.Default.ServerMods = smodsbox.Text;
                Settings.Default.Save();
            }
            else
            {
                Settings.Default.ServerMods = "";
                Settings.Default.Save();
            }
            updateParameter();
        }
        #endregion

        private void modlist_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            if (modlist.SelectedItems != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in modlist.SelectedItems)
                {
                    sb.Append(item);
                    sb.Append(";");
                }
                Settings.Default.Mods = sb.ToString();
                updateParameter();
            }
            else
            {
                Settings.Default.Mods = "";
                updateParameter();
            }
        }
    }
}
