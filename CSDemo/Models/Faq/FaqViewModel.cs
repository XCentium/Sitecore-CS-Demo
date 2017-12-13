using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Faq
{
	public class FaqItem
	{
		public string Question { get; set; }
		public string Answer { get; set; }
	}

	public class FaqViewModel
	{
		public List<FaqItem> Faqs { get; set; } = new List<FaqItem>();
	}
}