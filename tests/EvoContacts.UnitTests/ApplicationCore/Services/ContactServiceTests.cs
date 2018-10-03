using AutoMapper;
using EvoContacts.ApplicationCore.Entities;
using EvoContacts.ApplicationCore.Enums;
using EvoContacts.ApplicationCore.Interfaces;
using EvoContacts.ApplicationCore.Mapping;
using EvoContacts.ApplicationCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EvoContacts.IntegrationTests.Repositories
{
    public class ContactServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<ContactService>> _mockLogger;

        private readonly List<Contact> _testContacts;

        private readonly ContactService _sutContactService;

        public ContactServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<ContactService>>();

            _testContacts = Contacts;

            // add Auto Mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();

            IContactRepository _contactRepository = MockContactRepository(_testContacts);

            _sutContactService = new ContactService(
                _mockConfiguration.Object,
                _mockLogger.Object,
                mapper,
                _contactRepository
                );
        }

        #region Contact TEST DATA

        public static List<Contact> Contacts
        {
            get
            {
                return new List<Contact>
                {
                    new Contact{
                        FirstName = "Virginia",
                        LastName = "Thom",
                        Email = "vthom@evocontacts.com",
                        PhoneNumber = "555-718-1773",
                        ContactStatus = ApplicationCore.Enums.ContactStatusEnum.ActiveEnum
                    },
                    new Contact{
                        FirstName = "April",
                        LastName = "Baker",
                        Email = "abaker@evocontacts.com",
                        PhoneNumber = "555-468-0707",
                        ContactStatus = ApplicationCore.Enums.ContactStatusEnum.ActiveEnum
                    },
                    new Contact{
                        FirstName = "Brett",
                        LastName = "Vaughn",
                        Email = "bvaughn@evocontacts.com",
                        PhoneNumber = "555-465-3326",
                        ContactStatus = ApplicationCore.Enums.ContactStatusEnum.ActiveEnum
                    },
                    new Contact{
                        FirstName = "Ruth",
                        LastName = "Goggins",
                        Email = "rgoggins@evocontacts.com",
                        PhoneNumber = "555-784-7150",
                        ContactStatus = ApplicationCore.Enums.ContactStatusEnum.ActiveEnum
                    },
                    new Contact{
                        FirstName = "Milton",
                        LastName = "Fransen",
                        Email = "mfransen@evocontacts.com",
                        PhoneNumber = "555-358-9217",
                        ContactStatus = ApplicationCore.Enums.ContactStatusEnum.ActiveEnum
                    },
                    new Contact{
                        FirstName = "Kathy",
                        LastName = "Cervantes",
                        Email = "kcervantes@evocontacts.com",
                        PhoneNumber = "555-599-0192",
                        ContactStatus = ApplicationCore.Enums.ContactStatusEnum.ActiveEnum
                    },
                    new Contact{
                        FirstName = "Rebecca",
                        LastName = "Largent",
                        Email = "rlargent@evocontacts.com",
                        PhoneNumber = "555-384-0256",
                        ContactStatus = ApplicationCore.Enums.ContactStatusEnum.ActiveEnum
                    },
                    new Contact{
                        FirstName = "Amy",
                        LastName = "Newquist",
                        Email = "anewquist@evocontacts.com",
                        PhoneNumber = "555-677-5939",
                        ContactStatus = ApplicationCore.Enums.ContactStatusEnum.ActiveEnum
                    },
                    new Contact{
                        FirstName = "Chris",
                        LastName = "Falco",
                        Email = "cfalco@evocontacts.com",
                        PhoneNumber = "555-314-4913",
                        ContactStatus = ApplicationCore.Enums.ContactStatusEnum.ActiveEnum
                    },
                    new Contact{
                        FirstName = "Naomi",
                        LastName = "Higdon",
                        Email = "nhigdon@evocontacts.com",
                        PhoneNumber = "555-267-0550",
                        ContactStatus = ApplicationCore.Enums.ContactStatusEnum.ActiveEnum
                    },
                };
            }
        }

        #endregion

        #region Build Mock ContactRepository

        public static IContactRepository MockContactRepository(List<Contact> mockContacts)
        {
            // Initialise repository  
            var mockRepo = new Mock<IContactRepository>(MockBehavior.Default);

            // Setup mocking behavior  
            mockRepo.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Func<Guid, Contact>(id => mockContacts.Find(p => p.Id.Equals(id))));

            mockRepo.Setup(p => p.GetSingleAsync(It.IsAny<Expression<Func<Contact, bool>>>())).ReturnsAsync((Expression<Func<Contact, bool>> predicate) => mockContacts.AsQueryable().FirstOrDefault(predicate));

            mockRepo.Setup(p => p.ListAllAsync()).ReturnsAsync(mockContacts);

            mockRepo.Setup(p => p.GetPagedListAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(
                new Func<int, int, ApplicationCore.Models.PagedList<Contact>>(
               (page, pageSize) =>
               {
                   var totalRecords = mockContacts.Count;
                   var items = mockContacts.Skip((page - 1) * pageSize)
                       .Take(pageSize).ToList();

                   var pagedList = new ApplicationCore.Models.PagedList<Contact>()
                   {
                       Items = items,
                       Page = page,
                       PageSize = pageSize,
                       TotalRecords = totalRecords
                   };

                   return pagedList;
               })
               );

            mockRepo.Setup(p => p.CountAsync()).ReturnsAsync(mockContacts.Count);

            mockRepo.Setup(p => p.AddAsync((It.IsAny<Contact>()))).ReturnsAsync(true);

            mockRepo.Setup(p => p.UpdateAsync((It.IsAny<Contact>()))).ReturnsAsync(true);

            mockRepo.Setup(p => p.DeleteAsync(It.IsAny<Guid>(), It.IsAny<Guid?>())).ReturnsAsync(true);

            mockRepo.Setup(p => p.DeleteAsync(It.IsIn(Guid.Empty), It.IsAny<Guid?>())).ReturnsAsync(false);

            // Return mock implementation object  
            return mockRepo.Object;
        }

        #endregion

        [Theory]
        [InlineData(0, 20)]
        [InlineData(-4, 20)]
        public async Task GetPagedContactsReturnsBadRequestIfInvalidPageRequested(int page, int pageSize)
        {
            var contactsResult = await _sutContactService.GetPagedContacts(page, pageSize);

            Assert.NotNull(contactsResult);

            Assert.True(contactsResult.IsBadRequest);
        }

        [Theory]
        [InlineData(1, 1000)]
        [InlineData(1, 5)]
        [InlineData(2, 4)]
        public async Task GetPagedContactsReturnsValidPagedContacts(int page, int pageSize)
        {
            int expectedTotalRecords = _testContacts.Count(x => !x.IsDeleted);
            int expectedNumItems = expectedTotalRecords - ((page - 1) * pageSize) >= pageSize ? pageSize : expectedTotalRecords - ((page - 1) * pageSize);

            var contactsResult = await _sutContactService.GetPagedContacts(page, pageSize);

            Assert.NotNull(contactsResult);

            Assert.Equal(page, contactsResult.Page);
            Assert.Equal(pageSize, contactsResult.PageSize);

            Assert.Equal(expectedTotalRecords, contactsResult.TotalRecords);

            Assert.Equal(expectedNumItems, contactsResult.NumItems);
        }

        [Fact]
        public async Task GetContactReturnsEmptyDataIfInvalidIdPassed()
        {
            var invalidContactId = Guid.Empty;

            var contactResult = await _sutContactService.GetContact(invalidContactId);

            Assert.False(contactResult.IsBadRequest); //controller won't return 400

            Assert.Null(contactResult.Data); //controller will return 404
        }

        [Fact]
        public async Task GetContactReturnsContactIfValidIdPassed()
        {
            var validContactId = _testContacts.First().Id;

            var contactResult = await _sutContactService.GetContact(validContactId);

            Assert.False(contactResult.IsBadRequest); //controller won't return 400

            Assert.NotNull(contactResult.Data); //controller won't return 404

            Assert.Equal(validContactId, contactResult.Data.Id);
        }

        [Fact]
        public async Task CreateContactReturnsBadRequestIfEmailAlreadyInUse()
        {
            var contactCreate = new ApplicationCore.Models.ContactCreate()
            {
                FirstName = "Jack",
                LastName = "White",
                Email = _testContacts.First().Email, //duplicate email
                PhoneNumber = "555-164-7158",
                ContactStatus = ContactStatusEnum.ActiveEnum
            };

            var createResult = await _sutContactService.CreateContact(contactCreate);

            Assert.True(createResult.IsBadRequest); //controller returns 400
        }

        [Fact]
        public async Task CreateContactReturnsContactIfValidIdPassed()
        {
            var contactCreate = new ApplicationCore.Models.ContactCreate()
            {
                FirstName = "Jack",
                LastName = "White",
                Email = "jwhite@evocontacts.com",
                PhoneNumber = "555-164-7158",
                ContactStatus = ContactStatusEnum.ActiveEnum
            };

            var createResult = await _sutContactService.CreateContact(contactCreate);

            Assert.False(createResult.IsBadRequest); //controller won't return 400

            Assert.NotNull(createResult.Data); //controller won't return 404

            Assert.Equal(contactCreate.Email, createResult.Data.Email);
        }

        [Fact]
        public async Task UpdateContactReturnsFalseIfInvalidIdPassed()
        {
            var existingContact = _testContacts.First();

            var contactUpdate = new ApplicationCore.Models.ContactUpdate()
            {
                Id = Guid.Empty, //invalid Id
                FirstName = existingContact.FirstName, //not updating
                LastName = existingContact.LastName, //not updating
                Email = existingContact.Email, //not updating
                PhoneNumber = existingContact.PhoneNumber == "555-111-2222" ? "555-123-4567" : "555-111-2222", //updating PhoneNumber
                ContactStatus = existingContact.ContactStatus //not updating
            };

            var updateResult = await _sutContactService.UpdateContact(contactUpdate);

            Assert.False(updateResult.IsBadRequest); //controller won't return 400

            Assert.NotNull(updateResult.Data);

            Assert.False(updateResult.Data); //controller will return 404
        }

        [Fact]
        public async Task UpdateContactReturnsBadRequestIfNothingChanged()
        {
            var existingContact = _testContacts.First();

            var contactUpdate = new ApplicationCore.Models.ContactUpdate()
            {
                Id = existingContact.Id,
                FirstName = existingContact.FirstName, //not updating
                LastName = existingContact.LastName, //not updating
                Email = existingContact.Email, //not updating
                PhoneNumber = existingContact.PhoneNumber, //not updating
                ContactStatus = existingContact.ContactStatus //not updating
            };

            var updateResult = await _sutContactService.UpdateContact(contactUpdate);

            Assert.True(updateResult.IsBadRequest); //controller returns 400
        }

        [Fact]
        public async Task UpdateContactReturnsBadRequestIfEmailAlreadyInUse()
        {
            var existingContact = _testContacts.First();

            var contactUpdate = new ApplicationCore.Models.ContactUpdate()
            {
                Id = existingContact.Id,
                FirstName = existingContact.FirstName, //not updating
                LastName = existingContact.LastName, //not updating
                Email = _testContacts.First(x => x.Email != existingContact.Email).Email, //attempting to update to another Contact's Email
                PhoneNumber = existingContact.PhoneNumber, //not updating
                ContactStatus = existingContact.ContactStatus //not updating
            };

            var updateResult = await _sutContactService.UpdateContact(contactUpdate);

            Assert.True(updateResult.IsBadRequest); //controller returns 400
        }

        [Fact]
        public async Task UpdateContactReturnsTrueIfSuccessful()
        {
            var existingContact = _testContacts.First();

            var contactUpdate = new ApplicationCore.Models.ContactUpdate()
            {
                Id = existingContact.Id,
                FirstName = existingContact.FirstName, //not updating
                LastName = existingContact.LastName, //not updating
                Email = existingContact.Email, //not updating
                PhoneNumber = existingContact.PhoneNumber == "555-111-2222" ? "555-123-4567" : "555-111-2222", //updating PhoneNumber
                ContactStatus = existingContact.ContactStatus //not updating
            };

            var updateResult = await _sutContactService.UpdateContact(contactUpdate);

            Assert.False(updateResult.IsBadRequest); //controller won't return 400

            Assert.NotNull(updateResult.Data); //controller won't return 404

            Assert.True(updateResult.Data); //controller returns 200
        }

        [Fact]
        public async Task UpdateContactStatusReturnsFalseIfInvalidIdPassed()
        {
            var existingContact = _testContacts.First();

            var contactUpdateStatus = new ApplicationCore.Models.ContactUpdateStatus()
            {
                Id = Guid.Empty, //invalid Id
                ContactStatus = existingContact.ContactStatus == ContactStatusEnum.ActiveEnum ? ContactStatusEnum.InactiveEnum : ContactStatusEnum.ActiveEnum //updating ContactStatus
            };

            var updateStatusResult = await _sutContactService.UpdateContactStatus(contactUpdateStatus);

            Assert.False(updateStatusResult.IsBadRequest); //controller won't return 400

            Assert.NotNull(updateStatusResult.Data);

            Assert.False(updateStatusResult.Data); //controller will return 404
        }

        [Fact]
        public async Task UpdateContactStatusReturnsBadRequestIfNotChanged()
        {
            var existingContact = _testContacts.First();

            var contactUpdateStatus = new ApplicationCore.Models.ContactUpdateStatus()
            {
                Id = existingContact.Id,
                ContactStatus = existingContact.ContactStatus //not updating
            };

            var updateStatusResult = await _sutContactService.UpdateContactStatus(contactUpdateStatus);

            Assert.True(updateStatusResult.IsBadRequest); //controller returns 400
        }

        [Fact]
        public async Task UpdateContactStatusReturnsTrueIfSuccessful()
        {
            var existingContact = _testContacts.First();

            var contactUpdateStatus = new ApplicationCore.Models.ContactUpdateStatus()
            {
                Id = existingContact.Id,
                ContactStatus = existingContact.ContactStatus == ContactStatusEnum.ActiveEnum ? ContactStatusEnum.InactiveEnum : ContactStatusEnum.ActiveEnum //updating ContactStatus
            };

            var updateStatusResult = await _sutContactService.UpdateContactStatus(contactUpdateStatus);

            Assert.False(updateStatusResult.IsBadRequest); //controller won't return 400

            Assert.NotNull(updateStatusResult.Data); //controller won't return 404

            Assert.True(updateStatusResult.Data); //controller returns 200
        }

        [Fact]
        public async Task DeleteContactReturnsFalseIfInvalidIdPassed()
        {
            var invalidContactId = Guid.Empty;

            var deletedUserId = Guid.NewGuid();

            var deleteResult = await _sutContactService.DeleteContact(invalidContactId, deletedUserId);

            Assert.False(deleteResult.IsBadRequest); //controller won't return 400

            Assert.NotNull(deleteResult.Data);

            Assert.False(deleteResult.Data); //controller will return 404
        }

        [Fact]
        public async Task DeleteContactReturnsTrueIfSuccessful()
        {
            var contactId = _testContacts.First().Id;

            var deletedUserId = Guid.NewGuid();

            var deleteResult = await _sutContactService.DeleteContact(contactId, deletedUserId);

            Assert.False(deleteResult.IsBadRequest); //controller won't return 400

            Assert.NotNull(deleteResult.Data); //controller won't return 404

            Assert.True(deleteResult.Data); //controller returns 200
        }
    }
}
