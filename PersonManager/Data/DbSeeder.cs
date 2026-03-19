
using PersonManager.Domain;

namespace PersonManager.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (db.People.Any()) return;
            var rnd = new Random();

            // Dodaj adresy
            var addresses = new List<Address>();
            for (int i = 1; i <= 50; i++)
            {
                addresses.Add(new Address
                {
                    Street = $"Street_{i}",
                    City = $"City_{i % 10}",
                    Country = "Poland",
                    Residents = new List<Person>()
                });
            }
            db.Addresses.AddRange(addresses);

            // Dodaj firmy
            var companies = new List<Company>();
            for (int i = 1; i <= 20; i++)
            {
                companies.Add(new Company
                {
                    Name = $"Company_{i}",
                    Employees = new List<Person>()
                });
            }
            db.Companies.AddRange(companies);

            // Dodaj projekty (kilkaset)
            var projects = new List<Project>();
            for (int i = 1; i <= 300; i++)
            {
                projects.Add(new Project
                {
                    Title = $"Project_{i}",
                    Members = new List<Person>()
                });
            }
            db.Projects.AddRange(projects);

            // Dodaj osoby i relacje
            for (int i = 1; i <= 1250; i++)
            {
                var person = new Person
                {
                    Name = $"Person_{i}",
                    Age = rnd.Next(18, 120),
                    Address = addresses[rnd.Next(addresses.Count)],
                    Company = companies[rnd.Next(companies.Count)]
                };

                // Dodaj do mieszkańców adresu i pracowników firmy
                if (person.Address != null)
                    person.Address.Residents.Add(person);
                if (person.Company != null)
                    person.Company.Employees.Add(person);

                // Dodaj projekty (każda osoba bierze udział w kilku)
                var personProjects = rnd.Next(2, 8);
                for (int j = 0; j < personProjects; j++)
                {
                    var project = projects[rnd.Next(projects.Count)];
                    if (person.Projects != null)
                        person.Projects.Add(project);
                    if (project.Members != null)
                        project.Members.Add(person);
                }

                db.People.Add(person);
            }
            db.SaveChanges();
        }
    }
}
