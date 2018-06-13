using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LostWorlds
{
	public class Event
	{
		public double ActionTime = 0;
		public string Text = "";
		public Action act = null;
		public List<option> options = new List<option>();

		public struct option
		{
			public string name;
			public Action optionClick;

			public void click(object sender, EventArgs e)
			{
				optionClick();
			}
		}

		public void Load()
		{
			act?.Invoke();

			MainWindow.App.MainText.SelectAll();
			MainWindow.App.MainText.Selection.Text = Text;

			MainWindow.App.Options.Children.Clear();

			int index = 0;
			foreach(option o in options)
			{
				var btn = new Button()
				{
					Content = o.name,
					Name = "b"+index,
					Foreground = Brushes.White,
					Background = Brushes.Black,
					BorderBrush = Brushes.White,
				};
				btn.Click += new RoutedEventHandler(o.click);

				MainWindow.App.Options.Children.Add(btn);
				Grid.SetColumn(btn, (index % 2));
				Grid.SetRow(btn, index / 2);

				index++;
			}
			MainWindow.App.Update();
		}
	}
	public static class Events
	{
		public static Event PlayerResults = new Event() //dear god, this section is a wall of text. and as soon as I re-work the stats, it's only going to be worse. Fun time ahead of me.
		{
			act = (() =>
			{
				Characters.Player.race.StatInit(Characters.Player);

				string temp = "You look over yourself and asses your abilities. ";

				temp +=
				(Characters.Player.stats.Strength < 40) ? "You are abismally week, it's a wonder you can even move with how little muscle mass you have. " :
				(Characters.Player.stats.Strength < 55) ? "You are greatly weaker than most, you won't be doing much lifting. " :
				(Characters.Player.stats.Strength < 70) ? "You are far weaker than most, it'll be hard to do any lifting, or even hurting anything. " :
				(Characters.Player.stats.Strength < 85) ? "You are weaker than most, you'll still be able to do things, it'll just be harder than most others. " :
				(Characters.Player.stats.Strength < 100) ? "You are about average in strength, mybe a slightly weaker than most. " :
				(Characters.Player.stats.Strength < 115) ? "You are about average, maybe slightly stronger than most. " :
				(Characters.Player.stats.Strength < 130) ? "You are stronger than most, you'll be able to do more lifting, and your punches are going to hurt more. " :
				(Characters.Player.stats.Strength < 145) ? "You are far stronger than most, you'll be able to lift and carry things without problem, and you'll do quite some damage with a simple punch. " :
				(Characters.Player.stats.Strength < 160) ? "You are gratly stronger than most, lifting and hurting things will be absolutely no problem at all" :
				"Your strength is a thing of legends. Moving boulders, killing things in a single punch, all of these things are in your future. ";

				temp +=
				(Characters.Player.stats.Constitution < 40) ? "Your lack of constitution is a medical mystery, or maybe a statistical anomoly. You'll likely blead out from a splinter, don't get bruized, because you might just die. " :
				(Characters.Player.stats.Constitution < 55) ? "Your constitution falls greatly below the average, you'll have a hard time dealing with pain and could die quite easily. " :
				(Characters.Player.stats.Constitution < 70) ? "Your constitution is far less than most indaviduals, you'll get sick easily, have a hard time runing for long periods of time, and will go down rather easily, danger is in your future. " :
				(Characters.Player.stats.Constitution < 85) ? "Your constitution is less, you won't be able to run as far, or take as many hits. " :
				(Characters.Player.stats.Constitution < 100) ? "Your constitution is about average, maybe slightly less than most. " :
				(Characters.Player.stats.Constitution < 115) ? "Your constitution is about average, maybe only slightly higher than most. " :
				(Characters.Player.stats.Constitution < 130) ? "your constitution is higher than most, you'll be able to run farther and take more hits and shrug off pain easier. " :
				(Characters.Player.stats.Constitution < 145) ? "Your constitution is far higher than average, sickness and pain is not a common idea to you, and getting tired from running long distances is an acheivement in your case. " :
				(Characters.Player.stats.Constitution < 160) ? "Your constitution greatly exceeds your peers, you'll be the one to last the longest in a distance run, it'll be quite hard to take you down, pain nearly doesn't even register with you. " :
				"Your contitution is legendary, a medical miricle, you'll likely never get sick, you'll be able to run a marathon without even breaking a sweat, nearly loose limbs and still be perfectly fine. ";

				temp +=
				(Characters.Player.stats.Dextarity < 40) ? "It's a wonder that you can even move with how bad your dextarity is, a literal wooden board is more limber than you are, good luck dodging, you are going to need it. " :
				(Characters.Player.stats.Dextarity < 55) ? "Your dextarity falls greatly below the norm, you can move, but only barely, it'll be nearly impossible for you to dodge much of anything in your condition. " :
				(Characters.Player.stats.Dextarity < 70) ? "Your dextarity is far below the average, you strugle to turn your head, can't do any kind of fine motar skills, and you are about as hard to hit as a barn door. " :
				(Characters.Player.stats.Dextarity < 85) ? "You are less dextrous than your peers, you'll be easier to hit, and will have a harder time doing precise work on much of anything. " :
				(Characters.Player.stats.Dextarity < 100) ? "Your dextarity is about average, maybe slightly less than most. " :
				(Characters.Player.stats.Dextarity < 115) ? "Your dextarity is about average, maybe slightly more than most. " :
				(Characters.Player.stats.Dextarity < 130) ? "Your dextarity is higher than most, you'll be harder to hit, and will have an easier time dodging and doing fine motar skills. " :
				(Characters.Player.stats.Dextarity < 145) ? "Your dextarity is far higher than your peers, you'll be incredibly hard to hit, you're a fine candidate for surgury on even the smallest of creatures. " :
				(Characters.Player.stats.Dextarity < 160) ? "Your dextarity is greatly above your peers, you are at the level of a contortionist, it'll be hard to keep you trapped, or hold onto you for any length of time. " :
				"Your dextarity is legendary, you could eassily kiss your own ass if you wanted to, you are the slipperiest of the slippery, it's nearly impossible to hit you. ";

				temp +=
				(Characters.Player.stats.Intelegence < 40) ? "You probably have Lissencephaly, a rare condition that that prevents your brain from developing properly, and most indaviduals with this condition don't see the age of 10. " :
				(Characters.Player.stats.Intelegence < 55) ? "Your intelegence falls greatly below the average, you likely have a rare condition that prevents your brain from having ripples, giving you a literal smooth brain. You really can't learn anything, ever. " :
				(Characters.Player.stats.Intelegence < 70) ? "Your IQ falls far below the average, you have a hard time learning new things, and it takes quite a long time to understand new consepts. " :
				(Characters.Player.stats.Intelegence < 85) ? "Your intelegence is lower than average, you can still learn new things, with enough time that is. " :
				(Characters.Player.stats.Intelegence < 100) ? "You are about as intelegent as the average person, maybe only slightly below average. " :
				(Characters.Player.stats.Intelegence < 115) ? "You have a higher intelegence than most, you'll catch on a little faster than the average person. " :
				(Characters.Player.stats.Intelegence < 130) ? "You have a far higher intelegence than most indaviduals, you'll learn new things very quickly. " :
				(Characters.Player.stats.Intelegence < 145) ? "Your intelegence risses greatly above average, you will learn things incredibly fast. " :
				(Characters.Player.stats.Intelegence < 160) ? "You are considered a geniouse. Complex concepts are no obstical for you, and learning new things takes a blink of an eye. " :
				"You are the smartest person in the world. You are beond a geniouse, some would say you are the smartest person who will ever live. ";

				temp +=
				(Characters.Player.stats.Focus < 40) ? "Your ability to focus is non-existant. It isn't there... HEY! I'm talking over here! " :
				(Characters.Player.stats.Focus < 55) ? "Your focus falls greatly below the average, you can't focus on anything for any reasonable length of time. " :
				(Characters.Player.stats.Focus < 70) ? "Your ability to focus falls far below the average, you have a hard time staying on track, moving from one topic to another rapidly. You might even have a hard time finishing what you start. " :
				(Characters.Player.stats.Focus < 85) ? "Your focus is lower than average, you have a hard time staying on track and it takes you longer to complete tasks due to how easily distracted you are. " :
				(Characters.Player.stats.Focus < 100) ? "Your focus is about average, maybe slightly lower than most. " :
				(Characters.Player.stats.Focus < 115) ? "Your focus is about average, maybe slightly higher than most. " :
				(Characters.Player.stats.Focus < 130) ? "Your focus is higher than most, you'll be able to focus on tasks and see them through without issue. " :
				(Characters.Player.stats.Focus < 145) ? "Your focus is far higher than most, you have laser like focus on whatever catches your attention, and it can be hard to pull you away from something that you are doing. " :
				(Characters.Player.stats.Focus < 160) ? "Your focus rises greatly above everyone else, you have tunel vission when it comes to things that grab your attention, you'll see a single project all the wya throught to the end in a single sitting, and it is incredibly hard to pull you away from things you are focussed on. " :
				"Your focus is legendary, you are so focused that you are nearly dead to the world, anything that grabs your attention will likely never leave your attention unless it is no longer a viable point of intrest, and even still, it will be hard to pull you away even then. ";

				temp +=
				(Characters.Player.stats.Wisdom < 40) ? "Your mind is a blank slate, you ahve no wisdom to speek of. it's a wonder you can even speek with how little is up there. " :
				(Characters.Player.stats.Wisdom < 55) ? "Your wisdom falls greatly below average, you don't really know much of anything about the world around you. " :
				(Characters.Player.stats.Wisdom < 70) ? "Your wisdome falls far below average, you know very little about the world. " :
				(Characters.Player.stats.Wisdom < 85) ? "Your wisdom is lower than most, you know a little about the world. " :
				(Characters.Player.stats.Wisdom < 100) ? "Your wisdom is about average, maybe slightly lower than most. " :
				(Characters.Player.stats.Wisdom < 115) ? "Your wisdom is about average, maybe slightly higher than most. " :
				(Characters.Player.stats.Wisdom < 130) ? "Your wisdome is higher than most, you know quite a bit about the world. " :
				(Characters.Player.stats.Wisdom < 145) ? "Your wisdom rises far above average, you know minute details about all sourts of things. " :
				(Characters.Player.stats.Wisdom < 160) ? "Your wisdom risses greatly above your peers, you know many things about many things, identifying things will come easy to you. " :
				"Your wisdome is legendary, you know nearly everything about everything in the world, you'll be hard pressed to discover anything new. ";

				temp += "Are you happy with the way you've turned out?";

				PlayerResults.Text = temp;
			}),
			options = new List<Event.option>()
			{
				new Event.option()
				{
					name = "Yes",
					optionClick = (()=>
					{
						Areas.Starting.Load();
					})
				},
				new Event.option()
				{
					name = "No, re-roll",
					optionClick = (()=>
					{
						PlayerResults.Load();
					})
				}
			}
		};

		public static Event PlayerRace = new Event()
		{
			Text = "What is your race?",
			options = new List<Event.option>()
			{
				new Event.option()
				{
					name = "Human",
					optionClick = (() =>
					{
						Characters.Player.race = Races.Human;
						PlayerResults.Load();
					})
				},
				new Event.option()
				{
					name = "Wolf",
					optionClick = (() =>
					{
						Characters.Player.race = Races.Wolf;
						PlayerResults.Load();
					})
				}
			}
		};

		public static Event PlayerGender = new Event()
		{
			Text = "What is your gender?",
			options = new List<Event.option>()
			{
				new Event.option()
				{
					name = "Male",
					optionClick = (() =>
					{
						Characters.Player.gender = Genders.Male;
						PlayerRace.Load();
					})
				},
				new Event.option()
				{
					name = "Female",
					optionClick = (() =>
					{
						Characters.Player.gender = Genders.Female;
						PlayerRace.Load();
					})
				}
			}
		};
	}
}
