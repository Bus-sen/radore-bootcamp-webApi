using CodeFirstOrnek.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CodeFirstOrnek.Data
{
    public class ApplicationDbContext : DbContext
    {
        //türediğimiz classın constructorına parametre gönderiyoruz
        //application dbcontext classımız databasedeki her tablodan sorumlu olacak
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //databasedeki tablomun adı Kitap olacak
        public DbSet<Kitap> Kitap { get; set; }
    }
}
