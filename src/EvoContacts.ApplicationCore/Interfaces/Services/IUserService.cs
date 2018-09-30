using EvoContacts.ApplicationCore.Entities;
using EvoContacts.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoContacts.ApplicationCore.Interfaces
{
    public interface IUserService
    {
        Task<string> GetJwtRequestToken();

        #region USER

        Task<Models.PagedListResult<Models.User>> GetPagedUsers(int page = 1, int pageSize = 20);
        Task<Models.Result<Models.User>> GetUser(Guid userId);
        Task<Models.Result<Models.User>> CreateUser(Models.UserCreate userCreateModel);
        Task<Models.Result<bool?>> UpdateUser(Models.UserUpdate userUpdate);
        Task<Models.Result<bool?>> DeleteUser(Guid userId);

        #endregion

    }
}
