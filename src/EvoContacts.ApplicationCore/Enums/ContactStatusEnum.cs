using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EvoContacts.ApplicationCore.Enums
{
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ContactStatusEnum
    {
        /// <summary>
        /// Enum ActiveEnum for Active
        /// </summary>
        [EnumMember(Value = "Active")]
        ActiveEnum = 1,

        /// <summary>
        /// Enum InactiveEnum for Inactive
        /// </summary>
        [EnumMember(Value = "Inactive")]
        InactiveEnum = 2
    }
}
