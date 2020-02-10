using Microsoft.AspNetCore.Mvc;
using News.DAL;

namespace News.Controllers
{
    public class CookieController : Controller
    {
        [HttpPost]
        public int CheckCookie()
        {
            int userId;
            string sessionid = HttpContext.Request.Cookies["session_id"];
            if (sessionid != null)
            {
                CookiesContext cookieContext =
                    HttpContext.RequestServices.GetService(typeof(CookiesContext)) as CookiesContext;
                userId = cookieContext.GetSession(sessionid);
            }
            else
            {
                userId = -1;
            }

            return userId;
        }

        [HttpPost]
        public void RemoveCookie()
        {
            string sessionid = HttpContext.Request.Cookies["session_id"];

            CookiesContext cookieContext =
                HttpContext.RequestServices.GetService(typeof(CookiesContext)) as CookiesContext;
            cookieContext.DeleteSession(sessionid);
        }
        
        
    }
}