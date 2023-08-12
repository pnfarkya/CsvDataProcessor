using CsvDataProcessor.DataProcessor;
using CsvDataProcessor.Model;
using CsvDataProcessor.Test.TestData;
using CsvDataProcessor.ViewModels;
using System.Collections.ObjectModel;

namespace TestProject
{
    [TestClass]
    public class PersonDetailsVmTest
    {
        private Mock<IPersonFileReader> _readerMock;
        private PersonDetailsVm _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _readerMock = new Mock<IPersonFileReader>();
            _readerMock.Setup(c => c.Read("PersonsDemo.csv")).Returns(PersonData.Persons);
            _viewModel = new PersonDetailsVm(_readerMock.Object);
            _viewModel.Rows = new ObservableCollection<PersonModel>(PersonData.Persons);
            _viewModel.FilterVm.Reload(PersonData.Persons);
        }

        [TestMethod]
        public void LoadAll_DataCountTest()
        {
            Assert.AreEqual(_viewModel.Rows.Count, 6);
            Assert.AreEqual(_viewModel.FilterVm.Countries.Count, 4);
        }

        [TestMethod]
        public void FilterWithCountry_DataCountTest()
        {
            _viewModel.FilterVm.Countries.First().IsSelected = true;
            _viewModel.FilterVm.CountryFilterCmd.Execute(null);
            Thread.Sleep(1000);
            Assert.AreEqual(_viewModel.FilterVm.CountryDisplayText, "Aus");
        }

        [TestMethod]
        public void SortWithCountry_Test()
        {
            _viewModel.FilterVm.SelectedSort = "Country";
            Thread.Sleep(500);
            Assert.AreEqual(_viewModel.Rows.Count, 6);
            Assert.AreEqual(_viewModel.Rows.First().Country, "Aus");
        }


        [TestMethod]
        public void SortWithName_Test()
        {
            _viewModel.FilterVm.SelectedSort = "Name";
            Thread.Sleep(500);
            Assert.AreEqual(_viewModel.Rows.Count, 6);
            Assert.AreEqual(_viewModel.Rows.First().Name, "ABCD");
        }
    }
}