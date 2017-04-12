using CSDemo.Models.Account;
using CSDemo.Models.Checkout.Cart;
using CSDemo.Models.Product;
using Sitecore.Forms.Mvc.ViewModels;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSDemo.Configuration.WFFM
{
    public class OrderInfoField : ValuedFieldViewModel<string>
    {
        private OrderDetailViewModel _order;
        public OrderDetailViewModel Order
        {
            get
            {
                if (_order != null)
                    return _order;

                var cartHelper = new CartHelper();
                var customerOrderDetail = ProductHelper.GetCustomerOrderDetail(OrderId, cartHelper);

                if (customerOrderDetail != null)
                    _order = customerOrderDetail;

                return customerOrderDetail;
            }
        }

        public OrderDetailViewModel GetOrderInfo()
        {
            var cartHelper = new CartHelper();
            return ProductHelper.GetCustomerOrderDetail(OrderId, cartHelper);
        }

        public string OrderId
        {
            get
            {
                var orderId = WebUtil.GetUrlName(1);
                orderId = orderId.Replace(" ", "-");

                return orderId;
            }
        }

        [DataType(DataType.Text)]
        public override string Value
        {
            get
            {
                var output = new List<string> {"Order #" + Order.TrackingNumber};
                return string.Join(Environment.NewLine, output);
            }
        }
    }
}