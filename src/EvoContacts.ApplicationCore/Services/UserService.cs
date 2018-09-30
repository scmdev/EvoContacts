using ApplicationCore.Models;
using AutoMapper;
using EvoContacts.ApplicationCore.Extensions;
using EvoContacts.ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EvoContacts.ApplicationCore.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(
            IConfiguration configuration,
            ILogger<UserService> logger,
            IMapper mapper,
            IUserRepository userRepository
            ) : base(configuration, logger)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<string> GetJwtRequestToken()
        {
            var claimUser = new ClaimUser()
            {
                UserId = Guid.NewGuid(),
                FirstName = "Evo",
                LastName = "Developer",
                Username = "dev@evocontacts.com"
            };

            var createdDateTimeOffset = DateTimeOffset.UtcNow;

            var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Sub, claimUser.UserId.ToString()),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat, ConvertUtcDateToJsonString(createdDateTimeOffset), ClaimValueTypes.Integer64),
                 new Claim("ClaimUser", claimUser.ToJson())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var signingCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: createdDateTimeOffset.AddMinutes(30).UtcDateTime,
                signingCredentials: signingCreds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string ConvertUtcDateToJsonString(DateTimeOffset dateTimeOffset)
        {
            DateTime centuryBegin = new DateTime(1970, 1, 1);
            return Convert.ToInt64(new TimeSpan(dateTimeOffset.Ticks - centuryBegin.Ticks).TotalSeconds).ToString();
        }

        #region USER

        public async Task<Models.PagedListResult<Models.User>> GetPagedUsers(int page = 1, int pageSize = 20)
        {
            var result = new Models.PagedListResult<Models.User>(page, pageSize);

            try
            {
                var userEntitiesPagedList = await _userRepository.GetPagedListAsync(page: page, pageSize: pageSize);

                //Map Models.PagedListResult<Entities.User> to Models.PagedListResult<Models.User>
                result = new Models.PagedListResult<Models.User>(page, pageSize)
                {
                    Items = _mapper.Map<List<Models.User>>(userEntitiesPagedList.Items),
                    Page = userEntitiesPagedList.Page,
                    PageSize = userEntitiesPagedList.PageSize,
                    TotalRecords = userEntitiesPagedList.TotalRecords
                };
            }
            catch (Exception e)
            {
                result.ErrorMessage = LogError(e, "GetPagedUsers", new object[] { page, pageSize });
            }

            return result;
        }

        public async Task<Models.Result<Models.User>> GetUser(Guid userId)
        {
            var result = new Models.Result<Models.User>();

            try
            {
                var userEntity = await _userRepository.GetByIdAsync(userId);

                if (userEntity == null)
                {
                    return result; //result.Data = null => controller will return 404
                }

                result.Data = _mapper.Map<Models.User>(userEntity);
            }
            catch (Exception e)
            {
                result.ErrorMessage = LogError(e, "GetUser", new object[] { userId });
            }

            return result;
        }

        public async Task<Models.Result<Models.User>> CreateUser(Models.UserCreate userCreate)
        {
            var result = new Models.Result<Models.User>();

            try
            {
                //TBC: Must add ClaimUser claimUser
                var createdUserId = Guid.NewGuid(); //var createdUserId = claimUser.UserId;

                //check User with same Username does not already exist
                var checkUsernameEntity = await _userRepository.GetSingleAsync(x => x.Username == userCreate.Username);

                if (checkUsernameEntity != null)
                {
                    result.ErrorMessage = ERROR_CREATE_USER_DUPLICATE_USERNAME;
                    return result;
                }

                userCreate.CreatedUserId = createdUserId;

                //Create new userEntity by mapping userCreate to Entities.User
                var userEntity = _mapper.Map<Entities.User>(userCreate);

                //Add userEntity
                await _userRepository.AddAsync(userEntity);

                result.Data = _mapper.Map<Models.User>(userEntity);
            }
            catch (Exception e)
            {
                result.ErrorMessage = LogError(e, "CreateUser", new object[] { userCreate });
            }

            return result;
        }

        public async Task<Models.Result<bool?>> UpdateUser(Models.UserUpdate userUpdate)
        {
            var result = new Models.Result<bool?>();

            try
            {
                //TBC: Must add ClaimUser claimUser
                var updatedUserId = Guid.NewGuid(); //var createdUserId = claimUser.UserId;

                //check User with same Username does not already exist
                var checkUsernameEntity = await _userRepository.GetSingleAsync(x => x.Username == userUpdate.Username);

                if (checkUsernameEntity != null && checkUsernameEntity.Id != userUpdate.Id)
                {
                    result.ErrorMessage = ERROR_UPDATE_USER_DUPLICATE_USERNAME;
                    return result;
                }

                //Get userEntity using GetSingleAsync to avoid tracking
                var userEntity = await _userRepository.GetSingleAsync(x => x.Id == userUpdate.Id);

                if (userEntity == null)
                {
                    result.Data = false; //result.Data = false => controller will return 404
                    return result;
                }

                userUpdate.UpdatedUserId = updatedUserId;

                //Check/Update userEntity properties as per userUpdate provided
                if (!userUpdate.MapToEntity(ref userEntity))
                {
                    result.ErrorMessage = ERROR_UPDATE_USER_NO_CHANGES_DETECTED;
                    return result;
                }

                result.Data = await _userRepository.UpdateAsync(userEntity);
            }
            catch (Exception e)
            {
                result.ErrorMessage = LogError(e, "UpdateUser", new object[] { userUpdate });
            }

            return result;
        }

        public async Task<Models.Result<bool?>> DeleteUser(Guid userId)
        {
            var result = new Models.Result<bool?>();

            try
            {
                //TBC: Must add ClaimUser claimUser
                var deletedUserId = Guid.NewGuid();

                result.Data = await _userRepository.DeleteAsync(userId, deletedUserId);
            }
            catch (Exception e)
            {
                result.ErrorMessage = LogError(e, "DeleteUser", new object[] { userId });
            }

            return result;
        }

        #endregion

    }
}
