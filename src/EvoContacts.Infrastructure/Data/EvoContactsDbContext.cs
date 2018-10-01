using EvoContacts.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvoContacts.Infrastructure.Data
{
    public class EvoContactsDbContext : DbContext
    {
        public EvoContactsDbContext(DbContextOptions<EvoContactsDbContext> options) : base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region ADD UNIQUE INDEXES

            modelBuilder.Entity<Contact>().HasIndex(x => new { x.Email }).HasFilter("[Email] IS NOT NULL AND IsDeleted = 0").IsUnique();

            #endregion
        }

    }
}
