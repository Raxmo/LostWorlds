using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace LostWorlds
{
	public class Area
	{
		public List<Area> Options = new List<Area>();
		public Encounter Encounter = null;
		public string Name;
		public string Text;
		public uint TravelTime;
		public uint ActionTime;
		public uint ReturnTime;
		public bool IsFirstVisit = true;
		public uint FirstVisitTime;
		public string FirstVisitText;
		public Action Act = null;
		public double Hrate = 7500;
		public double Trate = 3500;

		private void LoadOptions()
		{
			MainWindow.App.Options.Children.Clear();
			if (Options.Count > 0 && Options != null)
			{
				for (var i = 0; i < Options.Count(); i++)
				{
					var b = new Button();

					b.Content = Options[i].Name;
					b.Name = "b" + i;
					b.Foreground = Brushes.White;
					b.Background = Brushes.Black;
					b.BorderBrush = Brushes.White;
					b.Click += new RoutedEventHandler(OptionClick);

					MainWindow.App.Options.Children.Add(b);
					Grid.SetColumn(b, (i % 2));
					Grid.SetRow(b, i / 2);
				}
			}
		}

		private void OptionClick(object sender, EventArgs e)
		{
			var clicked = (Button)sender;
			var index = Int32.Parse(clicked.Name.Split('b')[1]);
			Areas.Back.Add(this);
			Time.Delta = Options[index].TravelTime;
			Options[index].Load();
		}

		public void Load()
		{
			Areas.Curr = this;
			Act?.Invoke();
			LoadOptions();

			Time.Delta += ActionTime;
			Characters.Hrate = Hrate / Time.Day;
			Characters.Trate = Trate / Time.Day;

			if (IsFirstVisit)
			{
				Time.Delta += FirstVisitTime;
				MainWindow.App.MainText.SelectAll();
				MainWindow.App.MainText.Selection.Text = FirstVisitText;
				IsFirstVisit = false;
			}
			else
			{
				MainWindow.App.MainText.SelectAll();
				MainWindow.App.MainText.Selection.Text = Text;
			}
            //this could be elegantly solved by null propagation using the null conditional ? would simply look like Encounter?.Load();
            if (Encounter != null)
			{
				Encounter.Load();
			}
			MainWindow.App.Update();
		}
	}
	public static class Areas
	{
		public static List<Area> Back = new List<Area>();
		public static Area Curr = new Area();

		// You'll need to list the areas in reverse order, due to limitations in hoisting, the FIRST area needs to be at the BOTTOM, and the FARTHEST area needs to be at the TOP.

		/*
		 * Each area is stored here there is a particular way to set them up however
		 * 
		 * name = the name that is displayed on the button to go to this particular area/menue
		 * 
		 */

        //all of these could probably be made readonly
		public static Area Harvest = new Area()
		{
			Name = "Harvest",
			ActionTime = Time.Hour,
			ReturnTime = 15 * 60,
			IsFirstVisit = false,
			Text = "You decide to pull some of the berries and some of the other vegies in the medow to bring back to your little camp for future consumption.",
			Act = (() =>
			{
				Larder.Add(Foods.Berries, 1000);
			})
		};

		public static Area Medow = new Area()
		{
			Name = "Medow",
			FirstVisitTime = 15 * Time.Minute,
			TravelTime =15 * Time.Minute,
			ReturnTime = 15 * Time.Minute,
			FirstVisitText = "You go off to the near-by medow and look around at all the lovely flowers and berry bushes. You can tell that there are probably plenty of eatable veggies around in this area if you decide to harvest them from the wild.",
			Text = "You return to the near-by medow and look around the place for a moment, thinking about what you should do. You still remember where all the good harvestable food stuffs are located in the medow.",
			Options = new List<Area>()
			{
				Harvest
			}
		};

		public static Area Cliff = new Area()
		{
			Name = "Cliff",
			ActionTime = 90 * Time.Minute,
			ReturnTime = 15 * Time.Minute,
			TravelTime = 15 * Time.Minute,
			Hrate = 15000,
			Trate = 7000,
			Text = "You look up at the cliff, noticing quite a good number of hand-holds. You become confident that you can climb the whole thing in one go, so you start to climb up the side of the cliff. You take a good while to reach the top, carefully climbing as you go, given you have no safty net of any kind to keep you from falling off, but you do end up reaching the top to see a beautifull flowery meddow before you. you do notice a relatively fast and safe way back down the cliff side to your little camp below.",
			FirstVisitText = "You look up at the cliff, noticing quite a good number of hand-holds. You become confident that you can climb the whole thing in one go, so you start to climb up the side of the cliff. You take a good while to reach the top, carefully climbing as you go, given you have no safty net of any kind to keep you from falling off, but you do end up reaching the top to see a beautifull flowery meddow before you. you do notice a relatively fast and safe way back down the cliff side to your little camp below.",
			Act = (() =>
			{
				if(Cliff.IsFirstVisit)
				{
					Home.Options.Add(Medow);
				}
			})
		};

		public static Area Fish = new Area()
		{
			Name = "Go Fish",
			ActionTime = Time.Hour,
			ReturnTime = 15 * Time.Minute,
			IsFirstVisit = false,
			Text = "You look into the stream and spot some fish swimming around so you decide to try and catch some. You sit down at the edge of the stream and rest your hand in the water, gently wrigling your finger to entice the fish in the stream. After some time, you manage to catch enough for maybe a single meal, a job well done.",
			Act = (() =>
			{
				Larder.Add(Foods.Fish, 1000);
			})
		};

		public static Area Stream = new Area()
		{
			Name = "Stream",
			FirstVisitTime = 15 * Time.Minute,
			TravelTime = 15 * Time.Minute,
			ReturnTime = 15 * 60,
			Text = "Your curiosity about the sound of the stream draws you towards it, and you decide that you should perhapse go and visit the stream and see what kinds of things you can find there. On ariving at the stream, you see that the water is cristal clear, and there are even some small fish swimming in it. Intrigued, you try and catch but it slips from your grasp, maybe you should spend some more time and try a little harder.",
			FirstVisitText = "Your curiosity about the sound of the stream draws you towards it, and you decide that you should perhapse go and visit the stream and see what kinds of things you can find there. On ariving at the stream, you see that the water is cristal clear, and there are even some small fish swimming in it. Intrigued, you try and catch but it slips from your grasp, maybe you should spend some more time and try a little harder.",
			Options = new List<Area>()
			{
				Fish
			},
			Act = (() =>
			{
				var rand = new Random();
				var tart = rand.NextDouble();
				if (tart < 0.25)
				{
					Stream.Encounter = Encounters.Test;
				}
			})
		};

		public static Area Drink = new Area()
		{
			Name = "Drink",
			ActionTime = 5 * Time.Minute,
			ReturnTime = 0,
			IsFirstVisit = false,
			Text = "You move over to the fresh spring and take a drink of fresh water. the cool water tastes rather lovely, and clean. The water is likely the best quality that you could manage in this area",
			Act = (() =>
			{
				foreach(var c in Characters.Active)
				{
					c.Drink(1000);
				}
			})
		};

		public static Area EatDrink = new Area()
		{
			Name = "Eat and Drink",
			ActionTime = 30 * 60,
			ReturnTime = 0,
			IsFirstVisit = false,
			Act = (() =>
			{
				if(Larder.Volume > 0)
				{
					if(Larder.Volume >= 750)
					{
						EatDrink.Text = "You decide to sit down and have a little something to eat and drink. You walk over to the pile of food you have set up and gather a little bit of it to sit down and eat. While eating you listen to the trees swaying in the gentle breeze, and you move over to the spring in the side of the cliff to have a little water for yourself.";
						Larder.Volume -= 750;
						foreach(var c in Characters.Active)
						{
							var room = c.Capacity - c.Stomach;
							c.EatDrink(Larder.Density * (room * 75) / 100, Larder.Water * (room * 75) / 100 + (room * 250) / 1000, 1000);
						}
					}
					else
					{
						EatDrink.Text = "You decide to have something to eat, but you come to the conclusion that there isn't really enough for a full meal, but you decide to eat the rest of it anyway and perhaps go to gather up some more food at a later time.";
						uint eaten = 0;

						foreach(var c in Characters.Active)
						{
							var room = c.Capacity - c.Stomach;
							if(room > Larder.Volume)
							{
								eaten = Larder.Volume;
								Larder.Density = 0;
							}
							else
							{
								eaten = Larder.Volume - c.Stomach;
							}
							c.EatDrink(Larder.Density * (eaten * 75) / 100, Larder.Water * (eaten * 75) / 100 + (room * 250) / 1000, 1000);
						}
						
						Larder.Volume -= eaten;
					}
				}
				else
				{
					EatDrink.Text = "You are saddened by the fact that you have no food at the moment, but you can still get some water to drink from the stream. it won't stop you from being hungry, but at least you won't be dehidrated";
					foreach(var c in Characters.Active)
					{
						c.Drink(c.Capacity - c.Stomach);
					}
				}
			})
		};

		public static Area Sleep = new Area()
		{
			Name = "Sleep",
			ActionTime = 60 * 60 * 8,
			ReturnTime = 0,
			Hrate = 5000,
			Trate = 5000,
			IsFirstVisit = false,
			Text = "You decide to sleep for the next 8 hours or so. You wake up well rested, and when the time comes for it, you would have saved as well, but that's currently not a thing yet, come back later for that one."
		};

		public static Area Home = new Area()
		{
			Name = "Home",
			IsFirstVisit = false,
			Options = new List<Area>()
			{
				EatDrink,
				Drink,
				Sleep,
				Stream,
				Cliff
			},
			Act = (() =>
			{
				Back = new List<Area>()
				{
					Home
				};
				string larder;
				if(Larder.Volume == 0)
				{
					larder = "You look at a small alcove under the overhang and notice an absence of food there.";
				}
				else if(Larder.Volume  <= 1000)
				{
					larder = "You look at the small pile of food in the corner of your little camp. You think that you might be able to get maybe a single meal out of it in it's current state, and that meal isn't even going to be a full meal.";
				}
				else if(Larder.Volume <= 3000)
				{
					larder = "you glance over at the pile of food in the alcove, and you feel like you have only enough food for the day, tops. might be wise to use the day to go out and search for some more food.";
				}
				else if(Larder.Volume <= 21000)
				{
					larder = "You look over at the noticable pile of food in the corner and smile. It should be enough for a few days at least.";
				}
				else
				{
					larder = "You spot the large pile of food in the little alcove you are using to store it all. The pile seems big enough to last you more than a week.";
				}
				Home.Text = "You are at the little camp site that you've made for yourself at the base of a small cliff, under an outcropping of rock to shield you from the elements a little bit. " + larder;
			})
		};

		public static Area Start = new Area()
		{
			Name = "Opening",
			ActionTime = 0,
			FirstVisitTime = 0,
			TravelTime = 0,
			Text = "Welcome to the world of Lost Worlds. For the time being, you'll only be able to visit a few areas, and not exactly be able to interact with anything. But it will take some time to do these things, so, you'll get to see how things work and change. For now, you have unlimited water, but will need to go and find food out in the woods, and you'll need to go back to ''base'' in order to consume the food and water. Enjoy this tiny little bit for now ^-^",
			IsFirstVisit = false,
			Options = new List<Area>()
			{
				Home
			}
		};

	}
}
