using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWorlds
{
	public class Enemy : Entity
	{
		
	}
	
	public static class Enemies
	{
		public class test : Enemy //inheriting from the Enemy class so it can be used as a templait in encounters, rather than mutating them during run-time.
		{
			public test()
			{
				name = "tester thing";
				description = "a small thing with no real form or figure, only used for testing.";
				init = "You spot an odd shape aproaching you, it doesn't have much of a form, but it does look like it could be dangerouse if you don't deal with it.";

				AT = new attackText()
				{
					attacking = new List<string>
					{
						"The weird... thing ungulates and shoots out a blob of amorphous mass right at you, ",
						"The test thing wobbles and swings an... arm? appendage? some form of limb or some such at you ",
						"The odd thing in front of you quivers and moves at you quickly, "
					},
					hit = new List<string>
					{
						"and it hits you squarely, ",
						"and it is able to land a hit on you. ",
						"landing a hit, and hurting you a bit. "
					},
					miss = new List<string>
					{
						"but it misses you completely. ",
						"the odd thing doesn't even connect with you. ",
						"it simply whiffs the attack, missing you entirely. "
					},
					death = new List<string>
					{
						"The odd thing ungulates and collapses, bursting into a shower of flakes that dissapear into the air ",
						"The thing siply worbles and then collapses into a pile of shards, that quickly pop into a shower of flakes that dissipates into the wind. ",
						"The target convulsses, lunging at you but before it can connect, it pops into a shower of disolving flakes. "
					}
				};

				DT = new damageText()
				{
					unhurt = "It seems to be unscathed. ",
					healthy = "It seems to be doing just fine, you can't see any real difference. ", 
					damaged = "It is starting to glitch here and there, almost like, parts of it's body shifting left and right, or even lagging behind the rest of it's movements. ",
					critical = "The thing is berally holding itself together, each part of it's body taking seconds to catch up to where it is supposed to be, and never syncronously. " 
				};

				stats = new Stats()
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
