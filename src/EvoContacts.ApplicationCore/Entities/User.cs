using EvoContacts.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EvoContacts.ApplicationCore.Entities
{
    public class User : BaseEntityDeletable
    {
        public User() : base()
        {

        }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }


    }
}
