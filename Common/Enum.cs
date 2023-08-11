using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvDataProcessor.Common
{
    public enum PersonColumn
    {
        [Column("name", 0)]
        Name,

        [Column("country", 1)]
        Country,

        [Column("address", 2)]
        Address,

        [Column("postalzip", 3)]
        Zip,

        [Column("email", 4)]
        Email,

        [Column("phone", 5)]
        Phone
    }

    public static class EnumHelper
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T GetAttributes<T>(Enum val) where T : Attribute
        {
            var type = val.GetType();
            var members = type.GetMember(val.ToString());
            var attrs = members[0].GetCustomAttributes(typeof(T), false);

            return attrs.Length > 0 ? (T)attrs[0] : null;
        }
    }
}
