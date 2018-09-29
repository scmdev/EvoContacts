using AutoMapper;
using EvoContacts.ApplicationCore.Extensions;
using EvoContacts.ApplicationCore.Interfaces;
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

        public async Task<Models.ListResult<Models.Contact>> GetAllContacts()
        {
            var result = new Models.ListResult<Models.Contact>();

            try
            {
                var contactEntitiesList = await _contactRepository.ListAllAsync();

                result.Items = _mapper.Map<List<Models.Contact>>(contactEntitiesList);
            }
            catch (Exception e)
            {
                result.ErrorMessage = LogError(e, "GetAllContacts");
            }

            return result;
        }

        public async Task<Models.PagedListResult<Models.Contact>> GetPagedContacts(int page = 1, int pageSize = 20)
        {
            var result = new Models.PagedListResult<Models.Contact>(page, pageSize);

            try
            {
                var contactEntitiesPagedList = await _contactRepository.GetPagedListAsync(page: page, pageSize: pageSize);

                var contactsPagedList = new Models.PagedListResult<Models.Contact>(page, pageSize)
                {
                    Items = _mapper.Map<List<Models.Contact>>(contactEntitiesPagedList.Items),
                    Page = contactEntitiesPagedList.Page,
                    PageSize = contactEntitiesPagedList.PageSize,
                    TotalRecords = contactEntitiesPagedList.TotalRecords
                };

                result = contactsPagedList;
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
                    return result;
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
                //TBC: Must add ClaimUser claimUser
                var createdUserId = Guid.NewGuid(); //var createdUserId = claimUser.UserId;

                contactCreate.CreatedUserId = createdUserId;

                var contactEntity = _mapper.Map<Entities.Contact>(contactCreate);

                await _contactRepository.AddAsync(contactEntity);

                contactEntity = await _contactRepository.GetByIdAsync(contactEntity.Id);

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
                //TBC: Must add ClaimUser claimUser
                var updatedUserId = Guid.NewGuid(); //var createdUserId = claimUser.UserId;

                //use GetSingleAsync to avoid tracking
                var contactEntity = await _contactRepository.GetSingleAsync(x => x.Id == contactUpdate.Id);

                if (contactEntity == null)
                {
                    result.Data = false;
                    return result;
                }

                contactUpdate.UpdatedUserId = updatedUserId;

                if (!contactUpdate.MapToEntity(ref contactEntity))
                {
                    result.ErrorMessage = MSG_ERROR_UPDATE_FAILED_NO_CHANGES_DETECTED;
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

        public async Task<Models.Result<bool?>> DeleteContact(Guid contactId)
        {
            var result = new Models.Result<bool?>();

            try
            {
                //TBC: Must add ClaimUser claimUser
                var deletedUserId = Guid.NewGuid();

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
