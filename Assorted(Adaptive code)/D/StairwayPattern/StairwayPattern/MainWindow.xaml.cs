using System.Windows;
using Unity.Attributes;
using ViewModels;

namespace StairwayPattern
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // need that for IoC using containers
        [Dependency]
        public MainViewModel ViewModel
        {
            set => DataContext = value;
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
