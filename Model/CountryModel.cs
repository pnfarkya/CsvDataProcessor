using CsvDataProcessor.Infra;

namespace CsvDataProcessor.Model
{
    public sealed class CountryModel : ViewModel
    {
        public CountryModel(string name)
        {
            Name = name;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private bool _selected;
        public bool IsSelected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged("IsSelected");
            }
        }
    }
}
