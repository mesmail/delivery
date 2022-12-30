using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HungryNow.Utilites
{
	public class MapHelper
	{
		private static readonly int TimePerUnit = 2;

		public static double CalculateDistance(Location source, Location dest)
		{
			return Math.Sqrt(Math.Pow(dest.X - source.X, 2) + Math.Pow(dest.Y - source.Y, 2));
		}

		public static double CalculateTime(Location source, Location dest)
		{
			return CalculateDistance(source, dest) * TimePerUnit;
		}
	}
}
