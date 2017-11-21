using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.Diagnostics;


namespace KeefePOC.Rules.Conditions
{
	public class PreTaxTotal<T> : StringOperatorCondition<T> where T : RuleContext
	{
		public string PreTaxTotalField { get; set; }
		public string FieldValue { get; set; }

		protected override bool Execute(T ruleContext)
		{
			Assert.ArgumentNotNull(ruleContext, "ruleContext");
			if (ruleContext.Item != null)
			{

				return base.Compare(childItem[FieldName], FieldValue);
			}
			return true;
		}
	}
}
