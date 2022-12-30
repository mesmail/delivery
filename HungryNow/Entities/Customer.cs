using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HungryNow.Payments;

namespace HungryNow.Entities
{
	public class Customer : Entity
	{
		public Customer(IPayable account) : base(account)
		{
		}

       public override string Notify(string info)
        {
            string displayedInfo = "Customer Info - " + DateTime.Now + ": " + info;
            Console.WriteLine(displayedInfo);
            return displayedInfo;
        }

        public override void Contact()
		{
			throw new NotImplementedException();
		}

		public bool Confirm(double estimatedTime)
		{
			// Customer takes the decision based on estimated time.
			// For simulation purposes we consider that the customer always approves the order.
			return true;
		}
	}
}
