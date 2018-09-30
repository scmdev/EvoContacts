using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace EvoContacts.API.Controllers
{
    public class BaseController : Controller
    {
        internal ClaimUser _claimUser
        {
            get
            {
                if (!_isAccessToken)
                    return null;

                return JsonConvert.DeserializeObject<ClaimUser>(Request.HttpContext.User.Claims.First(x => x.Type == "ClaimUser")?.Value.ToString() ?? "");
            }
        }

        private bool _isAccessToken
        {
            get
            {
                return JsonConvert.DeserializeObject<bool>(Request.HttpContext.User.Claims.First(x => x.Type == "IsAccessToken")?.Value.ToString() ?? "false");
            }
        }
    }
}
