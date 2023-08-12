using CsvDataProcessor.DataProcessor;
using CsvDataProcessor.Infra;
using CsvDataProcessor.Model;
using CsvDataProcessor.ViewModels.Support;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CsvDataProcessor.ViewModels
{
    public class PersonDetailsVm : ViewModel
    {
        private const string _defaultFile = "PersonsDemo.csv";


        private readonly IPersonFileReader _reader;
        private List<PersonModel> _allRows;

        /// <summary>
        /// PersonDetailsVm Constructor
        /// </summary>
        /// <param name="reader"></param>
        public PersonDetailsVm(IPersonFileReader reader)
        {
            FilePath = _defaultFile;
            FilterVm = new PersonFilterVm(OnFilterChange, OnSortChange);
            Rows = new ObservableCollection<PersonModel>();
            DataStats = new ObservableCollection<string>();
            _reader = reader;
            Init();
        }

        #region Command and Executers 

        public ICommand UploadCmd => new RelayCommand<object>(OnUpload);

        private void OnUpload(object obj)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            var fileSelected = openFileDlg.ShowDialog();
            if (fileSelected != null && (bool)fileSelected && openFileDlg.FileName.Contains(".csv"))
            {
                IsBusy = true;
                FilePath = openFileDlg.FileName;
                Rows.Clear();
                Task.Factory.StartNew(ProcessFile);
            }
            else
            {
                UpdateDataStats($"DataLog: {DateTime.Now.ToString("dd-MM-yyyy:HH:MM:ss")} : Selected file is invalid, only .csv file is acceptable");
            }
        }

        #endregion

        #region BindableProps


        private PersonFilterVm _filterVm;
        public PersonFilterVm FilterVm
        {
            get { return _filterVm; }
            set
            {
                _filterVm = value;
                OnPropertyChanged("FilterVm");
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }


        private ObservableCollection<string> _dataStats;
        public ObservableCollection<string> DataStats
        {
            get { return _dataStats; }
            set { _dataStats = value; OnPropertyChanged("DataStats"); }
        }


        private ObservableCollection<PersonModel> _rows;
        public ObservableCollection<PersonModel> Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
                OnPropertyChanged("Rows");
            }
        }

        #endregion

        #region FilterAndSorting

        //using tuple so this can support multiple filters in future if requires
        private void OnFilterChange(Tuple<List<string>> filters)
        {
            var rows = _allRows;
            IsBusy = true;
            Task.Factory.StartNew(() =>
            {
                if (filters.Item1 != null && filters.Item1.Any())
                {
                    var selectedCountries = filters.Item1;
                    rows = _allRows.Where(c => selectedCountries.Contains(c.Country)).ToList();
                }
                DispatcherUtils.Instance.Invoke(() =>
                    {
                        Rows.Clear();
                        Rows.AddRange(rows);
                        OnSortChange(FilterVm.SelectedSort);
                        OnComplete();
                    });
            });
        }

        private void OnSortChange(string sortBy)
        {
            switch (sortBy?.ToLower())
            {
                case "name":
                    Rows = new ObservableCollection<PersonModel>(Rows.OrderBy(c => c.Name));
                    break;

                case "country":
                    Rows = new ObservableCollection<PersonModel>(Rows.OrderBy(c => c.Country));
                    break;
            }
        }

        #endregion

        #region HelperMethods

        private void Init()
        {
            Reset();
            IsBusy = true;
            Task.Factory.StartNew(ProcessFile);
        }

        private void ProcessFile()
        {
            List<PersonModel> rows = null;
            StringBuilder message = new StringBuilder(string.Empty);
            try
            {
                rows = _reader.Read(FilePath);
                _allRows = rows;
                message.Append($"DataLog: {DateTime.Now.ToString("dd-MM-yyyy:HH:MM:ss")}: Data file {FilePath} , has {rows?.Count} valid rows");
                if (_reader.RowsWithError.Any())
                {
                    message.Append($" and {_reader.RowsWithError.Count} invalid rows at row number(s): {string.Join(", ", _reader.RowsWithError)}");
                }
                if (rows != null && rows.Any())
                {
                    DispatcherUtils.Instance.Invoke(() =>
                    {
                        FilterVm.Reload(rows);
                        Rows.AddRange(rows);
                        OnComplete();
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while processing file: {ex.Message}");
                message.Append($"DataLog: {DateTime.Now.ToString("dd-MM-yyyy:HH:MM:ss")}: {ex.Message}");
                DispatcherUtils.Instance.BeginInvoke(() =>
                {
                    OnComplete();
                });
            }

            DispatcherUtils.Instance.BeginInvoke(() =>
            {
                UpdateDataStats(message.ToString());
            });
        }

        private void UpdateDataStats(string message)
        {
            DataStats.Add(message);
        }

        private void OnComplete()
        {
            IsBusy = false;
        }
        private void Reset()
        {
            Rows.Clear();
        }

        #endregion
    }
}
