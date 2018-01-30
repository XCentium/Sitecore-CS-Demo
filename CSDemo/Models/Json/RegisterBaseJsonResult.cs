using Sitecore.Commerce.Entities.Customers;
using Sitecore.Commerce.Services.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Json
{

	public class RegisterBaseJsonResult : BaseJsonResult
	{
		public RegisterBaseJsonResult()
		{
		}

		public RegisterBaseJsonResult(CreateUserResult result) : base(result)
		{
		}

		public virtual void Initialize(CommerceUser user)
		{
			this.UserName = user.UserName;
		}

		public bool IsSignupFlow { get; set; }

		public string UserName { get; set; }
	}
}