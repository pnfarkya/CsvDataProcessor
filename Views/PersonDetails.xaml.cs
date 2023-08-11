using CsvDataProcessor.Common;
using CsvDataProcessor.DataProcessor;
using CsvDataProcessor.ViewModels;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace CsvDataProcessor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PersonDetails : Window
    {
        public PersonDetails()
        {
            InitializeComponent();
            if(AppComposition.Container == null)
            {
                AppComposition.Init();
            }
            DataContext = new PersonDetailsVm(AppComposition.Container.Resolve<IPersonFileReader>());
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
