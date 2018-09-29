using EvoContacts.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvoContacts.Infrastructure.Data
{
    public class EvoContactsSeed
    {
        public static async Task SeedAsync(EvoContactsDbContext dbContext,
                          ILoggerFactory loggerFactory, int retry = 0)
        {
            int retryForAvailability = retry;

            var logger = loggerFactory.CreateLogger<EvoContactsSeed>();

            try
            {
                //if (!dbContext.AllMigrationsApplied())
                //{
                //    logger.LogWarning("Some migrations still need to be applied. Seeding will be skipped.");
                //    return;
                //}

                logger.LogInformation("Checking whether seeding is required.");

                #region SEED CONTACTS

                var existingContacts = await dbContext.Contacts.Where(x => !x.IsDeleted).ToListAsync();

                var seedContacts = SeedData.Contacts.Where(x => !existingContacts.Any(c => c.Email == x.Email));

                if (seedContacts.Any())
                {
                    logger.LogInformation("Seed Contacts started.");

                    dbContext.Contacts.AddRange(
                        seedContacts
                    );
                    await dbContext.SaveChangesAsync();

                    logger.LogInformation("Seed Contacts completed.");
                }

                #endregion

                logger.LogInformation("Seeding completed.");
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    logger.LogError(ex.Message);
                    await SeedAsync(dbContext, loggerFactory, retryForAvailability);
                }
            }
        }

    }

    public class SeedData
    {
        #region CONTACTS

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
    }
}
