using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeefePOC.Models
{
	public class Inmate
	{
		public string Id { get; set; }

		public string InmateNumber { get; set; }

		private string firstName;
		public string FirstName
		{
			get { return IsHippa ? "******" : firstName; }
			set { firstName = value; }
		}

		private string middleName;

		public string MiddleName
		{
			get { return IsHippa ? "******" : middleName; }
			set { middleName = value; }
		}


		private string lastName;
		public string LastName
		{
			get { return IsHippa ? "******" : lastName; }
			set { lastName = value; }
		}


		public bool IsHippa { get; set; }


		public string FullName { get { return string.Concat(FirstName, " ", LastName); } }

		public string Tier { get; set; }
		public string Block { get; set; }
		public string Cell { get; set; }
		public string AssociatedFacilityId { get; set; }


		public List<string> Restrictions { get; set; } = new List<string>();
		public double CurrentQuarterTotalOrderWeight { get; set; }
		public decimal CurrentQuarterTotalOrderPrice { get; set; }
	}
}
