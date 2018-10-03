using EvoContacts.ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace EvoContacts.API.Controllers
{
    public class BaseController : Controller
    {
        protected ClaimsUser _claimsUser
        {
            get
            {
                ClaimsUser claimsUser = null;

                var claim = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "ClaimsUser");

                if (claim != null)
                {
                    claimsUser = JsonConvert.DeserializeObject<ClaimsUser>(claim.Value.ToString());
                }

                return claimsUser;
            }
        }
    }
}
