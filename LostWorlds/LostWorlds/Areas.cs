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
		public List<Area> options = new List<Area>();
		public Encounter encounter = null;
		public string name;
		public string text;
		public uint travelTime;
		public uint actionTime;
		public uint returnTime;
		public bool isFirstVisit = true;
		public uint firstVisitTime;
		public string firstVisitText;
		public Action act = null;
		public double hrate = 7500;
		public double trate = 3500;

		private void loadOptions()
		{
			MainWindow.app.options.Children.Clear();
			if (options.Count > 0 && options != null)
			{
				for (int i = 0; i < options.Count(); i++)
				{
					Button b = new Button();

					b.Content = options[i].name;
					b.Name = "b" + i;
					b.Foreground = Brushes.White;
					b.Background = Brushes.Black;
					b.BorderBrush = Brushes.White;
					b.Click += new RoutedEventHandler(optionClick);

					MainWindow.app.options.Children.Add(b);
					Grid.SetColumn(b, (i % 2));
					Grid.SetRow(b, i / 2);
				}
			}
		}

		private void optionClick(object sender, EventArgs e)
		{
			Button clicked = (Button)sender;
			int index = Int32.Parse(clicked.Name.Split('b')[1]);
			Areas.back.Add(this);
			Time.delta = options[index].travelTime;
			options[index].load();
		}

		public void load()
		{
			Areas.curr = this;
			act?.Invoke();
			loadOptions();

			Time.delta += actionTime;
			Characters.hrate = hrate / Time.day;
			Characters.trate = trate / Time.day;

			if (isFirstVisit)
			{
				Time.delta += firstVisitTime;
				MainWindow.app.mainText.SelectAll();
				MainWindow.app.mainText.Selection.Text = firstVisitText;
				isFirstVisit = false;
			}
			else
			{
				MainWindow.app.mainText.SelectAll();
				MainWindow.app.mainText.Selection.Text = text;
			}
			if(encounter != null)
			{
				encounter.load();
			}
			MainWindow.app.update();
		}
	}
	public static class Areas
	{
		public static List<Area> back = new List<Area>();
		public static Area curr = new Area();

		// You'll need to list the areas in reverse order, due to limitations in hoisting, the FIRST area needs to be at the BOTTOM, and the FARTHEST area needs to be at the TOP.

		/*
		 * Each area is stored here there is a particular way to set them up however
		 * 
		 * name = the name that is displayed on the button to go to this particular area/menue
		 * 
		 */

		public static Area harvest = new Area()
		{
			name = "Harvest",
			actionTime = Time.hour,
			returnTime = 15 * 60,
			isFirstVisit = false,
			text = "You decide to pull some of the berries and some of the other vegies in the medow to bring back to your little camp for future consumption.",
			act = (() =>
			{
				Larder.add(Foods.berries, 1000);
			})
		};

		public static Area medow = new Area()
		{
			name = "Meadow",
			firstVisitTime = 15 * Time.minute,
			travelTime =15 * Time.minute,
			returnTime = 15 * Time.minute,
			firstVisitText = "You go off to the near-by meadow and look around at all the lovely flowers and berry bushes. You can tell that there are probably plenty of eatable veggies around in this area if you decide to harvest them from the wild.",
			text = "You return to the near-by meadow and look around the place for a moment, thinking about what you should do. You still remember where all the good harvestable foodstuffs are located in the meadow.",
			options = new List<Area>()
			{
				harvest
			}
		};

		public static Area cliff = new Area()
		{
			name = "Cliff",
			actionTime = 90 * Time.minute,
			returnTime = 15 * Time.minute,
			travelTime = 15 * Time.minute,
			hrate = 15000,
			trate = 7000,
			text = "You look up at the cliff, noticing quite a good number of hand-holds. You become confident that you can climb the whole thing in one go, so you start to climb up the side of the cliff. You take a good while to reach the top, carefully climbing as you go, given you have no safty net of any kind to keep you from falling off, but you do end up reaching the top to see a beautifull flowery meddow before you. you do notice a relatively fast and safe way back down the cliff side to your little camp below.",
			firstVisitText = "You look up at the cliff, noticing quite a good number of hand-holds. You become confident that you can climb the whole thing in one go, so you start to climb up the side of the cliff. You take a good while to reach the top, carefully climbing as you go, given you have no safty net of any kind to keep you from falling off, but you do end up reaching the top to see a beautifull flowery meddow before you. you do notice a relatively fast and safe way back down the cliff side to your little camp below.",
			act = (() =>
			{
				if(cliff.isFirstVisit)
				{
					home.options.Add(medow);
				}
			})
		};

		public static Area fish = new Area()
		{
			name = "Go Fish",
			actionTime = Time.hour,
			returnTime = 15 * Time.minute,
			isFirstVisit = false,
			text = "You look into the stream and spot some fish swimming around so you decide to try and catch some. You sit down at the edge of the stream and rest your hand in the water, gently wrigling your finger to entice the fish in the stream. After some time, you manage to catch enough for maybe a single meal, a job well done.",
			act = (() =>
			{
				Larder.add(Foods.fish, 1000);
			})
		};

		public static Area stream = new Area()
		{
			name = "Stream",
			firstVisitTime = 15 * Time.minute,
			travelTime = 15 * Time.minute,
			returnTime = 15 * 60,
			text = "Your curiosity about the sound of the stream draws you towards it, and you decide that you should perhapse go and visit the stream and see what kinds of things you can find there. On ariving at the stream, you see that the water is cristal clear, and there are even some small fish swimming in it. Intrigued, you try and catch but it slips from your grasp, maybe you should spend some more time and try a little harder.",
			firstVisitText = "Your curiosity about the sound of the stream draws you towards it, and you decide that you should perhapse go and visit the stream and see what kinds of things you can find there. On ariving at the stream, you see that the water is cristal clear, and there are even some small fish swimming in it. Intrigued, you try and catch but it slips from your grasp, maybe you should spend some more time and try a little harder.",
			options = new List<Area>()
			{
				fish
			},
			act = (() =>
			{
				Random rand = new Random();
				double tart = rand.NextDouble();
				if (tart < 0.25)
				{
					stream.encounter = Encounters.test;
				}
			})
		};

		public static Area drink = new Area()
		{
			name = "Drink",
			actionTime = 5 * Time.minute,
			returnTime = 0,
			isFirstVisit = false,
			text = "You move over to the fresh spring and take a drink of fresh water. the cool water tastes rather lovely, and clean. The water is likely the best quality that you could manage in this area",
			act = (() =>
			{
				foreach(Character c in Characters.active)
				{
					c.drink(1000);
				}
			})
		};

		public static Area eatDrink = new Area()
		{
			name = "Eat and Drink",
			actionTime = 30 * 60,
			returnTime = 0,
			isFirstVisit = false,
			act = (() =>
			{
				if(Larder.volume > 0)
				{
					if(Larder.volume >= 750)
					{
						eatDrink.text = "You decide to sit down and have a little something to eat and drink. You walk over to the pile of food you have set up and gather a little bit of it to sit down and eat. While eating you listen to the trees swaying in the gentle breeze, and you move over to the spring in the side of the cliff to have a little water for yourself.";
						Larder.volume -= 750;
						foreach(Character c in Characters.active)
						{
							uint room = c.capacity - c.stomach;
							c.eatDrink(Larder.density * (room * 75) / 100, Larder.water * (room * 75) / 100 + (room * 250) / 1000, 1000);
						}
					}
					else
					{
						eatDrink.text = "You decide to have something to eat, but you come to the conclusion that there isn't really enough for a full meal, but you decide to eat the rest of it anyway and perhaps go to gather up some more food at a later time.";
						uint eaten = 0;

						foreach(Character c in Characters.active)
						{
							uint room = c.capacity - c.stomach;
							if(room > Larder.volume)
							{
								eaten = Larder.volume;
								Larder.density = 0;
							}
							else
							{
								eaten = Larder.volume - c.stomach;
							}
							c.eatDrink(Larder.density * (eaten * 75) / 100, Larder.water * (eaten * 75) / 100 + (room * 250) / 1000, 1000);
						}
						
						Larder.volume -= eaten;
					}
				}
				else
				{
					eatDrink.text = "You are saddened by the fact that you have no food at the moment, but you can still get some water to drink from the stream. it won't stop you from being hungry, but at least you won't be dehidrated";
					foreach(Character c in Characters.active)
					{
						c.drink(c.capacity - c.stomach);
					}
				}
			})
		};

		public static Area sleep = new Area()
		{
			name = "Sleep",
			actionTime = 60 * 60 * 8,
			returnTime = 0,
			hrate = 5000,
			trate = 5000,
			isFirstVisit = false,
			text = "You decide to sleep for the next 8 hours or so. You wake up well rested, and when the time comes for it, you would have saved as well, but that's currently not a thing yet, come back later for that one."
		};

		public static Area home = new Area()
		{
			name = "Home",
			isFirstVisit = false,
			options = new List<Area>()
			{
				eatDrink,
				drink,
				sleep,
				stream,
				cliff
			},
			act = (() =>
			{
				back = new List<Area>()
				{
					home
				};
				string larder;
				if(Larder.volume == 0)
				{
					larder = "You look at a small alcove under the overhang and notice an absence of food there.";
				}
				else if(Larder.volume  <= 1000)
				{
					larder = "You look at the small pile of food in the corner of your little camp. You think that you might be able to get maybe a single meal out of it in it's current state, and that meal isn't even going to be a full meal.";
				}
				else if(Larder.volume <= 3000)
				{
					larder = "you glance over at the pile of food in the alcove, and you feel like you have only enough food for the day, tops. might be wise to use the day to go out and search for some more food.";
				}
				else if(Larder.volume <= 21000)
				{
					larder = "You look over at the noticable pile of food in the corner and smile. It should be enough for a few days at least.";
				}
				else
				{
					larder = "You spot the large pile of food in the little alcove you are using to store it all. The pile seems big enough to last you more than a week.";
				}
				home.text = "You are at the little camp site that you've made for yourself at the base of a small cliff, under an outcropping of rock to shield you from the elements a little bit. " + larder;
			})
		};

		public static Area start = new Area()
		{
			name = "Opening",
			actionTime = 0,
			firstVisitTime = 0,
			travelTime = 0,
			text = "Welcome to the world of Lost Worlds. For the time being, you'll only be able to visit a few areas, and not exactly be able to interact with anything. But it will take some time to do these things, so, you'll get to see how things work and change. For now, you have unlimited water, but will need to go and find food out in the woods, and you'll need to go back to ''base'' in order to consume the food and water. Enjoy this tiny little bit for now ^-^",
			isFirstVisit = false,
			options = new List<Area>()
			{
				home
			}
		};

	}
}
