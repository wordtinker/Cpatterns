using ModelInterfaces;
using Prism.Mvvm;

namespace ViewModels
{
    /// <summary>
    /// Class does not depent on assembly with implementation,
    /// only on interface.
    /// </summary>
    public class MainViewModel : BindableBase
    {
        private IDataProvider dataProvider;
        private int number;

        public int Number
        {
            get => number;
            set => SetProperty(ref number, value);
        }

        public MainViewModel(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            Number = dataProvider.GetSingleNumger();
        }
    }
}
