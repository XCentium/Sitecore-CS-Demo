using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSDemo.Models.Search;
using Glass.Mapper.Sc;
using Sitecore.Links;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;

namespace CSDemo.Controllers
{
    public class GeneralSearchController : SitecoreController
    {
        #region Fields

        private readonly ISitecoreService _service;

        #endregion

        #region Constructors

        public GeneralSearchController(ISitecoreService service)
        {
            _service = service;
        }

        public GeneralSearchController() : this(new SitecoreContext()) { }

        #endregion

        #region Methods

        public ActionResult SearchResults()
        {
            return View(new Search());
        }

        public ActionResult SearchInput(SearchInput model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model?.Query)) return View(new SearchInput());
            var searchResultsPagePath = RenderingContext.Current.Rendering.DataSource;
            if(string.IsNullOrWhiteSpace(searchResultsPagePath))return View(new SearchInput());

            var searchResultsPageItem = _service.Database.GetItem(searchResultsPagePath);
            if (searchResultsPageItem == null) return View(new SearchInput());

            var redirectUrl = $"{LinkManager.GetItemUrl(searchResultsPageItem)}?q={Server.UrlEncode(model.Query)}";
            return Redirect(redirectUrl);
        }

        #endregion
    }
}