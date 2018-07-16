using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * TODO:
 * Enemies are fine for now, everything seems to be doing just well enough, however, figuring out a way to have per-type knowledge rating rather than per instance knowledge rating would be ideal.
 */

namespace LostWorlds
{
	public class Enemy : Entity
	{
		
	}
	/* Stat block for ease:
	 
	   Strength = 100,
	   PainTolerance = 100,
	   Flexibility = 100,
	   FineMoter = 100,
	   Analysis = 100,
	   Reflex = 100,
	   Intelegence = 100,
	   Knowledge = 100,
	   Focus = 100, 


		full block :

				name = " ";
				description = " ";
				init = " ";

				AT = new attackText()
				{
					attacking = new List<string>
					{
						""
					},
					hit = new List<string>
					{
						""
					},
					miss = new List<string>
					{
						""
					},
					death = new List<string>
					{
						""
					}
				};
		
				DT = new damageText()
				{
					unhurt = "",
					healthy = "",
					damaged = "",
					critical = ""
				};

				stats = new Stats()
				{
					Strength = 100,
					PainTolerance = 100,
					Flexibility = 100,
					FineMoter = 100,
					Analysis = 100,
					Reflex = 100,
					Intelegence = 100,
					Knowledge = 100,
					Focus = 100,
				};
	 */

	public static class Enemies
	{
		public class Alagator : Enemy
		{
			public Alagator()
			{
				name = "Alagator";
				description = "The alagator is a large reptilian animal with powerfull jaws and a pain to kill. You'll be wise to be carefull around them. ";
				init = "You spot a large aligator comming up from the swamp. You'll want to be carefull around it. ";

				AT = new attackText()
				{
					attacking = new List<string>
					{
						"The aligator snaps it's jaws at you, "
					},
					hit = new List<string>
					{
						"and bites down hard, latching on and rending your flesh. "
					},
					miss = new List<string>
					{
						"and snaps at air as you manage to step out of the way. "
					},
					death = new List<string>
					{
						"The aligator twitches and lays lifeless. "
					}
				};

				DT = new damageText()
				{
					unhurt = "The aligator is more than ready to tear you appart. ",
					healthy = "The aligator doesn't even seem that hurt yet. ",
					damaged = "The aligator is bleeding, but more than ready to take you out. ",
					critical = "The aligator is bleeding profusely and does not have much time left. "
				};

				stats = new Stats()
				{
					Strength = 150,
					PainTolerance = 150,
					Flexibility = 100,
					FineMoter = 100,
					Analysis = 100,
					Reflex = 100,
					Intelegence = 100,
					Knowledge = 100,
					Focus = 100,
				};
			}
		}

		public class Wolf : Enemy
		{
			public Wolf()
			{
				name = "Wolf";
				description = "The wolf is a medium sized woodland creature. One that on it's own shouldn't cause too much issue, but can still pose a threat.";
				init = "A wolf jumps from the brush releasing a Forlorn howl before eyeing you. Tongue caught between bared teeth.The growl rumbling as you realize it will not back down from the hunt.";

				AT = new attackText()
				{
					attacking = new List<string>
					{
						""
					},
					hit = new List<string>
					{
						"The wolf lunges at you, though you react too slowy as you feel the fangs tear into your flesh, hot blood flowing over the creature's jaws. Kicking the wolf off you prepare yourself. ",
						"The wolf snarls and latches on with it's jaws, piercing your flesh deep, drawing blood. "
					},
					miss = new List<string>
					{
						"The wolf lunges at you, however your arm catches it under the jaw as it snaps wildly at you finding nothing but air for purchase. ",
						"The wolf lunges at you, but you are able to side-step it in time, missing you completely. "
					},
					death = new List<string>
					{
						"Your weapon connects as the wolf hits the ground, trying to raise it self it falls to it's side. It's eye wild as you approach it. It breathing rapid for a few moments before it releases a sigh and stirs no more. "
					}
				};

				DT = new damageText()
				{
					unhurt = "The wolf paces around you, sizing you up grey fur unmarred as it growls hungrily toward you. ",
					healthy = "The wounds you have left seem to slow down the wolf very little. Small mattings of it's fur  are darkened with it's own blood. ",
					damaged = "The wolf pants heavily as it's own vermillion life essence drips from it's now matted fur. It refuses to cower and back down though moves slower, more cautious of you. ",
					critical = "The wolf has become a pathetic sight. It whimpers softly as it limps around you. It's tongue hangs lazily from it's toothy maw as it pants even harder. Each step causing it to wince. It does not have much left in it. "
				};

				stats = new Stats()
				{
					Strength = 100,
					PainTolerance = 100,
					Flexibility = 100,
					FineMoter = 100,
					Analysis = 100,
					Reflex = 100,
					Intelegence = 100,
					Knowledge = 100,
					Focus = 100,
				};
			}
		}

		public class Raygolyth : Enemy
		{
			public Raygolyth()
			{
				name = "Raygolyth";
				description = "A massive, creature riddled with spikes and blades. Fearsome enough to rip appart just about anything that gets in it's way";
				init = "You spot a massive creature aproaching you, blades and spikes covering it's body. Looks like it could take on just about anything you throw at it.";

				AT = new attackText()
				{
					attacking = new List<string>
					{
						"The Raygolyth launches a fearsome attack in your direction, ",
						"The massive creature swings hard at you with one of its many blades, "
					},
					hit = new List<string>
					{
						"hitting you sqarely. ",
						"landing a solid hit. "
					},
					miss = new List<string>
					{
						"but manages to miss you. ",
						"missing you completely. "
					},
					death = new List<string>
					{
						"The Raygolyth staggars and crumples to the ground, lifeless. ",
						"The Raygolyth cries out in pain, and falls to the ground in a crumpled heap. "
					}
				};

				DT = new damageText()
				{
					unhurt = "The Raygolyth is standing as strong as ever. ",
					healthy = "The Raygolyth only hase a few scrapes on it, but it is still as powerfull as it was before. ",
					damaged = "The Raygolyth shows signs of damage, a little bloody here and there. ",
					critical = "The Raygolyth has seen better days, you might actually ba able to kill it, if it doesn't finish you off first. "
				};

				stats = new Stats()
				{
					Strength = 205,
					PainTolerance = 205,
					Flexibility = 85,
					FineMoter = 85,
					Analysis = 85,
					Reflex = 85,
					Intelegence = 85,
					Knowledge = 85,
					Focus = 85,
				};
			}
		}
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
						"The thing simply worbles and then collapses into a pile of shards, that quickly pop into a shower of flakes that dissipates into the wind. ",
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
					Strength = 85,
					PainTolerance = 85,
					Flexibility = 85,
					FineMoter = 85,
					Analysis = 85,
					Reflex = 85,
					Intelegence = 85,
					Knowledge = 85,
					Focus = 85,
				};
			}
		}
	}
}
