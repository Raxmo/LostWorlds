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
			Energy = 2.815,
			Water = 0.414
		};

		public static readonly Food Berries = new Food()
		{
			Energy = 1.141,
			Water = 0.445
		};
	}
}
