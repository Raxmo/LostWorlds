using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LostWorlds
{
	public struct Stats
	{
		public double damage;
		public double dodge;
		public double endurance;
		public double attack;
	}

	public class Character
	{
		public int hunger = 7500;
		public int energy = 7500;
		public int thirst = 3500;
		public uint stomach = 500;
		public uint capacity = 1000;

		public Stats stats = new Stats()
		{
			damage = 0,
			attack = 100,
			dodge = 100,
			endurance = 100
		};

		public void update()
		{
			hunger -= (int)(Characters.hrate * Time.delta);
			thirst -= (int)(Characters.trate * Time.delta);
			stomach -= (uint)(Characters.srate * Time.delta);
			stats.damage = Math.Max(0, stats.damage - Characters.drate * Time.delta);

		}

		public void eatDrink(uint food, uint water, uint volume)
		{
			hunger += (int)food;
			thirst += (int)water;
			stomach += volume;
		}

		public void drink(uint water)
		{
			thirst += (int)water;
			capacity += water;
		}

		public SolidColorBrush hungerColor()
		{
			byte col = (byte)(255 * Math.Pow(Math.E, -Math.Pow(hunger, 2) / Math.Pow(7500 / 3, 2))); //this logic plots hunger on a more accurate scale, that is asymtotic, so the entire number line maps to between 0, and 255. 7500 is the daily requirement in kJ for energy

			if (hunger >= 0)
			{
				return new SolidColorBrush(Color.FromRgb(col, col, col));
			}
			else
			{
				return new SolidColorBrush(Color.FromRgb(255, col, col));
			}
		}
		public SolidColorBrush thirstColor()
		{
			byte col = (byte)(255 * Math.Pow(Math.E, -thirst * thirst / Math.Pow(3500 / 3, 2))); // This is a slightly different requirement, so, there is that. But hey, water is important as well, so, the player will be able to manage their water intake a little better, or more accurately, more accurately.

			if (thirst >= 0)
			{
				return new SolidColorBrush(Color.FromRgb(col, col, col));
			}
			else
			{
				return new SolidColorBrush(Color.FromRgb(255, col, col));
			}
		}
	}

	public static class Characters
	{
		public static double hrate = 7500 / Time.day;
		public static double trate = 3500 / Time.day;
		public static double srate = 1 / (4 * Time.hour);
		public static double drate = 100 / Time.day;

		public static Character player = new Character();
		public static Character partner = new Character();

		public static List<Character> active = new List<Character>()
		{
			player
		};

		public static void update()
		{
			foreach(Character c in active)
			{
				c.update();
			}
		}
	}
}
