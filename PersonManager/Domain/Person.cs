namespace PersonManager.Domain
{
    public class Person
    {
        public int Id { get; set; } // Klucz główny dla EF Core
        public required string Name { get; set; }
        public int Age { get; set; }
        public int RowVersion { get; set; } // Concurrency token

        // One-to-Many: Person -> Address
        public int? AddressId { get; set; }
        public Address? Address { get; set; }

        // Many-to-One: Person -> Company
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        // Many-to-Many: Person <-> Project
        public ICollection<Project> Projects { get; set; }

        public Person()
        {
            Projects = new List<Project>();
        }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
            Projects = new List<Project>();
        }
    }
}
