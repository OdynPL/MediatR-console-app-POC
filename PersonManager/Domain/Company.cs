namespace PersonManager.Domain
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Relacja: Many-to-One (Person -> Company)
        public ICollection<Person> Employees { get; set; }
    }
}
