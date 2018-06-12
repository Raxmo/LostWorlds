using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWorlds
{
	public class Race
	{
		public string Name;
		public Stats BaseStats = new Stats();

		public void StatInit(Entity e)
		{
			e.stats.Strength = Utils.Gaussian(BaseStats.Strength, 15);
			e.stats.Constitution = Utils.Gaussian(BaseStats.Constitution, 15);
			e.stats.Dextarity = Utils.Gaussian(BaseStats.Dextarity, 15);
			e.stats.Intelegence = Utils.Gaussian(BaseStats.Intelegence, 15);
			e.stats.Wisdom = Utils.Gaussian(BaseStats.Wisdom, 15);
			e.stats.Focus = Utils.Gaussian(BaseStats.Focus, 15);
		}
	}

	public static class Races
	{
		public static Race Wolf = new Race()
		{
			Name = "Wolf",
			BaseStats = new Stats()
			{
				Strength = 110,
				Constitution = 110,
				Dextarity = 90,
				Intelegence = 95,
				Wisdom = 95,
				Focus = 100
			}
		};

		public static Race Human = new Race()
		{
			Name = "Human",
			BaseStats = new Stats()
			{
				Strength = 100,
				Constitution = 100,
				Dextarity = 100,
				Intelegence = 100,
				Wisdom = 100,
				Focus = 100,
			}
		};
	}
}
