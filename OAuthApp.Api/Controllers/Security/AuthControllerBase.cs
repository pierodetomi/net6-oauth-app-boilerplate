using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using OAuthApp.Data.Entities;
using System.Net;

namespace OAuthApp.Api.Controllers.Security
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [ApiController]
    public abstract class AuthControllerBase : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        private AppUser _loggedInUser;

        public AppUser LoggedInUser
        {
            get
            {
                if (_loggedInUser != null)
                    return _loggedInUser;

                _loggedInUser = _userManager
                    .FindByNameAsync(User.Identity.Name)
                    .GetAwaiter()
                    .GetResult();

                return _loggedInUser;
            }
        }

        public AuthControllerBase(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Try(Func<IActionResult> callback)
        {
            try
            {
                return callback();
            }
            catch (ApplicationException appException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, appException.Message);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An unknown error has occurred");
            }
        }

        public TResult Try<TResult>(Func<TResult> callback)
        {
            try
            {
                return callback();
            }
            catch (ApplicationException appException)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Response.WriteAsync(appException.Message).GetAwaiter().GetResult();
                return default;
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Response.WriteAsync("An unknown error has occurred").GetAwaiter().GetResult();
                return default;
            }
        }
    }
}
