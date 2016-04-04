#region

using System;
using System.Collections.Generic;
using System.Linq;
using CSDemo.Models.Account;
using CSDemo.Models.Checkout.Cart;
using Glass.Mapper.Sc;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Analytics.Data;
using Sitecore.Analytics.Tracking;
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

#endregion

namespace CSDemo.Models.Product
{
    public static class ProductHelper
    {
        /// <summary>
        ///     CSDEMO#115
        /// </summary>
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
            var ProductImage = string.Empty;
            if (product != null)
            {
                var ProductImageIds = product[Constants.Products.ImagesField];
                if (!string.IsNullOrEmpty(ProductImageIds))
                {
                    ProductImage = GetFirstImageFromField(ProductImageIds);
                }
            }
            return ProductImage;
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
                var images = imageIds.Split(Constants.Products.IDSeparator.ToCharArray());
                if (!string.IsNullOrEmpty(images[0]))
                {
                    image = string.Format(Constants.Products.ImagesUrlFormat, ID.Parse(images[0]).ToShortID());
                }
            }
            return image;
        }

        /// <summary>
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        public static string GetFirstImageFromItems(IEnumerable<Item> images)
        {
            var image = string.Empty;
            if (images != null)
            {
                if (images.Count() > 0)
                {
                    var firstImage = images.First();
                    image = string.Format(Constants.Products.ImagesUrlFormat, firstImage.ID.ToShortID());
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
                    foreach (var categoryID in categoryIDs.Split(Constants.Products.IDSeparator.ToCharArray()))
                    {
                        var category = new Category();
                        var searchedItem = GetSearchResultItemById(categoryID);
                        category = searchedItem.GetItem().GlassCast<Category>();
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
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static SearchResultItem GetSearchResultItemById(string itemID)
        {
            var index = ContentSearchManager.GetIndex(Constants.WebIndex);
            try
            {
                var culture = Context.Language.CultureInfo;
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Context.Language.Name);
                    return queryable.FirstOrDefault(x => x.ItemId == ID.Parse(itemID));
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
            var categoryProductVM = new CategoryProductViewModel();
            categoryProductVM.PaginationViewModel = model;
            var category = new Category();
            categoryProductVM.CategoryMenulist = GetCategoryMenuList(Constants.Products.CategoriesParentId);
            var searchedItem = GetSearchResultItemById(model.CategoryID);
            if (searchedItem != null)
            {
                var catItem = searchedItem.GetItem();
                category = catItem.GlassCast<Category>();
                if (catItem.HasChildren)
                {
                    var catChildren = catItem.GetChildren().Select(x => x.GlassCast<Product>()).ToList();
                    model.TotalItems = catChildren.Count();
                    model.TotalPages = (long)Math.Ceiling((double)model.TotalItems / model.PageSize);
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
                        .Skip(model.PageSize * (model.CurrentPage - 1))
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
                            BuildUIVariants(product);
                        }
                        var stockInfo = cartHelper.GetProductStockInformation(product.ProductId, product.CatalogName);
                    }
                }
            }
            categoryProductVM.Category = category;
            return categoryProductVM;
        }

        /// <summary>
        /// </summary>
        /// <param name="product"></param>
        private static void BuildUIVariants(Product product)
        {
            var cultureInfo = Context.Culture;
            if (product.ProductVariants.Count() > 0)
            {
                var variantBox = new VariantBox();
                var variantBoxLines = new List<VariantBoxLine>();
                foreach (var productVariant in product.ProductVariants)
                {
                    var variantBoxLine = new VariantBoxLine();
                    variantBoxLine.VariantID = productVariant.VariantId;
                    variantBoxLine.Size = !string.IsNullOrEmpty(productVariant.ProductSize)
                        ? productVariant.ProductSize.Trim()
                        : string.Empty;
                    variantBoxLine.Color = !string.IsNullOrEmpty(productVariant.ProductColor)
                        ? productVariant.ProductColor.Trim()
                        : string.Empty;
                    variantBoxLine.Price =
                        decimal.Parse(productVariant.ListPrice)
                            .ToString(Constants.Products.CurrencyDecimalFormat, cultureInfo);



                    //if (productVariant.Variant_Images != null && productVariant.Variant_Images.Count() > 0)
                    //{
                    //    variantBoxLine.Images =
                    //        productVariant.Variant_Images.Select(x => x.ID.ToShortID().ToString())
                    //            .ToList()
                    //            .Aggregate(
                    //                (i, j) =>
                    //                    string.Format(Constants.Products.ImagesUrlFormat, i) + Constants.Common.Comma +
                    //                    string.Format(Constants.Products.ImagesUrlFormat, j));
                    //}
                    //else
                    //{
                    //    variantBoxLine.Images =
                    //        product.Images.Select(x => x.Src)
                    //            .ToList()
                    //            .Aggregate((i, j) => i + Constants.Common.Comma + j);
                    //}
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
                if (availSizes.Count() > 0)
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
                        if (availColors.Count() > 0)
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
                    if (availColors.Count() > 0)
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
        /// <param name="parentID"></param>
        /// <returns></returns>
        private static IEnumerable<CategoryMenulistViewModel> GetCategoryMenuList(string parentID)
        {
            var CategoryMenulistViewModel = new List<CategoryMenulistViewModel>();
            var categories = GetCategoryMenuListByParentID(parentID);
            if (categories != null)
            {
                foreach (var category in categories)
                {
                    var c = new CategoryMenulistViewModel();
                    c.ID = category.ItemId.ToString();
                    c.Name = category.Name;
                    c.Url = LinkManager.GetItemUrl(category.GetItem());
                    var categoryChildern = category.GetItem().GetChildren();
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
                    CategoryMenulistViewModel.Add(c);
                }
            }
            CategoryMenulistViewModel = CategoryMenulistViewModel.OrderBy(i => i.Name).ToList();
            return CategoryMenulistViewModel;
        }

        /// <summary>
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private static List<SearchResultItem> GetCategoryMenuListByParentID(string parentID)
        {
            var index = ContentSearchManager.GetIndex(Constants.WebIndex);
            try
            {
                var culture = Context.Language.CultureInfo;
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Context.Language.Name);
                    return queryable.Where(x => x.Parent == ID.Parse(parentID) && x.TemplateName == "GeneralCategory").ToList();
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
        /// <param name="parentID"></param>
        /// <returns></returns>
        internal static string GetItemIDFromName(string itemName, string parentID)
        {
            var index = ContentSearchManager.GetIndex(Constants.WebIndex);
            try
            {
                var culture = Context.Language.CultureInfo;
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Context.Language.Name);
                    return
                        queryable.FirstOrDefault(
                            x =>
                                string.Equals(x.Name, itemName, StringComparison.CurrentCultureIgnoreCase) &&
                                x.TemplateName == "GeneralCategory").ItemId.ToString();

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, ex);
            }
            return string.Empty;

            //                                 (x.TemplateName == "GeneralCategory")).ToString();
            //try
            //{
            //    var culture = Context.Language.CultureInfo;
            //    using (var context = index.CreateSearchContext())
            //    {
            //        var queryable = context.GetQueryable<SearchResultItem>()
            //            .Where(x => x.Language == Context.Language.Name);
            //        return
            //            queryable.FirstOrDefault(
            //                x =>
            //                    string.Equals(x.Name, itemName, StringComparison.CurrentCultureIgnoreCase) &&
            //                    x.Parent == ID.Parse(parentID)).ItemId.ToString();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex.StackTrace, ex);
            //}
        }

        /// <summary>
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        internal static SearchResultItem GetItemByProductID(string productID)
        {
            var index = ContentSearchManager.GetIndex(Constants.WebIndex);
            try
            {
                var culture = Context.Language.CultureInfo;
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Context.Language.Name);
                    return
                        queryable.FirstOrDefault(
                            x => string.Equals(x.Name, productID, StringComparison.CurrentCultureIgnoreCase));
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
            var orders = cartHelper.GetOrders(cartHelper.GetVisitorID(), cartHelper.ShopName);
            if (orders != null)
            {
                foreach (var order in orders.OrderHeaders)
                {
                    var orderDetail = new OrderDetailViewModel();
                    var orderHead = cartHelper.GetOrderHead(order.OrderID, order.CustomerId, order.ShopName);
                    var commerceOrderHead = orderHead.Order as CommerceOrder;
                    orderDetail.OrderDate = commerceOrderHead.Created.ToString(Constants.Products.DateFormat);
                    orderDetail.OrderID = commerceOrderHead.OrderID;
                    orderDetail.OrderStatus = commerceOrderHead.Status;
                    orderDetail.UserID = commerceOrderHead.UserId;
                    orderDetail.TotalPrice = commerceOrderHead.Total.Amount.ToString(Constants.Products.CurrencyFormat);
                    orderDetail.NumberofItems = commerceOrderHead.LineItemCount;
                    orderDetail.ExternalID = commerceOrderHead.ExternalId;
                    orderDetails.Add(orderDetail);
                }
                ordersViewModel.Orders = orderDetails;
            }
            return ordersViewModel;
        }

        /// <summary>
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="cartHelper"></param>
        /// <returns></returns>
        internal static OrderDetailViewModel GetCustomerOrderDetail(string orderID, CartHelper cartHelper)
        {
            var orderDetail = new OrderDetailViewModel();
            var orderHead = cartHelper.GetOrderHead(orderID, cartHelper.GetVisitorID(), cartHelper.ShopName);
            var commerceOrderHead = orderHead.Order as CommerceOrder;
            if (commerceOrderHead != null)
            {
                orderDetail.OrderID = commerceOrderHead.OrderID;
                orderDetail.OrderDate = commerceOrderHead.Created.ToString(Constants.Products.DateTimeFormat);
                orderDetail.NumberofItems = commerceOrderHead.LineItemCount;
                var commerceTotal = commerceOrderHead.Total as CommerceTotal;
                orderDetail.SubTotalPrice = commerceTotal.Subtotal.ToString(Constants.Products.CurrencyFormat);
                orderDetail.TotalPrice = commerceTotal.Amount.ToString(Constants.Products.CurrencyFormat);
                orderDetail.Tax = commerceTotal.TaxTotal.Amount.ToString(Constants.Products.CurrencyFormat);
                orderDetail.ShippingCost = commerceTotal.ShippingTotal.ToString(Constants.Products.CurrencyFormat);
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
                    var paymentMethodID = commerceOrderHead.Payment.ElementAt(0).PaymentMethodID;
                    orderDetail.PaymentMethod = GetPaymentMethod(paymentMethodID);
                }
                if (commerceOrderHead.Shipping.ElementAtOrDefault(0) != null)
                {
                    var shippingMethodID = commerceOrderHead.Shipping.ElementAt(0).ShippingMethodID;
                    orderDetail.ShippingMethod = GetShippingMethod(shippingMethodID);
                }
                var Lines = commerceOrderHead.Lines.ToList();
                orderDetail.OrderLines = GetOrderLines(Lines);
            }
            return orderDetail;
        }

        /// <summary>
        /// </summary>
        /// <param name="paymentMethodID"></param>
        /// <returns></returns>
        private static string GetPaymentMethod(string paymentMethodID)
        {
            var resp = string.Empty;
            try
            {
                var provider = new PaymentServiceProvider();
                var paymentRequest = new CommerceGetPaymentMethodsRequest(Context.Language.ToString());
                var paymentResult = provider.GetPaymentMethods(paymentRequest);
                var paymentMethods = paymentResult.PaymentMethods;
                resp = paymentResult.PaymentMethods.FirstOrDefault(x => x.ExternalId == paymentMethodID).Name;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            return resp;
        }

        /// <summary>
        /// </summary>
        /// <param name="Lines"></param>
        /// <returns></returns>
        private static IEnumerable<OrderLine> GetOrderLines(IEnumerable<CartLine> Lines)
        {
            var orderLines = new List<OrderLine>();
            foreach (CommerceCartLine line in Lines)
            {
                //   var line = cartLine as CommerceCartLine;
                var orderLine = new OrderLine();
                orderLine.UnitPrice = line.Product.Price.Amount.ToString(Constants.Products.CurrencyFormat);
                orderLine.Quantity = line.Quantity;
                orderLine.SubTotal =
                    (line.Product.Price.Amount * line.Quantity).ToString(Constants.Products.CurrencyFormat);
                var product = line.Product as CommerceCartProduct;
                orderLine.ProductName = product.DisplayName;
                var sItem = GetItemByProductID(product.ProductId);
                if (sItem != null)
                {
                    orderLine.ImageUrl = GetFirstImageFromProductItem(sItem.GetItem());
                    orderLine.Url = LinkManager.GetItemUrl(sItem.GetItem());
                }
                orderLines.Add(orderLine);
            }
            return orderLines;
        }

        /// <summary>
        /// </summary>
        /// <param name="ShippingMethodID"></param>
        /// <returns></returns>
        private static string GetShippingMethod(string ShippingMethodID)
        {
            var resp = string.Empty;
            try
            {
                var provider = new ShippingServiceProvider();
                var shippingRequest = new CommerceGetShippingMethodsRequest(Context.Language.ToString());
                var shippingResult = provider.GetShippingMethods(shippingRequest);
                resp = shippingResult.ShippingMethods.FirstOrDefault(x => x.ExternalId == ShippingMethodID).Name;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            return resp;
        }

        /// <summary>
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        internal static Product GetProductByNameAndCategory(string productID, string categoryID)
        {
            var product = new Product();
            var cartHelper = new CartHelper();
            var productResult = GetItemByName(productID);
            if (productResult != null)
            {
                var productItem = productResult.GetItem();
                product = productItem.GlassCast<Product>();
                var ProductVariants = productItem.GetChildren().Select(x => x.GlassCast<ProductVariant>()).ToList();

                // Update Images and stockInfo in ProductVariant
                if (ProductVariants.Count() > 0)
                {
                    List<ProductVariant> theVariants = new List<ProductVariant>();

                    for (var i = 0; i < ProductVariants.Count(); i++)
                    {
                        ProductVariants[i] = UpdateVariantProperties(ProductVariants[i], product, cartHelper);
                        theVariants.Add(ProductVariants[i]);
                    }
                    product.ProductVariants = theVariants;
                }

                

                BuildUIVariants(product);



                // CSDEMO#99
                if (!string.IsNullOrEmpty(product.DefaultVariant))
                {
                    product.StockInformation = cartHelper.GetProductStockInformation(product.ProductId, product.CatalogName, product.DefaultVariant);
                }
                else
                {
                    product.StockInformation = cartHelper.GetProductStockInformation(product.ProductId, product.CatalogName);
                }
            }
            return product;
        }

        private static ProductVariant UpdateVariantProperties(ProductVariant productVariant, Product product, CartHelper cartHelper)
        {
            if (productVariant.Variant_Images == null || productVariant.Variant_Images.Count() == 0) { productVariant.Variant_Images = product.Images.Select(x => x); }

            productVariant.StockInformation = cartHelper.GetProductStockInformation(product.ProductId, product.CatalogName, productVariant.VariantId);

            if (productVariant.StockInformation != null)
            {
                productVariant.StockLabel = productVariant.StockInformation != null && productVariant.StockInformation.Count > 0 ? string.Format("{0:#,###0} In Stock", System.Convert.ToInt32(productVariant.StockInformation.Count)) : "Out Of Stock";
                productVariant.StockQuantity = System.Convert.ToInt32(productVariant.StockInformation.Count);
            }

            return productVariant;
        }

        /// <summary>
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        internal static SearchResultItem GetItemByName(string productID)
        {
            //
            var index = ContentSearchManager.GetIndex(Constants.WebIndex);
            try
            {
                var culture = Context.Language.CultureInfo;
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Context.Language.Name);
                    return
                        queryable.FirstOrDefault(
                            x => string.Equals(x.Name, productID, StringComparison.CurrentCultureIgnoreCase));
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
    }
}