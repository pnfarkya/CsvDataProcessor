using CsvDataProcessor.Infra;
using CsvDataProcessor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CsvDataProcessor.ViewModels.Support
{
    public class PersonFilterVm : ViewModel
    {
        private Tuple<List<string>> _filter;
        public PersonFilterVm(Action<Tuple<List<string>>> filterAction, Action<string> sortAction)
        {
            _filter = new Tuple<List<string>>(new List<string>());
            _filterAction = filterAction;
            _sortAction = sortAction;
            Countries = new ObservableCollection<CountryModel>();
        }

        private Action<Tuple<List<string>>> _filterAction { get; set; }
        private Action<string> _sortAction { get; set; }

        public ICommand CountryFilterCmd => new RelayCommand<object>(OnFilter);
        public ICommand ClearFilterCmd => new RelayCommand<object>(OnClearFilter);

        private void OnFilter(object obj)
        {
            _filter.Item1.Clear();
            var selectedCountries = Countries.Where(c => c.IsSelected);
            if (selectedCountries != null && selectedCountries.Any())
            {
                _filter.Item1.AddRange(selectedCountries.Select(c => c.Name));
            }
            OnPropertyChanged("CountryDisplayText");
            _filterAction.Invoke(_filter);
        }

        private void OnClearFilter(object obj)
        {
            foreach (var country in Countries)
            {
                country.IsSelected = false;
            }
            _filter.Item1.Clear();
            _filterAction.Invoke(_filter);
            OnPropertyChanged("CountryDisplayText");
        }

        /// <summary>
        /// This Methods reload all the reqiures filters based on the latest data
        /// </summary>
        /// <param name="rows"></param>
        public async void Reload(List<PersonModel> rows)
        {
            Reset();
            if (rows != null && rows.Any())
            {
                var countries = rows.Select(c => c.Country).Distinct();
                foreach (var item in countries.OrderBy(c => c))
                {
                    Countries.Add(new CountryModel(item));
                }
            }
        }
        private void Reset()
        {
            Countries.Clear();
        }

        private ObservableCollection<CountryModel> _countries;
        public ObservableCollection<CountryModel> Countries
        {
            get { return _countries; }
            set
            {
                _countries = value;
                OnPropertyChanged("Countries");
                OnPropertyChanged("CountryDisplayText");
            }
        }

        private string _countryDisplayText;
        public string CountryDisplayText
        {
            get
            {
                string text = "All";
                if (Countries.Any(c => c.IsSelected))
                {
                    text = string.Join(", ", Countries.Where(c => c.IsSelected).Select(c => c.Name));
                }
                return text;
            }
            set
            {
                OnPropertyChanged("CountryDisplayText");

            }
        }

        public List<string> SortOptions
        {
            get
            {
                return new List<string>()
                {
                    "Name",
                    "Country"
                };
            }
        }

        private string _selectedSort;
        public string SelectedSort
        {
            get { return _selectedSort; }
            set
            {
                _selectedSort = value;
                OnPropertyChanged("SelectedSort");
                _sortAction.Invoke(value);
            }
        }
    }
}
