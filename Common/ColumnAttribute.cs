using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvDataProcessor.Common
{
    public sealed class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string description, int index)
        {
            Description = description;
            Index = index;
        }

        public string Description { get; }
        public int Index { get; }
    }
}
