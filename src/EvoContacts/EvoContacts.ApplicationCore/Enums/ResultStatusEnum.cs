using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EvoContacts.ApplicationCore.Enums
{
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ResultStatusEnum
    {
        /// <summary>
        /// Enum SuccessfulEnum for Successful
        /// </summary>
        [EnumMember(Value = "Successful")]
        SuccessfulEnum = 0,

        /// <summary>
        /// Enum FailedEnum for Failed
        /// </summary>
        [EnumMember(Value = "Failed")]
        FailedEnum = 1
    }
}
