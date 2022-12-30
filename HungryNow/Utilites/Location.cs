using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HungryNow.Utilites
{
	public class Location
	{
		public Location(double x, double y)
		{
			X = x;
			Y = y;
		}

		internal double X { get; private set; }
		internal double Y { get; private set; }
	}
}
