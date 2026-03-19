namespace PersonManager.Domain
{
    public class Project
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        // Relacja: Many-to-Many (Person <-> Project)
        public required ICollection<Person> Members { get; set; }

        public Project()
        {
            Members = new List<Person>();
        }
    }
}
