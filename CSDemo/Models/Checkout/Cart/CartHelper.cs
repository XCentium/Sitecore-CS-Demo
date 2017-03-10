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
using Sitecore.Commerce.Services.Prices;
using Sitecore.Commerce.Engine.Connect.Pipelines.Arguments;
using Sitecore.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using AddPartiesRequest = Sitecore.Commerce.Services.Carts.AddPartiesRequest;
using UpdatePartiesRequest = Sitecore.Commerce.Services.Carts.UpdatePartiesRequest;
using Sitecore.Commerce.Services.Payments;
using Sitecore.Commerce.Entities.Payments;
using System.Diagnostics.CodeAnalysis;
using Sitecore.Commerce.Entities.Orders;
using WebGrease.Css.Extensions;
using Sitecore.Commerce.Entities.Shipping;

namespace CSDemo.Models.Checkout.Cart
{
    public class CartHelper
    {
        public string ShopName { get; set; }
        public string DefaultCartName { get; set; }
        private readonly InventoryServiceProvider _inventoryServiceProvider = new InventoryServiceProvider();
        private readonly PricingServiceProvider _pricingServiceProvider = new PricingServiceProvider();
        private readonly CartServiceProvider _serviceProvider = new CommerceCartServiceProvider();

        private readonly PaymentServiceProvider _paymentServiceProvider = new PaymentServiceProvider();
        private readonly OrderServiceProvider _orderServiceProvider = new OrderServiceProvider();
        // 
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

        public CommerceCart AddToCart(CartLineItem cartLine)
        {
            // create cartitem
            var cartItem = new CommerceCartLine(cartLine.CatalogName, cartLine.ProductId,
                cartLine.VariantId == "-1" ? null : cartLine.VariantId, cartLine.Quantity);

            // update stock in formation
            // push cart to commerce server
            UpdateStockInformation(cartItem, cartLine.CatalogName);

            // get userID
            var visitorId = GetVisitorId();
            var request = new AddCartLinesRequest(GetCart(visitorId, false), new[] { cartItem });
            var info = CartRequestInformation.Get(request);
            if (info == null)
            {
                info = new CartRequestInformation(request, true);
            }
            else
            {
                info.Refresh = true;
            }
            var cartResult = _serviceProvider.AddCartLines(request);

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
            var CookieExpirationInDays = 365;
            var CookieName = Constants.Cart.CookieName;
            var cartCookie = HttpContext.Current.Request.Cookies[CookieName] ?? new HttpCookie(CookieName);
            cartCookie.Values[Constants.Cart.VisitorID] = customerId;
            cartCookie.Expires = DateTime.Now.AddDays(CookieExpirationInDays);
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
        private bool DeleteCustomerCartCookie(string id)
        {
            var CookieName = Constants.Cart.CookieName;
            var cartCookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cartCookie == null)
            {
                return false;
            }
            // Render cookie invalid
            HttpContext.Current.Response.Cookies.Remove(CookieName);
            cartCookie.Expires = DateTime.Now.AddDays(-10);
            cartCookie.Values[Constants.Cart.VisitorID] = null;
            cartCookie.Value = null;
            HttpContext.Current.Response.SetCookie(cartCookie);
            return true;
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
            var result = _serviceProvider.LoadCart(request);
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
            var commerceInventoryProduct = new CommerceInventoryProduct();
            commerceInventoryProduct.ProductId = productId;
            commerceInventoryProduct.CatalogName = catalogName;
            if (!string.IsNullOrEmpty(variantId)) { commerceInventoryProduct.VariantId = variantId; }
            var products = new List<InventoryProduct>();
            products.Add(commerceInventoryProduct);

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
            var virtualData = virtualCatalogId.Replace(")", String.Empty).Split('(');

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
            var VisitorTrackingCookieName = Constants.Cart.VisitorTrackingCookieName;
            var VisitorIdKeyName = Constants.Cart.VisitorIdKeyName;
            var VisitorCookieExpiryInDays = 1;
            var visitorCookie = HttpContext.Current.Request.Cookies[VisitorTrackingCookieName] ??
                                new HttpCookie(VisitorTrackingCookieName);
            if (string.IsNullOrEmpty(visitorCookie.Values[VisitorIdKeyName]))
            {
                visitorCookie.Values[VisitorIdKeyName] = Guid.NewGuid().ToString("D");
            }
            visitorCookie.Expires = DateTime.Now.AddDays(VisitorCookieExpiryInDays);
            HttpContext.Current.Response.SetCookie(visitorCookie);
            return visitorCookie.Values[VisitorIdKeyName];
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
            if (Sitecore.Context.IsLoggedIn &&
                !Context.User.Name.ToLower().Contains(Constants.Commerce.CommerceUserDomain.ToLower())) return new ShoppingCart();

            var cart = cartFromCommServer == true ? GetCart(GetVisitorId(), true) : GetCustomerCart();
            var shoppingCartTotal = cart.Total as CommerceTotal;
            var shoppingCart = new ShoppingCart();
            if (cart == null || shoppingCartTotal == null) return shoppingCart;

            shoppingCart.LineTotal = cart.Total as CommerceTotal == null
                ? 0
                : (cart.Total as CommerceTotal).Subtotal;
            shoppingCart.Total = cart.LineItemCount;

           
            var commerceTotal = (CommerceTotal)cart.Total;

            shoppingCart.LineDiscount = commerceTotal.LineItemDiscountAmount;
            shoppingCart.OrderLevelDiscountAmount = commerceTotal.OrderLevelDiscountAmount;
            
            shoppingCart.Shipping = commerceTotal == null ? 0.00m : commerceTotal.ShippingTotal;
            shoppingCart.Tax = cart.Total.TaxTotal.Amount == null ? 0.00m : cart.Total.TaxTotal.Amount;
            shoppingCart.GrandTotal = cart.Total.Amount == null ? 0.00m : cart.Total.Amount;

            if (Sitecore.Context.User.IsInRole("CommerceUsers\\Dealer")) { shoppingCart.LineTotal = shoppingCart.LineTotal > 0 ? (decimal)0.90 * shoppingCart.LineTotal : shoppingCart.LineTotal; }
            if (Sitecore.Context.User.IsInRole("CommerceUsers\\Retailer")) { shoppingCart.LineTotal = shoppingCart.LineTotal > 0 ? (decimal)0.75 * shoppingCart.LineTotal : shoppingCart.LineTotal; }

            if (Sitecore.Context.User.IsInRole("CommerceUsers\\Dealer")) { shoppingCart.GrandTotal = shoppingCart.GrandTotal > 0 ? (decimal)0.90 * shoppingCart.GrandTotal : shoppingCart.GrandTotal; }
            if (Sitecore.Context.User.IsInRole("CommerceUsers\\Retailer")) { shoppingCart.GrandTotal = shoppingCart.GrandTotal > 0 ? (decimal)0.75 * shoppingCart.GrandTotal : shoppingCart.GrandTotal; }



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
                    cartItem.SubTotal = cartLine.Total.Amount;
                    cartItem.ExternalId = cartLine.ExternalCartLineId;



                    if (Sitecore.Context.User.IsInRole("CommerceUsers\\Dealer")) { cartItem.UnitPrice = cartItem.UnitPrice > 0 ? (decimal)0.90 * cartItem.UnitPrice : cartItem.UnitPrice; }
                    if (Sitecore.Context.User.IsInRole("CommerceUsers\\Retailer")) { cartItem.UnitPrice = cartItem.UnitPrice > 0 ? (decimal)0.75 * cartItem.UnitPrice : cartItem.UnitPrice; }

                    if (Sitecore.Context.User.IsInRole("CommerceUsers\\Dealer")) { cartItem.SubTotal = cartItem.SubTotal > 0 ? (decimal)0.90 * cartItem.SubTotal : cartItem.SubTotal; }
                    if (Sitecore.Context.User.IsInRole("CommerceUsers\\Retailer")) { cartItem.SubTotal = cartItem.SubTotal > 0 ? (decimal)0.75 * cartItem.SubTotal : cartItem.SubTotal; }


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
            var CookieName = Constants.Cart.CookieName;
            var cartCookie = HttpContext.Current.Request.Cookies[CookieName];
            return cartCookie != null && cartCookie.Values[Constants.Cart.VisitorID] == customerId;
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
        internal bool ApplyShippingToCart(Address shippingAddress)
        {

            var visitorId = GetVisitorId();

            var cart = GetCustomerCart();

            var shipping = new CommerceParty();

            shipping.ExternalId =  "0";
            shipping.Name = Constants.Products.ShippingAddress;
            shipping.PartyId = "0";

            shipping.FirstName = shippingAddress.FirstName;
            shipping.LastName = shippingAddress.LastName;
            shipping.Address1 = shippingAddress.Address1;
            shipping.Address2 = shippingAddress.Address2;
            shipping.City = shippingAddress.City;
            shipping.State = shippingAddress.State;
            shipping.Company = shippingAddress.Company;
            shipping.Email = shippingAddress.Email;
            shipping.FaxNumber = shippingAddress.FaxNumber;
            shipping.Country = shippingAddress.Country;
            shipping.CountryCode = shippingAddress.CountryCode;
            shipping.ZipPostalCode = shippingAddress.ZipPostalCode;

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

            var billing = new CommerceParty();
            billing.ExternalId = "0";
            billing.PartyId = "1";
            billing.Name = Constants.Products.BillingAddress;

            billing.FirstName = firstname;
            billing.LastName = lastname;
            billing.Address1 = address;
            billing.Address2 = addressline1;
            billing.City = city;
            billing.State = state;
            billing.Company = company;

            billing.Email = email;
            billing.FaxNumber = fax;
            billing.Country = countryName;
            billing.CountryCode = country;
            billing.ZipPostalCode = zip;


            var shipping = new CommerceParty();
           
            shipping.ExternalId = "0";
            shipping.Name = Constants.Products.ShippingAddress;
            shipping.PartyId = "0";

            shipping.FirstName = firstname2;
            shipping.LastName = lastname2;
            shipping.Address1 = address2;
            shipping.Address2 = addressline12;
            shipping.City = city2;
            shipping.State = state2;
            shipping.Company = company2;
            shipping.Email = email2;
            shipping.FaxNumber = fax2;
            shipping.Country = countryName2;
            shipping.CountryCode = country2;
            shipping.ZipPostalCode = zip2;

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
            var parties = this.GetPartieByName(cart, partyName);
            var request = new Sitecore.Commerce.Services.Carts.RemovePartiesRequest(cart, parties);
            var response = this._serviceProvider.RemoveParties(request);
            if (response.Success)
            {
                return response.Cart as CommerceCart;
            }

            return cart;
        }

        protected virtual List<Party> GetPartieByName(CommerceCart cart, string name)
        {
            List<Party> partyList = new List<Party>();

            foreach (Party party in cart.Parties)
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
            // clear cart cache and add updated cart to cache
            ClearCartFromCache();
            // add cart to cache
            if (updatedCart == null || updatedCart.Properties[Constants.Cart.BasketErrors] != null)
            {
                // no cart OR _basket_errors present
            }
            else
            {
                AddCartToCache(updatedCart);
            }
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
            var result = _serviceProvider.UpdateParties(request);
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
            var result = _serviceProvider.AddParties(request);
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

            // add shipping info
            var addShippingInfoResult = new AddShippingInfoResult { Success = false };

            // Shipping methods
            var shippingMethodList = new List<ShippingMethodInputModelItem>();
            var shippingMethod = new ShippingMethodInputModelItem
            {
                ShippingMethodID = shippingData[0],
                ShippingMethodName = shippingData[1],
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
            addShippingInfoResult = _serviceProvider.AddShippingInfo(addShippingInfoRequest);
            if (!addShippingInfoResult.Success)
            {
                return false;

            }

            if (addShippingInfoResult.Success && addShippingInfoResult.Cart != null)
            {
                AddCartToCache(addShippingInfoResult.Cart as CommerceCart);
            }

            var updatedCart = GetCustomerCart();

            if (updatedCart == null)
            {
                return false;

            }

            return true;
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
            var billingParty = new CommerceParty();

            billingParty.ExternalId = "0";
            billingParty.Name = Constants.Products.BillingAddress;
            billingParty.PartyId = "0";

            billingParty.FirstName = cartPayment.BillingAddress.FirstName;
            billingParty.LastName = cartPayment.BillingAddress.LastName;
            billingParty.Address1 = cartPayment.BillingAddress.Address1;
            billingParty.Address2 = cartPayment.BillingAddress.Address2;
            billingParty.City = cartPayment.BillingAddress.City;
            billingParty.State = cartPayment.BillingAddress.State;
            billingParty.Company = cartPayment.BillingAddress.Company;
            billingParty.Email = cartPayment.BillingAddress.Email;
            billingParty.FaxNumber = cartPayment.BillingAddress.FaxNumber;
            billingParty.Country = cartPayment.BillingAddress.Country;
            billingParty.CountryCode = cartPayment.BillingAddress.CountryCode;
            billingParty.ZipPostalCode = cartPayment.BillingAddress.ZipPostalCode;

            var updatedCart = GetCustomerCart();

            var payments = new List<PaymentInfo>();

            // Add billing party
            List<Party> parties = updatedCart.Parties.ToList();
            parties.Add(billingParty);
            updatedCart.Parties = parties.AsSafeReadOnly();

            // Add payment info
            var addPaymentInfoResult = new AddPaymentInfoResult { Success = false };
            var federatedPaymentModel = new FederatedPaymentInputModelItem
            {
                CardToken = cartPayment.Token,
                Amount = updatedCart.Total.Amount,
                CardPaymentAcceptCardPrefix = cartPayment.CardPrefix
            };

            var federatedPayment = federatedPaymentModel.ToCreditCardPaymentInfo();
            federatedPayment.PartyID = billingParty.PartyId;
            payments.Add(federatedPayment);

            var addPaymentInfoRequest = new AddPaymentInfoRequest(updatedCart, payments);
            addPaymentInfoResult = _serviceProvider.AddPaymentInfo(addPaymentInfoRequest);
            if (!addPaymentInfoResult.Success)
            {
                return false;
            }

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
            List<Party> parties = updatedCart.Parties.ToList();
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
            addPaymentInfoResult = _serviceProvider.AddPaymentInfo(addPaymentInfoRequest);
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
            var paymentResult = _serviceProvider.AddPaymentInfo(paymentRequest);
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
            var removeResult = _serviceProvider.RemovePaymentInfo(removePaymentRequest);
            return removeResult.Cart as CommerceCart;
            ;
        }

        /// <summary>
        /// Submit Cart to Commerce Server
        /// </summary>
        /// <returns></returns>
        internal string SubmitCart()
        {
            var ret = string.Empty;

            // Get visitor identifier
            var visitorId = GetVisitorId();

            var updatedCart = GetCustomerCart();

            var msg = string.Empty;

            var submitVisitorOrderResult = new SubmitVisitorOrderResult { Success = false };

            if (updatedCart.Lines.Count == 0)
            {
                return "No items in cart";
            }

            updatedCart.Email = "testorder@mail.com";
           
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

                // return commerceOrderId;

                return getVisitorOrderResult.Order.TrackingNumber;

            }

            return "Error in cart! Order not submitted.";
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
            SubmitVisitorOrderResult visitorOrderResult = new SubmitVisitorOrderResult();
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
            var cartResult = _serviceProvider.RemoveCartLines(request);
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
            var cartResult = _serviceProvider.UpdateCartLines(updateRequest);
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
                var result = _serviceProvider.MergeCart(mergeCartRequest);
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
                result = _serviceProvider.UpdateCartLines(updateCartRequest);
                _serviceProvider.DeleteCart(new DeleteCartRequest(anonymousCart));
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
                var uid = _accountService.GetCommerceUserId(Context.User.Name);
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

            AddPromoCodeResult result = new AddPromoCodeResult { Success = false };
            var cart = GetCustomerCart();
            ClearCartFromCache();
            var request = new AddPromoCodeRequest(cart, promoCode);

            result = ((CommerceCartServiceProvider)_serviceProvider).AddPromoCode(request);

            if (result.Success && result.Cart != null)
            {
                AddCartToCache(result.Cart as CommerceCart);

                return true;
            } else
            {
                AddCartToCache(cart);
                return false;
            }


            

            //////  -----------------------------------------------
            ////AddPromoCodeResult result = new AddPromoCodeResult { Success = false };

            //// get cart
            //var cart = GetCustomerCart();
            //if (cart == null) return result.Success;


            //var request = new AddPromoCodeRequest(cart, promoCode);
            //var info = CartRequestInformation.Get(request);
            //if (info == null)
            //{
            //    info = new CartRequestInformation(request, true);
            //}
            //else
            //{
            //    info.Refresh = true;
            //}

            //var provider = new CommerceCartServiceProvider();
            //result = provider.AddPromoCode(request);
            //if (result.Success && result.Cart != null)
            //{
            //    AddCartToCache(result.Cart as CommerceCart);
            //}

            //return result.Success;

        }

        public IEnumerable<PaymentOption> GetPaymentOptions()
        {
            var result = new GetPaymentOptionsResult { Success = false };
            var cartResult = GetCustomerCart();

            var request = new GetPaymentOptionsRequest(ShopName, cartResult);
            result = _paymentServiceProvider.GetPaymentOptions(request);

            if (!result.Success)
            {
                return null;
            }

            return result.PaymentOptions.ToList();
        }

        public IEnumerable<PaymentMethod> GetPaymentMethods(PaymentOption paymentOption)
        {
            if(paymentOption == null) { paymentOption = GetPaymentOptions().FirstOrDefault(); }
            var result = new GetPaymentMethodsResult { Success = false };
            var cartResult = this.GetCustomerCart();

            var request = new Sitecore.Commerce.Engine.Connect.Services.Payments.GetPaymentMethodsRequest(cartResult, paymentOption);
            result = _paymentServiceProvider.GetPaymentMethods(request);

            if (!result.Success)
            {
                return null;
            }

            return result.PaymentMethods.ToList();
        }


        public string GetPaymentClientToken()
        {
            var result = new PaymentClientTokenResult { Success = false };
            var request = new ServiceProviderRequest();
            result = _paymentServiceProvider.RunPipeline<ServiceProviderRequest, PaymentClientTokenResult>("commerce.payments.getClientToken", request);

            if (!result.Success)
            {
                return string.Empty;
            }

            return result.ClientToken;
        }


        public PaymentClientTokenResult GetPaymentClient()
        {
            var result = new PaymentClientTokenResult { Success = false };
            var request = new ServiceProviderRequest();
            result = _paymentServiceProvider.RunPipeline<ServiceProviderRequest, PaymentClientTokenResult>("commerce.payments.getClientToken", request);

            if (!result.Success)
            {
                return null;
            }

            return result;
        }

        public bool SetPaymentMethods(PaymentInputModel inputModel)
        {
            var billingGuid = new Guid().ToString();
            inputModel.BillingAddress.ExternalId = billingGuid;
            inputModel.BillingAddress.PartyId = billingGuid;

            var result = new AddPaymentInfoResult { Success = false };
            var response = GetCustomerCart();

            var payments = new List<PaymentInfo>();
            var cart = (CommerceCart)response;
            if (inputModel.CreditCardPayment != null && !string.IsNullOrEmpty(inputModel.CreditCardPayment.PaymentMethodID) && inputModel.BillingAddress != null)
            {
                CommerceParty billingParty = inputModel.BillingAddress.ToParty();
                List<Party> parties = cart.Parties.ToList();
                parties.Add(billingParty);
                cart.Parties = parties.AsSafeReadOnly();

                payments.Add(inputModel.CreditCardPayment.ToCreditCardPaymentInfo());
            }

            if (inputModel.FederatedPayment != null && !string.IsNullOrEmpty(inputModel.FederatedPayment.CardToken) && inputModel.BillingAddress != null)
            {
                CommerceParty billingParty = inputModel.BillingAddress.ToParty();
                List<Party> parties = cart.Parties.ToList();
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
            result = _serviceProvider.AddPaymentInfo(request);
            if (!result.Success)
            {
               return false;
            }
            UpdateCartInCache(cart);
            return true;

        }

        public string SubmitOrder(SubmitOrderInputModel inputModel)
        {
            var ret = string.Empty;
            SubmitVisitorOrderResult errorResult = new SubmitVisitorOrderResult { Success = false };

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

            errorResult = _orderServiceProvider.SubmitVisitorOrder(request);
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
            else {

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
            //var addPartiesResult = _serviceProvider.AddParties(addPartiesRequest);

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
            addShippingInfoResult = _serviceProvider.AddShippingInfo(addShippingInfoRequest);
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
            List<Party> parties = updatedCart.Parties.ToList();
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
            addPaymentInfoResult = _serviceProvider.AddPaymentInfo(addPaymentInfoRequest);
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
            var shippingParty = new CommerceParty() { ExternalId = "0", Name="Shipping", PartyId = "0", FirstName = "Jane", LastName = "Smith", Address1 = "234 Street", City = "Toronto", State = "ON", Country = "Canada", ZipPostalCode = "12345", CountryCode = "CA" };

            //var partyList = new List<Party> { billingParty, shippingParty };
            //var addPartiesRequest = new AddPartiesRequest(cart, partyList);
            //var addPartiesResult = _serviceProvider.AddParties(addPartiesRequest);

            // add shipping info
            var addShippingInfoResult = new AddShippingInfoResult { Success = false };

            // set parties
            var cartParties = cart.Parties.ToList();
            var partyList = new List<Party> { shippingParty };
            cartParties.AddRange(partyList);
            cart.Parties = cartParties.AsReadOnly();

            // Shipping methods
            var shippingMethodList = new List<ShippingMethodInputModelItem>();
            var shippingMethod = new ShippingMethodInputModelItem {
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
            addShippingInfoResult = _serviceProvider.AddShippingInfo(addShippingInfoRequest);
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
            List<Party> parties = updatedCart.Parties.ToList();
            parties.Add(billingParty);
            updatedCart.Parties = parties.AsSafeReadOnly();

            // Add payment info
            var addPaymentInfoResult = new AddPaymentInfoResult { Success = false };
            var federatedPaymentModel = new FederatedPaymentInputModelItem {
                CardToken = "fdc99443-2c41-04e3-17ac-a9bbda610f88",
                Amount = updatedCart.Total.Amount,
                CardPaymentAcceptCardPrefix = "paypal"
            };

            var federatedPayment = federatedPaymentModel.ToCreditCardPaymentInfo();
            federatedPayment.PartyID = billingParty.PartyId;
            payments.Add(federatedPayment);

            var addPaymentInfoRequest = new AddPaymentInfoRequest(updatedCart, payments);
            addPaymentInfoResult = _serviceProvider.AddPaymentInfo(addPaymentInfoRequest);
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
            var addPartiesResult = _serviceProvider.AddParties(addPartiesRequest);

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
            //var addResult = _serviceProvider.AddShippingInfo(request);

            var addShippingInfoRequest = new Sitecore.Commerce.Engine.Connect.Services.Carts.AddShippingInfoRequest(cart, new List<ShippingInfo> { shippingInfo }, ShippingOptionType.ShipToAddress);
            var addShippingInfoResult = _serviceProvider.AddShippingInfo(addShippingInfoRequest);


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
            var addPaymentInfoResult = _serviceProvider.AddPaymentInfo(addPaymentInfoRequest);
            if (!addPaymentInfoResult.Success)
            {

            }
            UpdateCartInCache(cart);

            // Submit the order
            var msg = string.Empty;
            SubmitVisitorOrderResult submitVisitorOrderResult = new SubmitVisitorOrderResult { Success = false };

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

    }
}
