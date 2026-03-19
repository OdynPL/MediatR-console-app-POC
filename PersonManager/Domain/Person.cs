namespace PersonManager.Domain
{
    public class Person
    {
        public int Id { get; set; } // Klucz główny dla EF Core
        public required string Name { get; set; }
        public int Age { get; set; }
        public int RowVersion { get; set; } // Concurrency token

        public Person() { }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}
