using CSDemo.Models.Account;
using CSDemo.Models.Checkout.Cart;
using CSDemo.Models.Product;
using Sitecore.Forms.Mvc.Interfaces;
using Sitecore.Forms.Mvc.ViewModels;
using Sitecore.Web;
using Sitecore.WFFM.Abstractions.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSDemo.Configuration.WFFM
{
    public class OrderInfoField : ValuedFieldViewModel<string>, IFieldResult
    {
        private string _value;
        private OrderDetailViewModel _order;
        public OrderDetailViewModel Order
        {
            get
            {
                if (_order != null)
                    return _order;

                CartHelper cartHelper = new CartHelper();
                OrderDetailViewModel customerOrderDetail = ProductHelper.GetCustomerOrderDetail(this.OrderId, cartHelper);

                if (customerOrderDetail != null)
                    _order = customerOrderDetail;

                return customerOrderDetail;
            }
        }

        public OrderDetailViewModel GetOrderInfo()
        {
            CartHelper cartHelper = new CartHelper();
            return ProductHelper.GetCustomerOrderDetail(this.OrderId, cartHelper);
        }

        public override ControlResult GetResult()
        {
            return base.GetResult();
        }

        public string OrderId
        {
            get
            {
                return WebUtil.GetUrlName(1);
            }
        }

        [DataType(DataType.Text)]
        public override string Value
        {
            get
            {
                List<string> output = new List<string>();
                var customerOrderDetail = Order;

                output.Add("Order Guid: " + OrderId);
                output.Add("Parent Url: " + WebUtil.GetUrlName(2));

                if (customerOrderDetail != null)
                    output.Add("Order Number: " + customerOrderDetail.OrderId);
                
                return String.Join(Environment.NewLine, output);
            }
            set
            {
                this._value = value;
            }
        }
    }
}