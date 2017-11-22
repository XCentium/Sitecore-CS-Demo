using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Pipelines;
using Sitecore.Commerce.Services.Carts;
using Sitecore.Commerce.Services;
using KeefePOC.Models;
using KeefePOC.Services;
using KeefePOC.Repositories;


namespace KeefePOC.Pipelines.Validations.AddToCart
{
	// This validation should really be done in SAP and return error codes
	public class InmateProductValidation : PipelineProcessor<ServicePipelineArgs>
	{
		public override void Process(ServicePipelineArgs args)
		{
			CartLinesRequest request = (CartLinesRequest)args.Request;

			//if(args.Result.Success)
			//{
			//	args.Result.Success = false;
			//	return;
			//}

			var inmateNumber = (string)request.Properties["InmateNumber"];

			if(string.IsNullOrEmpty(inmateNumber))
			{
				args.Result.Success = false;
				args.Result.SystemMessages.Add(new SystemMessage()
				{
					Message = "Inmate is empty"
				});
			}
			else
			{
				var dataService = new KeefeDataService(new DemoFacilityRepository(), new DemoProgramRepository(), new DemoInmateRepository());

				var blacklistedProducts = dataService.GetBlacklistedItemsForInmate(inmateNumber);

				var itemsBeindAddedToCart = new List<string>();

				itemsBeindAddedToCart = request.Lines.Select(x => x.Product.ProductId).ToList();

				foreach(var product in itemsBeindAddedToCart)
				{
					if (blacklistedProducts.Contains(product))
					{
						args.Result.Success = false;
						args.Result.SystemMessages.Add(new SystemMessage()
						{
							Message = $"Inmate {inmateNumber} Not Allowed To Purchase {product}"
						});
						break;
					}
					else
					{
						args.Result.Success = true;
					}
				}		


			}

		}
	}
}
