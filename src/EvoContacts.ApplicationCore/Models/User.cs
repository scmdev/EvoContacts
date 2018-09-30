using EvoContacts.ApplicationCore.Enums;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EvoContacts.ApplicationCore.Models
{
    [DataContract]
    public partial class User : BaseGetModel, IEquatable<User>
    {
        /// <summary>
        /// Gets or Sets FirstName
        /// </summary>
        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets LastName
        /// </summary>
        [DataMember(Name = "LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or Sets Username
        /// </summary>
        [DataMember(Name = "Username")]
        public string Username { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class User {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Username: ").Append(Username).Append("\n");
            sb.Append("  FirstName: ").Append(FirstName).Append("\n");
            sb.Append("  LastName: ").Append(LastName).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((User)obj);
        }

        /// <summary>
        /// Returns true if User instances are equal
        /// </summary>
        /// <param name="other">Instance of User to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) &&
                (
                    Username == other.Username ||
                    Username != null &&
                    Username.Equals(other.Username)
                ) &&
                (
                    FirstName == other.FirstName ||
                    FirstName != null &&
                    FirstName.Equals(other.FirstName)
                ) &&
                (
                    LastName == other.LastName ||
                    LastName != null &&
                    LastName.Equals(other.LastName)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                if (Id != null)
                {
                    hashCode = hashCode * 59 + Id.GetHashCode();
                }

                if (Username != null)
                {
                    hashCode = hashCode * 59 + Username.GetHashCode();
                }

                if (FirstName != null)
                {
                    hashCode = hashCode * 59 + FirstName.GetHashCode();
                }

                if (LastName != null)
                {
                    hashCode = hashCode * 59 + LastName.GetHashCode();
                }

                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(User left, User right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(User left, User right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }

    [DataContract]
    public partial class UserCreate : BaseCreateModel
    {
        /// <summary>
        /// Gets or Sets FirstName
        /// </summary>
        [Required]
        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets LastName
        /// </summary>
        [Required]
        [DataMember(Name = "LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or Sets Username
        /// </summary>
        [Required]
        [DataMember(Name = "Username")]
        public string Username { get; set; }
    }

    [DataContract]
    public partial class UserUpdate : BaseUpdateModel
    {
        /// <summary>
        /// Gets or Sets FirstName
        /// </summary>
        [Required]
        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets LastName
        /// </summary>
        [Required]
        [DataMember(Name = "LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or Sets Username
        /// </summary>
        [Required]
        [DataMember(Name = "Username")]
        public string Username { get; set; }
    }
}
