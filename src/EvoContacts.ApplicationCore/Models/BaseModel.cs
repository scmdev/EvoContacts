using EvoContacts.ApplicationCore.Interfaces;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace EvoContacts.ApplicationCore.Models
{
    [DataContract]
    public class BaseGetModel
    {
        /// <summary>
        /// Gets or Sets CreatedUserId
        /// </summary>
        [DataMember(Name = "CreatedUserId")]
        public Guid? CreatedUserId { get; set; }

        /// <summary>
        /// Gets or Sets CreatedDateTimeOffset
        /// </summary>
        [DataMember(Name = "CreatedDateTimeOffset")]
        public DateTimeOffset? CreatedDateTimeOffset { get; set; }

        /// <summary>
        /// Gets or Sets UpdatedUserId
        /// </summary>
        [DataMember(Name = "UpdatedUserId")]
        public Guid? UpdatedUserId { get; set; }

        /// <summary>
        /// Gets or Sets UpdatedDateTimeOffset
        /// </summary>
        [DataMember(Name = "UpdatedDateTimeOffset")]
        public DateTimeOffset? UpdatedDateTimeOffset { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "Id")]
        public Guid Id { get; set; }
    }

    public class BaseCreateModel
    {
        [JsonIgnore]
        public Guid? CreatedUserId { get; set; }
    }

    public class BaseUpdateModel
    {
        [JsonIgnore]
        public Guid? UpdatedUserId { get; set; }

        /// <summary>
        /// From Route
        /// </summary>
        [JsonIgnore]
        public Guid Id { get; set; }
    }

}
