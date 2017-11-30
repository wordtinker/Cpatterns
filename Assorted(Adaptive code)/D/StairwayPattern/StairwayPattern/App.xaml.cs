using System.Windows;
using ModelInterfaces;
using Models;
using ViewModels;

namespace StairwayPattern
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The entry point will naturally and necessarily reference everything that
        /// is needed to construct whatever are your resolution roots
        /// (controllers, services, etc.) But this is the only place that has such knowledge.
        /// </summary>
        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            IDataProvider dataProvider = new DataProvider();
            MainViewModel viewModel = new MainViewModel(dataProvider);
            MainWindow window = new MainWindow
            {
                DataContext = viewModel
            };
            window.Show();
        }
    }
}
