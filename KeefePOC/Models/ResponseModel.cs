using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace KeefePOC.Models
{
	public class ResponseModel
	{
		public bool IsSuccessful { get; set; }

		public List<string> Messages { get; set; }

		// Regex Format - {StatusCode} - {"message":"(Message)"}
		// E.g. Bad Request - {"message":"Proceed"}
		public List<string> FormattedMessages
		{
			get
			{
				var list = new List<string>();

				if (Messages == null || !Messages.Any())
				{
					return list;
				}

				Messages.ForEach(m =>
				{
					var match = Regex.Match(m, ".*?\\s?-\\s?{\"message\":\"(?<Message>.*?)\"}");

					list.Add(match.Success ? match.Groups["Message"].Value : m);
				});

				return list;
			}
		}

		public string Message { get; set; }

		public object Data { get; set; }
		
	}
}
