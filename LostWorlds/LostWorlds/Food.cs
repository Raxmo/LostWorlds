using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWorlds
{
	public class Food
	{
		public double energy;
		public double water;
	}

	public static class Foods
	{
		public static Food fish = new Food()
		{
			energy = 2.815,
			water = 0.414
		};

		public static Food berries = new Food()
		{
			energy = 1.141,
			water = 0.445
		};
	}
}
