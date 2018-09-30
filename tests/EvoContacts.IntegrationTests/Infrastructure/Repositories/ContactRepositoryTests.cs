using EvoContacts.ApplicationCore.Entities;
using EvoContacts.Infrastructure.Data;
using EvoContacts.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EvoContacts.IntegrationTests.Repositories
{
    public class ContactRepositoryTests
    {
        private readonly EvoContactsDbContext _dbContext;
        private readonly ContactRepository _contactRepository;

        private readonly ITestOutputHelper _output;

        public ContactRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
            var dbOptions = new DbContextOptionsBuilder<EvoContactsDbContext>()
                .UseInMemoryDatabase(databaseName: "EvoContacts-Tests")
                .Options;
            _dbContext = new EvoContactsDbContext(dbOptions);

            var seedContacts = SeedData.Contacts.Where(x => !_dbContext.Contacts.Where(c => !c.IsDeleted).Any(c => c.Email == x.Email));

            //Seed missing Contacts
            if (seedContacts.Any())
            {
                _dbContext.Contacts.AddRange(seedContacts);
                _dbContext.SaveChanges();
            }

            _contactRepository = new ContactRepository(_dbContext);
        }

        [Fact]
        public async Task GetByIdAsyncReturnsExistingContact()
        {
            //Must add a Contact directly to Contacts collection for use when checking GetByIdAsync
            var contactEntity = new Contact
            {
                FirstName = "Jerry",
                LastName = "Mills",
                Email = "jmills@evocontacts.com",
                PhoneNumber = "555-690-2814",
                ContactStatus = ApplicationCore.Enums.ContactStatusEnum.ActiveEnum
            };
            _dbContext.Contacts.Add(contactEntity);
            _dbContext.SaveChanges();

            _output.WriteLine(string.Format("ContactId: {0}", contactEntity.Id));

            //get Contact entity from repo using contactId
            var repoContactEntity = await _contactRepository.GetByIdAsync(contactEntity.Id);

            Assert.NotNull(repoContactEntity);

            Assert.Equal(contactEntity.Email, repoContactEntity.Email);
        }

        [Fact]
        public async Task GetSingleAsyncReturnsExistingContact()
        {
            //Must add a Contact directly to Contacts collection for use when checking GetSingleAsync
            var contactEntity = new Contact
            {
                FirstName = "William",
                LastName = "Geist",
                Email = "wgeist@evocontacts.com",
                PhoneNumber = "555-451-1765",
                ContactStatus = ApplicationCore.Enums.ContactStatusEnum.ActiveEnum
            };
            _dbContext.Contacts.Add(contactEntity);
            _dbContext.SaveChanges();

            _output.WriteLine(string.Format("ContactId: {0}", contactEntity.Id));

            //get Contact entity from repo using contactId
            var repoContactEntity = await _contactRepository.GetSingleAsync(x => x.Id == contactEntity.Id);

            Assert.NotNull(repoContactEntity);

            Assert.Equal(contactEntity.Email, repoContactEntity.Email);
        }

        [Theory]
        [InlineData(1, 1000)]
        [InlineData(1, 5)]
        [InlineData(2, 4)]
        public async Task GetPagedListAsyncReturnsValidPagedContacts(int page, int pageSize)
        {
            int expectedTotalRecords = _dbContext.Contacts.Count(x => !x.IsDeleted);
            int expectedNumItems = expectedTotalRecords - ((page - 1) * pageSize) >= pageSize ? pageSize : expectedTotalRecords - ((page - 1) * pageSize);

            var repoEntitiesPagedList = await _contactRepository.GetPagedListAsync(page, pageSize);

            Assert.NotNull(repoEntitiesPagedList);

            Assert.Equal(page, repoEntitiesPagedList.Page);
            Assert.Equal(pageSize, repoEntitiesPagedList.PageSize);

            Assert.Equal(expectedTotalRecords, repoEntitiesPagedList.TotalRecords);

            Assert.Equal(expectedNumItems, repoEntitiesPagedList.NumItems);
        }

        [Fact]
        public async Task CountAsyncReturnsValidContactsCount()
        {
            int expectedTotalRecords = _dbContext.Contacts.Count(x => !x.IsDeleted);

            //get Contact entity from repo using contactId
            int repoContactsCount = await _contactRepository.CountAsync();

            Assert.Equal(expectedTotalRecords, repoContactsCount);
        }

        [Fact]
        public async Task AddAsyncFunctioningCorrectly()
        {
            var createEntity = new Contact
            {
                FirstName = "Sarah",
                LastName = "Morris",
                Email = "smorris@evocontacts.com",
                PhoneNumber = "555-274-5989",
                ContactStatus = ApplicationCore.Enums.ContactStatusEnum.ActiveEnum
            };

            Assert.True(await _contactRepository.AddAsync(createEntity));

            //Check Contacts collection contains newly added Contact
            Assert.Contains(createEntity, _dbContext.Contacts.Where(x => !x.IsDeleted).ToList());
        }

        [Fact]
        public async Task UpdateAsyncFunctioningCorrectly()
        {
            var updateEntity = _dbContext.Contacts.Where(x => !x.IsDeleted).First();

            string updatedFirstName = updateEntity.FirstName + " UPDATED";
            string updatedLastName = updateEntity.LastName + " UPDATED";
            updateEntity.FirstName = updatedFirstName;
            updateEntity.LastName = updatedLastName;

            Assert.True(await _contactRepository.UpdateAsync(updateEntity));

            var existingContacts = _dbContext.Contacts.Where(x => !x.IsDeleted).ToList();

            //Check Contacts collection contains updated Contact
            Assert.Contains(updateEntity, existingContacts);

            Assert.Equal(updatedFirstName, existingContacts.First(x => x == updateEntity).FirstName);

            Assert.Equal(updatedLastName, existingContacts.First(x => x == updateEntity).LastName);
        }

        [Fact]
        public async Task DeleteAsyncFunctioningCorrectly()
        {
            var deleteEntity = _dbContext.Contacts.Where(x => !x.IsDeleted).Last();

            //TBC: Must add TestUser user
            var deletedUserId = Guid.NewGuid();

            Assert.True(await _contactRepository.DeleteAsync(deleteEntity.Id, deletedUserId));

            var existingContacts = _dbContext.Contacts.Where(x => !x.IsDeleted).ToList();

            //Check deleteEntity still exists in Contacts collection
            Assert.Contains(deleteEntity, _dbContext.Contacts);

            //Check deleteEntity has been marked IsDeleted
            Assert.True(_dbContext.Contacts.First(x => x == deleteEntity).IsDeleted);

            //Check deleteEntity not returned by GetByIdAsync
            Assert.Null(await _contactRepository.GetByIdAsync(deleteEntity.Id));

            //Check deleteEntity not returned by GetSingleAsync
            Assert.Null(await _contactRepository.GetSingleAsync(x => x.Id == deleteEntity.Id));
        }

    }
}
