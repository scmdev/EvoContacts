using EvoContacts.ApplicationCore.Entities;
using EvoContacts.ApplicationCore.Interfaces;
using EvoContacts.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvoContacts.Infrastructure.Repositories
{
    public class ContactRepository : EfRepository<Contact>, IContactRepository
    {
        public ContactRepository(EvoContactsDbContext dbContext) : base(dbContext)
        {
        }
    }
}
