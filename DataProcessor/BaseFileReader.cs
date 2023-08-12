
using CsvDataProcessor.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace CsvDataProcessor.DataProcessor
{
    public interface IBaseReader<T> where T : class
    {
        List<int> RowsWithError { get; }
        List<T> Read(string path);
    }

    public abstract class BaseFileReader<T> where T : class, new()
    {
        public BaseFileReader()
        {
            RowsWithError = new List<int>();
        }

        #region Abstract Members

        protected abstract void PopulateMetaData();
        protected abstract T ProcessRow(string[] values);

        #endregion

        #region Protect Members

        protected readonly Dictionary<string, ColumnAttribute> Attributes
                = new Dictionary<string, ColumnAttribute>();

        // Considering all columns will in string
        protected readonly List<string> AllColumns = new List<string>();

        #endregion

        #region IBaseReader Members

        public List<int> RowsWithError
        {
            get;
            protected set;
        }

        /// <summary>
        /// Read The CSV file with FilePath and validate the columns and data
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<T> Read(string path)
        {
            RowsWithError.Clear();
            List<T> records = new List<T>();

            List<string> lines = new List<string>();

            try
            {
                AllColumns.Clear();
                using (var reader = new StreamReader(File.OpenRead(@path)))
                {
                    var columnStr = reader.ReadLine();

                    if (!string.IsNullOrEmpty(columnStr))
                    {
                        var headers = columnStr.Split(',');
                        ValidateColumns(headers);

                        int dataRowCount = 2;
                        while (!reader.EndOfStream)
                        {
                            string row = reader.ReadLine();
                            var values = row.Split(',');
                            if (values.Length != AllColumns.Count)
                            {
                                RowsWithError.Add(dataRowCount);
                            }
                            else
                            {
                                var model = ProcessRow(values);
                                records.Add(model);
                            }
                            dataRowCount++;
                        }
                    }

                    //Additonally  in depth Data Validation can be perform on this steps with we have
                    //multiple types of data , considering we just only have string
                    //hense skipping data validation step.

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while reading file {path} :{ex.Message}");
                throw ex;
            }

            return records;
        }

        #endregion

        #region Helper Methods

        private void ValidateColumns(string[] headers)
        {
            PopulateMetaData();

            if (AllColumns.Count != headers.Length)
            {
                throw new Exception($"All columns are not supplied, expected column:{AllColumns.Count}  , supplied cloumns:{headers.Length}");
            }
            int colIdx = 0;
            foreach (var cell in headers)
            {
                if (!Attributes.ContainsKey(cell.ToLower()))
                {
                    throw new Exception($"Actual Column: {cell} is not a expected column");
                }

                if (Attributes[cell.ToLower()].Index != colIdx)
                {
                    throw new Exception($"Actual column:{cell} index is incorrect");
                }
                colIdx++;
            }
        }

        #endregion
    }
}
