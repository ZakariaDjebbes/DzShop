namespace Core.Entities.Order
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string firstName, string lastName, string city, string street, string zipCode, string country)
        {
            FirstName = firstName;
            LastName = lastName;
            City = city;
            Street = street;
            ZipCode = zipCode;
            Country = country;
        }

        public string FirstName { get; set; }
		public string LastName { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string ZipCode { get; set; }
		public string Country { get; set; }
    }
}