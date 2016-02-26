#region

using System.Web.Mvc;

#endregion

namespace CSDemo.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
    }
}