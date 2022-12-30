using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HungryNow.Payments
{
	public interface IPayable
	{
		double Amount { get; }
		bool DeductAmount(double amount);
		void AddAmount(double amount);
	}
}
