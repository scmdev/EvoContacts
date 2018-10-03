using EvoContacts.ApplicationCore.Interfaces;
using EvoContacts.ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EvoContacts.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger) : base()
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Requests a JWT Authorization Token
        /// </summary>

        /// <response code="201"></response>
        /// <response code="400"></response>
        /// <response code="401"></response>
        [HttpPost]
        [Route("/token")]
        [SwaggerOperation("RequestToken")]
        [ProducesResponseType(statusCode: 201, type: typeof(string))]
        public async Task<IActionResult> RequestToken()
        {
            string jwtToken = await _authService.GetJwtRequestToken();

            return Ok(jwtToken);
        }

    }
}
