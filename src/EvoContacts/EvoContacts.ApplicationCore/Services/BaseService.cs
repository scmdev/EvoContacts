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

            return MSG_ERROR_UNSPECIFIED;
        }

        public const string MSG_ERROR_UNSPECIFIED = "An error has occurred.";

        public const string MSG_ERROR_UPDATE_FAILED_NO_CHANGES_DETECTED = "No changes detected.";

    }
}
