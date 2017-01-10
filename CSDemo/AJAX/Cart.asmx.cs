#region

using CSDemo.Models.Account;
using CSDemo.Models.Checkout.Cart;
using CSDemo.Models.Product;
using Glass.Mapper.Sc;
using Newtonsoft.Json;
using Sitecore.Analytics;
using Sitecore.Analytics.Data.Items;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;


#endregion

namespace CSDemo.AJAX
{
    /// <summary>
    /// Summary description for Cart
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    public class Cart : WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ShoppingCart LoadCart()
        {
            return CartHelper.GetMiniCart();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CheckIfCommerceActionsAllowed()
        {
            if (!string.IsNullOrEmpty(ActionAllowed)) { return ActionAllowed; }

            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AddProductToCart(string quantity, string productId, string catalogName, string variantId, string contextItemId)
        {
            var ret = CartHelper.AddProductToCart(quantity, productId, catalogName, variantId);
            RegisterGoal(Constants.Marketing.AddToCartGoalId, contextItemId);
            return ret;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SubmitOrder(string contextItemId, string orderTotal)
        {
            string ret = CartHelper.SubmitCart();
            RegisterGoal(Constants.Marketing.SubmitOrderGoalId, contextItemId);

            if (!Sitecore.Context.User.IsAuthenticated) return ret;
            decimal total = 0;
            decimal.TryParse(orderTotal, out total);
            var newStatus = UserStatus.GetStatus(total);
            var user = Sitecore.Context.User;
            if (user.Domain.Name.ToLower() != "commerceusers") return ret;

            
            if (newStatus == null)
                return ret;
            var statuses = UserStatus.GetAllStatuses();
            if (string.IsNullOrWhiteSpace(user.Profile.Comment))
                user.Profile.Comment = newStatus.Name;
            else
            {
                try
                {
                    var currentStatus = statuses.First(s => s.Name == user.Profile.Comment);
                    if (currentStatus == null) return ret;
                    if (currentStatus.Amount < newStatus.Amount)
                        user.Profile.Comment = newStatus.Name;
                }
                catch (Exception ex)
                {

                    return ret;
                }
            }

            return ret;
        }
       
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ApplyShippingAndBillingToCart(string firstname, string lastname, string email, string company,
            string address, string addressline1, string city, string state, string country, string countryName, string fax, string phone, string zip,
            string firstname2, string lastname2, string email2, string company2, string address2, string addressline12,
            string city2, string state2, string country2, string countryName2, string fax2, string phone2, string zip2, string billandshipping)
        {


            // var app = CartHelper.AddShippingMethodToCart("e14965b9-306a-43c4-bffc-3c67be8726fa");
            return CartHelper.ApplyShippingAndBillingToCart(firstname, lastname, email, company, address, addressline1,
                city, state, country, countryName, fax, phone, zip, firstname2, lastname2, email2, company2, address2, addressline12, city2, state2,
                country2, countryName2, fax2, phone2, zip2);

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ApplyShippingToCart(Models.Checkout.Cart.Address shipping)
        {
            return CartHelper.ApplyShippingToCart(shipping);

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ApplyShippingMethodToCart(string shippingMethodId)
        {
            return CartHelper.AddShippingMethodToCart(shippingMethodId);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ApplyPaymentMethodToCart(string paymentExternalID, string nameoncard, string creditcard,
            string expmonth, string expyear, string ccv)
        {
            return CartHelper.ApplyPaymentMethodToCart(paymentExternalID, nameoncard, creditcard, expmonth, expyear, ccv);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ApplyNewPaymentMethodToCart(Payment cartPayment)
        {
            return CartHelper.ApplyNewPaymentMethodToCart(cartPayment);

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ApplyPaymentMethodToCart2(string nounceData, string cardPrefixData)
        {
            return CartHelper.ApplyPaymentMethodToCart2(nounceData, cardPrefixData);

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SubmitCartOrder(string inputModel)
        {
            var inputModelObj = new JavaScriptSerializer().Deserialize<SubmitOrderInputModel>(inputModel);
            return CartHelper.SubmitOrder(inputModelObj);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool SetPaymentMethodToCart(string inputModel)
        {
            var inputModelObj = new JavaScriptSerializer().Deserialize<PaymentInputModel>(inputModel);
            //var result = "{'FederatedPayment':{'CardToken':'83f75be7 - 5f73 - 0f07 - 190e-df2017045f5b','Amount':'198.00','CardPaymentAcceptCardPrefix':'paypal'},'BillingAddress':{'Name':'billing','Address1':'Test AddressLine1','Country':'Test Country','City':'Test City','State':'Test State','ZipPostalCode':'12345','ExternalId':'','PartyId':''}}";
            //var inputModel = new JavaScriptSerializer().Deserialize<PaymentInputModel>(result);
            //    "{"FederatedPayment":{"CardToken":"83f75be7 - 5f73 - 0f07 - 190e-df2017045f5b","Amount":"198.00","CardPaymentAcceptCardPrefix":"paypal"},"BillingAddress":{"Name":"billing","Address1":"Test AddressLine1","Country":"Test Country","City":"Test City","State":"Test State","ZipPostalCode":"12345","ExternalId":"","PartyId":""}}"
            return CartHelper.SetPaymentMethods(inputModelObj);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool SetPaymentMethodToCart2(string nounce)
        {
            return CartHelper.CompleteACheckout4(nounce);
        }

        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public bool SetPaymentMethodToCart(PaymentInputModel inputModel)
        //{
        //    return CartHelper.SetPaymentMethods(inputModel);
        //}

        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public bool SetPaymentMethodToCart(PaymentInputModel inputModel)
        //{
        //    return CartHelper.SetPaymentMethods(inputModel);
        //}

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool RemoveFromCart(string externalId)
        {
            return CartHelper.RemoveItemFromCart(externalId);
        }

        public struct CurrentCartItem
        {
            public string ExternalId { get; set; }
            public string Quantity { get; set; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool UpdateCartList(List<CurrentCartItem> currentCartItems)
        {
            // check if user is logged in and not commerce customer, if true, return false

            var ret = false;

            foreach (var c in currentCartItems)
            {
                // ensure q can only be an integer
                var q = c.Quantity.Trim();
                if (string.IsNullOrEmpty(q))
                {
                    q = "0";
                }

                if (q.All(Char.IsDigit))
                {
                    ret = CartHelper.UpdateCartItem(c.ExternalId, q);
                }
            }

            return ret;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ApplyPromoCode(string promoCode)
        {
            if(string.IsNullOrEmpty(promoCode.Trim())) return false;
            return CartHelper.ApplyPromoCode(promoCode);
        }


        public CartHelper CartHelper { get; set; }
        public string ActionAllowed { get; set; }
        public Cart()
        {
            CartHelper = new CartHelper();

            ActionAllowed = CartHelper.CustomerOrAnonymous();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetProduct(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId)) return null;
            var product = Product.GetProduct(productId);

            var result = JsonConvert.SerializeObject(product);
            return result;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetProducts(string[] productIds)
        {
            var products = new List<Product>();
            foreach(var productId in productIds)
            {
                var product = Product.GetProduct(productId);
                products.Add(product);
            }

            var result = JsonConvert.SerializeObject(products);
            return result;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool SetUserCatalogChoice(string catalogName)
        {

            var userHelper = new AccountHelper();

            userHelper.SetUserCatalogChoice(catalogName);

            return true;
        }

            
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ClearSessionTimeOutCookies()
        {
            if (!Sitecore.Context.User.IsAuthenticated)
            {
                var userHelper = new AccountHelper();
                userHelper.ClearUserCatalogCookies();

            }

            return true;
        }

                
        //[ScriptMethod(UseHttpGet = true,ResponseFormat = ResponseFormat.Json)]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductsResult(string query)
        {
            var products = new List<object>();
            
            var productsResult = ProductHelper.GetProductsByName(query);

            if (productsResult != null && productsResult.Any())
            {
                foreach (var product in productsResult)
                {
                    products.Add(new {Id = product.Id, CategoryName = product.CategoryName, CatalogId = product.CatalogId, Guid = product.Guid, Title  = product.Title, Price = product.Price, CatalogName = product.CatalogName, ImageSrc= product.ImageSrc, VariantId = product.VariantId });
                  
                }
            }

            return new { success = true, total = products.Count , products = products };
        }

                        
        //[ScriptMethod(UseHttpGet = true,ResponseFormat = ResponseFormat.Json)]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCategoriesResult(string query)
        {
            var categories = new List<object>();

            //return "aaaa,bbbb,ccccc,dd,eeeee,fff,gggggg hhh".Split(',').ToList();
            
            var categoriesResult = ProductHelper.GetCategoriesByName(query);

            if (categoriesResult != null && categoriesResult.Any())
            {
                foreach (var product in categoriesResult)
                {
                    categories.Add(new {Id = product.Id, CatalogId = product.CatalogId, Guid = product.Guid, Title  = product.Title, Price = product.Price, CatalogName = product.CatalogName, ImageSrc= product.ImageSrc});
                  
                }
            }

            return new { success = true, total = categories.Count , products = categories };
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSubCategories(string category)
        {
            var categories = new List<object>();
            var products = new List<object>();

            var catalogId = ProductHelper.GetSiteRootCatalogId();

            var catalogName = ProductHelper.GetSiteRootCatalogName();

            var categoryId = ProductHelper.GetItemIdsFromName(category, catalogId);


            var categoryItem = Sitecore.Context.Database.GetItem(new ID(categoryId));

            if (categoryItem != null && categoryItem.HasChildren)
            {
                var subCategories =
                    categoryItem.GetChildren()
                        .Where(x => x.TemplateName == Constants.Products.GeneralCategoryTemplateName).ToList();

                                
                if (subCategories.Count >0 && subCategories.Any())
                {
                    foreach (Item subCategory in subCategories)
                    {
                        var gsubCategory = subCategory.GlassCast<Category>();

                        if(gsubCategory.DefinitionName == "GeneralCategory")
                        {
                            categories.Add(new {text = subCategory.Name, value = subCategory.Name  });
                        }
                    }
                }

                var subProducts =
                    categoryItem.GetChildren()
                        .Where(x => x.TemplateName != Constants.Products.GeneralCategoryTemplateName).ToList();

                                
                if (subProducts.Count >0 && subProducts.Any())
                {
                    foreach (var subProduct in subProducts)
                    {
                        var gsubProduct = subProduct.GlassCast<Product>();

                        if(gsubProduct.DefinitionName  != "GeneralCategory")
                        {

                            //products.Add(new {text = gsubProduct.Title, value = gsubProduct.Title  });

                            var guid = Sitecore.Data.ID.Parse(gsubProduct.ID).ToString();

                            //var prodid = gsubProduct.ProductId;

                            products.Add(new {cat = catalogId, text = gsubProduct.Title, pid = gsubProduct.ProductId, pguid = guid, img = gsubProduct.FirstImage, price = gsubProduct.Price, catName = catalogName });
                        }
                    }
                }
            }

            return new { success = true, id = category , categories = categories, products = products };
        }



        #region Private Helpers

        private void RegisterGoal(string goalId, string itemId)
        {
            try {
                if (!Tracker.Current.IsActive)
                    Tracker.Current.StartTracking();

                if (!Tracker.Current.IsActive
                    || Tracker.Current.CurrentPage == null)
                    return;

                var deliveryDatabase = Sitecore.Configuration.Factory.GetDatabase(Constants.DeliveryDatabase);
                if (deliveryDatabase == null) return;
                var goalItem = deliveryDatabase.GetItem(goalId);
                if (goalItem == null)
                {
                    Sitecore.Diagnostics.Log.Error($"Unable to register goal with ID {goalId}. Make sure everything is deployed and published correctly.", this);
                }
                var goal = new PageEventItem(goalItem);
                if (goal == null)
                {
                    Sitecore.Diagnostics.Log.Error($"Unable to register page event goal with ID {goalId}. Make sure everything is deployed and published correctly.", this);
                }
                var pageEventsRow = Tracker.Current.CurrentPage.Register(goal);

                pageEventsRow.Data = "Product added to cart - "
                    + DateTime.Now.ToString("F");

                if (string.IsNullOrWhiteSpace(itemId)) return;
                var contextItem = deliveryDatabase.GetItem(itemId);
                if (contextItem == null) return;

                pageEventsRow.ItemId = contextItem.ID.Guid;
                pageEventsRow.DataKey = contextItem.Paths.Path;
            }
            catch(Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Unable to register goal with ID {goalId}.", ex);
            }
        }


        #endregion
    }
}