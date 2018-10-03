using AutoMapper;
using EvoContacts.ApplicationCore.Interfaces;
using EvoContacts.ApplicationCore.Models;
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
    public class AuthService : BaseService, IAuthService
    {
        private readonly IMapper _mapper;

        public AuthService(
            IConfiguration configuration,
            ILogger<AuthService> logger,
            IMapper mapper
            ) : base(configuration, logger)
        {
            _mapper = mapper;
        }

        public async Task<string> GetJwtRequestToken()
        {
            //TBC: Stubbed claimsUser must be replaced with Login validation
            var claimsUser = new ClaimsUser()
            {
                UserId = Guid.NewGuid(),
                FirstName = "Evo",
                LastName = "Developer",
                Username = "dev@evocontacts.com"
            };

            var createdDateTimeOffset = DateTimeOffset.UtcNow;

            var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Sub, claimsUser.UserId.ToString()),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat, ConvertUtcDateToJsonString(createdDateTimeOffset), ClaimValueTypes.Integer64),
                 new Claim("ClaimsUser", claimsUser.ToJson())
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

    }
}
