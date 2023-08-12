using CsvDataProcessor.Model;

namespace CsvDataProcessor.Test.TestData
{
    public class PersonData
    {
        public static List<PersonModel> Persons
        {
            get
            {
                return new List<PersonModel>()
                {
                    new PersonModel(){Name="ABCD", Country="Eng",Phone="1234"},
                    new PersonModel(){Name="CDEF", Country="Aus",Phone="45645"},
                    new PersonModel(){Name="Prinesh", Country="Ind",Phone="798"},
                    new PersonModel(){Name="Richard", Country="Ind",Phone="79878"},
                    new PersonModel(){Name="Marian", Country="Usa",Phone="54564"},
                    new PersonModel(){Name="Paul", Country="Usa",Phone="8749711"}
                };
            }
        }
    }
}
