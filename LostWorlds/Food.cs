using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * TODO:
 * I should add in more foods, and expand on dietary needs. I should also make the variouse races have variable dietary needs
 */

namespace LostWorlds
{
	public class Food
	{
		public double Energy;
		public double Water;
	}

    //you never assign to those again, might as well be readonly... <- very good point, will remember for later.
	public static class Foods
	{
		public static readonly Food Fish = new Food()
		{
			Energy = 4.453,
			Water = 0.722
		};

		public static readonly Food Berries = new Food()
		{
			Energy = 2.995,
			Water = 0.831
		};

		public static readonly Food Mushrooms = new Food()
		{
			Energy = 0.275,
			Water = 0.273
		};

		public static readonly Food Rye = new Food()
		{
			Energy = 10.101,
			Water = 0.075
		};

		public static readonly Food Cactus = new Food()
		{
			Energy = 0.246,
			Water = 0.95
		};
	}
}
