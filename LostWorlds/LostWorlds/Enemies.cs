using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWorlds
{
	public class Enemy
	{
		public Stats Stats = new Stats();
		public string Attack(Character target)
		{
			string temp = "triggered";
			return temp;
		}
	}
	
	public static class Enemies
	{
		public class test : Enemy //inheriting from the Enemy class so it can be used as a templait in encounters, rather than mutating them during run-time.
		{
			public test()
			{
				this.Stats = new Stats()
				{
					Dodge = 85,
					Endurance = 85,
					Attack = 85,
					Damage = 0
				};
			}
		}
	}
}
