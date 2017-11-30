using System.Windows;
using ModelInterfaces;
using Models;
using ViewModels;
using Unity;

namespace StairwayPattern
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer container;
        /// <summary>
        /// The entry point will naturally and necessarily reference everything that
        /// is needed to construct whatever are your resolution roots
        /// (controllers, services, etc.) But this is the only place that has such knowledge.
        /// </summary>
        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            // Basic form of dependecy injection
            // see method injection and property injection for more shallow and in-place injections
            IDataProvider dataProvider = new DataProvider();
            MainViewModel viewModel = new MainViewModel(dataProvider);
            MainWindow = new MainWindow
            {
                DataContext = viewModel
            };
            MainWindow.Show();
            // More complicated form using Inversion of control
            // container
            //container = new UnityContainer();
            //container.RegisterType<IDataProvider, DataProvider>();
            //container.RegisterType<MainViewModel>();
            //container.RegisterType<MainWindow>();
            //MainWindow = container.Resolve<MainWindow>();
            //MainWindow.Show();
        }
        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            // NB!
            container.Dispose();
        }
    }
}
