using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace LostWorlds
{
	/* How do we handle events within biomes?
	 * 
	 * What we have:
	 * - we know the biome of each pixel
	 * - we know where the center of each biome is located
	 * 
	 * What we need
	 * - global description of the biome
	 * - encounters for the biomes
	 * - biome specific actions
	 */

	public class BiomeEvents
	{
		public byte ID;
		public double[] EncounterRate;
		public string Description;
		public List<Encounter> EncounterList;
		public List<Event> Options;
		public Area host;

		public void Load()
		{
			host.Load();

			//MainWindow.App.MainText.SelectAll();
			//MainWindow.App.MainText.Selection.Text = Description;

			MainWindow.CurrentBiome = this;

			Console.WriteLine(BiomeEventData.Distance);


			var mean = EncounterRate[0];
			var sd =  EncounterRate[1];

			var sigmoid = 1 / (1 + Math.Exp(-((BiomeEventData.Distance - mean) / sd)));

			var doEncounter =  Utils.rand.NextDouble() < sigmoid;

			for (int i = 0; i < EncounterList.Count; i++)
			{
				var type = EncounterList[i].GetType();
			}

			if (doEncounter)
			{
				BiomeEventData.Distance = 0;
				EncounterList[Utils.rand.Next(EncounterList.Count)].Load();
			}
		}
	}

	public static class BiomeEventData
	{
		public static double Distance = 0;
	}

	public static class BiomeEventList
	{
		/* Biome ID's
		 * 0 - badlands
		 * 1 - mountains
		 * 2 - swamp
		 * 3 - river
		 * 4 - forest
		 * 5 - ruins
		 * 6 - desert
		 * 7 - plains
		 */

		public static BiomeEvents Forest = new BiomeEvents()
		{
			ID = 4,
			host = Areas.Forest,
			EncounterRate = new double[2] { 500, 20 },
			Description = "You find yourself in a vast forest. Plenty of trees and animals fill the forest. You'll have no troubles finding food or water here.",
			EncounterList = new List<Encounter>
			{
				new Encounters.Wolf()
			}
		};

		public static BiomeEvents Mountains = new BiomeEvents()
		{
			ID = 1,
			host = Areas.Mountains,
			EncounterRate = new double[2] { 500, 20 },
			Description = "You find yourself on a mountain, the rocky terraign does not lend itself well for food, but you'll be able to gather resources here relatively easily."
		};

		public static BiomeEvents Swamp = new BiomeEvents()
		{
			host = Areas.Swamp,
			ID = 2,
			EncounterRate = new double[2] { 500, 20 },
			Description = "You are in a swamp. The scent in the air is acrid and sour. You should be able to find mushrooms and fish, but clean water will be rather dificult for you to find.",
			EncounterList = new List<Encounter>
			{
				new Encounters.Aligator()
			}
		};

		public static BiomeEvents River = new BiomeEvents()
		{
			host = Areas.River,
			ID = 3,
			EncounterRate = new double[2] { 500, 20 },
			Description = "You find yourself in an area filled with small ponds, lakes and rivers. Fish and water will not be much of an issue for you here."
		};

		public static BiomeEvents Ruins = new BiomeEvents()
		{
			ID = 5,
			host = Areas.Ruins,
			EncounterRate = new double[2] { 500, 20 },
			Description = "You are in a kind of ruin, seems to be what once was industrial, with structures that are now very much no longer functional, or even likely safe to enter."
		};

		public static BiomeEvents Desert = new BiomeEvents()
		{
			ID = 6,
			host = Areas.Desert,
			EncounterRate = new double[2] { 500, 20 },
			Description = "You find yourself in a sea of sand. Water will be scarce here, and you'll likely have a hard time finding any of it. You will still be able to gather resources here, and even find food if you look hard enough."
		};

		public static BiomeEvents Badlands = new BiomeEvents()
		{
			ID = 0,
			host = Areas.Badlands,
			EncounterRate = new double[2] { 600, 100 },
			Description = "The badlands are berren, nothing around you of any use. The dirt is hard, the sky is red, and you already know that there are very unpleasant things lurking around you, ready to take your life at any moment. Surviving here is not something that can be done, you should leave as soon as you can.",
			EncounterList = new List<Encounter>
			{
				new Encounters.Raygolyth()
			}
		};

		public static BiomeEvents Plains = new BiomeEvents()
		{
			ID = 7,
			host = Areas.Plains,
			EncounterRate = new double[2] { 500, 20 },
			Description = "The Plains of the world are full of lush grasses and rolling hills. It's easy to move around in the plains, with not much in your way. You'll ba able to find sources of water relatively easy here, as well as plenty of food to find and eat."
		};

		public static BiomeEvents[] BiomeList = new BiomeEvents[8]
		{
			Badlands,		// 0
			Mountains,		// 1
			Swamp,			// 2
			River,			// 3
			Forest,			// 4
			Ruins,			// 5
			Desert,			// 6
			Plains			// 7
		};

	}
}
