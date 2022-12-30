using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HungryNow.Payments;
using HungryNow.Utilites;

namespace HungryNow.Entities
{
	public abstract class Entity : IEntity
	{
		public string Name { get; set; }
		public Address Address { get; set; }
		public string PhoneNumber { get; set; }
		public IPayable Payment { get; private set; }

		public Entity(IPayable payment)
		{
			this.Payment = payment;
		}

        public virtual string Notify(string info)
        {
            string displayedInfo = "Info - " + DateTime.Now + ": " + info;
            Console.WriteLine(displayedInfo);
            return displayedInfo;
        }
        public abstract void Contact();
	}
}
