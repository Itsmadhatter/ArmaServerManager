using System;
using System.ComponentModel;
using System.IO;
using System.Timers;
using System.Windows;
using ArmaServerManager.Helper;

namespace ArmaServerManager
{

    public partial class Monitor : Window
    {

        private string _watchFile;
        private long _lastFileSize;
        public bool AutoScrollToBottom { get; set; }
        public bool AutoUpdate { get; set; }
        public Timer MonitorTimer { get; set; }

        public Monitor(String file)
        {
            InitializeComponent();

            _watchFile = file;
            _lastFileSize = 0;

            AutoUpdate = true;
            AutoScrollToBottom = true;
        }

        private void SetTimer()
        {
            MonitorTimer = new Timer { Interval = 3000, Enabled = true };
            MonitorTimer.Elapsed += new ElapsedEventHandler(TimerEvent);
            MonitorTimer.Start();
        }

        private void TimerEvent(object sender, ElapsedEventArgs e)
        {
            if (AutoUpdate)
            {
                ReadFile();
            }
        }

        private void ReadFile()
        {
            MonitorTimer.Stop();

            if (!File.Exists(_watchFile)) return;

            Dispatcher.Invoke(() =>
            {
                var fileText = MonitorHelper.ReadFile(_watchFile);
                if (fileText.Length > _lastFileSize)
                {
                    _lastFileSize = fileText.Length;

                    MonitorRtf.Text = fileText;
                    Title = "Monitor - Updated " + DateTime.Now.ToLongTimeString();

                    if (AutoScrollToBottom)
                    {
                        ScrollDown();
                    }
                }
            });
            MonitorTimer.Start();
        }

        private void ScrollDown()
        {
            var selectionStart = MonitorRtf.TextLength - 1;
            MonitorRtf.SelectionStart = selectionStart > -1 ? selectionStart : 0;

            MonitorRtf.ScrollToCaret();
            MonitorRtf.Refresh();
        }

        private void AutoScrollChk_Checked(object sender, RoutedEventArgs e)
        {
            AutoScrollToBottom = true;
        }

        private void AutoScrollChk_Unchecked(object sender, RoutedEventArgs e)
        {
            AutoScrollToBottom = false;
        }

        private void AutoUpdateChk_Checked(object sender, RoutedEventArgs e)
        {
            AutoUpdate = true;
        }

        private void AutoUpdateChk_Unchecked(object sender, RoutedEventArgs e)
        {
            AutoUpdate = false;
        }

        private void WordWrapChk_Checked(object sender, RoutedEventArgs e)
        {
            MonitorRtf.WordWrap = true;
        }

        private void WordWrapChk_Unchecked(object sender, RoutedEventArgs e)
        {
            MonitorRtf.WordWrap = false;
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            ReadFile();
            if (AutoScrollToBottom)
            {
                ScrollDown();
            }
        }

        private void Monitor_FormClosing(object sender, CancelEventArgs e)
        {
            MonitorTimer.Stop();
            MonitorTimer.Enabled = false;
            MonitorTimer.Dispose();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetTimer();
            ReadFile();
        }
    }
}
