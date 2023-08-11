using CsvDataProcessor.Common;
using CsvDataProcessor.Model;
using System.Collections.Generic;

namespace CsvDataProcessor.DataProcessor
{
    public interface IPersonFileReader : IBaseReader<PersonModel>
    {
    }
    public sealed class PersonFileReader : BaseFileReader<PersonModel>, IPersonFileReader
    {
        protected override void PopulateMetaData()
        {
            var columns = EnumHelper.GetValues<PersonColumn>();

            foreach (var item in columns)
            {
                var att = EnumHelper.GetAttributes<ColumnAttribute>(item);

                Attributes.Add(att.Description, att);
                AllColumns.Add(item.ToString());
            }
        }

        protected override PersonModel ProcessRow(string[] values)
        {
            return new PersonModel()
            {
                Name = values[0],
                Country = values[1],
                Address = values[2],
                PostalZip = values[3],
                Email = values[4],
                Phone = values[5]
            };
        }
    }
}
