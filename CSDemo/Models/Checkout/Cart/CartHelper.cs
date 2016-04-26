using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
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
using AddPartiesRequest = Sitecore.Commerce.Services.Carts.AddPartiesRequest;
using UpdatePartiesRequest = Sitecore.Commerce.Services.Carts.UpdatePartiesRequest;

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
            var cart = cartFromCommServer == true ? GetCart(GetVisitorId(), true) : GetCustomerCart();
            var shoppingCartTotal = cart.Total as CommerceTotal;
            var shoppingCart = new ShoppingCart();
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
        ///   Add Shipping Method Info to Cart
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

        /// <summary>
        /// Submit Order
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
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
            var submitRequest = new GetVisitorOrdersRequest(customerId, shopName);
            var provider = new CommerceOrderServiceProvider();
            var submitResult = provider.GetVisitorOrders(submitRequest);
            return submitResult;
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
