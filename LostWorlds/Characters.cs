using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

/*
 * TODO:
 * no work needs to be done here quite yet, will need to create an NPC class at some point, and also handle the partner logic.
 */

namespace LostWorlds
{

	/* Overhaul stats
	 * 
	 * Strength           +Male
	 * Pain Tolerance     +Female
	 * flexibility        +female
	 * fine moter skills  +male
	 * Analysis           +female
	 * Reflex             +male
	 * Intelegence        -----
	 * Knowledge          -----
	 * Focus              -----
	 */
	public struct Stats
	{
		public double Damage;

		public double Strength;
		public double PainTolerance;
		public double Flexibility;
		public double FineMoter;
		public double Analysis;
		public double Reflex;
		public double Intelegence;
		public double Knowledge;
		public double Focus;
	}

	public class Character : Entity
	{
		public double Hunger = 7500;
		public double Energy = 7500;
		public double Thirst = 250;
		public double Stomach = 500;
		public double Capacity = 1000;

		public Race race = Races.Human;

		public Character()
		{
			canLevel = true;
		}

		public void Update()
		{
			Hunger -= (Characters.Hrate * Time.Delta);
			Thirst -= (Characters.Trate * Time.Delta);
			Stomach -= (Characters.Srate * Time.Delta);
			Stomach = Math.Max(0, Stomach);
			stats.Damage = Math.Max(0, stats.Damage - Characters.Drate * Time.Delta);

		}

		public void Sleep()
		{
			Characters.Hrate = 5000 / Time.Day;
			Characters.Trate = 500 / Time.Day;
			Characters.Drate = 300 / Time.Day;

			MainWindow.App.Update();
		}

		public void Eat(Food item, double volume)
		{
			volume = Math.Min(volume, Capacity - Stomach);

			Stomach += volume;
			var food = item.Energy * volume;
			var water = item.Water * volume;

			Hunger += food;
			Thirst += water;
		}

		public void EatDrink(uint food, uint water, uint volume)
		{
			Hunger += food;
			Thirst += water;
			Stomach += volume;
		}

		public void Drink(double water)
		{
			water = Math.Min(water, (Capacity - Stomach));

			Thirst += water;
			Stomach += water;
		}

		public SolidColorBrush HungerColor()
		{
			var col = (byte)(255 * Math.Pow(Math.E, -Math.Pow(Hunger, 2) / Math.Pow(7500 / 3, 2))); //this logic plots hunger on a more accurate scale, that is asymtotic, so the entire number line maps to between 0, and 255. 7500 is the daily requirement in kJ for energy

			return Hunger >= 0 ? new SolidColorBrush(Color.FromRgb(col, col, col)) : new SolidColorBrush(Color.FromRgb(255, col, col));
		}
		public SolidColorBrush ThirstColor()
		{
			var col = (byte)(255 * Math.Pow(Math.E, -Thirst * Thirst / Math.Pow(250 / 3, 2))); // This is a slightly different requirement, so, there is that. But hey, water is important as well, so, the player will be able to manage their water intake a little better, or more accurately, more accurately.
			
			return Thirst >= 0 ? new SolidColorBrush(Color.FromRgb(col, col, col)) : new SolidColorBrush(Color.FromRgb(255, col, col));
		}
	}

	public static class Characters
	{
		public static double Hrate = 40000;
		public static double Trate = 2000;
		public static double Srate = 1000 / (4.0 * Time.Hour);
		public static double Drate = 100 / Time.Day;

		public static Character Player = new Character()
		{
			AT = new Entity.attackText()
			{				
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
					"You pass out from the pain, only to wake up back you last fell asleep.",
					"The pain becomes too much for you, and you collapse into unconsiousness, waking up where you last fell asleep.",
					"You stumble back from the last hit, and drop to one knee as your vission collapses to a point, and fading into black. You wake up where you last fell asleep."
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
