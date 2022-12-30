using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HungryNow.Payments;
using HungryNow.Utilites;

namespace HungryNow.Entities
{
	public interface IEntity
	{
		string Notify(string info);

		void Contact();
	}
}
