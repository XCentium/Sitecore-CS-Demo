#region

using System;
using System.Collections.Generic;
using System.Linq;
using CSDemo.Configuration;
using CSDemo.Helpers;
using CSDemo.Models.Account;
using CSDemo.Models.Checkout.Cart;
using CSDemo.Models.GeneralSearch;
using CSDemo.Models.Page;
using Glass.Mapper.Sc;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Analytics.Data;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
using Sitecore.Commerce.Entities.Carts;
using Sitecore.Commerce.Services.Payments;
using Sitecore.Commerce.Services.Prices;
using Sitecore.Commerce.Services.Shipping;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Convert = System.Convert;
using Log = Sitecore.Diagnostics.Log;
using Sitecore.Collections;
using Sitecore.Commerce.Connect.CommerceServer;
using Sitecore.Commerce.Connect.CommerceServer.Search;
using Sitecore.Commerce.Connect.CommerceServer.Search.Models;
using Sitecore.ContentSearch.Linq;

#endregion

namespace CSDemo.Models.Product
{
	public static class ProductHelper
	{
		/// <summary>
		/// Profile Product
		/// </summary>
		/// <param name="model"></param>
		/// <param name="context"></param>
		public static void ProfileProduct(this Product model, ISitecoreContext context)
		{
			var productItem = context.Database.GetItem(new ID(model.ID));

			if (productItem == null) return;

			var trackingField = new TrackingField(productItem.Fields[Constants.Products.TrackingFieldId]);

			// Process the tracking profiles associated with this product
			TrackingFieldProcessor.ProcessProfiles(Tracker.Current.Interaction, trackingField);

			// Finally score the profile and update this visitor's pattern
			var trackerProfiles = Tracker.Current.Interaction.Profiles;

			if (trackingField.Profiles != null && trackingField.Profiles.Any() && trackerProfiles != null)
			{
				var profiles = trackingField.Profiles.Where(t => t.IsSavedInField).ToList();
				foreach (var profile in profiles)
				{
					if (!trackerProfiles.ContainsProfile(profile.Name)) continue;
					var profileKeys = profile.Keys;
					if (profileKeys == null) continue;
					var scoreProfile = trackerProfiles[profile.Name];
					scoreProfile.Score(profile);
					scoreProfile.UpdatePattern();
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="product"></param>
		/// <returns></returns>
		public static string GetFirstImageFromProductItem(Item product)
		{
			var productImage = string.Empty;
			if (product != null)
			{
				var productImageIds = product[Constants.Products.ImagesField];
				if (!string.IsNullOrEmpty(productImageIds))
				{
					productImage = GetFirstImageFromField(productImageIds);
				}
			}
			return productImage;
		}

		/// <summary>
		/// </summary>
		/// <param name="imageIds"></param>
		/// <returns></returns>
		public static string GetFirstImageFromField(string imageIds)
		{
			var image = string.Empty;
			if (!string.IsNullOrEmpty(imageIds))
			{
				var images = imageIds.Split(Constants.Products.IdSeparator.ToCharArray());
				if (!string.IsNullOrEmpty(images[0]))
				{
					image = string.Format(Constants.Products.ImagesUrlFormat, ID.Parse(images[0]).ToShortID());
				}
			}
			return image;
		}

		/// <summary>
		/// Function to get all catalog categories
		/// </summary>
		/// <returns>List of Categories</returns>
		public static List<Category> GetCategories()
		{
			try
			{
				const string templateName = "GeneralCategory";
				return GetSearchResultItemsByTemplate(templateName).Select(c => c.Document.GetItem().GlassCast<Category>()).ToList();
			}
			catch (Exception ex)
			{
				Log.Error(ex.StackTrace, ex);
			}

			return new List<Category>();
		}

		/// <summary>
		/// </summary>
		/// <param name="categoryIDs"></param>
		/// <returns></returns>
		public static List<Category> GetCategories(string categoryIDs)
		{
			var categories = new List<Category>();
			if (!string.IsNullOrEmpty(categoryIDs))
			{
				try
				{
					foreach (var categoryId in categoryIDs.Split(Constants.Products.IdSeparator.ToCharArray()))
					{
						var searchedItem = GetSearchResultItemById(categoryId);
						var category = searchedItem.GetItem().GlassCast<Category>(); //todo: refactor

						categories.Add(category);
					}
				}
				catch (Exception ex)
				{
					Log.Error(ex.StackTrace, ex);
				}
			}
			return categories;
		}

		/// <summary>
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public static SearchResultItem GetSearchResultItemById(string itemId)
		{
			var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetSearchIndex());
			try
			{
				var culture = Context.Language.CultureInfo;
				using (var context = index.CreateSearchContext())
				{
					var queryable = context.GetQueryable<SearchResultItem>()
						.Where(x => x.Language == Context.Language.Name);
					return queryable.FirstOrDefault(x => x.ItemId == ID.Parse(itemId));
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.StackTrace, ex);
			}
			return null;
		}

		public static IEnumerable<SearchHit<SearchResultItem>> GetSearchResultItemsByTemplate(string templateName)
		{
			var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetSearchIndex());
			try
			{
				using (var context = index.CreateSearchContext())
				{
					return context.GetQueryable<SearchResultItem>()
						.Where(x => x.Language == Context.Language.Name &&
						x["_latestversion"] == "1" &&
						x.Path.Contains("/sitecore/commerce/catalog") &&
						x.TemplateName == templateName &&
						string.Equals(x.TemplateName, templateName, StringComparison.CurrentCultureIgnoreCase)
						).GetResults().ToList();
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.StackTrace, ex);
			}
			return null;
		}

		/// <summary>
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		internal static CategoryProductViewModel GetCategoryProducts(PaginationViewModel model, bool includeVariants = true, string mainCategoryId = "")
		{
			var categoryProductVm = new CategoryProductViewModel { PaginationViewModel = model };
			var category = new Category();
			if (mainCategoryId == "")
				categoryProductVm.CategoryMenulist = GetCategoryMenuList(model.CategoryId);
			else
				categoryProductVm.CategoryMenulist = GetCategoryMenuList(mainCategoryId);

			var searchedItem = GetSearchResultItemById(model.CategoryId);
			if (searchedItem != null)
			{
				var catItem = searchedItem.GetItem();
				category = GlassHelper.Cast<Category>(catItem);

				if (catItem.HasChildren)
				{
					var childrenList = new List<Product>();

					foreach (Item subcat in catItem.GetChildren())
					{
						if (subcat.TemplateName == "GeneralCategory")
						{
							foreach (Item prod in subcat.GetChildren())
							{
								if (prod.TemplateName == "GeneralCategory")
								{
									foreach (Item subProd in prod.GetChildren())
									{
										var productItem = subProd.GlassCast<Product>();
										childrenList.Add(productItem);

										var variants = subProd.GetChildren();
									}
								}
								else
								{
									var productItem = prod.GlassCast<Product>();
									childrenList.Add(productItem);
								}
							}
						}
						else
						{

							var productItem = subcat.GlassCast<Product>();
							childrenList.Add(productItem);

						}

					}
					//var catChildren = catItem.GetChildren().Select(x => x.GlassCast<Product>()).ToList(); //todo: refactor

					var catChildren = childrenList;
					catChildren = ProductHelper.FilterProductsByRestrictions(catChildren);

					model.TotalItems = catChildren.Count();
					model.TotalPages = (long)Math.Ceiling((double)model.TotalItems / model.PageSize);

					// do sorting
					if (model.OrderBy != null)
					{
						switch (model.OrderBy)
						{
							case Constants.Products.OrderByBrand:
								catChildren = catChildren.OrderBy(x => x.Brand).ToList();
								break;
							case Constants.Products.OrderByNewness:
								catChildren = catChildren.OrderByDescending(x => x.DateOfIntroduction).ToList();
								break;
							case Constants.Products.OrderByRatings:
								catChildren = catChildren.OrderByDescending(x => x.Rating).ToList();
								break;
							case Constants.Products.OrderByPriceAsc:
								catChildren = catChildren.OrderBy(x => x.ListPrice).ToList();
								break;
							case Constants.Products.OrderByPriceDesc:
								catChildren = catChildren.OrderByDescending(x => x.ListPrice).ToList();
								break;
							default:
								catChildren = catChildren.OrderBy(x => x.SortId).ThenBy(x => x.Title).ToList();
								break;
						}
					}

					// do paging
					category.Products = catChildren
						.Skip(model.PageSize * (model.CurrentPage - 1))
						.Take(model.PageSize);

					if (includeVariants)
					{
						// Process ProductVariants
						foreach (var product in category.Products)
						{

							var catProdItem = catItem.GetChildren().FirstOrDefault(x => x.ID.ToGuid() == product.ID);
							if (catProdItem != null && catProdItem.HasChildren)
							{
								// Update ProductVariants
								product.ProductVariants =
									catProdItem.GetChildren().Select(x => x.GlassCast<ProductVariant>()); //todo: refactor

								BuildUiVariants(product);
							}

							//var stockInfo = cartHelper.GetProductStockInformation(product.ProductId, product.CatalogName);
						}
					}
				}
			}
			categoryProductVm.Category = category;
			categoryProductVm.ParentCategory = category.ParentCategories;
			return categoryProductVm;
		}

		/// <summary>
		/// </summary>
		/// <param name="product"></param>
		internal static void BuildUiVariants(Product product)
		{
			var cultureInfo = Context.Culture;
			var any = false;

			foreach (var variant in product.ProductVariants)
			{
				any = true;
				break;
			}

			if (product.ProductVariants != null || any)
			{
				var variantBox = new VariantBox();
				var variantBoxLines = new List<VariantBoxLine>();
				if (product.ProductVariants != null)
					foreach (var productVariant in product.ProductVariants)
					{
						var variantBoxLine = new VariantBoxLine
						{
							VariantID = productVariant.VariantId,
							Size = !string.IsNullOrEmpty(productVariant.ProductSize)
								? productVariant.ProductSize.Trim()
								: string.Empty,
							Color = !string.IsNullOrEmpty(productVariant.ProductColor)
								? productVariant.ProductColor.Trim()
								: string.Empty,
							Price =
								!string.IsNullOrEmpty(productVariant.ListPrice)
									? decimal.Parse(productVariant.ListPrice)
										.ToString(Constants.Products.CurrencyDecimalFormat, cultureInfo)
									: "0.00"
						};

						variantBoxLines.Add(variantBoxLine);
					}
				variantBoxLines.OrderBy(s => s.Size).ThenBy(c => c.Color);
				variantBox.VariantBoxLines = variantBoxLines;
				product.VariantBox = variantBox;
				var variantSize = new VariantSize();
				var variantColors = new List<VariantColor>();
				// get size
				var availSizes =
					variantBoxLines.Where(s => s.Size != Constants.Common.Empty)
						.GroupBy(s => s.Size)
						.Select(g => g.First())
						.OrderBy(s => s.Size)
						.ThenBy(c => c.Color)
						.ToList();
				if (availSizes.Any())
				{
					var variantSizeLines = new List<VariantSizeLine>();
					var pos = 0;
					foreach (var s in availSizes)
					{
						var variantSizeLine = new VariantSizeLine();
						variantSizeLine.Size = s.Size;
						variantSizeLine.Value = string.Format(Constants.Products.VariantColorLineFormat, s.VariantID,
							s.Price, s.Images);
						variantSizeLines.Add(variantSizeLine);
						// set default variant
						if (pos < 1)
						{
							product.DefaultVariant = s.VariantID;
						}
						// build the colors for the current size
						var availColors =
							variantBoxLines.Where(t => t.Size.Equals(s.Size) && s.Color != Constants.Common.Empty)
								.GroupBy(c => c.Color)
								.Select(g => g.First())
								.OrderBy(c => c.Color)
								.ToList();
						if (availColors.Any())
						{
							var variantColor = new VariantColor();
							variantColor.Name = string.Format(Constants.Products.VariantColorNameFormat, s.Size,
								Constants.Products.VariantColorName);
							variantColor.Display = pos == 0
								? Constants.Products.VariantColorDisplay
								: Constants.Products.VariantColorDisplayNone;
							var variantColorLines = new List<VariantColorLine>();
							foreach (var c in availColors)
							{
								var variantColorLine = new VariantColorLine();
								variantColorLine.Color = c.Color;
								variantColorLine.Value = string.Format(Constants.Products.VariantColorLineFormat,
									c.VariantID, c.Price, c.Images);
								variantColorLines.Add(variantColorLine);
							}
							variantColor.VariantColorLines = variantColorLines;
							variantColors.Add(variantColor);
						}
						pos++;
					}
					variantSize.VariantSizeLines = variantSizeLines;
					product.VariantSize = variantSize;
					product.VariantColors = variantColors;
				}
				else
				{
					// No sizes, let us focus on color
					var availColors =
						variantBoxLines.Where(c => c.Color != Constants.Common.Empty)
							.GroupBy(c => c.Color)
							.Select(g => g.First())
							.OrderBy(c => c.Color)
							.ToList();
					if (availColors.Any())
					{
						var pos = 0;
						var variantColor = new VariantColor();
						variantColor.Name = Constants.Products.VariantColorName;
						variantColor.Display = Constants.Products.VariantColorDisplay;
						var variantColorLines = new List<VariantColorLine>();
						foreach (var c in availColors)
						{
							var variantColorLine = new VariantColorLine();
							variantColorLine.Color = c.Color;
							variantColorLine.Value = string.Format(Constants.Products.VariantColorLineFormat,
								c.VariantID, c.Price, c.Images);
							variantColorLines.Add(variantColorLine);
							if (pos < 1)
							{
								product.DefaultVariant = c.VariantID;
							}
							pos++;
						}
						variantColor.VariantColorLines = variantColorLines;
						variantColors.Add(variantColor);
						product.VariantColors = variantColors;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="parentIds"></param>
		/// <returns></returns>
		private static IEnumerable<CategoryMenulistViewModel> GetCategoryMenuList(string parentIds)
		{
			var categoryMenulistViewModel = new List<CategoryMenulistViewModel>();
			if (!string.IsNullOrEmpty(parentIds))
			{
				var parentIdArr = parentIds.Split(Constants.Common.PipeSeparator);
				foreach (var parentId in parentIdArr)
				{

					var categories = GetCategoryMenuListByParentId(parentId);
					if (categories != null)
					{
						var cnt = 0;
						foreach (var category in categories)
						{
							cnt++;
							var c = new CategoryMenulistViewModel();
							c.ID = category.ID.ToString();
							c.Name = category.Name;
							if (category.Parent != null && !category.Parent.Name.Equals("departments", StringComparison.OrdinalIgnoreCase) && !category.Parent.Name.Equals("ca", StringComparison.OrdinalIgnoreCase))
								c.ParentName = category.Parent.Name;
							else
								c.ParentName = string.Empty;
							//c.Url = LinkManager.GetItemUrl(category);
							c.Url = "/categories/" + c.Name;


							// need to rewrite to boost performance
							//  var i = 0;
							//   if (2 < i)
							//   {
							var categoryChildern = category
								.GetChildren()
								.Where(i => i.TemplateName.ToString() == "GeneralCategory")
								.Select(GlassHelper.Cast<Product>).ToList();

							categoryChildern = ProductHelper.FilterProductsByRestrictions(categoryChildern);

							c.ProductsCount = categoryChildern.Count();

							var pList = categoryChildern.Select(categoryChild => new ProductMenulistViewModel
							{
								Name = categoryChild.Title,
								Url = categoryChild.Url
							})
								.ToList();

							var count = pList.Count;
							c.ProductMenulistViewModel = pList;

							if (categoryChildern.Count > 0)
								categoryMenulistViewModel.Add(c);
						}
					}

				}
			}

			categoryMenulistViewModel = categoryMenulistViewModel.OrderBy(i => i.Name).ToList();
			return categoryMenulistViewModel;
		}

		/// <summary>
		/// </summary>
		/// <param name="parentId"></param>
		/// <returns></returns>
		private static List<Item> GetCategoryMenuListByParentId(string parentId)
		{
			var output = new List<Item>();
			var catParentItem = Context.Database.GetItem(new ID(parentId));

			try
			{
				if (catParentItem != null)
				{

					var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetSearchIndex());
					try
					{
						using (var context = index.CreateSearchContext())
						{

							var queryable = context.GetQueryable<SearchResultItem>()
									.Where(x => x.Language == Context.Language.Name);
							var result =
								queryable.Where(
									x =>
										(x.TemplateName == "GeneralCategory") && x.Path.Contains(catParentItem.Paths.Path)
										 ).ToList();



							if (result.Any())
							{
								output.AddRange(result.Select(r => r.GetItem()));
							}
						}
					}
					catch (Exception ex)
					{
						Log.Error(ex.StackTrace, ex);

					}

					return output;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.StackTrace, ex);
			}
			return null;
		}

		/// <summary>
		/// </summary>
		/// <param name="itemName"></param>
		/// <param name="parentId"></param>
		/// <returns></returns>
		internal static string GetItemIdFromName(string itemName, string parentId)
		{
			var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetSearchIndex());
			try
			{
				using (var context = index.CreateSearchContext())
				{
					var queryable = context.GetQueryable<SearchResultItem>()
						.Where(x => x.Language == Context.Language.Name);
					var searchResultItem = queryable.FirstOrDefault(
						x =>
							string.Equals(x.Name, itemName, StringComparison.CurrentCultureIgnoreCase) &&
							x.TemplateName == "GeneralCategory");
					if (searchResultItem != null)
						return
							searchResultItem.ItemId.ToString();
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.StackTrace, ex);
			}
			return string.Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		internal static SearchResultItem GetItemByProductId(string productId)
		{
			var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetSearchIndex());
			try
			{
				using (var context = index.CreateSearchContext())
				{
					var queryable = context.GetQueryable<SearchResultItem>()
						.Where(x => x.Language == Context.Language.Name);
					return
						queryable.FirstOrDefault(
							x => string.Equals(x.Name, productId, StringComparison.CurrentCultureIgnoreCase));
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.StackTrace, ex);
			}
			return null;
		}

		internal static SearchResultItem GetCommerceItemByProductId(string productId)
		{
			var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetSearchIndex());
			try
			{
				using (var context = index.CreateSearchContext())
				{
					var queryable = context.GetQueryable<SearchResultItem>()
						.Where(x => x.Language == Context.Language.Name
						&& x["commercesearchitemtype"] == "product"
						&& x["catalogname"] == GetSiteRootCatalogName()
						&& x["productid"] == productId.ToLower()
						).GetResults().ToList();

					return queryable.Count > 0 ? queryable[0].Document : null;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.StackTrace, ex);
			}
			return null;
		}

		internal static string GetOrders()
		{
			// GetVisitorOrdersResult
			return null;
		}

		/// <summary>
		/// </summary>
		/// <param name="cartHelper"></param>
		/// <returns></returns>
		internal static OrdersViewModel GetCustomerOrders(CartHelper cartHelper)
		{
			var ordersViewModel = new OrdersViewModel();
			var orderDetails = new List<OrderDetailViewModel>();
			var orders = cartHelper.GetOrders(cartHelper.GetVisitorId(), cartHelper.ShopName);

			if (orders != null)
			{
				foreach (var order in orders.OrderHeaders)
				{
					var fullOrderDetail = GetCustomerOrderDetail(order.ExternalId, cartHelper);
					orderDetails.Add(fullOrderDetail);
				}
				ordersViewModel.Orders = orderDetails;
			}
			return ordersViewModel;
		}

		/// <summary>
		/// </summary>
		/// <param name="orderId"></param>
		/// <param name="cartHelper"></param>
		/// <returns></returns>
		internal static OrderDetailViewModel GetCustomerOrderDetail(string orderId, CartHelper cartHelper)
		{
			var orderDetail = new OrderDetailViewModel();
			var orderHead = cartHelper.GetOrderHead(orderId, cartHelper.GetVisitorId(), cartHelper.ShopName);
			var commerceOrderHead = orderHead.Order as CommerceOrder;
			if (commerceOrderHead != null)
			{
				orderDetail.OrderId = commerceOrderHead.OrderID;
				orderDetail.TrackingNumber = commerceOrderHead.TrackingNumber;
				orderDetail.OrderDate = commerceOrderHead.Created.ToString(Constants.Products.DateTimeFormat);
				orderDetail.NumberofItems = commerceOrderHead.LineItemCount;
				var commerceTotal = commerceOrderHead.Total as CommerceTotal;
				if (commerceTotal != null)
				{
					orderDetail.SubTotalPrice = commerceTotal.Subtotal.ToString(Constants.Products.CurrencyFormat);
					orderDetail.TotalPrice = commerceTotal.Amount.ToString(Constants.Products.CurrencyFormat);
					orderDetail.Tax = commerceTotal.TaxTotal.Amount.ToString(Constants.Products.CurrencyFormat);
					orderDetail.ShippingCost = commerceTotal.ShippingTotal.ToString(Constants.Products.CurrencyFormat);
				}
				orderDetail.OrderStatus = commerceOrderHead.Status;
				orderDetail.UserId = commerceOrderHead.UserId;
				orderDetail.ExternalId = commerceOrderHead.ExternalId;
				orderDetail.Billing = commerceOrderHead.Parties.ElementAt(1) as CommerceParty;
				orderDetail.Shipping = commerceOrderHead.Parties.ElementAt(0) as CommerceParty;
				if (orderDetail.Billing == null)
				{
					orderDetail.Billing = new CommerceParty();
				}
				if (orderDetail.Shipping == null)
				{
					orderDetail.Shipping = new CommerceParty();
				}
				else
				{
					orderDetail.Email = orderDetail.Shipping.Email;
					orderDetail.Phone = orderDetail.Shipping.PhoneNumber;
				}
				if (commerceOrderHead.Payment.ElementAtOrDefault(0) != null)
				{
					var paymentMethodId = commerceOrderHead.Payment.ElementAt(0).PaymentMethodID;
					orderDetail.PaymentMethod = GetPaymentMethod(paymentMethodId);
				}
				if (commerceOrderHead.Shipping.ElementAtOrDefault(0) != null)
				{
					var shippingMethodId = commerceOrderHead.Shipping.ElementAt(0).ShippingMethodID;
					orderDetail.ShippingMethod = commerceOrderHead.Shipping[0].Properties["ShippingMethodName"].ToString();
				}
				var lines = commerceOrderHead.Lines.ToList();
				orderDetail.OrderLines = GetOrderLines(lines);

			}
			return orderDetail;
		}

		/// <summary>
		/// </summary>
		/// <param name="paymentMethodId"></param>
		/// <returns></returns>
		private static string GetPaymentMethod(string paymentMethodId)
		{
			var resp = string.Empty;
			try
			{
				var provider = new PaymentServiceProvider();
				var paymentRequest = new CommerceGetPaymentMethodsRequest(Context.Language.ToString());
				var paymentResult = provider.GetPaymentMethods(paymentRequest);
				var firstOrDefault = paymentResult.PaymentMethods.FirstOrDefault(x => x.ExternalId == paymentMethodId);
				if (firstOrDefault != null)
					resp = firstOrDefault.Name;
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex);
			}
			return resp;
		}

		/// <summary>
		/// </summary>
		/// <param name="lines"></param>
		/// <returns></returns>
		private static IEnumerable<OrderLine> GetOrderLines(IEnumerable<CartLine> lines)
		{
			var orderLines = new List<OrderLine>();
			foreach (var cartLine in lines)
			{
				var line = (CommerceCartLine)cartLine;
				var orderLine = new OrderLine();
				orderLine.UnitPrice = line.Product.Price.Amount.ToString(Constants.Products.CurrencyFormat);
				orderLine.Quantity = line.Quantity;
				orderLine.SubTotal =
					(line.Product.Price.Amount * line.Quantity).ToString(Constants.Products.CurrencyFormat);
				var product = line.Product as CommerceCartProduct;
				if (product != null)
				{
					orderLine.ProductName = product.DisplayName;
					var sItem = GetItemByProductId(product.ProductId);
					if (sItem != null)
					{
						orderLine.ImageUrl = GetFirstImageFromProductItem(sItem.GetItem());
						orderLine.Url = LinkManager.GetItemUrl(sItem.GetItem());
					}
				}
				orderLines.Add(orderLine);
			}
			return orderLines;
		}

		/// <summary>
		/// </summary>
		/// <param name="shippingMethodId"></param>
		/// <returns></returns>
		private static string GetShippingMethod(string shippingMethodId)
		{
			var resp = string.Empty;
			try
			{
				var provider = new ShippingServiceProvider();
				var shippingRequest = new CommerceGetShippingMethodsRequest(Context.Language.ToString());
				var shippingResult = provider.GetShippingMethods(shippingRequest);
				var firstOrDefault = shippingResult.ShippingMethods.FirstOrDefault(x => x.ExternalId == shippingMethodId);
				if (firstOrDefault != null)
					resp = firstOrDefault.Name;
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex);
			}
			return resp;
		}

		/// <summary>
		/// </summary>
		/// <param name="productId"></param>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		internal static Product GetProductByNameAndCategory(string productId, string categoryId)
		{
			var product = new Product();
			var cartHelper = new CartHelper();
			var productResult = GetItemByName(productId);
			if (productResult != null)
			{
				var productItem = productResult.GetItem();
				product = GlassHelper.Cast<Product>(productItem);

				var productVariants = productItem.GetChildren().Select(x => x.GlassCast<ProductVariant>()).ToList(); //todo: refactor

				// Update Images and stockInfo in ProductVariant
				if (productVariants.Any())
				{
					var theVariants = new List<ProductVariant>();

					for (var i = 0; i < productVariants.Count(); i++)
					{
						productVariants[i] = UpdateVariantProperties(productVariants[i], product, cartHelper);
						theVariants.Add(productVariants[i]);
						if (i < 1 && string.IsNullOrEmpty(product.DefaultVariant)) { product.DefaultVariant = productVariants[i].VariantId; }
					}
					product.ProductVariants = theVariants;
				}
				if (productItem.HasChildren)
				{
					BuildUiVariants(product);
				}


				if (!string.IsNullOrEmpty(product.DefaultVariant))
				{
					product.StockInformation = cartHelper.GetProductStockInformation(product.ProductId,
						product.CatalogName, product.DefaultVariant);
				}
				else
				{
					product.StockInformation = cartHelper.GetProductStockInformation(product.ProductId,
						product.CatalogName);
				}
			}
			return product;
		}

		internal static int GetProductViewCount(Guid productId, string productName)
		{
			var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetAnayticsIndex());
			try
			{
				using (var context = index.CreateSearchContext())
				{
					var productIdIdString = productId.ToString().ToLower().Replace("-", string.Empty);

					var productViews = context.GetQueryable<SearchResultItem>()
						.Where(x =>
						x["visitpage.url"].Contains(productName.ToLower()) &&
						x["visitpageevent.name"] == "view product" &&
						x["type"] == "visitpageevent"
						).GetResults().ToList();


					return productViews.Count;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.StackTrace, ex);
			}

			return 0;
		}

		internal static List<Product> GetProductsByCategory(Guid categoryId)
		{
			var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetSearchIndex());
			try
			{
				using (var context = index.CreateSearchContext())
				{
					var categoryIdString = categoryId.ToString().ToLower().Replace("-", string.Empty);

					var products = context.GetQueryable<SearchResultItem>()
						.Where(x => x.Language == Context.Language.Name &&
						x["_latestversion"] == "1" &&
						x["commercesearchitemtype"] == "product" &&
						x["commerceancestorids"] == categoryIdString &&
						x.Path.Contains("/sitecore/commerce/catalog")
						).GetResults().ToList();


					return products.Select(p => p.Document.GetItem().GlassCast<Product>()).ToList();
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.StackTrace, ex);
			}
			return null;
		}

		internal static ProductVariant UpdateVariantProperties(ProductVariant productVariant, Product product,
			CartHelper cartHelper)
		{
			if (productVariant.Images == null || !productVariant.Images.Any())
			{
				productVariant.Images = product.Images.Select(x => x);
			}
			productVariant.StockInformation = cartHelper.GetProductStockInformation(product.ProductId,
				product.CatalogName, productVariant.VariantId);
			if (productVariant.StockInformation != null)
			{
				productVariant.StockLabel = productVariant.StockInformation != null &&
											productVariant.StockInformation.Count > 0
					? $"{Convert.ToInt32(productVariant.StockInformation.Count):#,###0} In Stock"
					: "Out Of Stock";
				productVariant.StockQuantity = Convert.ToInt32(productVariant.StockInformation.Count);
			}
			return productVariant;
		}

		/// <summary>
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		internal static SearchResultItem GetItemByName(string productId)
		{
			var index = ContentSearchManager.GetIndex($"sitecore_{(Context.Database == null || Context.Database.Name.ToLower() == "core" ? "master" : Context.Database.Name)}_index");
			try
			{
				using (var context = index.CreateSearchContext())
				{
					var queryable = context.GetQueryable<SearchResultItem>()
						.Where(x => x.Language == Context.Language.Name);
					return
						queryable.FirstOrDefault(
							x => string.Equals(x.Name, productId, StringComparison.CurrentCultureIgnoreCase));
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.StackTrace, ex);
			}
			return null;
		}

		public static bool Contains(this List<string> source, string value)
		{
			foreach (var t in source)
			{
				if (string.Equals(t, value, StringComparison.CurrentCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}


		/// <summary>
		/// Gets the Site's selected root catalogID that was set on the catalog field of the site item.
		/// </summary>
		/// <returns></returns>
		internal static string GetSiteRootCatalogId()
		{

			// Fetch the start item from Site definition

			var rootItem = Context.Database.GetItem(Context.Site.ContentStartPath);

			if (rootItem == null) return string.Empty;
			var rootModel = GlassHelper.Cast<Root>(rootItem);

			try
			{
				return rootModel.Catalog?.ID.ToString() ?? string.Empty;
			}
			catch (Exception)
			{

				return string.Empty;
			}

		}

		internal static string GetSiteRootCatalogName()
		{
			// Fetch the start item from Site definition
			var rootItem = Context.Database.GetItem(Context.Site.ContentStartPath);

			if (rootItem == null) return string.Empty;
			var rootModel = GlassHelper.Cast<Root>(rootItem);

			return rootModel.Catalog != null ? rootModel.Catalog.Name : string.Empty;
		}


		/// <summary>
		/// Returns a catalog item based on the catalog name
		/// </summary>
		/// <param name="catalogName"></param>
		/// <returns></returns>
		internal static Item GetCatalogItemByName(string catalogName)
		{
			var catalogRoot = Context.Database.GetItem(Constants.Products.CatalogsRootPath);

			if (catalogRoot == null || !catalogRoot.HasChildren) return null;

			var catalogs = catalogRoot.GetChildren();

			if (catalogs != null && catalogs.Any())
			{
				var catalog = catalogs.FirstOrDefault(c => c.Name.ToLower().Equals(catalogName.ToLower()));

				return catalog;
			}

			return null;
		}

		/// <summary>
		/// Returns the catalog to display to the current user
		/// </summary>
		/// <param name="catalogIds"></param>
		/// <returns></returns>
		internal static List<Category> GetCatalogCategories(string catalogIds)
		{

			var catalogCategories = new List<Category>();
			var catalogs = catalogIds.Split(Constants.Common.PipeSeparator);
			foreach (var catalogId in catalogs)
			{
				var catalog = Context.Database.GetItem(new ID(catalogId));
				if (catalog != null)
				{
					var categories = GetGeneralCategoryChildren(catalog).ToList();

					if (categories.Any())
					{

						foreach (var catItem in categories)
						{
							var category = GlassHelper.Cast<Category>(catItem);

							if (catItem.HasChildren)
							{
								var childCategories = GetGeneralCategoryChildren(catItem).ToList();
								if (childCategories.Any())
								{
									try
									{
										category.ChildCategories.AddRange(
											childCategories.Select(c => c.GlassCast<Category>())); //todo: refactor
									}
									catch (Exception ex)
									{
										Log.Error(ex.StackTrace, ex);

									}
								}
								catalogCategories.Add(category);
							}

						}
					}
				}
			}
			return catalogCategories;
		}

		private static IEnumerable<Item> GetGeneralCategoryChildren(Item categoryItem)
		{
			return categoryItem.GetChildren()
				.Where(
					x =>
						x.TemplateName.Equals(Constants.Products.GeneralCategoryTemplateName,
							StringComparison.InvariantCultureIgnoreCase));
		}

		/// <summary>
		/// Get CategoryProducts item IDS from a given name and assigned catalog
		/// </summary>
		/// <param name="categoryName"></param>
		/// <param name="catalogIds"></param>
		/// <returns></returns>
		internal static string GetItemIdsFromName(string categoryName, string catalogIds)
		{
			// get the catalog ids for this user
			// loop through the ids and get ids of children items with the given name
			var categoryChildIds = new List<string>();

			if (!string.IsNullOrEmpty(catalogIds))
			{
				var catalogs = catalogIds.Split(Constants.Common.PipeSeparator);

				foreach (var catalogId in catalogs)
				{
					var catalog = Context.Database.GetItem(new ID(catalogId));
					if (catalog != null)
					{
						var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetSearchIndex());
						try
						{
							using (var context = index.CreateSearchContext())
							{

								var queryable = context.GetQueryable<SearchResultItem>()
										.Where(x => x.Language == Context.Language.Name);
								var result =
									queryable.Where(
										x =>
											(x.Name.Contains(categoryName)
											&& x.Path.Contains(catalog.Paths.Path)
											&&
											 x.TemplateName == "GeneralCategory")).ToList();

								if (result.Any())
								{
									foreach (var r in result)
									{
										if (r.Name.ToLower() == categoryName.ToLower())
										{
											categoryChildIds.Add(r.ItemId.ToString());
										}
									}
								}
							}
						}
						catch (Exception ex)
						{
							Log.Error(ex.StackTrace, ex);
							return string.Empty;
						}

					}
				}
				return (categoryChildIds.Count < 1) ? String.Empty : categoryChildIds.Aggregate((current, next) => current + Constants.Common.PipeStringSeparator + next);

			}

			return string.Empty;
		}

		/// <summary>
		/// Set cookie to show coupon only if user is a logged in commerce user
		/// </summary>
		/// <param name="personalizedProducts"></param>
		internal static void SetPersonalizedCoupon(PersonalizedProducts personalizedProducts)
		{
			if (Context.User.IsAuthenticated &&
				Context.User.Name.ToLower().Contains(Constants.Commerce.CommerceUserDomain.ToLower()))
			{
				var showCouponCookie = Cookie.Get(Constants.Commerce.ShowCoupon);
				if (showCouponCookie == null)
				{
					Cookie.Set(Constants.Commerce.ShowCoupon, Constants.Common.True);
					var couponMessage = string.Format("{0}|{1}", personalizedProducts.CouponCode,
						personalizedProducts.CouponCodeDescription);

					Cookie.Set(Constants.Commerce.CouponMessage, couponMessage);
				}
			}
		}


		/// <summary>
		/// </summary>
		/// <param name="query"></param>
		/// <param name="searchFor">Search for either product or category</param>
		/// <returns></returns>
		public static IEnumerable<SearchHit<SearchResultItem>> GetSearchResultItemByNameOrId(string query, string searchFor = "products")
		{
			var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetSearchIndex());
			try
			{
				using (var context = index.CreateSearchContext())
				{

					var queryable = context.GetQueryable<SearchResultItem>()
							.Where(x => x.Language == Context.Language.Name);
					if (searchFor == "products")
					{

						return
							queryable.Where(
								x =>
									(x.Name.Contains(query) || x["_displayname"].Contains(query)) &&
									x.Path.Contains("/sitecore/commerce/catalog") &&
									x["_latestversion"] == "1" &&
									x["catalogname"] == GetSiteRootCatalogName() &&
									x.TemplateName != "GeneralCategory").Page(0, 5).GetResults().ToList();
					}
					return
						queryable.Where(
							x =>
								(x.Name.Contains(query) &&
								 x.Path.Contains("/sitecore/commerce/catalog") &&
								 x.TemplateName == "GeneralCategory") &&
								 x["catalogname"] == GetSiteRootCatalogName() &&
								 x["_latestversion"] == "1").Page(0, 5).GetResults().ToList();
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.StackTrace, ex);
			}
			return null;
		}


		internal static List<ProductMini> GetProductsByName(string query)
		{

			var catalogId = GetSiteRootCatalogId();

			var catalogName = GetSiteRootCatalogName();

			var productList = new List<ProductMini>();

			var products = GetSearchResultItemByNameOrId(query);

			if (products != null && products.Any())
			{
				foreach (var searchResultItem in products)
				{
					try
					{
						var productItem = searchResultItem.Document.GetItem();
						var product = GlassHelper.Cast<Product>(productItem);
						var parentName = productItem.Parent.Name;

						if (product.ProductId != null)
						{
							var variantId = "-1";
							if (productItem.HasChildren)
							{
								var child = productItem.Children.FirstOrDefault();
								variantId = child.Name;
							}
							productList.Add(new ProductMini { Id = product.ProductId, CategoryName = parentName, CatalogId = catalogId, Guid = ID.Parse(product.ID).ToString(), Title = product.Title, Price = product.Price, CatalogName = catalogName, ImageSrc = product.FirstImage, VariantId = variantId, Url = product.Url, IsOnSale = product.IsOnSale, SalePrice = product.SalePrice });
						}
					}
					catch (Exception ex)
					{
						Log.Error(ex.StackTrace, ex);
					}
				}
			}

			return productList;
		}


		internal static List<ProductMini> GetCategoriesByName(string query)
		{
			var categoryList = new List<ProductMini>();
			var categories = GetSearchResultItemByNameOrId(query, "categories");

			if (categories != null && categories.Any())
			{
				foreach (var searchResultItem in categories)
				{
					try
					{
						var item = searchResultItem.Document.GetItem();
						var category = GlassHelper.Cast<Category>(item);

						if (category.Name != null)
						{
							categoryList.Add(new ProductMini { Title = category.Name, Url = category.Url });
						}
					}
					catch (Exception ex)
					{
						Log.Error(ex.StackTrace, ex);
					}
				}
			}

			return categoryList;
		}

		public static bool SaveProductSortIds(List<string> products, Guid categoryId)
		{
			var isSuccess = false;

			try
			{
				if (products != null)
				{
					for (var ctr = 0; ctr < products.Count; ctr++)
					{
						var prodId = products[ctr];
						var prodItem = GetItemByName(prodId).GetItem();

						if (prodItem != null)
						{
							prodItem.Editing.BeginEdit();
							prodItem.Fields["SortId"].Value = ctr.ToString();
							prodItem.Editing.EndEdit();
						}
					}
				}

				isSuccess = true;
			}
			catch (Exception ex)
			{
				Log.Error("Error in ProductHelper.SaveProductSortIds; ex.message = " + ex.Message, ex);
			}


			return isSuccess;
		}

		public static decimal GetProductSalePrice(string productId)
		{
			try
			{
				var pricingServiceProvider = new PricingServiceProvider();

				// Create a GetProductPricesRequest object, specify the product's ID and do not  
				// specify any price types. Default price type is ListPrice 
				var catalogName = GetSiteRootCatalogName();
				var request = new Sitecore.Commerce.Engine.Connect.Services.Prices.GetProductPricesRequest(catalogName, productId);

				// Call the service provider and receive the result. 
				var result = pricingServiceProvider.GetProductPrices(request);

				// Result prices contains the list price only. 
				if (!result.Success)
				{
					throw new Exception(result.SystemMessages[0].Message);
				}

				var price = ((Sitecore.Commerce.Engine.Connect.Entities.Prices.ExtendedCommercePrice)result.Prices.First().Value).ListPrice;

				return price;
			}
			catch (Exception ex)
			{
				Log.Error($"ProductHelper.GetProductSalePrice, Error = {ex.Message}", ex);
				return 0;
			}
		}

		public static List<Guid> GetProductGroups(Item productItem)
		{
			try
			{
				Assert.ArgumentNotNull(productItem?.Parent, "productItem.Parent");
				var productCategoryItem = productItem.Parent;

				var db = Sitecore.Configuration.Factory.GetDatabase("master");
				Assert.ArgumentNotNull(db, "Database");

				//get all groups
				var groupItem = db.GetItem("/sitecore/content/Global Configuration/Product Groups");

				Assert.ArgumentNotNull(groupItem, "groupItem");

				var group = GlassHelper.Cast<ProductGroups>(groupItem);

				Assert.ArgumentNotNull(group, "group");

				return group.Groups.Where(g => g.ProductCategories.Any(c => c.ID == productCategoryItem.ID.ToGuid()))
					.Select(g => g.ID).ToList();
			}
			catch (Exception ex)
			{
				Log.Error($"ProductHelper.GetProductGroups, Error = {ex.Message}", ex);
				return new List<Guid>();
			}
		}

		public static IQueryable<CustomCommerceSearchResultItem> FilterProductsByRestrictions(IQueryable<CustomCommerceSearchResultItem> searchResults)
		{
			try
			{
				if (searchResults.ToList().Count > 0)
				{
					searchResults = FilterByFacilityBlacklist(searchResults);
					searchResults = FilterByInmateRestrictions(searchResults);
					searchResults = FilterByInmateBlacklist(searchResults);
				}
			}
			catch (Exception e)
			{
				Log.Error($"ProductHelper.FilterProductsByRestrictions(), Error = {e.Message}", e);
			}

			return searchResults;
		}

		private static IQueryable<CustomCommerceSearchResultItem> FilterByInmateBlacklist(IQueryable<CustomCommerceSearchResultItem> searchResults)
		{
			try
			{
				if (searchResults == null || searchResults.ToList().Count <= 0)
					return searchResults;

				var productBlacklistIds = InmateHelper.GetProductBlacklist();
				if (productBlacklistIds != null && productBlacklistIds.Count > 0)
				{
					//TODO: refactor, running to errors using .ANY with LINQ to Sitecore
					foreach (var productBlacklistId in productBlacklistIds)
					{
						searchResults = searchResults
							.Where(sr => !sr.Name.Contains(productBlacklistId.ToLower()));
					}
				}
			}
			catch (Exception e)
			{
				Log.Error($"ProductHelper.FilterByInmateBlacklist(), Error = {e.Message}", e);
			}

			return searchResults;
		}

		public static List<Product> FilterProductsByRestrictions(List<Product> searchResults)
		{
			try
			{
				if (searchResults.Count > 0)
				{
					searchResults = FilterByFacilityBlacklist(searchResults);
					searchResults = FilterByInmateRestrictions(searchResults);
					searchResults = FilterByInmateBlacklist(searchResults);
				}
			}
			catch (Exception e)
			{
				Log.Error($"ProductHelper.FilterProductsByRestrictions(), Error = {e.Message}", e);
			}

			return searchResults;
		}

		private static List<Product> FilterByInmateBlacklist(List<Product> searchResults)
		{
			try
			{
				if (searchResults == null || searchResults.ToList().Count <= 0)
					return searchResults;

				var productBlacklistIds = InmateHelper.GetProductBlacklist();
				if (productBlacklistIds != null && productBlacklistIds.Count > 0)
				{
					//TODO: refactor, running to errors using .ANY with LINQ to Sitecore
					foreach (var productBlacklistId in productBlacklistIds)
					{
						searchResults = searchResults
							.Where(sr => !sr.ProductId.ToLower().Contains(productBlacklistId.ToLower())).ToList();
					}
				}
			}
			catch (Exception e)
			{
				Log.Error($"ProductHelper.FilterByInmateBlacklist(), Error = {e.Message}", e);
			}

			return searchResults;
		}

		private static List<Product> FilterByFacilityBlacklist(List<Product> searchResults)
		{
			try
			{
				if (searchResults == null || searchResults.Count <= 0)
					return searchResults;

				var productCategoryBlacklist = FacilityHelper.GetProgramProductCategoryBlacklist();
				if (productCategoryBlacklist != null && productCategoryBlacklist.Count > 0)
				{
					var blacklistedCategoryIds = productCategoryBlacklist.Select(p => p.ID).ToList();

					//TODO: refactor, running to errors using .ANY with LINQ to Sitecore
					foreach (var blacklistedCategoryId in blacklistedCategoryIds)
					{
						var id = ID.Parse(blacklistedCategoryId);

						searchResults = searchResults
							.Where(sr => !sr.Paths.Contains(id)).ToList();
					}
				}
			}
			catch (Exception e)
			{
				Log.Error($"ProductHelper.FilterByFacilityBlackList(), Error = {e.Message}", e);
			}

			return searchResults;
		}

		private static IQueryable<CustomCommerceSearchResultItem> FilterByFacilityBlacklist(IQueryable<CustomCommerceSearchResultItem> searchResults)
		{
			try
			{
				if (searchResults == null || searchResults.ToList().Count <= 0)
					return searchResults;

				var productCategoryBlacklist = FacilityHelper.GetProgramProductCategoryBlacklist();
				if (productCategoryBlacklist != null && productCategoryBlacklist.Count > 0)
				{
					var blacklistedCategoryIds = productCategoryBlacklist.Select(p => p.ID).ToList();

					//TODO: refactor, running to errors using .ANY with LINQ to Sitecore
					foreach (var blacklistedCategoryId in blacklistedCategoryIds)
					{
						var id = ID.Parse(blacklistedCategoryId);

						searchResults = searchResults
							.Where(sr => !sr.Paths.Contains(id));
					}
				}
			}
			catch (Exception e)
			{
				Log.Error($"ProductHelper.FilterByFacilityBlackList(), Error = {e.Message}", e);
			}

			return searchResults;
		}

		private static IQueryable<CustomCommerceSearchResultItem> FilterByInmateRestrictions(IQueryable<CustomCommerceSearchResultItem> searchResults)
		{
			//TODO: refactor
			if (InmateHelper.IsRestrictedMale())
			{
				searchResults = searchResults.Where(item => item.RestrictionMale);
			}
			if (InmateHelper.IsRestrictedFemale())
			{
				searchResults = searchResults.Where(item => item.RestrictionFemale);
			}
			if (InmateHelper.IsRestrictedSugarFree())
			{
				searchResults = searchResults.Where(item => item.RestrictionSugarFree);
			}
			if (InmateHelper.IsRestrictedKosher())
			{
				searchResults = searchResults.Where(item => item.RestrictionKosher);
			}
			if (InmateHelper.IsRestrictedGlutenFree())
			{
				searchResults = searchResults.Where(item => item.RestrictionGlutenFree);
			}

			return searchResults;
		}

		private static List<Product> FilterByInmateRestrictions(List<Product> searchResults)
		{
			//TODO: refactor
			if (InmateHelper.IsRestrictedMale())
			{
				searchResults = searchResults.Where(item => item.RestrictionMale).ToList();
			}
			if (InmateHelper.IsRestrictedFemale())
			{
				searchResults = searchResults.Where(item => item.RestrictionFemale).ToList();
			}
			if (InmateHelper.IsRestrictedSugarFree())
			{
				searchResults = searchResults.Where(item => item.RestrictionSugarFree).ToList();
			}
			if (InmateHelper.IsRestrictedKosher())
			{
				searchResults = searchResults.Where(item => item.RestrictionKosher).ToList();
			}
			if (InmateHelper.IsRestrictedGlutenFree())
			{
				searchResults = searchResults.Where(item => item.RestrictionGlutenFree).ToList();
			}

			return searchResults;
		}

		public static List<string> GetFreeProducts()
		{
			//var query = "Free-";
			var productList = new List<ProductMini>();

			var catalogId = GetSiteRootCatalogId();

			var catalogName = GetSiteRootCatalogName();

			var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetProductSearchIndex());
			try
			{
				using (var context = index.CreateSearchContext())
				{

					var queryable = context.GetQueryable<SearchResultItem>()
							.Where(x => x.Language == Context.Language.Name);


					var products =
						queryable.Where(
							x =>
								(x.Name.Contains("Promotional Items") || x["_displayname"].Contains("Promotional Goods")) &&
								x.Path.Contains("/sitecore/commerce/catalog") &&
								x["_latestversion"] == "1" &&
								x["catalogname"] == GetSiteRootCatalogName() &&
								x.TemplateName != "GeneralCategory").Page(0, 5).GetResults().ToList();

					if (products != null && products.Any())
					{
						foreach (var searchResultItem in products)
						{
							try
							{
								var productItem = searchResultItem.Document.GetItem();
								var product = GlassHelper.Cast<Product>(productItem);
								var parentName = productItem.Parent.Name;

								if (product.ProductId != null)
								{
									var variantId = "-1";
									if (productItem.HasChildren)
									{
										var child = productItem.Children.FirstOrDefault();
										variantId = child.Name;
									}
									productList.Add(new ProductMini { Id = product.ProductId, CategoryName = parentName, CatalogId = catalogId, Guid = ID.Parse(product.ID).ToString(), Title = product.Title, Price = product.Price, CatalogName = catalogName, ImageSrc = product.FirstImage, VariantId = variantId, Url = product.Url, IsOnSale = product.IsOnSale, SalePrice = product.SalePrice });
								}
							}
							catch (Exception ex)
							{
								Log.Error(ex.StackTrace, ex);
							}
						}
					}

				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.StackTrace, ex);
			}
			return null;

		}

		private static SearchResponse SearchCatalogItemsByKeyword(string keyword, string catalogName,
			CommerceSearchOptions searchOptions)
		{
			Assert.ArgumentNotNullOrEmpty(catalogName, "catalogName");

			var searchManager = CommerceTypeLoader.CreateInstance<ICommerceSearchManager>();
			var searchIndex = searchManager.GetIndex(ConfigurationHelper.GetProductSearchIndex());

			using (var context = searchIndex.CreateSearchContext())
			{
				var searchResults = context.GetQueryable<CustomCommerceSearchResultItem>()
					.Where(item => item.Content.Contains(keyword) || item.ProductTags.Contains(keyword))
					.Where(item => item.CommerceSearchItemType == CommerceSearchResultItemType.Product)
					.Where(item => item.CatalogName == catalogName)
					.Where(item => item.Language == Sitecore.Context.Language.Name)
					.Where(item => !item.Name.StartsWith("Free-"));

				searchResults = ProductHelper.FilterProductsByRestrictions(searchResults);

				searchResults = searchResults
					.Select(p => new CustomCommerceSearchResultItem()
					{
						ItemId = p.ItemId,
						Uri = p.Uri
					});

				searchResults = searchManager.AddSearchOptionsToQuery<CustomCommerceSearchResultItem>(searchResults, searchOptions);

				var results = searchResults.GetResults();

				var response = SearchResponse.CreateFromSearchResultsItems(searchOptions, results);

				return response;
			}
		}
	}
}