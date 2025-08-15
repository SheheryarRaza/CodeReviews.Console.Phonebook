using Microsoft.EntityFrameworkCore;
using PhoneBook.SheheryarRaza.Models;

namespace PhoneBook.SheheryarRaza.Data
{
    public class ApplicationDBContext: DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = DESKTOP-JMBBD5O\\SQLEXPRESS; Database = PhoneBook; User ID = Blocks-Administrator; Password = sheri1234; TrustServerCertificate = True; ");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Family" },
                new Category { Id = 2, Name = "Friends" },
                new Category { Id = 3, Name = "Work" }
            );
        }
    }
}
