using CSDemo.Models.Account;
using CSDemo.Models.Product;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Commerce.Connect.CommerceServer;
using Sitecore.Commerce.Connect.CommerceServer.Caching;
using Sitecore.Commerce.Connect.CommerceServer.Inventory.Models;
using Sitecore.Commerce.Connect.CommerceServer.Orders;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
using Sitecore.Commerce.Contacts;
using Sitecore.Commerce.Entities;
using Sitecore.Commerce.Entities.Carts;
using Sitecore.Commerce.Entities.Inventory;
using Sitecore.Commerce.Services.Carts;
using Sitecore.Commerce.Services.Customers;
using Sitecore.Commerce.Services.Inventory;
using Sitecore.Commerce.Services.Orders;
using Sitecore.Commerce.Engine.Connect.Pipelines.Arguments;
using Sitecore.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using CSDemo.Configuration;
using CSDemo.Models.Page;
using AddPartiesRequest = Sitecore.Commerce.Services.Carts.AddPartiesRequest;
using UpdatePartiesRequest = Sitecore.Commerce.Services.Carts.UpdatePartiesRequest;
using Sitecore.Commerce.Services.Payments;
using Sitecore.Commerce.Entities.Payments;
using WebGrease.Css.Extensions;
using Sitecore.Commerce.Entities.Shipping;
using Sitecore.Commerce.Services.Shipping;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Commerce.Pipelines;
using CSDemo.Helpers;

namespace CSDemo.Models.Checkout.Cart
{
    public class CartHelper
    {
        public string ShopName { get; set; }
        public string DefaultCartName { get; set; }

        private readonly InventoryServiceProvider _inventoryServiceProvider = new InventoryServiceProvider();
        private readonly KeefePOC.Pipelines.Providers.CartServiceProvider _cartServiceProvider = new KeefePOC.Pipelines.Providers.CartServiceProvider();
        private readonly PaymentServiceProvider _paymentServiceProvider = new PaymentServiceProvider();
        private readonly OrderServiceProvider _orderServiceProvider = new OrderServiceProvider();
        private readonly ContactFactory _contactFactory = new ContactFactory();
        private readonly AccountHelper _accountService;

        public CartHelper(string shopName)
        {
            if (Tracker.Current == null)
            {
                Tracker.StartTracking();
            }
            ShopName = shopName;
            DefaultCartName = CommerceConstants.CartSettings.DefaultCartName;
            _accountService = new AccountHelper(this, new CustomerServiceProvider());
        }

        public CartHelper()
        {
            if (Tracker.Current == null)
            {
                Tracker.StartTracking();
            }
            ShopName = Context.Site.Name;
            DefaultCartName = CommerceConstants.CartSettings.DefaultCartName;
            _accountService = new AccountHelper(this, new CustomerServiceProvider());
        }

        public string AddProductToCart(string quantity, string productId, string catalogName, string variantId)
        {
            var loggedIn = Sitecore.Context.User.IsAuthenticated;
            if (!loggedIn)
            {
                return "Anonymous";
            }


			var inmateId = InmateHelper.GetSelectedInmateId();

			if (string.IsNullOrEmpty(inmateId))
			{
				return "No Inmate Selected";
			}


			var ret = string.Empty;
            // Create cart object
            var cartLineItem = new CartLineItem
            {
                Quantity = uint.Parse(quantity),
                ProductId = productId,
                CatalogName = catalogName,
                VariantId = variantId
            };
            var cart = AddToCart(cartLineItem);
            if (cart == null || cart.Properties[Constants.Cart.BasketErrors] != null)
            {
                // no cart OR _basket_errors present
                ret = Constants.Cart.ErrorInBasket;
            }
            return ret;
        }

        public bool ViewCartPromo()
        {
            ServiceProviderRequest request = new ServiceProviderRequest();

            var result = _cartServiceProvider.ViewCart(request);

            return result.Success;

        }

        public CommerceCart AddToCart(CartLineItem cartLine)
        {

			var inmateId = InmateHelper.GetSelectedInmateId();

			if (string.IsNullOrEmpty(inmateId))
			{
				return null;
			}

			// create cartitem
			var cartItem = new CommerceCartLine(cartLine.CatalogName, cartLine.ProductId,
                cartLine.VariantId == "-1" ? null : cartLine.VariantId, cartLine.Quantity);

            // update stock in formation
            // push cart to commerce server
            UpdateStockInformation(cartItem, cartLine.CatalogName);

            // get userID
            var visitorId = GetVisitorId();
            var request = new AddCartLinesRequest(GetCart(visitorId), new[] { cartItem });
            var info = CartRequestInformation.Get(request);
            if (info == null)
            {
                info = new CartRequestInformation(request, true);
            }
            else
            {
                info.Refresh = true;
            }
					

			request.Properties.Add("InmateNumber", inmateId);
			

            var cartResult = _cartServiceProvider.AddCartLines(request);

            // add cart to cache
            var cart = cartResult.Cart as CommerceCart;
            UpdateCartInCache(cart);

            return cartResult.Cart as CommerceCart;
        }


        /// <summary>
        ///     AddCart To Cache
        /// </summary>
        /// <param name="cart"></param>
        private void AddCartToCache(CommerceCart cart)
        {
            var cacheProvider = GetCacheProvider();
            var id = Guid.Parse(cart.CustomerId).ToString("D");
            if (cacheProvider.Contains(CommerceConstants.KnownCachePrefixes.Sitecore,
                CommerceConstants.KnownCacheNames.CommerceCartCache, id))
            {
                var msg = string.Format(CultureInfo.InvariantCulture,
                    Constants.Cart.CartAlreadyInCache, id);
                CommerceTrace.Current.Write(msg);
            }
            cacheProvider.AddData(CommerceConstants.KnownCachePrefixes.Sitecore,
                CommerceConstants.KnownCacheNames.CommerceCartCache, id, cart);
            CreateCustomerCartCookie(id);
        }

        /// <summary>
        ///     Create Customer Cart Cookie
        /// </summary>
        /// <param name="customerId"></param>
        private void CreateCustomerCartCookie(string customerId)
        {
            const int cookieExpirationInDays = 365;
            const string cookieName = Constants.Cart.CookieName;
            var cartCookie = HttpContext.Current.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);

            cartCookie.Values[Constants.Cart.VisitorID] = customerId;
            cartCookie.Expires = DateTime.Now.AddDays(cookieExpirationInDays);
            HttpContext.Current.Response.Cookies.Add(cartCookie);
        }


        /// <summary>
        /// Clear Cart From Cache
        /// </summary>
        private void ClearCartFromCache()
        {
            var id = Guid.Parse(GetVisitorId()).ToString("D");
            ClearUserCartFromCache(id);
        }


        /// <summary>
        /// Clear UserCart From Cache
        /// </summary>
        /// <param name="userid"></param>
        private void ClearUserCartFromCache(string userid)
        {
            var cacheProvider = GetCacheProvider();
            if (
                !cacheProvider.Contains(CommerceConstants.KnownCachePrefixes.Sitecore,
                    CommerceConstants.KnownCacheNames.CommerceCartCache, userid))
            {
                var msg = string.Format(CultureInfo.InvariantCulture,
                    Constants.Cart.CartInvalidInCache, userid);
                CommerceTrace.Current.Write(msg);
            }
            cacheProvider.RemoveData(CommerceConstants.KnownCachePrefixes.Sitecore,
                CommerceConstants.KnownCacheNames.CommerceCartCache, userid);
            DeleteCustomerCartCookie(userid);
        }

        /// <summary>
        ///     Delete Customer Cart Cookie
        /// </summary>
        /// <param name="id"></param>
        private void DeleteCustomerCartCookie(string id)
        {
            const string cookieName = Constants.Cart.CookieName;
            var cartCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cartCookie == null)
            {
                return;
            }
            // Render cookie invalid
            HttpContext.Current.Response.Cookies.Remove(cookieName);
            cartCookie.Expires = DateTime.Now.AddDays(-10);
            cartCookie.Values[Constants.Cart.VisitorID] = null;
            cartCookie.Value = null;
            HttpContext.Current.Response.SetCookie(cartCookie);
        }

        /// <summary>
        ///     Get Cache Provider
        /// </summary>
        /// <returns></returns>
        private ICacheProvider GetCacheProvider()
        {
            var cacheProvider = CommerceTypeLoader.GetCacheProvider(CommerceConstants.KnownCacheNames.CommerceCartCache);
            return cacheProvider;
        }


        /// <summary>
        /// Get cart from Commerce server
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="refreshCart"></param>
        /// <returns></returns>
        private CommerceCart GetCart(string userName, bool refreshCart = false)
        {

            var request = new LoadCartByNameRequest(ShopName, DefaultCartName, userName);
            var info = CartRequestInformation.Get(request);
            if (info == null)
            {
                info = new CartRequestInformation(request, refreshCart);
            }
            else
            {
                info.Refresh = refreshCart;
            }
            var result = _cartServiceProvider.LoadCart(request);
            return result.Cart as CommerceCart;
        }

        /// <summary>
        ///     Get a commerceuser cart From Cache
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public CommerceCart GetFromCacheCart(string customerId)
        {
            var cacheProvider = GetCacheProvider();
            var id = Guid.Parse(customerId).ToString("D");
            var cart = cacheProvider.GetData<CommerceCart>(CommerceConstants.KnownCachePrefixes.Sitecore,
                CommerceConstants.KnownCacheNames.CommerceCartCache, id);
            if (cart == null)
            {
                var msg = string.Format(CultureInfo.InvariantCulture,
                    Constants.Cart.CartNotInCache, id);
                CommerceTrace.Current.Write(msg);
            }
            return cart;
        }

        /// <summary>
        /// Get Product Stock Information
        ///  </summary>
        /// <param name="productId"></param>
        /// <param name="catalogName"></param>
        /// <returns></returns>
        internal StockInformation GetProductStockInformation(string productId, string catalogName, string variantId = "")
        {
            if (productId.Contains("("))
            {
                var virtualProductId = productId;
                productId = GetRealDataFromVirtual(virtualProductId, 0);
                catalogName = GetRealDataFromVirtual(virtualProductId, 1);

                if (variantId.Contains("("))
                {
                    variantId = GetRealDataFromVirtual(variantId, 0);
                }
            }

            var commerceInventoryProduct = new CommerceInventoryProduct
            {
                ProductId = productId,
                CatalogName = catalogName
            };

            if (!string.IsNullOrEmpty(variantId)) { commerceInventoryProduct.VariantId = variantId; }
            var products = new List<InventoryProduct> { commerceInventoryProduct };

            var stockInfoRequest = new GetStockInformationRequest(ShopName, products, StockDetailsLevel.All);
            var stockInfoResult = _inventoryServiceProvider.GetStockInformation(stockInfoRequest);
            return stockInfoResult.StockInformation.FirstOrDefault();
        }

        /// <summary>
        /// Get real Ids and catalog name from virtual ids
        /// </summary>
        /// <param name="virtualCatalogId"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private string GetRealDataFromVirtual(string virtualCatalogId, int position)
        {
            var virtualData = virtualCatalogId.Replace(")", string.Empty).Split('(');

            return virtualData[position];
        }

        /// <summary>
        /// Update Stock Information
        /// </summary>
        /// <param name="cartLine"></param>
        /// <param name="catalogName"></param>
        private void UpdateStockInformation(CommerceCartLine cartLine, string catalogName)
        {
            var products = new List<InventoryProduct>
            {
                new CommerceInventoryProduct {ProductId = cartLine.Product.ProductId, CatalogName = catalogName}
            };
            var stockInfoRequest = new GetStockInformationRequest(ShopName, products, StockDetailsLevel.Status);
            var stockInfoResult = _inventoryServiceProvider.GetStockInformation(stockInfoRequest);
            if (stockInfoResult.StockInformation == null || !stockInfoResult.StockInformation.Any())
            {
                return;
            }
            var stockInfo = stockInfoResult.StockInformation.FirstOrDefault();
            var orderableInfo = new OrderableInformation();
            if (stockInfo != null && stockInfo.Status != null)
            {
                if (Equals(stockInfo.Status, StockStatus.PreOrderable))
                {
                    var preOrderableRequest = new GetPreOrderableInformationRequest(ShopName, products);
                    var preOrderableResult = _inventoryServiceProvider.GetPreOrderableInformation(preOrderableRequest);
                    if (preOrderableResult.OrderableInformation != null && preOrderableResult.OrderableInformation.Any())
                    {
                        orderableInfo = preOrderableResult.OrderableInformation.FirstOrDefault();
                    }
                }
                else if (Equals(stockInfo.Status, StockStatus.BackOrderable))
                {
                    var backOrderableRequest = new GetBackOrderableInformationRequest(ShopName, products);
                    var backOrderableResult = _inventoryServiceProvider.GetBackOrderableInformation(backOrderableRequest);
                    if (backOrderableResult.OrderableInformation != null &&
                        backOrderableResult.OrderableInformation.Any())
                    {
                        orderableInfo = backOrderableResult.OrderableInformation.FirstOrDefault();
                    }
                }
            }
            if (stockInfo != null)
            {
                cartLine.Product.StockStatus = stockInfo.Status;
            }
            if (orderableInfo == null)
            {
                return;
            }
            cartLine.Product.InStockDate = orderableInfo.InStockDate;
            cartLine.Product.ShippingDate = orderableInfo.ShippingDate;
        }

        /// <summary> 
        ///  If visitor is logged in, use the commerce ID else use randomly generated guid and store in cookie
        /// </summary>
        /// <returns></returns>
        public string GetVisitorId()
        {
            if (Tracker.Current != null && Context.User.IsAuthenticated)
            {
                return GetLoggedInUserId();
            }
            return GetAnonymousUserId();
        }


        /// <summary>
        /// Get LoggedIn UserId
        /// </summary>
        /// <returns></returns>
        public string GetLoggedInUserId()
        {
            if (Tracker.Current != null && Context.User.IsAuthenticated)
            {
                var userName = _contactFactory.GetContact();
                var commerceUser = _accountService.GetUser(userName);
                if (commerceUser != null && commerceUser.Customers != null)
                {
                    return commerceUser.Customers.FirstOrDefault();
                }
            }
            return GetAnonymousUserId();
        }


        /// <summary>
        /// Get Anonymous UserId
        /// </summary>
        /// <returns></returns>
        public string GetAnonymousUserId()
        {
            const string visitorTrackingCookieName = Constants.Cart.VisitorTrackingCookieName;
            const string visitorIdKeyName = Constants.Cart.VisitorIdKeyName;
            const int visitorCookieExpiryInDays = 1;
            var visitorCookie = HttpContext.Current.Request.Cookies[visitorTrackingCookieName] ??
                                new HttpCookie(visitorTrackingCookieName);

            if (string.IsNullOrEmpty(visitorCookie.Values[visitorIdKeyName]))
            {
                visitorCookie.Values[visitorIdKeyName] = Guid.NewGuid().ToString("D");
            }

            visitorCookie.Expires = DateTime.Now.AddDays(visitorCookieExpiryInDays);
            HttpContext.Current.Response.SetCookie(visitorCookie);
            return visitorCookie.Values[visitorIdKeyName];
        }


        /// <summary>
        /// Get Customer Cart
        /// </summary>
        /// <returns></returns>
        public CommerceCart GetCustomerCart()
        {
            // Get visitor's cart from cache
            var cart = GetFromCacheCart(GetVisitorId());
            if (cart == null && CustomerHasCookie(GetVisitorId()))
            {
                // cart exists, but cart cache is empty; lets get it and add it
                cart = GetCart(GetVisitorId(), true);
                if (cart != null)
                {
                    AddCartToCache(cart);
                }
            }
            return cart ?? new CommerceCart();
        }

        /// <summary>
        /// Get Mini Cart
        /// </summary>
        /// <param name="cartFromCommServer"></param>
        /// <returns></returns>
        public ShoppingCart GetMiniCart(bool cartFromCommServer = false)
        {
            if (Context.IsLoggedIn &&
                !Context.User.Name.ToLower().Contains(Constants.Commerce.CommerceUserDomain.ToLower())) return new ShoppingCart();

            var cart = cartFromCommServer ? GetCart(GetVisitorId(), true) : GetCustomerCart();
            var shoppingCartTotal = cart.Total as CommerceTotal;
            var shoppingCart = new ShoppingCart();
            if (shoppingCartTotal == null) return shoppingCart;

            shoppingCart.LineTotal = cart.Total as CommerceTotal == null
                ? 0
                : (cart.Total as CommerceTotal).Subtotal;
            shoppingCart.Total = cart.LineItemCount;

            var commerceTotal = (CommerceTotal)cart.Total;

            shoppingCart.LineDiscount = commerceTotal.LineItemDiscountAmount;
            shoppingCart.OrderLevelDiscountAmount = commerceTotal.OrderLevelDiscountAmount;

            shoppingCart.Shipping = commerceTotal?.ShippingTotal ?? 0.00m;
            shoppingCart.Tax = cart.Total.TaxTotal.Amount == null ? 0.00m : cart.Total.TaxTotal.Amount;
            shoppingCart.GrandTotal = cart.Total.Amount == null ? 0.00m : cart.Total.Amount;

            if (Context.User.IsInRole("CommerceUsers\\Dealer")) { shoppingCart.LineTotal = shoppingCart.LineTotal > 0 ? (decimal)0.90 * shoppingCart.LineTotal : shoppingCart.LineTotal; }
            if (Context.User.IsInRole("CommerceUsers\\Retailer")) { shoppingCart.LineTotal = shoppingCart.LineTotal > 0 ? (decimal)0.75 * shoppingCart.LineTotal : shoppingCart.LineTotal; }

            if (Context.User.IsInRole("CommerceUsers\\Dealer")) { shoppingCart.GrandTotal = shoppingCart.GrandTotal > 0 ? (decimal)0.90 * shoppingCart.GrandTotal : shoppingCart.GrandTotal; }
            if (Context.User.IsInRole("CommerceUsers\\Retailer")) { shoppingCart.GrandTotal = shoppingCart.GrandTotal > 0 ? (decimal)0.75 * shoppingCart.GrandTotal : shoppingCart.GrandTotal; }

            var cartItems = new List<CartItem>();
            if (cart.LineItemCount > 0)
            {
                foreach (CustomCommerceCartLine cartLine in cart.Lines)
                {
                    var cartItem = new CartItem();
                    var product = cartLine.Product as CommerceCartProduct;
                    cartItem.ProductName = product.DisplayName;

                    var productSItemSearch = ProductHelper.GetItemByName(product.ProductId);
                    if (productSItemSearch != null)
                    {
                        var item = productSItemSearch.GetItem();
                        if (item != null)
                        {
                            cartItem.ProductId = item.ID.ToString();
                            cartItem.ImageUrl = (!string.IsNullOrEmpty(item[Constants.Products.VariantImage1])) ? item[Constants.Products.VariantImage1] : ProductHelper.GetFirstImageFromProductItem(item);

                            // If it is a variant and there is an image for the variant, lets use that.
                            if (!string.IsNullOrEmpty(product.ProductVariantId) && product.ProductVariantId != "-1")
                            {
                                var variantItem = item.Axes.GetDescendants()
                                    .AsQueryable()
                                    .FirstOrDefault(x => x.Name.Equals(product.ProductVariantId));

                                if (variantItem != null)
                                {
                                    cartItem.ImageUrl = (!string.IsNullOrEmpty(variantItem[Constants.Products.VariantFirstImage])) ? variantItem[Constants.Products.VariantFirstImage] : cartItem.ImageUrl;
                                }
                            }

                            cartItem.Category = item.Parent.Name;
                        }
                        else
                        {
                            cartItem.ImageUrl = string.Empty;
                        }
                    }

                    cartItem.CsProductId = product.ProductId;
                    cartItem.Quantity = (int)cartLine.Quantity;
                    cartItem.UnitPrice = product.Price.Amount;

                    var total = (CommerceTotal)cartLine.Total;
                    cartItem.SubTotal = total.Subtotal;
                    cartItem.Amount = total.Amount;
                    cartItem.Discount = total.LineItemDiscountAmount;

                    cartItem.Adjustments = new List<string>();
                    cartItem.Adjustments.AddRange(cartLine.Adjustments.Select(a => a.Description));

                    cartItem.ExternalId = cartLine.ExternalCartLineId;

                    if (Context.User.IsInRole("CommerceUsers\\Dealer")) { cartItem.UnitPrice = cartItem.UnitPrice > 0 ? (decimal)0.90 * cartItem.UnitPrice : cartItem.UnitPrice; }
                    if (Context.User.IsInRole("CommerceUsers\\Retailer")) { cartItem.UnitPrice = cartItem.UnitPrice > 0 ? (decimal)0.75 * cartItem.UnitPrice : cartItem.UnitPrice; }

                    if (Context.User.IsInRole("CommerceUsers\\Dealer")) { cartItem.SubTotal = cartItem.SubTotal > 0 ? (decimal)0.90 * cartItem.SubTotal : cartItem.SubTotal; }
                    if (Context.User.IsInRole("CommerceUsers\\Retailer")) { cartItem.SubTotal = cartItem.SubTotal > 0 ? (decimal)0.75 * cartItem.SubTotal : cartItem.SubTotal; }


                    if (string.IsNullOrEmpty(cartItem.ImageUrl))
                    {
                        if (cartLine.Images != null && cartLine.Images.Count > 0)
                        {
                            cartItem.ImageUrl = cartLine.Images[0];
                        }
                    }
                    cartItems.Add(cartItem);
                }
                shoppingCart.CartItems = cartItems;

                shoppingCart.Adjustments = new List<string>();
                shoppingCart.Adjustments.AddRange(cart.Adjustments.Select(a => a.Description));
            }

            return shoppingCart;
        }


        /// <summary>
        ///   Check if a Customer Has Cookie
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private bool CustomerHasCookie(string customerId)
        {
            const string cookieName = Constants.Cart.CookieName;
            var cartCookie = HttpContext.Current.Request.Cookies[cookieName];
            return cartCookie != null && cartCookie.Values[Constants.Cart.VisitorID] == customerId;
        }

        /// <summary>
        /// Apply Shipping and Billing to Cart
        /// </summary>
        /// <param name="shippingAddress"></param>
        /// <returns></returns>
        internal bool ApplyShippingToCart(Address shippingAddress)
        {
            var cart = GetCustomerCart();

            var shipping = new CommerceParty
            {
                ExternalId = "0",
                Name = Constants.Products.ShippingAddress,
                PartyId = "0",
                FirstName = shippingAddress.FirstName,
                LastName = shippingAddress.LastName,
                Address1 = shippingAddress.Address1,
                Address2 = shippingAddress.Address2,
                City = shippingAddress.City,
                State = shippingAddress.State,
                Company = shippingAddress.Company,
                Email = shippingAddress.Email,
                FaxNumber = shippingAddress.FaxNumber,
                Country = shippingAddress.Country,
                CountryCode = shippingAddress.CountryCode,
                ZipPostalCode = shippingAddress.ZipPostalCode,
				
            };

			shipping.Properties = new Sitecore.Commerce.PropertyCollection();

			shipping.Properties.Add("InmateId", shippingAddress.InmateId);

			try
            {
                var cartParties = cart.Parties.ToList();
                var partyList = new List<Party> { shipping };

                cartParties.AddRange(partyList);
                cart.Parties = cartParties.AsReadOnly();

                // clear cart cache and add updated cart to cache
                UpdateCartInCache(cart);
                return true;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Add to cart error", ex.Message);
                return false;
            }
        }




        /// <summary>
        /// Apply Shipping and Billing to Cart
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="email"></param>
        /// <param name="company"></param>
        /// <param name="address"></param>
        /// <param name="addressline1"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="fax"></param>
        /// <param name="phone"></param>
        /// <param name="zip"></param>
        /// <param name="firstname2"></param>
        /// <param name="lastname2"></param>
        /// <param name="email2"></param>
        /// <param name="company2"></param>
        /// <param name="address2"></param>
        /// <param name="addressline12"></param>
        /// <param name="city2"></param>
        /// <param name="country2"></param>
        /// <param name="fax2"></param>
        /// <param name="phone2"></param>
        /// <param name="zip2"></param>
        /// <returns></returns>
        internal bool ApplyShippingAndBillingToCart(string firstname, string lastname, string email, string company,
            string address, string addressline1, string city, string state, string country, string countryName, string fax, string phone, string zip,
            string firstname2, string lastname2, string email2, string company2, string address2, string addressline12,
            string city2, string state2, string country2, string countryName2, string fax2, string phone2, string zip2)
        {

            var visitorId = GetVisitorId();
            var cart = GetCustomerCart();
            var billingParty = new CommerceParty() { ExternalId = "0", Name = "Billing", PartyId = "0", FirstName = "Joe", LastName = "Smith", Address1 = "123 Street", City = "Ottawa", State = "ON", Country = "Canada", ZipPostalCode = "12345", CountryCode = "CA" };
            var shippingParty = new CommerceParty() { ExternalId = "0", Name = "Shipping", PartyId = "0", FirstName = "Jane", LastName = "Smith", Address1 = "234 Street", City = "Toronto", State = "ON", Country = "Canada", ZipPostalCode = "12345", CountryCode = "CA" };

            //----------------------------------------------------------

            var billing = new CommerceParty
            {
                ExternalId = "0",
                PartyId = "1",
                Name = Constants.Products.BillingAddress,
                FirstName = firstname,
                LastName = lastname,
                Address1 = address,
                Address2 = addressline1,
                City = city,
                State = state,
                Company = company,
                Email = email,
                FaxNumber = fax,
                Country = countryName,
                CountryCode = country,
                ZipPostalCode = zip
            };

            var shipping = new CommerceParty
            {
                ExternalId = "0",
                Name = Constants.Products.ShippingAddress,
                PartyId = "0",
                FirstName = firstname2,
                LastName = lastname2,
                Address1 = address2,
                Address2 = addressline12,
                City = city2,
                State = state2,
                Company = company2,
                Email = email2,
                FaxNumber = fax2,
                Country = countryName2,
                CountryCode = country2,
                ZipPostalCode = zip2
            };


            var cartParties = cart.Parties.ToList();
            var partyList = new List<Party> { shipping };
            cartParties.AddRange(partyList);
            cart.Parties = cartParties.AsReadOnly();

            // clear cart cache and add updated cart to cache
            UpdateCartInCache(cart);
            return true;
        }

        private CommerceCart RemoveParties(CommerceCart cart, string partyName)
        {
            var parties = GetPartieByName(cart, partyName);
            var request = new Sitecore.Commerce.Services.Carts.RemovePartiesRequest(cart, parties);
            var response = _cartServiceProvider.RemoveParties(request);
            if (response.Success)
            {
                return response.Cart as CommerceCart;
            }

            return cart;
        }

        protected virtual List<Party> GetPartieByName(CommerceCart cart, string name)
        {
            var partyList = new List<Party>();

            foreach (var party in cart.Parties)
            {
                if (party is CommerceParty)
                {
                    if (((CommerceParty)party).Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        partyList.Add(party);
                    }
                }
                else if (party is EmailParty)
                {
                    if (((EmailParty)party).Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        partyList.Add(party);
                    }
                }
            }

            return partyList;
        }

        /// <summary>
        ///  Update an updated cart in cache
        /// </summary>
        /// <param name="updatedCart"></param>
        private void UpdateCartInCache(CommerceCart updatedCart)
        {
            //clear and update only if cart is valid
            if (updatedCart == null || updatedCart.Properties[Constants.Cart.BasketErrors] != null) return;

            ClearCartFromCache();
            AddCartToCache(updatedCart);
        }

        /// <summary>
        /// Update Parties In Cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="parties"></param>
        /// <returns></returns>
        private CommerceCart UpdatePartiesInCart(CommerceCart cart, List<Party> parties)
        {
            if (!parties.Any())
            {
                return cart;
            }
            var request = new UpdatePartiesRequest(cart, parties.Cast<Party>().ToList());
            var result = _cartServiceProvider.UpdateParties(request);
            return result.Cart as CommerceCart;
        }

        /// <summary>
        ///   Check For Party Info in Cart
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        private CommerceCart CheckForPartyInfoOnCart(CommerceCart cart)
        {
            var updatedCart = cart;
            if (cart.Parties.Cast<CommerceParty>().FirstOrDefault(party => party.Name == Constants.Products.BillingAddress) == null)
            {
                var billing = new CommerceParty
                {
                    ExternalId = Guid.NewGuid().ToString("B"),
                    Name = Constants.Products.BillingAddress
                };
                updatedCart = AddPartyToCart(updatedCart, billing);
            }
            if (cart.Parties.Cast<CommerceParty>().FirstOrDefault(party => party.Name == Constants.Products.ShippingAddress) == null)
            {
                var shipping = new CommerceParty
                {
                    ExternalId = Guid.NewGuid().ToString("B"),
                    Name = Constants.Products.ShippingAddress
                };
                updatedCart = AddPartyToCart(updatedCart, shipping);
            }
            return updatedCart;
        }

        /// <summary>
        /// Add Party to Cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="party"></param>
        /// <returns></returns>
        public CommerceCart AddPartyToCart([NotNull] CommerceCart cart, [NotNull] CommerceParty party)
        {
            var request = new AddPartiesRequest(cart, new List<Party> { party });
            var result = _cartServiceProvider.AddParties(request);
            return result.Cart as CommerceCart;
        }

        /// <summary>
        /// Add Shipping Method to Cart
        /// </summary>
        /// <param name="shippingMethodId"></param>
        /// <returns></returns>
        internal bool AddShippingMethodToCart(string shippingMethodId)
        {
            var shippingData = shippingMethodId.Split('|');
            var cart = GetCustomerCart();

            // prepare shipping methods list with chosen shipping method
            var shippingMethodList = new List<ShippingMethodInputModelItem>
            {
                new ShippingMethodInputModelItem
                    {
                        ShippingMethodID = shippingData[0],
                        ShippingMethodName = shippingData[1],
                        ShippingPreferenceType = "1",
                        PartyID = "0"
                    }
            };

            //prepare shipping list - items to be shipped
            var internalShippingList = shippingMethodList.ToShippingInfoList();
            var orderPreferenceType = InputModelExtension.GetShippingOptionType("1");

            if (orderPreferenceType != ShippingOptionType.DeliverItemsIndividually)
            {
                foreach (var shipping in internalShippingList)
                {
                    shipping.LineIDs = (from CommerceCartLine lineItem in cart.Lines select lineItem.ExternalCartLineId).ToList().AsReadOnly();
                }
            }

            var shipments = new List<ShippingInfo>();
            shipments.AddRange(internalShippingList);

            //update cart with shipping info
            var addShippingInfoRequest = new Sitecore.Commerce.Engine.Connect.Services.Carts.AddShippingInfoRequest(cart, shipments, orderPreferenceType);
            var addShippingInfoResult = _cartServiceProvider.AddShippingInfo(addShippingInfoRequest);
            if (!addShippingInfoResult.Success)
            {
                return false;
            }

            //update cart in cache
            if (addShippingInfoResult.Success && addShippingInfoResult.Cart != null)
            {
                UpdateCartInCache(addShippingInfoResult.Cart as CommerceCart);
            }

            //return true if cart has been updated
            var updatedCart = GetCustomerCart();
            return updatedCart != null;
        }

        /// <summary>
        ///   Add Shipping Method Info to Cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="shipping"></param>
        /// <param name="shippingMethodId"></param>
        /// <returns></returns>
        public CommerceCart AddShippingMethodInfoToCart(CommerceCart cart, CommerceParty shipping,
            string shippingMethodId)
        {
            var shippingInfo = new ShippingInfo
            {
                ShippingMethodID = shippingMethodId,
                PartyID = shipping.ExternalId,
                LineIDs =
                    (from CommerceCartLine lineItem in cart.Lines select lineItem.ExternalCartLineId).ToList()
                        .AsReadOnly()
            };
            var request = new Sitecore.Commerce.Engine.Connect.Services.Carts.AddShippingInfoRequest(cart, new List<ShippingInfo> { shippingInfo }, ShippingOptionType.ShipToAddress);

            //var request = new AddShippingInfoRequest(cart, new List<ShippingInfo> { shippingInfo });

            //var info = CartRequestInformation.Get(request);
            //if (info == null)
            //{
            //    info = new CartRequestInformation(request, true);
            //}
            //else
            //{
            //    info.Refresh = true;
            //}
            var result = new CommerceCartServiceProvider().AddShippingInfo(request);
            return result.Cart as CommerceCart;
        }

        /// <summary>
        /// Apply PaymentMethod to Cart
        /// </summary>
        /// <param name="paymentExternalId"></param>
        /// <param name="nameoncard"></param>
        /// <param name="creditcard"></param>
        /// <param name="expmonth"></param>
        /// <param name="expyear"></param>
        /// <param name="ccv"></param>
        /// <returns></returns>
        internal bool ApplyPaymentMethodToCart(string paymentExternalId, string nameoncard, string creditcard,
            string expmonth, string expyear, string ccv)
        {
            var cart = GetCustomerCart();
            var updatedCart = cart;

            // remove all payment info on cart
            updatedCart = RemovePaymentInfoFromCart(cart, cart.Payment.ToList());

            // add payment info to cart
            // get billing, if billing is not null 
            if (!string.IsNullOrEmpty(paymentExternalId))
            {
                var billing = updatedCart.Parties.Cast<CommerceParty>().FirstOrDefault(party => party.Name == Constants.Products.BillingAddress);
                if (billing != null)
                {
                    var paymentInfo = new CommerceCreditCardPaymentInfo();
                    paymentInfo.ExternalId = paymentExternalId;
                    paymentInfo.PaymentMethodID = paymentExternalId;
                    paymentInfo.CustomerNameOnPayment = nameoncard;
                    paymentInfo.CreditCardNumber = creditcard;
                    paymentInfo.ExpirationMonth = int.Parse(expmonth);
                    paymentInfo.ExpirationYear = int.Parse(expyear);
                    paymentInfo.ValidationCode = ccv;
                    updatedCart = AddPaymentInfoToCart(updatedCart, paymentInfo, billing, true);
                }
            }

            UpdateCartInCache(updatedCart);
            return true;
        }

        /// <summary>
        /// Apply PaymentMethod to Cart
        /// </summary>
        internal bool ApplyNewPaymentMethodToCart(Payment cartPayment)
        {
            var billingParty = new CommerceParty
            {
                ExternalId = "0",
                Name = Constants.Products.BillingAddress,
                PartyId = "0",
                FirstName = cartPayment.BillingAddress.FirstName,
                LastName = cartPayment.BillingAddress.LastName,
                Address1 = cartPayment.BillingAddress.Address1,
                Address2 = cartPayment.BillingAddress.Address2,
                City = cartPayment.BillingAddress.City,
                State = cartPayment.BillingAddress.State,
                Company = cartPayment.BillingAddress.Company,
                Email = cartPayment.BillingAddress.Email,
                FaxNumber = cartPayment.BillingAddress.FaxNumber,
                Country = cartPayment.BillingAddress.Country,
                CountryCode = cartPayment.BillingAddress.CountryCode,
                ZipPostalCode = cartPayment.BillingAddress.ZipPostalCode
            };

            // Add billing party to cart
            var updatedCart = GetCustomerCart();
            var parties = updatedCart.Parties.ToList();
            parties.Add(billingParty);
            updatedCart.Parties = parties.AsSafeReadOnly();

            // prepare payment info
            var federatedPaymentModel = new FederatedPaymentInputModelItem
            {
                CardToken = cartPayment.Token,
                Amount = updatedCart.Total.Amount,
                CardPaymentAcceptCardPrefix = cartPayment.CardPrefix
            };

            var federatedPayment = federatedPaymentModel.ToCreditCardPaymentInfo();
            federatedPayment.PartyID = billingParty.PartyId;

            var payments = new List<PaymentInfo> { federatedPayment };

            //add payment info to cart
            var addPaymentInfoResult = _cartServiceProvider.AddPaymentInfo(new AddPaymentInfoRequest(updatedCart, payments));
            if (!addPaymentInfoResult.Success)
            {
                return false;
            }

            //update cart in cache
            updatedCart = addPaymentInfoResult.Cart as CommerceCart;
            UpdateCartInCache(updatedCart);

            return true;
        }


        /// <summary>
        /// Apply PaymentMethod to Cart
        /// </summary>
        internal bool ApplyPaymentMethodToCart2(string nounceData, string cardPrefixData)
        {
            var billingParty = new CommerceParty() { ExternalId = "0", Name = "Billing", PartyId = "0", FirstName = "Joe", LastName = "Smith", Address1 = "123 Street", City = "Ottawa", State = "ON", Country = "Canada", ZipPostalCode = "12345", CountryCode = "CA" };

            var updatedCart = GetCustomerCart();

            var payments = new List<PaymentInfo>();

            // Add billing party
            var parties = updatedCart.Parties.ToList();
            parties.Add(billingParty);
            updatedCart.Parties = parties.AsSafeReadOnly();

            // Add payment info
            var addPaymentInfoResult = new AddPaymentInfoResult { Success = false };
            var federatedPaymentModel = new FederatedPaymentInputModelItem
            {
                CardToken = nounceData,
                Amount = updatedCart.Total.Amount,
                CardPaymentAcceptCardPrefix = cardPrefixData
            };

            var federatedPayment = federatedPaymentModel.ToCreditCardPaymentInfo();
            federatedPayment.PartyID = billingParty.PartyId;
            payments.Add(federatedPayment);

            var addPaymentInfoRequest = new AddPaymentInfoRequest(updatedCart, payments);
            addPaymentInfoResult = _cartServiceProvider.AddPaymentInfo(addPaymentInfoRequest);
            if (!addPaymentInfoResult.Success)
            {
                return false;
            }

            updatedCart = addPaymentInfoResult.Cart as CommerceCart;
            UpdateCartInCache(updatedCart);

            return true;
        }

        /// <summary>
        /// Add Payment Info to Cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="info"></param>
        /// <param name="party"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        private CommerceCart AddPaymentInfoToCart(CommerceCart cart, CommerceCreditCardPaymentInfo info,
            CommerceParty party, bool refresh)
        {
            var creditCardInfo = info;
            creditCardInfo.PartyID = party.ExternalId;
            creditCardInfo.Amount = cart.Total.Amount;
            var paymentRequest = new AddPaymentInfoRequest(cart, new List<PaymentInfo> { creditCardInfo });
            var paymentResult = _cartServiceProvider.AddPaymentInfo(paymentRequest);
            var req = CartRequestInformation.Get(paymentRequest);
            if (req == null)
            {
                req = new CartRequestInformation(paymentRequest, refresh);
            }
            else
            {
                req.Refresh = refresh;
            }
            return paymentResult.Cart as CommerceCart;
        }

        /// <summary>
        /// Remove Payment Info From Cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private CommerceCart RemovePaymentInfoFromCart(CommerceCart cart, List<PaymentInfo> list)
        {
            var removePaymentRequest = new RemovePaymentInfoRequest(cart, list);
            var info = CartRequestInformation.Get(removePaymentRequest);
            if (info == null)
            {
                info = new CartRequestInformation(removePaymentRequest, false);
            }
            else
            {
                info.Refresh = false;
            }
            var removeResult = _cartServiceProvider.RemovePaymentInfo(removePaymentRequest);
            return removeResult.Cart as CommerceCart;
            ;
        }

        /// <summary>
        /// Submit Cart to Commerce Server
        /// </summary>
        /// <returns></returns>
        internal string SubmitCart()
        {
            // Get visitor identifier
            var visitorId = GetVisitorId();
            var updatedCart = GetCustomerCart();

            if (updatedCart.Lines.Count == 0)
            {
                return "No items in cart";
            }

            updatedCart.Email = "testorder@mail.com";

            var submitVisitorOrderRequest = new SubmitVisitorOrderRequest(updatedCart);
            var submitVisitorOrderResult = _orderServiceProvider.SubmitVisitorOrder(submitVisitorOrderRequest);

            if (submitVisitorOrderResult.Success && submitVisitorOrderResult.Order != null
                && submitVisitorOrderResult.CartWithErrors == null)
            {
                var order = submitVisitorOrderResult.Order as CommerceOrder;
                if (order != null)
                {
                    var orderId = order.OrderID;

                    // clear cart from cache
                    ClearCartFromCache();

                    // Get order details
                    var getVisitorOrderRequest = new GetVisitorOrderRequest(orderId, visitorId, ShopName);
                    var getVisitorOrderResult = _orderServiceProvider.GetVisitorOrder(getVisitorOrderRequest);

                    //update recommendations API builds [Disabled for now]
                    //RecommendationsHelper.SendPurchaseEvent(order);

                    // return commerceOrderId;
                    return getVisitorOrderResult.Order.TrackingNumber;
                }
            }

            var error = submitVisitorOrderResult?.SystemMessages[0]?.Message ?? "NULL";
            Log.Error("CartHelper.SubmitCart Error, error = " + error, new Exception());

            return "Error in cart! Order not submitted. Error = " + error;
        }

        /// <summary>
        /// Submit Order
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        private SubmitVisitorOrderResult SubmitOrder(CommerceCart cart)
        {

            ////var provider = new Sitecore.Commerce.Services.Orders.OrderServiceProvider();
            ////var request = new Sitecore.Commerce.Engine.Connect.Services.).SubmitVisitorOrderRequest(cart);

            ////var result = provider.SubmitVisitorOrder(request);
            ////return result;

            //SubmitVisitorOrderResult visitorOrderResult = new SubmitVisitorOrderResult(cart);
            //int num = 0;
            //visitorOrderResult.Success = num != 0;
            //SubmitVisitorOrderResult errorResult = visitorOrderResult;

            //SubmitVisitorOrderRequest request = new Sitecore.Commerce.Services.Orders.SubmitVisitorOrderRequest((Sitecore.Commerce.Entities.Carts.Cart)cart);

            //return visitorOrderResult;

            ////var orderService = new OrderServiceProvider();
            ////var request = new SubmitVisitorOrderRequest(cart);

            ////var result = orderService.SubmitVisitorOrder(request);
            ////return result;

            ////var order = result.Order;
            ////var orderId = order.OrderID;

            cart.Email = "test@mail.com";
            var submitRequest = new SubmitVisitorOrderRequest(cart);
            var provider = new OrderServiceProvider();
            var req = CartRequestInformation.Get(submitRequest);
            if (req == null)
            {
                req = new CartRequestInformation(submitRequest, true);
            }
            else
            {
                req.Refresh = true;
            }
            var submitResult = provider.SubmitVisitorOrder(submitRequest);
            var visitorOrderResult = new SubmitVisitorOrderResult();
            return submitResult;
        }

        /// <summary>
        /// Remove item from Commerce Server Cart
        /// </summary>
        /// <param name="externalCartLineId"></param>
        /// <returns></returns>
        internal bool RemoveItemFromCart(string externalCartLineId)
        {
            try
            {
                var cart = RemoveFromCart(externalCartLineId);
                ClearCartFromCache();
                AddCartToCache(cart);

                return true;
            }
            catch (Exception ex)
            {
                CommerceLog.Current.Error(ex.ToString(), this);
                return false;
            }
        }

        /// <summary>
        ///   Remove Item From Cart
        /// </summary>
        /// <param name="externalCartLineId"></param>
        /// <returns></returns>
        private CommerceCart RemoveFromCart(string externalCartLineId)
        {
            var cart = GetCustomerCart();
            var lineToRemove = cart.Lines.SingleOrDefault(cl => cl.ExternalCartLineId == externalCartLineId);
            if (lineToRemove == null)
            {
                return cart;
            }
            var request = new RemoveCartLinesRequest(cart, new[] { lineToRemove });
            var info = CartRequestInformation.Get(request);
            if (info == null)
            {
                info = new CartRequestInformation(request, true);
            }
            else
            {
                info.Refresh = true;
            }
            var cartResult = _cartServiceProvider.RemoveCartLines(request);
            return cartResult.Cart as CommerceCart;
        }

        /// <summary>
        ///   Update Cart Item
        /// </summary>
        /// <param name="externalId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        internal bool UpdateCartItem(string externalId, string quantity)
        {
            try
            {
                // get the cart line to update
                // get the current cart quantity
                // if current new quantity is greater tnan current, Add new-current to cart
                // if less remove the item and add new
                var cart = ChangeCartItemQuantity(externalId, quantity);
                if (cart != null)
                {
                    UpdateCartInCache(cart);
                }
                return true;
            }
            catch (Exception ex)
            {
                CommerceLog.Current.Error(ex.ToString(), this);
                return false;
            }
        }

        /// <summary>
        ///   Change Cart Item Quantity
        /// </summary>
        /// <param name="externalCartLineId"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        private CommerceCart ChangeCartItemQuantity(string externalCartLineId, string q)
        {
            var quantity = uint.Parse(q);
            if (quantity == 0)
            {
                return RemoveFromCart(externalCartLineId);
            }
            var cart = GetCustomerCart();
            var cartLineToChange =
                cart.Lines.SingleOrDefault(cl => cl.Product != null && cl.ExternalCartLineId == externalCartLineId);
            if (cartLineToChange == null)
            {
                return cart;
            }
            cartLineToChange.Quantity = quantity;
            var updateRequest = new UpdateCartLinesRequest(cart, new[] { cartLineToChange });
            var info = CartRequestInformation.Get(updateRequest);
            if (info == null)
            {
                info = new CartRequestInformation(updateRequest, true);
            }
            else
            {
                info.Refresh = true;
            }
            var cartResult = _cartServiceProvider.UpdateCartLines(updateRequest);
            return cartResult.Cart as CommerceCart;
        }

        /// <summary>
        /// Merge anonymous user cart after login
        /// </summary>
        /// <param name="anonymousUserId"></param>
        /// <returns></returns>
        public CommerceCart MergeCarts(string anonymousUserId)
        {
            var userId = GetLoggedInUserId();
            var currentCart = GetCart(userId);
            if (userId != anonymousUserId)
            {
                ClearUserCartFromCache(userId);
                var cartFromAnonymous = GetCart(anonymousUserId);
                if (cartFromAnonymous != null && cartFromAnonymous.Lines.Any())
                {
                    ClearUserCartFromCache(anonymousUserId);
                    currentCart = MergeCarts(currentCart, cartFromAnonymous);
                }
            }
            AddCartToCache(currentCart);
            return currentCart;
        }


        /// <summary>
        /// Merge two carts
        /// </summary>
        /// <param name="userCart"></param>
        /// <param name="anonymousCart"></param>
        /// <returns></returns>
        private CommerceCart MergeCarts(CommerceCart userCart, CommerceCart anonymousCart)
        {
            if ((userCart.ShopName == anonymousCart.ShopName) && (userCart.ExternalId != anonymousCart.ExternalId))
            {
                var mergeCartRequest = new MergeCartRequest(anonymousCart, userCart);
                var result = _cartServiceProvider.MergeCart(mergeCartRequest);
                var updateCartRequest = new UpdateCartLinesRequest(result.Cart, anonymousCart.Lines);
                var info = CartRequestInformation.Get(updateCartRequest);
                if (info == null)
                {
                    info = new CartRequestInformation(updateCartRequest, true);
                }
                else
                {
                    info.Refresh = true;
                }
                result = _cartServiceProvider.UpdateCartLines(updateCartRequest);
                _cartServiceProvider.DeleteCart(new DeleteCartRequest(anonymousCart));
                return result.Cart as CommerceCart;
            }
            return userCart;
        }

        /// <summary>
        ///  Only Anonymous and customers allowed
        /// </summary>
        /// <returns></returns>
        internal string CustomerOrAnonymous()
        {
            var ret = string.Empty;
            if (Context.User.IsAuthenticated)
            {
                var uid = AccountHelper.GetCommerceUserId(Context.User.Name);
                if (string.IsNullOrEmpty(uid))
                {
                    return Constants.Cart.AnonUserActionDenied;
                }
            }
            return ret;
        }

        /// <summary>
        /// Get Customer Orders
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="shopName"></param>
        /// <returns></returns>
        public GetVisitorOrdersResult GetOrders(string customerId, string shopName)
        {

            var request = new GetVisitorOrdersRequest(customerId, shopName);
            var result = _orderServiceProvider.GetVisitorOrders(request);
            if (result.Success && result.OrderHeaders != null && result.OrderHeaders.Count > 0)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Get Visitor Order Result
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="customerId"></param>
        /// <param name="shopName"></param>
        /// <returns></returns>
        public GetVisitorOrderResult GetOrderHead(string orderId, string customerId, string shopName)
        {
            var submitRequest = new GetVisitorOrderRequest(orderId, customerId, shopName);
            var provider = new CommerceOrderServiceProvider();
            var submitResult = provider.GetVisitorOrder(submitRequest);
            return submitResult;
        }

        /// <summary>
        /// Apply Promo/Coupon Code
        /// </summary>
        /// <param name="promoCode"></param>
        /// <returns></returns>
        internal bool ApplyPromoCode(string promoCode)
        {
            var cart = GetCustomerCart();
            var request = new AddPromoCodeRequest(cart, promoCode);

            var cartService = new CommerceCartServiceProvider();
            var result = cartService.AddPromoCode(request);
            //var result = ((CommerceCartServiceProvider)_cartServiceProvider).AddPromoCode(request);

            if (!result.Success || result.Cart == null) return false;

            UpdateCartInCache(result.Cart as CommerceCart);
            return true;
        }

        public IEnumerable<PaymentOption> GetPaymentOptions()
        {
            var cartResult = GetCustomerCart();
            var request = new GetPaymentOptionsRequest(ShopName, cartResult);
            var result = _paymentServiceProvider.GetPaymentOptions(request);

            return !result.Success ? null : result.PaymentOptions.ToList();
        }

        public IEnumerable<PaymentMethod> GetPaymentMethods(PaymentOption paymentOption)
        {
            if (paymentOption == null) { paymentOption = GetPaymentOptions().FirstOrDefault(); }

            var cartResult = GetCustomerCart();
            var request = new Sitecore.Commerce.Engine.Connect.Services.Payments.GetPaymentMethodsRequest(cartResult, paymentOption);
            var result = _paymentServiceProvider.GetPaymentMethods(request);

            return !result.Success ? null : result.PaymentMethods.ToList();
        }


        public string GetPaymentClientToken()
        {
            var request = new ServiceProviderRequest();
            var result = _paymentServiceProvider.RunPipeline<ServiceProviderRequest, PaymentClientTokenResult>("commerce.payments.getClientToken", request);

            return !result.Success ? string.Empty : result.ClientToken;
        }


        public PaymentClientTokenResult GetPaymentClient()
        {
            var request = new ServiceProviderRequest();
            var result = _paymentServiceProvider.RunPipeline<ServiceProviderRequest, PaymentClientTokenResult>("commerce.payments.getClientToken", request);

            return !result.Success ? null : result;
        }

        public bool SetPaymentMethods(PaymentInputModel inputModel)
        {
            var billingGuid = new Guid().ToString();
            inputModel.BillingAddress.ExternalId = billingGuid;
            inputModel.BillingAddress.PartyId = billingGuid;

            var response = GetCustomerCart();
            var payments = new List<PaymentInfo>();
            var cart = (CommerceCart)response;

            if (inputModel.CreditCardPayment != null && !string.IsNullOrEmpty(inputModel.CreditCardPayment.PaymentMethodID) && inputModel.BillingAddress != null)
            {
                var billingParty = inputModel.BillingAddress.ToParty();
                var parties = cart.Parties.ToList();
                parties.Add(billingParty);
                cart.Parties = parties.AsSafeReadOnly();

                payments.Add(inputModel.CreditCardPayment.ToCreditCardPaymentInfo());
            }

            if (inputModel.FederatedPayment != null && !string.IsNullOrEmpty(inputModel.FederatedPayment.CardToken) && inputModel.BillingAddress != null)
            {
                var billingParty = inputModel.BillingAddress.ToParty();
                var parties = cart.Parties.ToList();
                parties.Add(billingParty);
                cart.Parties = parties.AsSafeReadOnly();

                var federatedPayment = inputModel.FederatedPayment.ToCreditCardPaymentInfo();
                federatedPayment.PartyID = billingParty.PartyId;
                payments.Add(federatedPayment);
            }

            if (inputModel.GiftCardPayment != null && !string.IsNullOrEmpty(inputModel.GiftCardPayment.PaymentMethodID))
            {
                payments.Add(inputModel.GiftCardPayment.ToGiftCardPaymentInfo());
            }

            if (inputModel.LoyaltyCardPayment != null && !string.IsNullOrEmpty(inputModel.LoyaltyCardPayment.PaymentMethodID))
            {
                payments.Add(inputModel.LoyaltyCardPayment.ToLoyaltyCardPaymentInfo());
            }

            var request = new AddPaymentInfoRequest(cart, payments);
            var result = _cartServiceProvider.AddPaymentInfo(request);

            if (!result.Success)
            {
                return false;
            }
            UpdateCartInCache(cart);
            return true;
        }

        public string SubmitOrder(SubmitOrderInputModel inputModel)
        {
            var response = GetCustomerCart();
            var cart = (CommerceCart)response;

            if (cart.Lines.Count == 0)
            {
                return "No items in cart";
            }

            cart.Email = inputModel.UserEmail;

            var request = new SubmitVisitorOrderRequest(cart);
            //request.RefreshCart(true);
            var req = CartRequestInformation.Get(request);
            //req.Refresh = true;
            if (req == null)
            {
                req = new CartRequestInformation(request, true);
            }
            else
            {
                req.Refresh = true;
            }

            var errorResult = _orderServiceProvider.SubmitVisitorOrder(request);
            string ret;
            if (errorResult.Success && errorResult.Order != null && errorResult.CartWithErrors == null)
            {
                // clear cart cache
                ClearCartFromCache();
                cart = errorResult.CartWithErrors as CommerceCart;
                ret = errorResult.Order.OrderID;
                return "Success! - id: " + ret;
                //var cartCache = CommerceTypeLoader.CreateInstance<CartCacheHelper>();
                //cartCache.InvalidateCartCache(visitorContext.GetCustomerId());
            }
            else
            {

                ret = "There was an error! - ";

                if (errorResult.SystemMessages != null && errorResult.SystemMessages.Any())
                {
                    ret = ret + errorResult.SystemMessages[0].Message;
                }
            }

            return ret;
        }
        internal bool CompleteACheckout4(string nounce)
        {
            // Get visitor identifier
            var visitorId = GetVisitorId();
            var cart = GetCustomerCart();

            // add billing and shipping parties 
            //var billingParty = new CommerceParty() { ExternalId = visitorId, PartyId = "{F73904C0-2A45-4A2F-A99BF934ABDCFC99}", FirstName = "Joe", LastName = "Smith", Address1 = "123 Street", City = "Ottawa", State = "Ontario", Country = "Canada" };
            //var shippingParty = new CommerceParty() { ExternalId = visitorId, PartyId = "{294B7DD1-7397-4322-996CE87E592EF621}", FirstName = "Jane", LastName = "Smith", Address1 = "234 Street", City = "Toronto", State = "Ontario", Country = "Canada" };
            var billingParty = new CommerceParty() { ExternalId = "0", Name = "Billing", PartyId = "0", FirstName = "Joe", LastName = "Smith", Address1 = "123 Street", City = "Ottawa", State = "ON", Country = "Canada", ZipPostalCode = "12345", CountryCode = "CA" };
            var shippingParty = new CommerceParty() { ExternalId = "0", Name = "Shipping", PartyId = "0", FirstName = "Jane", LastName = "Smith", Address1 = "234 Street", City = "Toronto", State = "ON", Country = "Canada", ZipPostalCode = "12345", CountryCode = "CA" };

            //var partyList = new List<Party> { billingParty, shippingParty };
            //var addPartiesRequest = new AddPartiesRequest(cart, partyList);
            //var addPartiesResult = _cartServiceProvider.AddParties(addPartiesRequest);

            // add shipping info
            var addShippingInfoResult = new AddShippingInfoResult { Success = false };

            // set parties
            var cartParties = cart.Parties.ToList();
            var partyList = new List<Party> { shippingParty };
            cartParties.AddRange(partyList);
            cart.Parties = cartParties.AsReadOnly();

            // Shipping methods
            var shippingMethodList = new List<ShippingMethodInputModelItem>();
            var shippingMethod = new ShippingMethodInputModelItem
            {
                ShippingMethodID = "e14965b9-306a-43c4-bffc-3c67be8726fa",
                //ShippingMethodID = "a14965b9-306a-53c4-bffc-5c69be8726fa",
                ShippingMethodName = "Ground",
                ShippingPreferenceType = "1",
                PartyID = "0"
            };

            shippingMethodList.Add(shippingMethod);
            var internalShippingList = shippingMethodList.ToShippingInfoList();

            var orderPreferenceType = InputModelExtension.GetShippingOptionType("1");

            if (orderPreferenceType != ShippingOptionType.DeliverItemsIndividually)
            {
                foreach (var shipping in internalShippingList)
                {
                    shipping.LineIDs = (from CommerceCartLine lineItem in cart.Lines select lineItem.ExternalCartLineId).ToList().AsReadOnly();
                }
            }

            // clear cart from cache
            ClearCartFromCache();

            var shipments = new List<ShippingInfo>();
            shipments.AddRange(internalShippingList);
            var addShippingInfoRequest = new Sitecore.Commerce.Engine.Connect.Services.Carts.AddShippingInfoRequest(cart, shipments, orderPreferenceType);
            addShippingInfoResult = _cartServiceProvider.AddShippingInfo(addShippingInfoRequest);

            string msg;
            if (!addShippingInfoResult.Success)
            {
                msg = addShippingInfoResult.SystemMessages[0].Message;
            }

            if (addShippingInfoResult.Success && addShippingInfoResult.Cart != null)
            {
                AddCartToCache(addShippingInfoResult.Cart as CommerceCart);
            }

            var updatedCart = GetCustomerCart();

            if (updatedCart == null)
            {
                msg = "No cart";

            }

            var payments = new List<PaymentInfo>();

            var miniCart = GetMiniCart();

            // Add billing party
            var parties = updatedCart.Parties.ToList();
            parties.Add(billingParty);
            updatedCart.Parties = parties.AsSafeReadOnly();

            // Add payment info
            var addPaymentInfoResult = new AddPaymentInfoResult { Success = false };
            var federatedPaymentModel = new FederatedPaymentInputModelItem
            {
                CardToken = nounce,
                Amount = updatedCart.Total.Amount,
                CardPaymentAcceptCardPrefix = "paypal"
            };

            var federatedPayment = federatedPaymentModel.ToCreditCardPaymentInfo();
            federatedPayment.PartyID = billingParty.PartyId;
            payments.Add(federatedPayment);

            var addPaymentInfoRequest = new AddPaymentInfoRequest(updatedCart, payments);
            addPaymentInfoResult = _cartServiceProvider.AddPaymentInfo(addPaymentInfoRequest);
            if (!addPaymentInfoResult.Success)
            {
                msg = addPaymentInfoResult.SystemMessages[0].Message;
            }

            updatedCart = addPaymentInfoResult.Cart as CommerceCart;
            UpdateCartInCache(updatedCart);

            // Submit Order

            var submitVisitorOrderResult = new SubmitVisitorOrderResult { Success = false };

            if (updatedCart.Lines.Count == 0)
            {
                msg = "No items in cart";
            }

            updatedCart.Email = "testorder@mail.com";
            // GetCart(string userName, bool refreshCart = false)
            var submitVisitorOrderRequest = new SubmitVisitorOrderRequest(updatedCart);
            submitVisitorOrderResult = _orderServiceProvider.SubmitVisitorOrder(submitVisitorOrderRequest);

            var orderId = string.Empty;

            if (submitVisitorOrderResult.Success && submitVisitorOrderResult.Order != null && submitVisitorOrderResult.CartWithErrors == null)
            {
                var order = submitVisitorOrderResult.Order as CommerceOrder;
                orderId = submitVisitorOrderResult.Order.OrderID;
                var commerceOrderId = order.OrderID;

                // clear cart from cache
                ClearCartFromCache();

                // Get order details
                var getVisitorOrderRequest = new GetVisitorOrderRequest(orderId, visitorId, ShopName);
                var getVisitorOrderResult = _orderServiceProvider.GetVisitorOrder(getVisitorOrderRequest);


            }

            return false;
        }


        internal bool CompleteACheckout3()
        {
            var msg = string.Empty;

            // Get visitor identifier
            var visitorId = GetVisitorId();
            var cart = GetCustomerCart();

            // add billing and shipping parties 
            //var billingParty = new CommerceParty() { ExternalId = visitorId, PartyId = "{F73904C0-2A45-4A2F-A99BF934ABDCFC99}", FirstName = "Joe", LastName = "Smith", Address1 = "123 Street", City = "Ottawa", State = "Ontario", Country = "Canada" };
            //var shippingParty = new CommerceParty() { ExternalId = visitorId, PartyId = "{294B7DD1-7397-4322-996CE87E592EF621}", FirstName = "Jane", LastName = "Smith", Address1 = "234 Street", City = "Toronto", State = "Ontario", Country = "Canada" };
            var billingParty = new CommerceParty() { ExternalId = "0", Name = "Billing", PartyId = "0", FirstName = "Joe", LastName = "Smith", Address1 = "123 Street", City = "Ottawa", State = "ON", Country = "Canada", ZipPostalCode = "12345", CountryCode = "CA" };
            var shippingParty = new CommerceParty() { ExternalId = "0", Name = "Shipping", PartyId = "0", FirstName = "Jane", LastName = "Smith", Address1 = "234 Street", City = "Toronto", State = "ON", Country = "Canada", ZipPostalCode = "12345", CountryCode = "CA" };

            //var partyList = new List<Party> { billingParty, shippingParty };
            //var addPartiesRequest = new AddPartiesRequest(cart, partyList);
            //var addPartiesResult = _cartServiceProvider.AddParties(addPartiesRequest);

            // add shipping info
            var addShippingInfoResult = new AddShippingInfoResult { Success = false };

            // set parties
            var cartParties = cart.Parties.ToList();
            var partyList = new List<Party> { shippingParty };
            cartParties.AddRange(partyList);
            cart.Parties = cartParties.AsReadOnly();

            // Shipping methods
            var shippingMethodList = new List<ShippingMethodInputModelItem>();
            var shippingMethod = new ShippingMethodInputModelItem
            {
                ShippingMethodID = "e14965b9-306a-43c4-bffc-3c67be8726fa",
                ShippingMethodName = "Ground",
                ShippingPreferenceType = "1",
                PartyID = "0"
            };

            shippingMethodList.Add(shippingMethod);
            var internalShippingList = shippingMethodList.ToShippingInfoList();

            var orderPreferenceType = InputModelExtension.GetShippingOptionType("1");

            if (orderPreferenceType != ShippingOptionType.DeliverItemsIndividually)
            {
                foreach (var shipping in internalShippingList)
                {
                    shipping.LineIDs = (from CommerceCartLine lineItem in cart.Lines select lineItem.ExternalCartLineId).ToList().AsReadOnly();
                }
            }

            // clear cart from cache
            ClearCartFromCache();

            var shipments = new List<ShippingInfo>();
            shipments.AddRange(internalShippingList);
            var addShippingInfoRequest = new Sitecore.Commerce.Engine.Connect.Services.Carts.AddShippingInfoRequest(cart, shipments, orderPreferenceType);
            addShippingInfoResult = _cartServiceProvider.AddShippingInfo(addShippingInfoRequest);
            if (!addShippingInfoResult.Success)
            {
                msg = addShippingInfoResult.SystemMessages[0].Message;
            }

            if (addShippingInfoResult.Success && addShippingInfoResult.Cart != null)
            {
                AddCartToCache(addShippingInfoResult.Cart as CommerceCart);
            }

            var updatedCart = GetCustomerCart();

            if (updatedCart == null)
            {
                msg = "No cart";

            }

            var payments = new List<PaymentInfo>();

            var miniCart = GetMiniCart();

            // Add billing party
            var parties = updatedCart.Parties.ToList();
            parties.Add(billingParty);
            updatedCart.Parties = parties.AsSafeReadOnly();

            // Add payment info
            var addPaymentInfoResult = new AddPaymentInfoResult { Success = false };
            var federatedPaymentModel = new FederatedPaymentInputModelItem
            {
                CardToken = "fdc99443-2c41-04e3-17ac-a9bbda610f88",
                Amount = updatedCart.Total.Amount,
                CardPaymentAcceptCardPrefix = "paypal"
            };

            var federatedPayment = federatedPaymentModel.ToCreditCardPaymentInfo();
            federatedPayment.PartyID = billingParty.PartyId;
            payments.Add(federatedPayment);

            var addPaymentInfoRequest = new AddPaymentInfoRequest(updatedCart, payments);
            addPaymentInfoResult = _cartServiceProvider.AddPaymentInfo(addPaymentInfoRequest);
            if (!addPaymentInfoResult.Success)
            {
                msg = addPaymentInfoResult.SystemMessages[0].Message;
            }

            updatedCart = addPaymentInfoResult.Cart as CommerceCart;
            UpdateCartInCache(updatedCart);

            // Submit Order

            var submitVisitorOrderResult = new SubmitVisitorOrderResult { Success = false };

            if (updatedCart.Lines.Count == 0)
            {
                msg = "No items in cart";
            }

            updatedCart.Email = "testorder@mail.com";
            // GetCart(string userName, bool refreshCart = false)
            var submitVisitorOrderRequest = new SubmitVisitorOrderRequest(updatedCart);
            submitVisitorOrderResult = _orderServiceProvider.SubmitVisitorOrder(submitVisitorOrderRequest);

            var orderId = string.Empty;

            if (submitVisitorOrderResult.Success && submitVisitorOrderResult.Order != null && submitVisitorOrderResult.CartWithErrors == null)
            {
                var order = submitVisitorOrderResult.Order as CommerceOrder;
                orderId = submitVisitorOrderResult.Order.OrderID;
                var commerceOrderId = order.OrderID;

                // clear cart from cache
                ClearCartFromCache();

                // Get order details
                var getVisitorOrderRequest = new GetVisitorOrderRequest(orderId, visitorId, ShopName);
                var getVisitorOrderResult = _orderServiceProvider.GetVisitorOrder(getVisitorOrderRequest);


            }



            return false;

        }

        internal bool CompleteACheckout2()
        {

            // Get visitor identifier
            var visitorId = GetVisitorId();

            var cart = GetCustomerCart();

            // add billing and shipping parties 
            var billingParty = new CommerceParty() { ExternalId = visitorId, PartyId = "{F73904C0-2A45-4A2F-A99BF934ABDCFC99}", FirstName = "Joe", LastName = "Smith", Address1 = "123 Street", City = "Ottawa", State = "Ontario", Country = "Canada" };
            var shippingParty = new CommerceParty() { ExternalId = visitorId, PartyId = "{294B7DD1-7397-4322-996CE87E592EF621}", FirstName = "Jane", LastName = "Smith", Address1 = "234 Street", City = "Toronto", State = "Ontario", Country = "Canada" };

            var partyList = new List<Party> { billingParty, shippingParty };
            var addPartiesRequest = new AddPartiesRequest(cart, partyList);
            var addPartiesResult = _cartServiceProvider.AddParties(addPartiesRequest);

            // add shipping info

            var shippingInfo = new ShippingInfo
            {
                ShippingMethodID = "e14965b9-306a-43c4-bffc-3c67be8726fa",
                PartyID = shippingParty.PartyId,
                ExternalId = visitorId,
                LineIDs =
        (from CommerceCartLine lineItem in cart.Lines select lineItem.ExternalCartLineId).ToList()
            .AsReadOnly()
            };
            //var addRequest = new AddShippingInfoRequest(cart, new List<ShippingInfo> { shippingInfo });
            //var addResult = _cartServiceProvider.AddShippingInfo(request);

            var addShippingInfoRequest = new Sitecore.Commerce.Engine.Connect.Services.Carts.AddShippingInfoRequest(cart, new List<ShippingInfo> { shippingInfo }, ShippingOptionType.ShipToAddress);
            var addShippingInfoResult = _cartServiceProvider.AddShippingInfo(addShippingInfoRequest);


            // Add Payment info
            var AddPaymentInfoResult = new AddPaymentInfoResult { Success = false };
            var payments = new List<PaymentInfo>();

            var federatedPaymentObj = new FederatedPaymentInputModelItem
            {
                CardToken = "83f75be7-5f73-0f07-190e-df2017045f5b",
                Amount = 198.00m,
                CardPaymentAcceptCardPrefix = "paypal",
                PaymentMethodID = "1"
            };
            var federatedPayment = federatedPaymentObj.ToCreditCardPaymentInfo();
            federatedPayment.PartyID = billingParty.PartyId;
            payments.Add(federatedPayment);

            var addPaymentInfoRequest = new AddPaymentInfoRequest(cart, payments);
            var addPaymentInfoResult = _cartServiceProvider.AddPaymentInfo(addPaymentInfoRequest);
            if (!addPaymentInfoResult.Success)
            {

            }
            UpdateCartInCache(cart);

            // Submit the order
            var msg = string.Empty;
            var submitVisitorOrderResult = new SubmitVisitorOrderResult { Success = false };

            if (cart.Lines.Count == 0)
            {
                msg = "No items in cart";
            }

            cart.Email = "testorder1@mail.com";

            var submitVisitorOrderRequest = new SubmitVisitorOrderRequest(cart);

            submitVisitorOrderResult = _orderServiceProvider.SubmitVisitorOrder(submitVisitorOrderRequest);
            if (submitVisitorOrderResult.Success && submitVisitorOrderResult.Order != null && submitVisitorOrderResult.CartWithErrors == null)
            {
                // clear cart cache
                ClearCartFromCache();
                cart = submitVisitorOrderResult.CartWithErrors as CommerceCart;
                msg = submitVisitorOrderResult.Order.OrderID;
            }
            else
            {

                msg = "There was an error! - ";

                if (submitVisitorOrderResult.SystemMessages != null && submitVisitorOrderResult.SystemMessages.Any())
                {
                    msg = msg + submitVisitorOrderResult.SystemMessages[0].Message;
                }
            }


            return false;

        }

        internal bool CompleteACheckout()
        {

            // Get visitor identifier
            var visitorId = GetVisitorId();

            var cartService = new CommerceCartServiceProvider();

            // get the cart 
            var cartReq = new CreateOrResumeCartRequest(ShopName, visitorId);
            var cart = cartService.CreateOrResumeCart(cartReq).Cart;

            // add parties, payment and shipping info 

            var partyList = new List<Party> {
                new Party() { ExternalId = visitorId, PartyId = "{F73904C0-2A45-4A2F-A99BF934ABDCFC99}", FirstName = "Joe", LastName = "Smith", Address1 = "123 Street", City = "Ottawa", State = "Ontario", Country = "Canada" },
                new Party() { ExternalId = visitorId, PartyId = "{294B7DD1-7397-4322-996CE87E592EF621}", FirstName = "Jane", LastName = "Smith", Address1 = "234 Street", City = "Toronto", State = "Ontario", Country = "Canada" }            };
            var addPartiesRequest = new AddPartiesRequest(cart, partyList);
            var addPartiesResult = cartService.AddParties(addPartiesRequest);



            cart.BuyerCustomerParty = new CartParty() { ExternalId = visitorId, PartyID = "{F73904C02A45-4A2F-A99B-F934ABDCFC99}" };
            cart.AccountingCustomerParty = new CartParty() { ExternalId = visitorId, PartyID = "{294B7DD1-7397-4322-996C-E87E592EF621}" };

            // Adding Payment info
            var paymentList = new List<PaymentInfo> {
                new PaymentInfo() { ExternalId = visitorId, PaymentMethodID = "1" },
            };
            var addPaymentRequest = new AddPaymentInfoRequest(cart, paymentList);
            var addPaymentResult = cartService.AddPaymentInfo(addPaymentRequest);

            // Adding shipping info
            var shippingList = new List<ShippingInfo> {
                new ShippingInfo() { ExternalId = visitorId, ShippingMethodID = "e14965b9-306a-43c4-bffc-3c67be8726fa" },
            };
            var addRequest = new AddShippingInfoRequest(cart, shippingList);
            var addResult = cartService.AddShippingInfo(addRequest);


            cartService.SaveCart(new SaveCartRequest(cart));

            var orderService = new OrderServiceProvider();

            var request = new SubmitVisitorOrderRequest(cart);
            var result = orderService.SubmitVisitorOrder(request);

            var order = result.Order; var orderId = order.OrderID;


            return false;
        }

        public GetShippingMethodsResult GetShippingMethods()
        {

			// Set Shippng Address as facility
			var inmate = InmateHelper.GetSelectedInmate();
			var faciltiyAddress = FacilityHelper.GetFacilityAddress();

			faciltiyAddress.FirstName = inmate.FirstName;
			faciltiyAddress.LastName = inmate.LastName;
			faciltiyAddress.CountryCode = faciltiyAddress.Country;
			faciltiyAddress.InmateId = inmate.Id;

			ApplyShippingToCart(faciltiyAddress);


			try
            {
                var shippingService = new ShippingServiceProvider();
                var shippingOption = new ShippingOption
                {
                    ShippingOptionType = new ShippingOptionType(1, "Ship To Address"),
                    ShopName = ShopName
                };

                var request =
                    new Sitecore.Commerce.Engine.Connect.Services.Shipping.GetShippingMethodsRequest(shippingOption,
                        null, GetCustomerCart());

                return shippingService.GetShippingMethods(request);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error in CartHelper.GetShippingMethods", ex);
                return null;
            }
        }

        /// <summary>
        /// Submits cart with existing line items using default shipping, billing and payment info (tokenized)
        /// </summary>
        /// <returns></returns>
        public object ExpressCheckout()
        {
            var result = new { success = false, orderId = string.Empty };

            try
            {
                //0 - validate cart
                var cart = GetCustomerCart();
                if (cart == null)
                {
                    throw new ArgumentException("Cannot find cart.");
                }

                if (cart.LineItemCount <= 0)
                {
                    throw new ArgumentException("There are no items in the cart.");
                }

                if (Context.User.Name.ToUpper().Contains("ANONYMOUS"))
                {
                    throw new Exception("You have to be logged in to use Express Checkout.");
                }

                //1 - add product if needed
                //AddToCart(new CartLineItem());

                //3 - add shipping info
                var user = new AccountHelper().GetUser(Context.User.Name);
                var address = new Address //TODO:  update on prod, for demo only
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address1 = "1234 NY Street",
                    City = "New York",
                    State = "NY",
                    ZipPostalCode = "10009",
                    CountryCode = "US"
                };

                if (!ApplyShippingToCart(address))
                    throw new Exception("ApplyShippingToCart failed.");

                //4 - add shipping method
                const string shippingMethodId = "e14965b9-306a-43c4-bffc-3c67be8726fa|Ground"; //TODO: update on prod, for demo only

                if (!AddShippingMethodToCart(shippingMethodId))
                    throw new Exception("AddShippingMethodToCart failed.");

                //5 - add payment info
                //VAULT METHOD
                var paymentMethodToken = ConfigurationHelper.GetBraintreeVaultPaymentToken();  //TODO: update on prod, for demo only
                var payment = new Payment
                {
                    BillingAddress = address,
                    CardPrefix = "paypal",
                    Token = $"vault|{paymentMethodToken}" //indicate use of Vault token, must use modified braintree plugin on CommerceAuthoring
                };

                if (!ApplyNewPaymentMethodToCart(payment))
                    throw new Exception("ApplyNewPaymentMethodToCart failed.");


                //6 - submit
                var submitCartResult = SubmitCart();

                if (!submitCartResult.Contains("Error"))
                {
                    result = new { success = true, orderId = submitCartResult };
                }

            }
            catch (Exception ex)
            {
                Log.Error($"CartHelper.ExpressCheckout(), Error={ex.Message}", ex);
            }

            return result;
        }

        public ValidatePurchaseResult ValidatePurchase(string productId, int qty)
        {
            var result = new ValidatePurchaseResult
            {
                ProductId = productId,
                Quantity = qty
            };

            try
            {
                var summary = GetProductGroupsSummaryByCart();

                if (summary == null || summary.Count <= 0)
                {
                    result.Message = "No Product Group Summary OR No items in cart";
                    result.IsValidForPurchase = true;
                    return result;
                }

                var product = ProductHelper.GetCommerceItemByProductId(productId);

                if (product == null)
                {
                    result.Message = "Product not found.";
                    return result;
                }

                if (!string.IsNullOrWhiteSpace(product["commerceproductgroupids"]))
                {
                    var productGroupIds = product["commerceproductgroupids"].Split('|');
                    var productGroupdIdGuids = productGroupIds.Select(i => Guid.Parse(i));

                    //remove all entries in summary not associated to product
                    summary = summary.Where(s => productGroupdIdGuids.Contains(s.Id)).ToList();

                    var isOverWeight = false;
                    var isOverPriced = false;

                    foreach (var productGroupId in productGroupIds)
                    {
                        foreach (var groupSummary in summary)
                        {
                            if (groupSummary.Id == Guid.Parse(productGroupId))
                            {
                                groupSummary.PriceTotal += qty * decimal.Parse(product["listprice"]);
                                groupSummary.WeightTotal += !string.IsNullOrWhiteSpace(product["weight"]) ? qty * double.Parse(product["weight"]) : 0;

                                if (!isOverPriced)
                                    isOverPriced = groupSummary.PriceTotal > groupSummary.PriceLimit;

                                if (!isOverWeight)
                                    isOverWeight = groupSummary.WeightTotal > groupSummary.WeightLimit;
                            }
                        }
                    }

                    result.ProductGroups = summary;
                    result.Message = isOverPriced ? "Price Limit reached|" : string.Empty;
                    result.Message += isOverWeight ? "Weight Limit reached" : string.Empty;
                    result.IsValidForPurchase = !isOverPriced && !isOverWeight;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"CartHelper.IsValidForPurchase(), Error = {ex.Message}", ex);
            }

            return result;
        }

        public static List<ProductGroup> GetProductGroupsSummaryByCart()
        {
            var summary = new List<ProductGroup>();

            try
            {
                //get all products in cart
                var cart = new CartHelper().GetCustomerCart();
                var cartLines = cart.Lines;

                if (cartLines == null || cartLines.Count <= 0) return summary;

                summary = GetProductGroupsByStorefront(Context.Site.RootPath);

                if (summary == null || summary.Count <= 0) return summary;

                //for each product, get all groups
                foreach (var cartLine in cartLines)
                {
                    var product = ProductHelper.GetCommerceItemByProductId(cartLine.Product.ProductId);

                    if (!string.IsNullOrWhiteSpace(product?["commerceproductgroupids"]))
                    {
                        var productGroupIds = product["commerceproductgroupids"].Split('|');

                        foreach (var productGroupId in productGroupIds)
                        {
                            foreach (var groupSummary in summary)
                            {
                                if (groupSummary.Id == Guid.Parse(productGroupId))
                                {
                                    groupSummary.PriceTotal += cartLine.Total.Amount;
                                    groupSummary.WeightTotal += !string.IsNullOrWhiteSpace(product["weight"]) ? cartLine.Quantity * double.Parse(product["weight"]) : 0;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error($"CartHelper.GetProductGroupsSummaryByCart(), Error = {ex.Message}", ex);
            }

            return summary;
        }

        /// <summary>
        /// Returns List of Product Groups for the site with configured limits
        /// </summary>
        /// <param name="storefront"></param>
        /// <returns></returns>
        public static List<ProductGroup> GetProductGroupsByStorefront(string storefrontRootPath)
        {
            var index = ContentSearchManager.GetIndex(ConfigurationHelper.GetSearchIndex());
            try
            {
                using (var context = index.CreateSearchContext())
                {
                    var siteRoot = ConfigurationHelper.GetSiteRoot();

                    var searchHits = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Context.Language.Name
                                    //&& x.Version == "1"
                                    && x.TemplateName == "Product Group Setting"
                                    && x.Parent == Sitecore.Data.ID.Parse(siteRoot.ProgramId)
                                    ).GetResults().ToList();

                    var groupSummaries = new List<ProductGroup>();

                    foreach (var searchHit in searchHits)
                    {
                        var item = searchHit.Document;

                        var productGroup = item["ProductGroup"];

                        if (!string.IsNullOrWhiteSpace(productGroup))
                        {
                            var groupSummary = new ProductGroup
                            {
                                Id = Guid.Parse(productGroup),
                                Name = GetProductGroupName(Guid.Parse(productGroup)),
                                SettingsName = item.Name,
                                PriceLimit = string.IsNullOrWhiteSpace(item["PriceLimit"]) ? 0 : decimal.Parse(item["PriceLimit"]),
                                PriceTotal = 0,
                                WeightLimit = string.IsNullOrWhiteSpace(item["WeightLimit"]) ? 0 : double.Parse(item["WeightLimit"]),
                                WeightTotal = 0
                            };

                            groupSummaries.Add(groupSummary);
                        }

                    }

                    return groupSummaries;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"CartHelper.GetProductGroupLimits(), Error = {ex.Message}", ex);
            }

            return new List<ProductGroup>();
        }

        private static string GetProductGroupName(Guid productGroupId)
        {
            try
            {
                var item = Context.Database.Items.GetItem(ID.Parse(productGroupId));

                if (item != null)
                {
                    return item.DisplayName;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"CartHelper.GetProductGroupName(), Error = {ex.Message}", ex);
            }

            return string.Empty;
        }

        public class ProductGroup
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string SettingsName { get; set; }
            public double WeightLimit { get; set; }
            public double WeightTotal { get; set; }
            public decimal PriceLimit { get; set; }
            public decimal PriceTotal { get; set; }
        }

        public class ValidatePurchaseResult
        {
            public string ProductId { get; set; }
            public List<ProductGroup> ProductGroups { get; set; }
            public bool IsValidForPurchase { get; set; }
            public string Message { get; set; }
            public int Quantity { get; set; }
        }
    }
}
