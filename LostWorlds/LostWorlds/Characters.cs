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
		public double Damage;
		public double Dodge;
		public double Endurance;
		public double Attack;
	}

	public class Character : Entity
	{
		public int Hunger = 7500;
		public int Energy = 7500;
		public int Thirst = 3500;
		public uint Stomach = 500;
		public uint Capacity = 1000;

		public Character()
		{
			stats = new Stats()
			{
				Attack = 100,
				Dodge = 100,
				Endurance = 100,
			};
		}

		public void Update()
		{
			Hunger -= (int)(Characters.Hrate * Time.Delta);
			Thirst -= (int)(Characters.Trate * Time.Delta);
			Stomach -= (uint)(Characters.Srate * Time.Delta);
			stats.Damage = Math.Max(0, stats.Damage - Characters.Drate * Time.Delta);

		}

		public void EatDrink(uint food, uint water, uint volume)
		{
			Hunger += (int)food;
			Thirst += (int)water;
			Stomach += volume;
		}

		public void Drink(uint water)
		{
			Thirst += (int)water;
			Capacity += water;
		}

		public SolidColorBrush HungerColor()
		{
			var col = (byte)(255 * Math.Pow(Math.E, -Math.Pow(Hunger, 2) / Math.Pow(7500 / 3, 2))); //this logic plots hunger on a more accurate scale, that is asymtotic, so the entire number line maps to between 0, and 255. 7500 is the daily requirement in kJ for energy

			//ternary would be much nicer here, but some people don't like it, it would be return Hunger >= 0 ? new SolidColorBrush(Color.FromRgb(col, col, col)) : new SolidColorBrush(Color.FromRgb(255, col, col));

			//awesome! I don't mind ternary at all, it tends to clean things up quite nicely.

			return Hunger >= 0 ? new SolidColorBrush(Color.FromRgb(col, col, col)) : new SolidColorBrush(Color.FromRgb(255, col, col));
		}
		public SolidColorBrush ThirstColor()
		{
			var col = (byte)(255 * Math.Pow(Math.E, -Thirst * Thirst / Math.Pow(3500 / 3, 2))); // This is a slightly different requirement, so, there is that. But hey, water is important as well, so, the player will be able to manage their water intake a little better, or more accurately, more accurately.

			//ternary would look like return Thirst >= 0 ? new SolidColorBrush(Color.FromRgb(col, col, col)) : new SolidColorBrush(Color.FromRgb(255, col, col));
			return Thirst >= 0 ? new SolidColorBrush(Color.FromRgb(col, col, col)) : new SolidColorBrush(Color.FromRgb(255, col, col));
		}
	}

	public static class Characters
	{
		public static double Hrate = 7500 / Time.Day;
		public static double Trate = 3500 / Time.Day;
		public static double Srate = 1 / (4 * Time.Hour);
		public static double Drate = 100 / Time.Day;

		public static Character Player = new Character()
		{
			AT = new Entity.attackText()
			{
				/*
				attacking = "You strike out at the enemy, ",
				hit = "and you manage to land a solid hit. ",
				miss = "but you miss it completely. ",
				death = "You pass out from the pain, only to wake up back at your home."
				*/
				attacking = new List<string>
				{
					"You strike out at the enemy, ",
					"You rear back and jab at the enemy, ",
					"You throw a nice kick towards the enemy "
				},
				hit = new List<string>
				{
					"and you manage to land a solid hit. ",
					"landing a nice strike on the opponent. ",
					"hitting squarely on your target. "
				},
				miss = new List<string>
				{
					"but you miss it completely. ",
					"but you mearly graze the target, doing nothing. ",
					"missing the target, leaving it unscathed. "
				},
				death = new List<string>
				{
					"You pass out from the pain, only to wake up back at your home.",
					"The pain becomes too much for you, and you collapse into unconsiousness, waking up at your home.",
					"You stumble back from the last hit, and drop to one knee as your vission collapses to a point, and fading into black. You wake up at your little home."
				}
			},
			DT = new Entity.damageText()
			{
				unhurt = "You don't even have a scratch on you. ",
				healthy = "You only have a few scrapes and bruizes on you. ",
				damaged = "You are a bit roughed up right now, but you should be able to go a little longer. ",
				critical = "You are in serious need of rest, you are bloodied and bruized, you really can't take much more abuse. "
			}
		};
		public static Character Partner = new Character();

		public static List<Character> Active = new List<Character>()
		{
			Player
		};

		public static void Update()
		{
			foreach(var c in Active)
			{
				c.Update();
			}
		}		
	}
}
