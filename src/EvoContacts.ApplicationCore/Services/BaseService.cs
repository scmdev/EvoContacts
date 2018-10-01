using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace EvoContacts.ApplicationCore.Services
{
    public class BaseService
    {
        protected readonly IConfiguration _configuration;
        private readonly ILogger<BaseService> _logger;

        public BaseService(IConfiguration configuration, ILogger<BaseService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        protected string LogError(Exception e, string message = null, object[] args = null)
        {
            _logger.LogError(e, message, args);

            return ERROR_UNSPECIFIED;
        }

        public const string ERROR_UNSPECIFIED = "An error has occurred.";

        public const string ERROR_GET_CONTACTS_INVALID_PAGE_NUMBER = "Invalid page number requested.";

        public const string ERROR_CREATE_CONTACT_DUPLICATE_EMAIL = "Create contact failed as a contact with this email already exists.";

        public const string ERROR_UPDATE_CONTACT_DUPLICATE_EMAIL = "Update contact failed as another contact with this email already exists.";

        public const string ERROR_UPDATE_CONTACT_NO_CHANGES_DETECTED = "Update contact failed as no changes were detected.";

        public const string ERROR_CREATE_USER_DUPLICATE_USERNAME = "Create user failed as a user with this username already exists.";

        public const string ERROR_UPDATE_USER_DUPLICATE_USERNAME = "Update user failed as another user with this username already exists.";

        public const string ERROR_UPDATE_USER_NO_CHANGES_DETECTED = "Update user failed as no changes were detected.";

    }
}
