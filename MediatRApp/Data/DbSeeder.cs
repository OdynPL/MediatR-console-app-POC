
using MediatRApp.Domain;

namespace MediatRApp.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (db.People.Any()) return;
            var rnd = new Random();
            for (int i = 1; i <= 1250; i++)
            {
                db.People.Add(new Person { Name = $"Person_{i}", Age = rnd.Next(18, 120) });
            }
            db.SaveChanges();
        }
    }
}
