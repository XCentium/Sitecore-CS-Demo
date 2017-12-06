#region

using Sitecore;
using Sitecore.Mvc.Controllers;
using System.Web.Mvc;
using CSDemo.Models.Checkout.Cart;
using System;
using System.Linq;
using Glass.Mapper.Sc;
using CSDemo.Models.Product;

#endregion

namespace CSDemo.Controllers
{
	public class CheckoutController : SitecoreController
	{
		// GET: Checkout
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Reviewx()
		{
			return View();
		}

		public ActionResult Reviewy()
		{
			return View();
		}

		public ActionResult AddToCart([CanBeNull] CartItem model)
		{

			return View();
		}

		public ActionResult Cart()
		{
			return View();
		}

		[HttpPost]
		public ActionResult ReorderItems(string orderId)
		{
			var cartHelper = new CartHelper();

			var existingOrder = cartHelper.GetOrderHead(orderId, cartHelper.GetVisitorId(), cartHelper.ShopName);
			var itemCount = existingOrder.Order.Lines.Count;
			foreach (Sitecore.Commerce.Entities.Carts.CartLine item in existingOrder.Order.Lines)
			{
				Sitecore.Commerce.Connect.CommerceServer.Orders.Models.CommerceCartProduct product = item.Product as Sitecore.Commerce.Connect.CommerceServer.Orders.Models.CommerceCartProduct;
				cartHelper.AddProductToCart(item.Quantity.ToString(), item.Product.ProductId, product.ProductCatalog, product.ProductVariantId);

				//var a = item.Product;
				//var b = item.Product.SitecoreProductItemId;
				//var c = Sitecore.Context.Database.GetItem(b);

				//var d = c.GetChildren();
				//if (d.Any())
				//{
				//	var variant = d.First().GlassCast<ProductVariant>();
				//	cartHelper.AddProductToCart(item.Quantity.ToString(), item.Product.ProductId, "", variant.VariantId);
				//}
				//else
				//{
				//	var parent = Sitecore.Context.Database.GetItem(c.ParentID);

				//	cartHelper.AddProductToCart(item.Quantity.ToString(), item.Product.ProductId, "", item.Product.ProductId);
				//}
			}

			return Redirect("/Cart");
		}

		public ActionResult Address()
		{
			return View();
		}

		public ActionResult Shipping()
		{
			return View();
		}

		public ActionResult Payment()
		{
			return View();
		}

		public ActionResult Review()
		{
			return View();
		}

		#region Testing

		public ActionResult ReviewNoHeaderFooter()
		{
			return View();
		}

		#endregion
	}
}