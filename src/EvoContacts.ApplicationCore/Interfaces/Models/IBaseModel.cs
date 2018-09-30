using EvoContacts.ApplicationCore.Entities;
using EvoContacts.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoContacts.ApplicationCore.Interfaces
{
    //public interface IBaseGetModel
    //{
    //    Guid? CreatedUserId { get; set; }
    //    DateTimeOffset? CreatedDateTimeOffset { get; set; }

    //    Guid? UpdatedUserId { get; set; }
    //    DateTimeOffset? UpdatedDateTimeOffset { get; set; }

    //    Guid Id { get; set; }
    //}

    public interface IBaseCreateModel
    {
        Guid? CreatedUserId { get; set; }
    }

    public interface IBaseUpdateModel
    {
        Guid? UpdatedUserId { get; set; }

        Guid Id { get; set; }
    }
}
