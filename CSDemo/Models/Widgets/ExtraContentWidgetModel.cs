using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Presentation;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration;

namespace CSDemo.Models.Widgets
{
	public class ExtraContentWidgetModel 
	{
		public string ExtraContent { get; set; }
	}

	public partial interface IExtraContent 
	{
		
		string ExtraContentItem
		{
			get;
			set;
		}
	}
	
	public partial class ExtraContent : IExtraContent
	{	
		public virtual string ExtraContentItem
		{
			get;
			set;
		}
	}	
}
