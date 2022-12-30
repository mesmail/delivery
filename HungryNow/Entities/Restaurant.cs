using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HungryNow.Payments;

namespace HungryNow.Entities
{
	public class Restaurant : Entity
	{
		public List<Food> AvailableFoods { get; set; }

		public Restaurant(IPayable account) : base(account)
		{
			AvailableFoods = new List<Food>();
		}

		public new string Notify(string info)
		{
			string displayedInfo = "Restaurant Info - " + DateTime.Now + ": " + info;
			Console.WriteLine(displayedInfo);
			return displayedInfo;
		}

		public override void Contact()
		{
			throw new NotImplementedException();
		}

		public bool Confirm(List<Food> foods)
		{
			// Restaurant takes the decision based on the foods they need to prepare.
			// For simulation purposes we consider that the restaurant always approves the order.
			return true;
		}
	}
}
