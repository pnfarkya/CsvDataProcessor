namespace CsvDataProcessor.Model
{
    // Since we are just reading the file and not updating any values
    // of person information hence created model without NotifyPropChange to avoid uncnecory props change event
    public sealed class PersonModel
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }

        public string PostalZip { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
