#region

using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.LuceneProvider;
using Sitecore.ContentSearch.Linq;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using CSDemo;
using Sitecore.Data;
using System;
using Sitecore.Diagnostics;
using Glass.Mapper.Sc;
using Sitecore.Links;
using CSDemo.Models.Checkout.Cart;
using CSDemo.Models.Account;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Entities.Carts;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;

#endregion

namespace CSDemo.Models.Product
{
    public static class ProductHelper
    {

        /// <summary>
        /// 
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
        /// 
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
        /// 
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
        /// 
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

                        SearchResultItem searchedItem = GetSearchResultItemById(categoryID);

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
        /// 
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static SearchResultItem GetSearchResultItemById(string itemID)
        {
            var index = ContentSearchManager.GetIndex(Constants.WebIndex);
            try
            {
                var culture = Sitecore.Context.Language.CultureInfo;
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Sitecore.Context.Language.Name);
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
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal static CategoryProductViewModel GetCategoryProducts(PaginationViewModel model)
        {
            var categoryProductVM = new CategoryProductViewModel();

            categoryProductVM.PaginationViewModel = model;

            var category = new Category();

            categoryProductVM.CategoryMenulist = GetCategoryMenuList(Constants.Products.CategoriesParentId);

            SearchResultItem searchedItem = GetSearchResultItemById(model.CategoryID);

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

                    // Update ProductVariants
                    foreach (var product in category.Products)
                    {
                        Item catProdItem = catItem.GetChildren().FirstOrDefault(x => x.ID.ToGuid() == product.ID);
                        if (catProdItem != null && catProdItem.HasChildren)
                        {
                            product.ProductVariants = catProdItem.GetChildren().Select(x=>x.GlassCast<ProductVariant>());                         
                        }
                    }  
                }

            }

            categoryProductVM.Category = category;

            return categoryProductVM;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private static IEnumerable<CategoryMenulistViewModel> GetCategoryMenuList(string parentID)
        {
            var CategoryMenulistViewModel = new List<CategoryMenulistViewModel>();
            
            List<SearchResultItem> categories = GetCategoryMenuListByParentID(parentID);

            if (categories != null)
            {

                foreach (SearchResultItem category in categories)
                {
                    var c = new CategoryMenulistViewModel();
                    c.ID = category.ItemId.ToString();
                    c.Name = category.Name;
                    var categoryChildern = category.GetItem().GetChildren();
                    c.ProductsCount = categoryChildern.Count();

                    var pList = new List<ProductMenulistViewModel>();

                    foreach (Item categoryChild in categoryChildern)
                    {
                        var p = new ProductMenulistViewModel();
                        p.Name = categoryChild.DisplayName;
                        p.Url = LinkManager.GetItemUrl(categoryChild);
                        pList.Add(p);
                    }
                    c.ProductMenulistViewModel = pList;
                    CategoryMenulistViewModel.Add(c);
                    
                }

            }
            CategoryMenulistViewModel = CategoryMenulistViewModel.OrderBy(i => i.Name).ToList();
            return CategoryMenulistViewModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private static List<SearchResultItem> GetCategoryMenuListByParentID(string parentID)
        {
            var index = ContentSearchManager.GetIndex(Constants.WebIndex);
            try
            {
                var culture = Sitecore.Context.Language.CultureInfo;
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Sitecore.Context.Language.Name);
                    return queryable.Where(x => x.Parent == ID.Parse(parentID)).ToList();

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, ex);

            }

            return null;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        internal static string GetItemIDFromName(string itemName, string parentID)
        {
            var index = ContentSearchManager.GetIndex(Constants.WebIndex);
            try
            {
                var culture = Sitecore.Context.Language.CultureInfo;
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Sitecore.Context.Language.Name);
                    return queryable.FirstOrDefault(x => String.Equals(x.Name, itemName, StringComparison.CurrentCultureIgnoreCase) && x.Parent == ID.Parse(parentID)).ItemId.ToString();

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
        /// <param name="itemName"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        internal static SearchResultItem GetItemByProductID(string productID)
        {
            var index = ContentSearchManager.GetIndex(Constants.WebIndex);
            try
            {
                var culture = Sitecore.Context.Language.CultureInfo;
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Sitecore.Context.Language.Name);
                    return queryable.FirstOrDefault(x => String.Equals(x.Name, productID, StringComparison.CurrentCultureIgnoreCase));

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
        /// 
        /// </summary>
        /// <param name="cartHelper"></param>
        /// <returns></returns>
        internal static OrdersViewModel GetCustomerOrders(CartHelper cartHelper)
        {
            var ordersViewModel = new OrdersViewModel();
            List<OrderDetailViewModel> orderDetails = new List<OrderDetailViewModel>();

            var orders = cartHelper.GetOrders(cartHelper.GetVisitorID(), cartHelper.ShopName);
            if (orders != null)
            {
                
                foreach (var order in orders.OrderHeaders) {
                    var orderDetail = new OrderDetailViewModel();

                    var orderHead = cartHelper.GetOrderHead(order.OrderID, order.CustomerId, order.ShopName);
                    var commerceOrderHead = orderHead.Order as CommerceOrder;
                  
                    orderDetail.OrderDate = commerceOrderHead.Created.ToString("MMMM dd, yyyy");
                    orderDetail.OrderID = commerceOrderHead.OrderID;
                    orderDetail.OrderStatus = commerceOrderHead.Status;
                    orderDetail.UserID = commerceOrderHead.UserId.ToString();
                    orderDetail.TotalPrice = commerceOrderHead.Total.Amount.ToString("C2");
                    orderDetail.NumberofItems = commerceOrderHead.LineItemCount;
                    orderDetail.ExternalID = commerceOrderHead.ExternalId.ToString();

                    orderDetails.Add(orderDetail);
                }
                ordersViewModel.Orders = orderDetails;
            }

            return ordersViewModel;
        }

        internal static OrderDetailViewModel GetCustomerOrderDetail(string orderID, CartHelper cartHelper)
        {
            var orderDetail = new OrderDetailViewModel();

            var orderHead = cartHelper.GetOrderHead(orderID, cartHelper.GetVisitorID(), cartHelper.ShopName);
            var commerceOrderHead = orderHead.Order as CommerceOrder;

            if (commerceOrderHead != null)
            {
                orderDetail.OrderID = commerceOrderHead.OrderID;
                orderDetail.OrderDate = commerceOrderHead.Created.ToString("MMMM dd, yyyy hh:mm");
                orderDetail.NumberofItems = commerceOrderHead.LineItemCount;

                var commerceTotal = commerceOrderHead.Total as CommerceTotal;

                orderDetail.SubTotalPrice = commerceTotal.Subtotal.ToString("C2"); 
                orderDetail.TotalPrice = commerceTotal.Amount.ToString("C2");
                orderDetail.Tax = commerceTotal.TaxTotal.Amount.ToString("C2");
                orderDetail.ShippingCost = commerceTotal.ShippingTotal.ToString("C2");
                orderDetail.OrderStatus = commerceOrderHead.Status;
                orderDetail.UserID = commerceOrderHead.UserId;
                orderDetail.ExternalID = commerceOrderHead.ExternalId;

                orderDetail.Billing = commerceOrderHead.Parties.Cast<CommerceParty>().FirstOrDefault(party => party.Name == Constants.Products.BillingAddress);
                orderDetail.Shipping = commerceOrderHead.Parties.Cast<CommerceParty>().FirstOrDefault(party => party.Name == Constants.Products.ShippingAddress);

                var paymentMethodID = commerceOrderHead.Payment.ElementAt(0).PaymentMethodID;
                var shippingMethodID = commerceOrderHead.Shipping.ElementAt(0).ShippingMethodID;
                orderDetail.ShippingMethod = GetShippingMethod(shippingMethodID);

                orderDetail.PaymentMethod = GetPaymentMethod(paymentMethodID);
                
                var Lines = commerceOrderHead.Lines.ToList();
                orderDetail.OrderLines = GetOrderLines(Lines);
              
            }

            return orderDetail;
        }

        private static string GetPaymentMethod(string paymentMethodID)
        {
                        
            var resp = string.Empty;

            try
            {
            var provider = new Sitecore.Commerce.Services.Payments.PaymentServiceProvider();
            var paymentRequest = new CommerceGetPaymentMethodsRequest(Sitecore.Context.Language.ToString());
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

        private static IEnumerable<OrderLine> GetOrderLines(IEnumerable<CartLine> Lines)
        {
            var orderLines = new List<OrderLine>();
            foreach (CommerceCartLine line in Lines)
            {
             //   var line = cartLine as CommerceCartLine;
                var ol = new OrderLine();
                ol.UnitPrice = line.Product.Price.Amount.ToString("C2");
                ol.Quantity = line.Quantity;
                ol.SubTotal = (line.Product.Price.Amount * line.Quantity).ToString("C2");
                var product = line.Product as CommerceCartProduct;
                ol.ProductName = product.DisplayName;
                var sItem = GetItemByProductID(product.ProductId);
                if (sItem != null)
                {
                    ol.ImageUrl = GetFirstImageFromProductItem(sItem.GetItem());
                    ol.Url = LinkManager.GetItemUrl(sItem.GetItem());
                }
                orderLines.Add(ol);
            }

            return orderLines;
        }

        private static string GetShippingMethod(string ShippingMethodID)
        {
            var resp = string.Empty;

            try
            {

                var provider = new Sitecore.Commerce.Services.Shipping.ShippingServiceProvider();
                var shippingRequest = new Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines.CommerceGetShippingMethodsRequest(Sitecore.Context.Language.ToString());
                var shippingResult = provider.GetShippingMethods(shippingRequest);

                resp = shippingResult.ShippingMethods.FirstOrDefault(x => x.ExternalId == ShippingMethodID).Name;

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }

            return resp;
        }


        internal static Product GetProductByNameAndCategory(string productID, string categoryID)
        {
            Product product = new Product();
            var productResult = GetItemByName(productID);
            if (productResult != null)
            {
                Item productItem = productResult.GetItem();
                product = productItem.GlassCast<Product>();
                product.ProductVariants = productItem.GetChildren().Select(x => x.GlassCast<ProductVariant>());                         
            }

            return product;
        }

        internal static SearchResultItem GetItemByName(string productID)
        {
            var index = ContentSearchManager.GetIndex(Constants.WebIndex);
            try
            {
                var culture = Sitecore.Context.Language.CultureInfo;
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Sitecore.Context.Language.Name);
                    return queryable.FirstOrDefault(x => String.Equals(x.Name, productID, StringComparison.CurrentCultureIgnoreCase));

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, ex);
            }

            return null;

        }
    }
}