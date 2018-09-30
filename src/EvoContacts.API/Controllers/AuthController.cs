using ApplicationCore.Models;
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
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserService userService,
            ILogger<AuthController> logger) : base()
        {
            _userService = userService;
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
            string jwtToken = await _userService.GetJwtRequestToken();

            return Ok(jwtToken);
        }

        #region USERS

        /// <summary>
        /// Get a paged List of Users
        /// </summary>

        /// <param name="page">The page number to return.</param>
        /// <param name="pageSize">The maximum number of records to return per page.</param>
        /// <response code="200"></response>
        [HttpGet]
        [Route("/users")]
        [SwaggerOperation("GetUsers")]
        [ProducesResponseType(statusCode: 200, type: typeof(PagedListResult<User>))]
        public async Task<IActionResult> GetUsers([FromQuery]int page = 1, [FromQuery]int pageSize = 20)
        {
            var usersResult = await _userService.GetPagedUsers(page, pageSize);

            if (usersResult.RequestFailed)
            {
                return BadRequest(usersResult.ErrorMessage);
            }

            return Ok(usersResult);
        }

        /// <summary>
        /// Get a specific User
        /// </summary>

        /// <param name="userId"></param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="404"></response>
        [HttpGet]
        [Route("/users/{userId}")]
        [SwaggerOperation("GetUser")]
        [ProducesResponseType(statusCode: 200, type: typeof(Result<User>))]
        public async Task<IActionResult> GetUser([FromRoute]Guid userId)
        {
            var userResult = await _userService.GetUser(userId);

            if (userResult.RequestFailed)
            {
                return BadRequest(userResult.ErrorMessage);
            }
            else if (userResult.Data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(userResult);
            }
        }

        /// <summary>
        /// Creates a new User
        /// </summary>

        /// <param name="userCreate"></param>
        /// <response code="201"></response>
        /// <response code="400"></response>
        /// <response code="401"></response>
        [HttpPost]
        [Route("/users")]
        [SwaggerOperation("CreateUser")]
        [ProducesResponseType(statusCode: 201, type: typeof(Result<User>))]
        public async Task<IActionResult> CreateUser([FromBody]UserCreate userCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userResult = await _userService.CreateUser(userCreate);

            if (userResult.RequestFailed)
            {
                return BadRequest(userResult.ErrorMessage);
            }
            else if (userResult.Data == null)
            {
                return NotFound();
            }
            else
            {
                var userId = userResult.Data.Id;

                return Created(string.Format($"/users/{userId}"), userResult.Data);
            }
        }

        /// <summary>
        /// Update a User
        /// </summary>

        /// <param name="userId"></param>
        /// <param name="userUpdate"></param>
        /// <response code="200"></response>
        /// <response code="400"></response>
        /// <response code="401"></response>
        /// <response code="404"></response>
        [HttpPut]
        [Route("/users/{userId}")]
        [SwaggerOperation("UpdateUser")]
        [ProducesResponseType(statusCode: 200)]
        public async Task<IActionResult> UpdateUser([FromRoute]Guid userId, [FromBody]UserUpdate userUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userUpdate.Id = userId;

            var updateResult = await _userService.UpdateUser(userUpdate);

            if (updateResult.RequestFailed)
            {
                return BadRequest(updateResult.ErrorMessage);
            }
            else if (updateResult.Data.Value)
            {
                return Ok();
            }
            else //if (!deleteResult.Data.Value)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete User
        /// </summary>

        /// <param name="userId"></param>
        /// <response code="204"></response>
        /// <response code="401"></response>
        /// <response code="404"></response>
        [HttpDelete]
        [Route("/users/{userId}")]
        [SwaggerOperation("DeleteUser")]
        [ProducesResponseType(statusCode: 204)]
        public async Task<IActionResult> DeleteUser([FromRoute]Guid userId)
        {
            var deleteResult = await _userService.DeleteUser(userId);

            if (deleteResult.RequestFailed)
            {
                return BadRequest(deleteResult.ErrorMessage);
            }
            else if (deleteResult.Data.Value)
            {
                return NoContent();
            }
            else //if (!deleteResult.Data.Value)
            {
                return NotFound();
            }
        }

        #endregion

    }
}
