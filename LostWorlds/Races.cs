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
			e.stats.PainTolerance = Utils.Gaussian(BaseStats.PainTolerance, 15) + e.gender.mod.PainTolerance;
			e.stats.Flexibility = Utils.Gaussian(BaseStats.Flexibility, 15) + e.gender.mod.Flexibility;
			e.stats.Intelegence = Utils.Gaussian(BaseStats.Intelegence, 15) + e.gender.mod.Intelegence;
			e.stats.Knowledge = Utils.Gaussian(BaseStats.Knowledge, 15) + e.gender.mod.Knowledge;
			e.stats.Focus = Utils.Gaussian(BaseStats.Focus, 15) + e.gender.mod.Focus;
			e.stats.FineMoter = Utils.Gaussian(BaseStats.FineMoter, 15) + e.gender.mod.FineMoter;
			e.stats.Analysis = Utils.Gaussian(BaseStats.Analysis, 15) + e.gender.mod.Analysis;
			e.stats.Reflex = Utils.Gaussian(BaseStats.Reflex, 15) + e.gender.mod.Reflex;
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
				PainTolerance = 100,
				Flexibility = 110,
				Intelegence = 90,
				Knowledge = 90,
				Focus = 100,
				Reflex = 100,
				Analysis = 90,
				FineMoter = 110,
			}
		};

		public static Race Human = new Race()
		{
			Name = "Human",
			BaseStats = new Stats()
			{
				Strength = 100,
				PainTolerance = 100,
				Flexibility = 100,
				Intelegence = 100,
				Knowledge = 100,
				Focus = 100,
				Reflex = 100,
				Analysis = 100,
				FineMoter = 100,
			}
		};
	}
}
