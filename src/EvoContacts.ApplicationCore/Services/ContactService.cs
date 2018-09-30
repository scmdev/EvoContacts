using AutoMapper;
using EvoContacts.ApplicationCore.Enums;
using EvoContacts.ApplicationCore.Extensions;
using EvoContacts.ApplicationCore.Interfaces;
using EvoContacts.ApplicationCore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoContacts.ApplicationCore.Services
{
    public class ContactService : BaseService, IContactService
    {
        private readonly IMapper _mapper;
        private readonly IContactRepository _contactRepository;

        public ContactService(
            IConfiguration configuration,
            ILogger<ContactService> logger,
            IMapper mapper,
            IContactRepository contactRepository
            ) : base(configuration, logger)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        #region CONTACT

        public async Task<Models.PagedListResult<Models.Contact>> GetPagedContacts(int page = 1, int pageSize = 20)
        {
            var result = new Models.PagedListResult<Models.Contact>(page, pageSize);

            try
            {
                if (page < 1)
                {
                    result.ErrorMessage = ERROR_GET_CONTACTS_INVALID_PAGE_NUMBER;
                    return result;
                }

                var contactEntitiesPagedList = await _contactRepository.GetPagedListAsync(page: page, pageSize: pageSize);

                //Map Models.PagedListResult<Entities.Contact> to Models.PagedListResult<Models.Contact>
                result = new Models.PagedListResult<Models.Contact>(page, pageSize)
                {
                    Items = _mapper.Map<List<Models.Contact>>(contactEntitiesPagedList.Items),
                    Page = contactEntitiesPagedList.Page,
                    PageSize = contactEntitiesPagedList.PageSize,
                    TotalRecords = contactEntitiesPagedList.TotalRecords
                };
            }
            catch (Exception e)
            {
                result.ErrorMessage = LogError(e, "GetPagedContacts", new object[] { page, pageSize });
            }

            return result;
        }

        public async Task<Models.Result<Models.Contact>> GetContact(Guid contactId)
        {
            var result = new Models.Result<Models.Contact>();

            try
            {
                var contactEntity = await _contactRepository.GetByIdAsync(contactId);

                if (contactEntity == null)
                {
                    return result; //result.Data = null => controller will return 404
                }

                result.Data = _mapper.Map<Models.Contact>(contactEntity);
            }
            catch (Exception e)
            {
                result.ErrorMessage = LogError(e, "GetContact", new object[] { contactId });
            }

            return result;
        }

        public async Task<Models.Result<Models.Contact>> CreateContact(Models.ContactCreate contactCreate)
        {
            var result = new Models.Result<Models.Contact>();

            try
            {
                //check Contact with same Email does not already exist
                var checkEmailEntity = await _contactRepository.GetSingleAsync(x => x.Email == contactCreate.Email);

                if (checkEmailEntity != null)
                {
                    result.ErrorMessage = ERROR_CREATE_CONTACT_DUPLICATE_EMAIL;
                    return result;
                }

                //Create new contactEntity by mapping contactCreate to Entities.Contact
                var contactEntity = _mapper.Map<Entities.Contact>(contactCreate);

                //Add contactEntity
                await _contactRepository.AddAsync(contactEntity);

                result.Data = _mapper.Map<Models.Contact>(contactEntity);
            }
            catch (Exception e)
            {
                result.ErrorMessage = LogError(e, "CreateContact", new object[] { contactCreate });
            }

            return result;
        }

        public async Task<Models.Result<bool?>> UpdateContact(Models.ContactUpdate contactUpdate)
        {
            var result = new Models.Result<bool?>();

            try
            {
                //Get contactEntity using GetSingleAsync to avoid tracking
                var contactEntity = await _contactRepository.GetSingleAsync(x => x.Id == contactUpdate.Id);

                if (contactEntity == null)
                {
                    result.Data = false; //result.Data = false => controller will return 404
                    return result;
                }

                //check Contact with same Email does not already exist
                var checkEmailEntity = await _contactRepository.GetSingleAsync(x => x.Email == contactUpdate.Email);

                if (checkEmailEntity != null && checkEmailEntity.Id != contactUpdate.Id)
                {
                    result.ErrorMessage = ERROR_UPDATE_CONTACT_DUPLICATE_EMAIL;
                    return result;
                }

                //Check/Update contactEntity properties as per contactUpdate provided
                if (!contactUpdate.MapUpdatesToEntity(ref contactEntity))
                {
                    result.ErrorMessage = ERROR_UPDATE_CONTACT_NO_CHANGES_DETECTED;
                    return result;
                }

                result.Data = await _contactRepository.UpdateAsync(contactEntity);
            }
            catch (Exception e)
            {
                result.ErrorMessage = LogError(e, "UpdateContact", new object[] { contactUpdate });
            }

            return result;
        }

        public async Task<Models.Result<bool?>> UpdateContactStatus(ContactUpdateStatus contactUpdateStatus)
        {
            var result = new Models.Result<bool?>();

            try
            {
                //Get contactEntity using GetSingleAsync to avoid tracking
                var contactEntity = await _contactRepository.GetSingleAsync(x => x.Id == contactUpdateStatus.Id);

                if (contactEntity == null)
                {
                    result.Data = false; //result.Data = false => controller will return 404
                    return result;
                }

                //Check/Update contactEntity properties as per contactUpdateStatus provided
                if (!contactUpdateStatus.MapUpdatesToEntity(ref contactEntity))
                {
                    result.ErrorMessage = ERROR_UPDATE_CONTACT_NO_CHANGES_DETECTED;
                    return result;
                }

                result.Data = await _contactRepository.UpdateAsync(contactEntity);
            }
            catch (Exception e)
            {
                result.ErrorMessage = LogError(e, "UpdateContactStatus", new object[] { contactUpdateStatus });
            }

            return result;
        }

        public async Task<Models.Result<bool?>> DeleteContact(Guid contactId, Guid deletedUserId)
        {
            var result = new Models.Result<bool?>();

            try
            {
                result.Data = await _contactRepository.DeleteAsync(contactId, deletedUserId);
            }
            catch (Exception e)
            {
                result.ErrorMessage = LogError(e, "DeleteContact", new object[] { contactId });
            }

            return result;
        }

        #endregion

    }
}
