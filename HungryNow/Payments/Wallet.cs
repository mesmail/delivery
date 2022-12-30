using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HungryNow.Payments
{
	public class Wallet : IPayable
	{
		public double Amount { get; private set; }

		public Wallet(double initialAmount)
		{
			Amount = initialAmount;
		}

		public bool DeductAmount(double amount)
		{
			if (amount < Amount)
			{
				Amount -= amount;
				return true;
			}

			return false;
		}

		public void AddAmount(double amount)
		{
			Amount += amount;
		}
	}
}
