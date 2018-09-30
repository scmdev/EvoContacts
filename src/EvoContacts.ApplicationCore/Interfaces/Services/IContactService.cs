using EvoContacts.ApplicationCore.Entities;
using EvoContacts.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoContacts.ApplicationCore.Interfaces
{
    public interface IContactService
    {
        #region CONTACT

        Task<Models.PagedListResult<Models.Contact>> GetPagedContacts(int page = 1, int pageSize = 20);
        Task<Models.Result<Models.Contact>> GetContact(Guid contactId);
        Task<Models.Result<Models.Contact>> CreateContact(Models.ContactCreate contactCreateModel);
        Task<Models.Result<bool?>> UpdateContact(Models.ContactUpdate contactUpdate);
        Task<Models.Result<bool?>> DeleteContact(Guid contactId);

        #endregion

    }
}
