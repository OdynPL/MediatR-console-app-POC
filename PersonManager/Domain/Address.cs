namespace PersonManager.Domain
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        // Relacja: One-to-Many (Person -> Address)
        public ICollection<Person> Residents { get; set; }
    }
}
