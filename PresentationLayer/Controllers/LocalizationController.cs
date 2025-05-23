using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace RentingCars.Controllers;

public class LocalizationController:Controller
{
    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl) 
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );
        return LocalRedirect(returnUrl);// returnUrl >> to make user stay
        // in same page when he change lang
    }
}