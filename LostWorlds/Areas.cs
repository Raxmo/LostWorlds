using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

/*
 * TODO:
 * The areas seem mostly fine, there might be a better way to structure them, but not entirely sure what that is quite yet.
 * should probably completely refactor code to use the new Event class instead of the messy logic in here.
 */

namespace LostWorlds
{
	public class Area
	{
		public List<Area> Options = new List<Area>();
		public Encounter Encounter = null;
		public Event Event = null;
		public string Name;
		public string Text = "";
		public uint TravelTime = 0;
		public uint ActionTime = 0;
		public uint ReturnTime = 0;
		public bool IsFirstVisit = true;
		public uint FirstVisitTime = 0;
		public string FirstVisitText = "";
		public Action Act = null;
		public double Hrate = 40000;
		public double Trate = 2000;

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
			//this could be elegantly solved by null propagation using the null conditional ? would simply look like Encounter?.Load(); <- thank you so much, I was trying to figure out how to use that actually.
			Event?.Load();
			Encounter?.Load();
			MainWindow.App.Update();
		}
	}
	public static class Areas
	{
		public static List<Area> Back = new List<Area>();
		public static Area Curr = new Area();

		// You'll need to list the areas in reverse order, due to limitations in hoisting, the FIRST area needs to be at the BOTTOM, and the FARTHEST area needs to be at the TOP.

		//all of these could probably be made readonly <- most areas will need to mutate depending on if the player had visited the area before or not.

		public static Area Badlands = new Area()
		{
			Name = "Badlands",
			IsFirstVisit = false,
			Text = "The badlands are unfair, and undying. You will not survive long in these dessolate wastelands.",
			Event = Events.BadlandsEvent

		};

		public static Area Forest = new Area()
		{
			Name = "Forest",
			IsFirstVisit = false,
			Text = "The forests are inviting, and calm. Plenty of wildlife to be had as well as fresh water.",
			Event = Events.ForestEvent
		};

		public static Area Plains = new Area()
		{
			Name = "Plains",
			IsFirstVisit = false,
			Text = "The plains spread wide, fields of tall grass everywhere, stretching out as far as you can see. Plenty of roaming herding animals, and even easily accessable water can be found.",
			Event = Events.PlainsEvent
		};

		public static Area Desert = new Area()
		{
			Name = "Desert",
			IsFirstVisit = false,
			Text = "The desert is a dessolate place, with nothing but sand all around you. Water will be very difficult to find, and it'll be hard to find food as well."
		};

		public static Area Mountains = new Area()
		{
			Name = "Mountain",
			IsFirstVisit = false,
			Text = "The mountains are plooms of rock. Not many things grow on the mountains, but you'll be able to find some animals living in caves and other naturaly forming structures",
			Event = Events.MountainEvent
		};

		public static Area Ruins = new Area()
		{
			Name = "Ruins",
			IsFirstVisit = false,
			Text = "The ruined remains of what once was civilization sprawls out in front of you, not much can be found here, at least unless you have a way to refine the things you might find laying around."
		};

		public static Area Swamp = new Area()
		{
			Name = "Swamp",
			IsFirstVisit = false,
			Event = Events.SwampEvent,
			Text = "The swamps stink, are unpleasant, and full of bugs. You'll have a hard time finding fresh water, but mushrooms and amfibians will be easy to come by."
		};

		public static Area River = new Area()
		{
			Name = "River",
			IsFirstVisit = false,
			Event = Events.RiverEvent,
			Text = "The waterlands are pleasant. Flowing water can be heard in just about every direction. Fish and fresh water will be plentifull."
		};

		public static Area Starting = new Area()
		{
			Name = "Starting",
			TravelTime = 0,
			IsFirstVisit = false,
			ReturnTime = 0,
			FirstVisitTime = 0,
			Text = "You look down at yourself and nod, seeing as your body is as it should. You look out and the world slowly comes into focus, only thing to do now is go on and live your new life.",
			Options = new List<Area>()
		};
		
		//choosing the player's gender
		public static Area PlayerGender = new Area()
		{
			Name = "Creation",
			Event = Events.PlayerGender
		};

		public static Area Start = new Area()
		{
			Name = "Opening",
			ActionTime = 0,
			FirstVisitTime = 0,
			TravelTime = 0,
			Text = "Welcome to Lost Worlds V1. This version has exploration, survival, and character creation. Some things to note in this version are: Be careful when crossing chunk edges (marked with a thin gray line) as if you are dragging too quickly, the map has a high chanse of braking and sending you an entire chunk away (solution in the works). The next version will focus on some optimizations, and adding in new features. Things the next version will add: 1) static locations [like your home base] 2) more enemies 3) more things to gather and find",
			IsFirstVisit = false,
			Options = new List<Area>()
			{
				PlayerGender
			}
		};

	}
}
