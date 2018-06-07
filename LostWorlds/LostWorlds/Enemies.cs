using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWorlds
{
	public class Enemy
	{
		public Stats stats = new Stats();
	}

	public static class Enemies
	{
		public static Enemy test = new Enemy()
		{
			stats = new Stats()
			{
				damage = 0,
				attack = 85,
				dodge = 85,
				endurance = 85
			}
		};
	}
}
