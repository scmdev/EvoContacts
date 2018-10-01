using EvoContacts.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EvoContacts.ApplicationCore.Entities
{
    public class Contact : BaseEntityDeletable
    {
        public Contact() : base()
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
        public string Email { get; set; }

        [StringLength(100)]
        public string PhoneNumber { get; set; }

        [Required]
        public ContactStatusEnum ContactStatus { get; set; }

    }
}
