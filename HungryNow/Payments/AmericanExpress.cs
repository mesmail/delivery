using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HungryNow.Payments
{
	public class AmericanExpress : IPayable
	{
		public double Amount => throw new NotImplementedException();
		public void AddAmount(double amount)
		{
			throw new NotImplementedException();
		}

		public bool DeductAmount(double amount)
		{
			throw new NotImplementedException();
		}
	}
}
