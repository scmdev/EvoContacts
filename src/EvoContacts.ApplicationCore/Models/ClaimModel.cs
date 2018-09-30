using Newtonsoft.Json;
using System;

namespace ApplicationCore.Models
{
    public class ClaimUser
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
