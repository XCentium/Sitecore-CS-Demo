using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using CSDemo.Models.Account;
using CSDemo.Models.Cart;
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
using Sitecore.Data;
using AddPartiesRequest = Sitecore.Commerce.Services.Carts.AddPartiesRequest;
using UpdatePartiesRequest = Sitecore.Commerce.Services.Carts.UpdatePartiesRequest;
using Sitecore.Commerce.Entities.Customers;

namespace CSDemo.Models.Checkout.Cart
{
    public class CartHelper
    {
        public string ShopName { get; set; }
        public string DefaultCartName { get; set; }
        private readonly InventoryServiceProvider _inventoryServiceProvider = new InventoryServiceProvider();
        private readonly PricingServiceProvider _pricingServiceProvider = new PricingServiceProvider();
        private readonly CartServiceProvider _serviceProvider = new CartServiceProvider();
        private readonly ContactFactory _contactFactory = new ContactFactory();
        private readonly AccountHelper _accountService;

        public CartHelper()
        {
            if (Tracker.Current == null)
            {
                Tracker.StartTracking();
            }
            ShopName = Context.Site.Name; // Constants.DefaultSiteName; //
            DefaultCartName = CommerceConstants.CartSettings.DefaultCartName;
            _accountService = new AccountHelper(this, new CustomerServiceProvider());
        }

        public string AddProductToCart(string Quantity, string ProductId, string CatalogName, string VariantId)
        {
            var ret = string.Empty;
            // Create cart object
            var cartLineItem = new CartLineItem();
            cartLineItem.Quantity = uint.Parse(Quantity);
            cartLineItem.ProductId = ProductId;
            cartLineItem.CatalogName = CatalogName;
            cartLineItem.VariantId = VariantId;
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
            var visitorID = GetVisitorID();
            var request = new AddCartLinesRequest(GetCart(visitorID, false), new[] { cartItem });
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
            // return cart
            return cartResult.Cart as CommerceCart;
        }

        /// <summary>
        ///     CCC
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
        ///     CCC
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

        private void ClearCartFromCache()
        {
            var id = Guid.Parse(GetVisitorID()).ToString("D");
            ClearUserCartFromCache(id);
        }

        private void ClearUserCartFromCache(string Userid)
        {
            var cacheProvider = GetCacheProvider();
            if (
                !cacheProvider.Contains(CommerceConstants.KnownCachePrefixes.Sitecore,
                    CommerceConstants.KnownCacheNames.CommerceCartCache, Userid))
            {
                var msg = string.Format(CultureInfo.InvariantCulture,
                    Constants.Cart.CartInvalidInCache, Userid);
                CommerceTrace.Current.Write(msg);
            }
            cacheProvider.RemoveData(CommerceConstants.KnownCachePrefixes.Sitecore,
                CommerceConstants.KnownCacheNames.CommerceCartCache, Userid);
            DeleteCustomerCartCookie(Userid);
        }

        /// <summary>
        ///     CCC
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
        ///     CCC
        /// </summary>
        /// <returns></returns>
        private ICacheProvider GetCacheProvider()
        {
            var cacheProvider = CommerceTypeLoader.GetCacheProvider(CommerceConstants.KnownCacheNames.CommerceCartCache);
            return cacheProvider;
        }

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
        ///     CCC
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
        /// CSDEMO#99
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="catalogName"></param>
        /// <returns></returns>
        internal StockInformation GetProductStockInformation(string productId, string catalogName, string variantId = "")
        {
            var commerceInventoryProduct = new CommerceInventoryProduct();
            commerceInventoryProduct.ProductId = productId;
            commerceInventoryProduct.CatalogName = catalogName;
            if (!string.IsNullOrEmpty(variantId)) { commerceInventoryProduct.VariantId = variantId; }
            var products = new List<InventoryProduct>();
            products.Add(commerceInventoryProduct);

            //var products = new List<InventoryProduct>
            //{
            //    new CommerceInventoryProduct {ProductId = productId, CatalogName = catalogName}
            //};
            var stockInfoRequest = new GetStockInformationRequest(ShopName, products, StockDetailsLevel.All);
            var stockInfoResult = _inventoryServiceProvider.GetStockInformation(stockInfoRequest);
            return stockInfoResult.StockInformation.FirstOrDefault();
        }

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
        ///     CCC
        ///     If visitor is logged in, use the commerce ID else use randomly generated guid and store in cookie
        /// </summary>
        /// <returns></returns>
        public string GetVisitorID()
        {
            if (Tracker.Current != null && Context.User.IsAuthenticated)
            {
                return GetLoggedInUserID();
            }
            return GetAnonymousUserID();
        }

        public string GetLoggedInUserID()
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
            return GetAnonymousUserID();
        }

        public string GetAnonymousUserID()
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

        public CommerceCart GetCustomerCart()
        {
            // Get visitor's cart from cache
            var cart = GetFromCacheCart(GetVisitorID());
            if (cart == null && CustomerHasCookie(GetVisitorID()))
            {
                // cart exists, but cart cache is empty; lets get it and add it
                cart = GetCart(GetVisitorID(), true);
                if (cart != null)
                {
                    AddCartToCache(cart);
                }
            }
            return cart ?? new CommerceCart();
        }

       

        public ShoppingCart GetMiniCart(bool cartFromCommServer = false)
        {
            var cart = cartFromCommServer == true ? GetCart(GetVisitorID(), true) : GetCustomerCart();
            var shoppingCartTotal = cart.Total as CommerceTotal;
            var shoppingCart = new ShoppingCart();
<<<<<<< Updated upstream
            if (cart != null)
            {
                shoppingCart.LineTotal = cart.Total as CommerceTotal == null
                    ? 0
                    : (cart.Total as CommerceTotal).Subtotal;
                shoppingCart.Total = cart.LineItemCount;

                var commerceTotal = (CommerceTotal)cart.Total;
                if(commerceTotal != null)
                    shoppingCart.Shipping = commerceTotal.ShippingTotal;
                if (cart.Total != null)
                {
                    shoppingCart.Tax = cart.Total.TaxTotal.Amount;
                    shoppingCart.GrandTotal = cart.Total.Amount;
                }
                
                var cartItems = new List<CartItem>();
                if (cart.LineItemCount > 0)
=======
            if (cart == null || shoppingCartTotal == null) return shoppingCart;

            shoppingCart.LineTotal = cart.Total as CommerceTotal == null
                ? 0
                : (cart.Total as CommerceTotal).Subtotal;
            shoppingCart.Total = cart.LineItemCount;

            var commerceTotal = (CommerceTotal)cart.Total;
            shoppingCart.Shipping = commerceTotal == null ? 0.00m : commerceTotal.ShippingTotal;
            shoppingCart.Tax = cart.Total.TaxTotal.Amount == null ? 0.00m : cart.Total.TaxTotal.Amount;
            shoppingCart.GrandTotal = cart.Total.Amount == null ? 0.00m : cart.Total.Amount;

            var cartItems = new List<CartItem>();
            if (cart.LineItemCount > 0)
            {
                foreach (CustomCommerceCartLine cartLine in cart.Lines)
>>>>>>> Stashed changes
                {
                    
                    var cartItem = new CartItem();
                    var product = cartLine.Product as CommerceCartProduct;
                    cartItem.ProductName = product.DisplayName;
                    // NOTE: use search when Lucene search is completed
                    // Get categoty root
                    var rootItem = Context.Database.GetItem(Constants.Products.AdventureWorksRootPath);
                    if (rootItem != null)
                    {
                        var children = rootItem.Axes.GetDescendants();
                        var item =
                            children.AsQueryable()
                                .FirstOrDefault(x => x.Name.Equals(product.ProductId));
                        if (item != null)
                        {
                            cartItem.ProductID = item.ID.ToString();
                            cartItem.ImageUrl = ProductHelper.GetFirstImageFromProductItem(item);
                            cartItem.Category = item.Parent.Name;
                        }
                        else
                        {
                            cartItem.ImageUrl = string.Empty;
                        }
                    }
                    cartItem.CSProductId = product.ProductId;
                    cartItem.Quantity = (int)cartLine.Quantity;
                    cartItem.UnitPrice = product.Price.Amount;
                    cartItem.SubTotal = cartLine.Total.Amount;
                    cartItem.ExternalID = cartLine.ExternalCartLineId;

                    if (string.IsNullOrEmpty(cartItem.ImageUrl))
                    {
                        if (cartLine.Images != null && cartLine.Images.Count > 0)
                        {
                            cartItem.ImageUrl = cartLine.Images[0];
                        }
                    }
                    //var images = cartLine.
                    cartItems.Add(cartItem);
                }
                shoppingCart.CartItems = cartItems;
            }

            return shoppingCart;
        }

        /// <summary>
        ///     CCC
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private bool CustomerHasCookie(string customerId)
        {
            var CookieName = Constants.Cart.CookieName;
            var cartCookie = HttpContext.Current.Request.Cookies[CookieName];
            return cartCookie != null && cartCookie.Values[Constants.Cart.VisitorID] == customerId;
        }

        internal bool ApplyShippingAndBillingToCart(string firstname, string lastname, string email, string company,
            string address, string addressline1, string city, string country, string fax, string phone, string zip,
            string firstname2, string lastname2, string email2, string company2, string address2, string addressline12,
            string city2, string country2, string fax2, string phone2, string zip2)
        {
            var cart = GetCustomerCart();
            var updatedCart = cart;
            var billing = updatedCart.Parties.Cast<CommerceParty>().FirstOrDefault(party => party.Name == Constants.Products.BillingAddress);
            if (billing == null)
            {
                billing = new CommerceParty();
                billing.ExternalId = Guid.NewGuid().ToString("B");
                billing.Name = Constants.Products.BillingAddress;
                updatedCart = AddPartyToCart(updatedCart, billing);
                billing = updatedCart.Parties.Cast<CommerceParty>().FirstOrDefault(party => party.Name == Constants.Products.BillingAddress);
            }
            billing.FirstName = firstname;
            billing.LastName = lastname;
            billing.Address1 = address;
            billing.Address2 = addressline1;
            billing.City = city;
            billing.Company = company;
            billing.Country = country;
            billing.Email = email;
            billing.FaxNumber = fax;
            billing.Country = country;
            var shipping = updatedCart.Parties.Cast<CommerceParty>().FirstOrDefault(party => party.Name == Constants.Products.ShippingAddress);
            if (shipping == null)
            {
                shipping = new CommerceParty();
                shipping.ExternalId = Guid.NewGuid().ToString("B");
                shipping.Name = Constants.Products.ShippingAddress;
                updatedCart = AddPartyToCart(updatedCart, shipping);
                shipping = updatedCart.Parties.Cast<CommerceParty>().FirstOrDefault(party => party.Name == Constants.Products.ShippingAddress);
            }
            shipping.Name = Constants.Products.ShippingAddress;
            shipping.FirstName = firstname2;
            shipping.LastName = lastname2;
            shipping.Address1 = address2;
            shipping.Address2 = addressline12;
            shipping.City = city2;
            shipping.Company = company2;
            shipping.Country = country2;
            shipping.Email = email2;
            shipping.FaxNumber = fax2;
            shipping.Country = country2;
            updatedCart = UpdatePartiesInCart(updatedCart, new List<CommerceParty> { billing, shipping });
            // clear cart cache and add updated cart to cache
            UpdateCartInCache(updatedCart);
            return true;
        }

        /// <summary>
        ///     Update a changed cart in cache
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

        private CommerceCart UpdatePartiesInCart(CommerceCart cart, List<CommerceParty> parties)
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
        ///     CCC
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

        public CommerceCart AddPartyToCart([NotNull] CommerceCart cart, [NotNull] CommerceParty party)
        {
            var request = new AddPartiesRequest(cart, new List<Party> { party });
            var result = _serviceProvider.AddParties(request);
            return result.Cart as CommerceCart;
        }

        internal bool AddShippingMethodToCart(string shippingMethodId)
        {
            var cart = GetCustomerCart();
            var updatedCart = cart;
            // if shippingMethodId is not empty, get Shipping Party, if shipping party is not empty, apply shippingMethodId to cart, refresh cart cache
            if (!string.IsNullOrEmpty(shippingMethodId))
            {
                var shipping =
                    updatedCart.Parties.Cast<CommerceParty>().FirstOrDefault(party => party.Name == Constants.Products.ShippingAddress);
                if (shipping != null)
                {
                    updatedCart = AddShippingMethodInfoToCart(cart, shipping, shippingMethodId);
                }
            }
            UpdateCartInCache(updatedCart);
            return true;
        }

        /// <summary>
        ///     CCC
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="shipping"></param>
        /// <param name="shippingMethodId"></param>
        /// <returns></returns>
        private CommerceCart AddShippingMethodInfoToCart(CommerceCart cart, CommerceParty shipping,
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
            var request = new AddShippingInfoRequest(cart, new List<ShippingInfo> { shippingInfo });
            var info = CartRequestInformation.Get(request);
            if (info == null)
            {
                info = new CartRequestInformation(request, true);
            }
            else
            {
                info.Refresh = true;
            }
            var result = _serviceProvider.AddShippingInfo(request);
            return result.Cart as CommerceCart;
        }

        internal bool ApplyPaymentMethodToCart(string paymentExternalID, string nameoncard, string creditcard,
            string expmonth, string expyear, string ccv)
        {
            var cart = GetCustomerCart();
            var updatedCart = cart;
            // remove all payment info on cart
            updatedCart = RemovePaymentInfoFromCart(cart, cart.Payment.ToList());
            // add payment info to cart
            // get billing, if billing is not null 
            if (!string.IsNullOrEmpty(paymentExternalID))
            {
                var billing = updatedCart.Parties.Cast<CommerceParty>().FirstOrDefault(party => party.Name == Constants.Products.BillingAddress);
                if (billing != null)
                {
                    var paymentInfo = new CommerceCreditCardPaymentInfo();
                    paymentInfo.ExternalId = paymentExternalID;
                    paymentInfo.PaymentMethodID = paymentExternalID;
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

        internal string SubmitCart()
        {
            var ret = string.Empty;
            var cart = GetCustomerCart();
            // check for cart errors 
            if (cart.Properties[Constants.Cart.BasketErrors] == null)
            {
                var submitResult = SubmitOrder(cart);
                // check for submit order errors
                if (submitResult.CartWithErrors == null)
                {
                    // clear cart cache
                    ClearCartFromCache();
                }
                cart = submitResult.CartWithErrors as CommerceCart;
                ret = submitResult.Order.OrderID;
            }
            else
            {
                return ret;
            }
            return ret;
        }

        private SubmitVisitorOrderResult SubmitOrder(CommerceCart cart)
        {
            var submitRequest = new SubmitVisitorOrderRequest(cart);
            var provider = new CommerceOrderServiceProvider();
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
            return submitResult;
        }

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
        ///     CCC
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
        ///     CCC
        /// </summary>
        /// <param name="externalID"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        internal bool UpdateCartItem(string externalID, string quantity)
        {
            try
            {
                // get the cart line to update
                // get the current cart quantity
                // if current new quantity is greater tnan current, Add new-current to cart
                // if less remove the item and add new
                var cart = ChangeCartItemQuantity(externalID, quantity);
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
        ///     CCC
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

        public CommerceCart MergeCarts(string anonymousUserId)
        {
            var userId = GetLoggedInUserID();
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
        ///     Only Anonymous and customers allowed
        /// </summary>
        /// <returns></returns>
        internal string CustomerOrAnonymous()
        {
            var ret = string.Empty;
            if (Context.User.IsAuthenticated)
            {
                var uid = _accountService.GetCommerceUserID(Context.User.Name);
                if (string.IsNullOrEmpty(uid))
                {
                    return Constants.Cart.AnonUserActionDenied;
                }
            }
            return ret;
        }

        public GetVisitorOrdersResult GetOrders(string customerID, string shopName)
        {
            var submitRequest = new GetVisitorOrdersRequest(customerID, shopName);
            var provider = new CommerceOrderServiceProvider();
            var submitResult = provider.GetVisitorOrders(submitRequest);
            return submitResult;
        }

        public GetVisitorOrderResult GetOrderHead(string orderID, string customerID, string shopName)
        {
            var submitRequest = new GetVisitorOrderRequest(orderID, customerID, shopName);
            var provider = new CommerceOrderServiceProvider();
            var submitResult = provider.GetVisitorOrder(submitRequest);
            return submitResult;
        }


        internal bool ApplyPromoCode(string promoCode)
        {
            AddPromoCodeResult result = new AddPromoCodeResult { Success = false };

            // get cart
            var cart = GetCustomerCart();
            if (cart == null) return result.Success;


            var request = new AddPromoCodeRequest(cart, promoCode);
            var info = CartRequestInformation.Get(request);
            if (info == null)
            {
                info = new CartRequestInformation(request, true);
            }
            else
            {
                info.Refresh = true;
            }

            var provider = new CommerceCartServiceProvider();
            result = provider.AddPromoCode(request);
            if (result.Success && result.Cart != null)
            {
                AddCartToCache(result.Cart as CommerceCart);
            }

            return result.Success;

        }
    }
}
