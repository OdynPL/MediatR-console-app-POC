namespace PersonManager.Domain
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        // Relacja: Many-to-Many (Person <-> Project)
        public ICollection<Person> Members { get; set; } = new List<Person>();
    }
}
