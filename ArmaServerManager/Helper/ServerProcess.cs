using System;
using System.Diagnostics;
using System.IO;
using System.Windows;


namespace ArmaServerManager.Helper
{
    class ServerProcess
    {
        static Process a3s;
        public static void startServer()
        {
            
            if (IsRunning())
            {
               MessageBox.Show("Please close the server first", "Server already started", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                a3s = new Process();
                a3s.StartInfo.WorkingDirectory = Path.GetDirectoryName(Properties.Settings.Default.ServerExePath);
                a3s.StartInfo.FileName = Path.GetFileName(Properties.Settings.Default.ServerExePath);
                a3s.StartInfo.Arguments = Properties.Settings.Default.Parameter + " " + Properties.Settings.Default.ExtraParameter;
                a3s.Start();
            }
        }

        public static void stopServer()
        {
            foreach (var process in Process.GetProcessesByName("Arma3Server"))
            {
                process.Kill();
            }
        }

        public static string getUptime()
        {

                if (a3s != null)
                {
                    return (DateTime.UtcNow - a3s.StartTime.ToUniversalTime()).ToString(@"hh\:mm");
                }
                else
                {
                    return "00:00";
                }
        }

        public static bool IsRunning()
        {
            Process[] pname = Process.GetProcessesByName("Arma3Server");
            if (pname.Length == 0)
                return false;
            else
                return true;
        }

        private static PerformanceCounter cpu = new PerformanceCounter("Process", "% Processor Time", "Arma3Server");
        private static PerformanceCounter mem = new PerformanceCounter("Process", "Private Bytes", "Arma3Server");

        public static string getCpuUsage()
        {
            if (IsRunning())
            {
                return Math.Round(cpu.NextValue() / Environment.ProcessorCount) + "%";
            }
            else
            {
                return "0%";
            }
            
        }

        public static string getMemUsage()
        {
            if (IsRunning())
            {
                return Math.Round((mem.NextValue() / 1024) / 1024) + " MB";
            }
            else
            {
                return "0 MB";
            }
        }
    }
}
