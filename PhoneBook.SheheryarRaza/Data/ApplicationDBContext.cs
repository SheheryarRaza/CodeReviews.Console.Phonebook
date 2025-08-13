using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneBook.SheheryarRaza.Models;

namespace PhoneBook.SheheryarRaza.Data
{
    public class ApplicationDBContext: DbContext
    {
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = DESKTOP-JMBBD5O\\SQLEXPRESS; Database = PhoneBook; User ID = Blocks-Administrator; Password = sheri1234; TrustServerCertificate = True; ");
        }
    }
}
