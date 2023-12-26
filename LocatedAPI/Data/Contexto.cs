using LocatedAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LocatedAPI.Data
{
    public class Contexto : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Moviment> Moviment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            //opt.UseSqlServer(@"Data Source=Avell\\SQLEXPRESS;initial Catalog=API_Located;User ID=usuario;password=senha;language=Portuguese;Trusted_Connection=True;");
            opt.UseSqlServer(@"Data Source=Avell\SQLEXPRESS;initial Catalog=db_located;Trusted_Connection=True;Integrated Security=True;language=Portuguese;");
        }
    }
}
