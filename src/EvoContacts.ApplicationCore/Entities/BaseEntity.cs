using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EvoContacts.ApplicationCore.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedDateTimeOffset = DateTimeOffset.UtcNow;
        }

        [Key]
        public Guid Id { get; set; }

        public Guid? CreatedUserId { get; set; }
        public DateTimeOffset CreatedDateTimeOffset { get; set; }
    }

    public class BaseEntityUpdatable : BaseEntity
    {
        public Guid? UpdatedUserId { get; set; }
        public DateTimeOffset? UpdatedDateTimeOffset { get; set; }
    }

    public class BaseEntityDeletable : BaseEntityUpdatable
    {
        public BaseEntityDeletable() : base()
        {
            IsDeleted = false;
        }

        public bool IsDeleted { get; set; }
    }
}
