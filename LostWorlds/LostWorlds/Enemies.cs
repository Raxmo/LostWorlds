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
	}

    //again the internal classes may be readonly, unless you intend to mutate during game...
	public static class Enemies
	{
		public static readonly Enemy Test = new Enemy()
		{
			Stats = new Stats()
			{
				Damage = 0,
				Attack = 85,
				Dodge = 85,
				Endurance = 85
			}
		};
	}
}
