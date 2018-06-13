using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * TODO:
 * The races could use a little work, mainly the fact that there are no dietary needs at the moment, will need to flesh out the Foods first before work can start on that particular subject
 */

namespace LostWorlds
{
	public class Race
	{
		public string Name;
		public Stats BaseStats = new Stats();

		public void StatInit(Entity e)
		{
			e.stats.Strength = Utils.Gaussian(BaseStats.Strength, 15) + e.gender.mod.Strength;
			e.stats.Constitution = Utils.Gaussian(BaseStats.Constitution, 15) + e.gender.mod.Constitution;
			e.stats.Dextarity = Utils.Gaussian(BaseStats.Dextarity, 15) + e.gender.mod.Dextarity;
			e.stats.Intelegence = Utils.Gaussian(BaseStats.Intelegence, 15) + e.gender.mod.Intelegence;
			e.stats.Wisdom = Utils.Gaussian(BaseStats.Wisdom, 15) + e.gender.mod.Wisdom;
			e.stats.Focus = Utils.Gaussian(BaseStats.Focus, 15) + e.gender.mod.Focus;


			Console.WriteLine("Strength: " + e.stats.Strength);
			Console.WriteLine("Constitution: " + e.stats.Constitution);
			Console.WriteLine("Dextarity: " + e.stats.Dextarity);
			Console.WriteLine("Intelegence: " + e.stats.Intelegence);
			Console.WriteLine("Wisdom: " + e.stats.Wisdom);
			Console.WriteLine("Focus: " + e.stats.Focus);
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
