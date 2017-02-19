using System;
using System.IO;
using System.Windows;

namespace ArmaServerManager
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string data = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string path = Path.Combine(data, name);

            if (Directory.Exists(path))
            {
                MainWindow mw = new MainWindow();
                mw.Show();
            }
            else
            {
                FirstStart fs = new FirstStart();
                fs.Show();
            }
        }
    }
}
