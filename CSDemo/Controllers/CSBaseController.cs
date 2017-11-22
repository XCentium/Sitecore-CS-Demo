using Sitecore.Commerce.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Controllers
{
    public class CSBaseController //: BaseController
    {
        //public CSBaseController(Sitecore.Reference.Storefront.Managers.AccountManager accountManager, ContactFactory contactFactory) : base(contactFactory)
        //{
        //    Assert.ArgumentNotNull(accountManager, "accountManager");
        //    this.AccountManager = accountManager;
        //}

        //public Sitecore.Reference.Storefront.Managers.AccountManager AccountManager { get; set; }

        //public override VisitorContext CurrentVisitorContext
        //{
        //    get
        //    {
        //        ISiteContext currentSiteContext = base.CurrentSiteContext;
        //        VisitorContext context2 = currentSiteContext.Items["__visitorContext"] as VisitorContext;
        //        if (context2 == null)
        //        {
        //            context2 = new VisitorContext(base.ContactFactory);
        //            if (Context.User.IsAuthenticated && !Context.User.Profile.IsAdministrator)
        //            {
        //                context2.SetCommerceUser(this.AccountManager.ResolveCommerceUser().Result);
        //            }
        //            currentSiteContext.Items["__visitorContext"] = context2;
        //        }
        //        return context2;
        //    }
        //}
    }
}