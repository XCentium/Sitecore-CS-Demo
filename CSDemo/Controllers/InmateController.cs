#region

using Sitecore;
using Sitecore.Mvc.Controllers;
using System.Web.Mvc;
using CSDemo.Models.Checkout.Cart;
using CSDemo.Models.Inmate;

#endregion

namespace CSDemo.Controllers
{
    public class InmateController : SitecoreController
    {
        [HttpGet]
        public ActionResult InmateSelector()
        {
            return View(new InmateSelector());
        }


        [HttpPost]
        public ActionResult InmateSelector(InmateSelector model)
        {
            if (!ModelState.IsValid) return Redirect("/");

            switch (model.SelectedPrisonId)
            {
                case "20021":
                    return Redirect(string.Format("http://keefeca.xcentium.net?pid={0}&iid={1}",
                        model.SelectedPrisonId, model.InmateId));
                case "20114":
                    return Redirect(string.Format("http://keefeoh.xcentium.net?pid={0}&iid={1}",
                        model.SelectedPrisonId,  model.InmateId));
                default:
                    return Redirect(string.Format("http://keefeca.xcentium.net?pid={0}&iid={1}", 
                        model.SelectedPrisonId, model.InmateId));
            }
        }
    }
}