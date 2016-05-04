#region

using System;
using System.Collections.Generic;
using System.Linq;
using CSDemo.Configuration;
using CSDemo.Models.Account;
using CSDemo.Models.Checkout.Cart;
using Glass.Mapper.Sc;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Analytics.Data;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
using Sitecore.Commerce.Entities.Carts;
using Sitecore.Commerce.Services.Payments;
using Sitecore.Commerce.Services.Shipping;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Convert = System.Convert;
using CSDemo.Models.Page;

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
                        var category = searchedItem.GetItem().GlassCast<Category>();
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

        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal static CategoryProductViewModel GetCategoryProducts(PaginationViewModel model)
        {
            var cartHelper = new CartHelper();
            var categoryProductVm = new CategoryProductViewModel {PaginationViewModel = model};

            var category = new Category();
            categoryProductVm.CategoryMenulist = GetCategoryMenuList(model.UserCatalogIds);

            var searchedItem = GetSearchResultItemById(model.CategoryId);
            if (searchedItem != null)
            {
                var catItem = searchedItem.GetItem();
                category = catItem.GlassCast<Category>();
                if (catItem.HasChildren)
                {
                    var catChildren = catItem.GetChildren().Select(x => x.GlassCast<Product>()).ToList();
                    model.TotalItems = catChildren.Count();
                    model.TotalPages = (long) Math.Ceiling((double) model.TotalItems/model.PageSize);

                    // do sorting
                    if (!string.IsNullOrEmpty(model.OrderBy))
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
                                break;
                        }
                    }

                    // do paging
                    category.Products = catChildren
                        .Skip(model.PageSize*(model.CurrentPage - 1))
                        .Take(model.PageSize);

                    // Process ProductVariants
                    foreach (var product in category.Products)
                    {
                        var catProdItem = catItem.GetChildren().FirstOrDefault(x => x.ID.ToGuid() == product.ID);
                        if (catProdItem != null && catProdItem.HasChildren)
                        {
                            // Update ProductVariants
                            product.ProductVariants =
                                catProdItem.GetChildren().Select(x => x.GlassCast<ProductVariant>());
                            BuildUiVariants(product);
                        }
                        var stockInfo = cartHelper.GetProductStockInformation(product.ProductId, product.CatalogName);
                    }
                }
            }
            categoryProductVm.Category = category;
            return categoryProductVm;
        }

        /// <summary>
        /// </summary>
        /// <param name="product"></param>
        internal static void BuildUiVariants(Product product)
        {
            var cultureInfo = Context.Culture;
            bool any = false;
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
                            Price = !string.IsNullOrEmpty(productVariant.ListPrice)? decimal.Parse(productVariant.ListPrice)
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
                        foreach (var category in categories)
                        {
                            var c = new CategoryMenulistViewModel();
                            c.ID = category.ID.ToString();
                            c.Name = category.Name;
                            c.Url = LinkManager.GetItemUrl(category);
                            var categoryChildern = category.GetChildren();
                            c.ProductsCount = categoryChildern.Count();
                            var pList = new List<ProductMenulistViewModel>();
                            foreach (Item categoryChild in categoryChildern)
                            {
                                if (categoryChild.TemplateID.ToString() != Constants.Products.CategoriesTemplateId)
                                {
                                    var p = new ProductMenulistViewModel();
                                    p.Name = categoryChild.DisplayName;
                                    p.Url = LinkManager.GetItemUrl(categoryChild);
                                    pList.Add(p);
                                }
                            }
                            c.ProductMenulistViewModel = pList;
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

            var catParentItem = Context.Database.GetItem(new ID(parentId));

            try
            {
                if (catParentItem != null)
                {
                    var catItem =
                        catParentItem.Axes.GetDescendants()
                            .Where(
                                x =>
                                    x.TemplateName.Equals(Constants.Products.GeneralCategoryTemplateName,
                                        StringComparison.InvariantCultureIgnoreCase))
                            .ToList();

                    return catItem;
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
                var culture = Context.Language.CultureInfo;
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
                var culture = Context.Language.CultureInfo;
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
                    var orderDetail = new OrderDetailViewModel();
                    var orderHead = cartHelper.GetOrderHead(order.OrderID, order.CustomerId, order.ShopName);
                    var commerceOrderHead = orderHead.Order as CommerceOrder;
                    if (commerceOrderHead != null)
                    {
                        orderDetail.OrderDate = commerceOrderHead.Created.ToString(Constants.Products.DateFormat);
                        orderDetail.OrderID = commerceOrderHead.OrderID;
                        orderDetail.OrderStatus = commerceOrderHead.Status;
                        orderDetail.UserID = commerceOrderHead.UserId;
                        orderDetail.TotalPrice = commerceOrderHead.Total.Amount.ToString(Constants.Products.CurrencyFormat);
                        orderDetail.NumberofItems = commerceOrderHead.LineItemCount;
                        orderDetail.ExternalID = commerceOrderHead.ExternalId;
                    }
                    orderDetails.Add(orderDetail);
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
                orderDetail.OrderID = commerceOrderHead.OrderID;
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
                orderDetail.UserID = commerceOrderHead.UserId;
                orderDetail.ExternalID = commerceOrderHead.ExternalId;
                orderDetail.Billing =
                    commerceOrderHead.Parties.Cast<CommerceParty>()
                        .FirstOrDefault(party => party.Name == Constants.Products.BillingAddress);
                orderDetail.Shipping =
                    commerceOrderHead.Parties.Cast<CommerceParty>()
                        .FirstOrDefault(party => party.Name == Constants.Products.ShippingAddress);
                if (orderDetail.Billing == null)
                {
                    orderDetail.Billing = new CommerceParty();
                }
                if (orderDetail.Shipping == null)
                {
                    orderDetail.Shipping = new CommerceParty();
                }
                if (commerceOrderHead.Payment.ElementAtOrDefault(0) != null)
                {
                    var paymentMethodId = commerceOrderHead.Payment.ElementAt(0).PaymentMethodID;
                    orderDetail.PaymentMethod = GetPaymentMethod(paymentMethodId);
                }
                if (commerceOrderHead.Shipping.ElementAtOrDefault(0) != null)
                {
                    var shippingMethodId = commerceOrderHead.Shipping.ElementAt(0).ShippingMethodID;
                    orderDetail.ShippingMethod = GetShippingMethod(shippingMethodId);
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
                var line = (CommerceCartLine) cartLine;
                var orderLine = new OrderLine();
                orderLine.UnitPrice = line.Product.Price.Amount.ToString(Constants.Products.CurrencyFormat);
                orderLine.Quantity = line.Quantity;
                orderLine.SubTotal =
                    (line.Product.Price.Amount*line.Quantity).ToString(Constants.Products.CurrencyFormat);
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
                product = productItem.GlassCast<Product>();
                var productVariants = productItem.GetChildren().Select(x => x.GlassCast<ProductVariant>()).ToList();
                // Update Images and stockInfo in ProductVariant
                if (productVariants.Any())
                {
                    var theVariants = new List<ProductVariant>();
                    for (var i = 0; i < productVariants.Count(); i++)
                    {
                        productVariants[i] = UpdateVariantProperties(productVariants[i], product, cartHelper);
                        theVariants.Add(productVariants[i]);
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
                    ? string.Format("{0:#,###0} In Stock", Convert.ToInt32(productVariant.StockInformation.Count))
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
            var index = ContentSearchManager.GetIndex($"sitecore_{Sitecore.Context.Database}_index");
            try
            {
                var culture = Context.Language.CultureInfo;
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

            var rootItem = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.ContentStartPath);

            if (rootItem == null) return string.Empty;
            var rootModel = rootItem.GlassCast<Root>();
            return rootModel.Catalog != null ? rootModel.Catalog.ID.ToString() : string.Empty;

        }


        /// <summary>
        /// Returns a catalog item based on the catalog name
        /// </summary>
        /// <param name="catalogName"></param>
        /// <returns></returns>
        internal static Item GetCatalogItemByName(string catalogName)
        {
            var catalogRoot = Sitecore.Context.Database.GetItem(Constants.Products.CatalogsRootPath);

            if (catalogRoot != null)
            {
                return catalogRoot.Axes.GetDescendants().FirstOrDefault(x => x.Name.Equals(catalogName,StringComparison.InvariantCultureIgnoreCase));
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
                var catalog = Sitecore.Context.Database.GetItem(new ID(catalogId));
                if (catalog != null)
                {
                    var categories =
                        catalog.GetChildren()
                        .Where(x => x.TemplateName.Equals(Constants.Products.GeneralCategoryTemplateName,StringComparison.InvariantCultureIgnoreCase));

                    catalogCategories.AddRange(categories.Select(c => c.GlassCast<Category>()));
                }
            }
            return catalogCategories;
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
                    var catalog = Sitecore.Context.Database.GetItem(new ID(catalogId));
                    if (catalog != null)
                    {
                        var childitems =
                            catalog.Axes.GetDescendants()
                                .Where(n => n.Name.Equals(categoryName,StringComparison.CurrentCultureIgnoreCase))
                                .ToList();

                        if (childitems.Any())
                        {
                            categoryChildIds.AddRange(childitems.Select(i=>i.ID.ToString()));
                        }

                    }
                }

                return categoryChildIds.Aggregate((current, next) => current + Constants.Common.PipeStringSeparator + next);
            }

            return string.Empty;
        }
    }
}