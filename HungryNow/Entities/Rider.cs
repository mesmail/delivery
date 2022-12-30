using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HungryNow.Payments;
using HungryNow.Utilites;

namespace HungryNow.Entities
{
	public class Rider : Entity
	{
		public double waitingTime;
		public Rider(IPayable account) : base(account)
		{
		}

		public override void Contact()
		{
			throw new NotImplementedException();
		}

		public new string Notify(string info)
		{
			string displayedInfo = "Rider Info - " + DateTime.Now + ": " + info;
			Console.WriteLine(displayedInfo);
			return displayedInfo;
		}

		public bool Confirm(Address from, Address to, double estimatedTime)
		{
			// Rider takes the decision based on the addresses and the estimated duration.
			// For simulation purposes we consider that the rider always approves the delivery.
			return true;
		}
	}
}
