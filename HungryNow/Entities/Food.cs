using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HungryNow.Utilites;

namespace HungryNow.Entities
{
	public class Food
	{
		public string Name { get; set; }
		public SpicyLevels SpicyLevel { get; set; }
		public double Price { get; set; }
		public int PreperationTime { get; set; }

		public void IncreaseSpicyLevel()
		{
			SpicyLevel = SpicyLevel++;
		}

		public void DecreaseSpicyLevel()
		{
			SpicyLevel = SpicyLevel--;
		}
	}
}
