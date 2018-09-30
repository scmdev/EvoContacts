using EvoContacts.ApplicationCore.Entities;
using EvoContacts.ApplicationCore.Interfaces;
using EvoContacts.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvoContacts.Infrastructure.Repositories
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(EvoContactsDbContext dbContext) : base(dbContext)
        {
        }
    }
}
