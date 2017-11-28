﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce;
using Sitecore.Commerce.Services.Carts;
using Sitecore.Commerce.Pipelines;
using Sitecore.Commerce.Services;

namespace KeefePOC.Pipelines.Providers
{
	public class CartServiceProvider : Sitecore.Commerce.Services.Carts.CartServiceProvider
	{
		public override CartResult AddCartLines(AddCartLinesRequest request)
		{
			var result = this.RunPipeline<CartLinesRequest, CartResult>("keef.commerce.carts.validateCartLine", (CartLinesRequest)request);

			var messages = result.SystemMessages;
			var cart = result.Cart;
			if (result.Success)
			{
				return this.RunPipeline<CartLinesRequest, CartResult>("commerce.carts.addCartLines", (CartLinesRequest)request);
			}

			return result;
		}

		public ServiceProviderResult ViewCart(ServiceProviderRequest request)
		{
			var result = this.RunPipeline<ServiceProviderRequest, ServiceProviderResult>("keef.commerce.carts.viewcartvalidation", request);
			return result;
		}
	}
}
