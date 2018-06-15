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

				/* set up for stat brackets
				 * 
				 * < 40 : legendarily low
				 * < 55 : incredibly low
				 * < 70 : far lower
				 * < 85 : lower
				 * < 115 : average
				 * < 130 : higher
				 * < 145 : far higher
				 * < 160 : incredibly high
				 * else : legendarily high
				 * 
				temp +=
				(Characters.Player.stats.Strength < 40) ? " ":
				(Characters.Player.stats.Strength < 55) ? " ":
				(Characters.Player.stats.Strength < 70) ? " ":
				(Characters.Player.stats.Strength < 85) ? " ":
				(Characters.Player.stats.Strength < 115) ? " ":
				(Characters.Player.stats.Strength < 130) ? " ":
				(Characters.Player.stats.Strength < 145) ? " ":
				(Characters.Player.stats.Strength < 160) ? " ":
				" ";
				 */

				string temp = "You look over yourself and asses your abilities. ";

				temp +=
				(Characters.Player.stats.Strength < 40) ? "Your lack of strength is a thing of legends, you have none to speek of. You probably have a form of Mucular Distrophy, you won't be doing a whole lot. " :
				(Characters.Player.stats.Strength < 55) ? "You have incredibly low strength, you can't even pull yourself up onto a ledge, your punches are tickles. " :
				(Characters.Player.stats.Strength < 70) ? "you have far lower strength than most, you can still pull yourself up, but not much else. You won't be hurting anything any time soon, that's for sure. " :
				(Characters.Player.stats.Strength < 85) ? "You have a lower strength than most, even when you flex, you can still easily squeeze your muscles, it'll be hard for you to carry much of anything. You should fight smarter, not harder. " :
				(Characters.Player.stats.Strength < 115) ? "Your strength is average, nothing special here. " :
				(Characters.Player.stats.Strength < 130) ? "You have above average strength, you can carry things easier than most, and hop up ledges with ease. " :
				(Characters.Player.stats.Strength < 145) ? "You have far higher strength than the average person, your punches hurt, and you can carry much heavier loads than most. " :
				(Characters.Player.stats.Strength < 160) ? "You have incredibly high strength, moving boulders, carying others comes easy to you. " :
				"your strength is a thing of legend, anyone who pisses you off will not be conciouse for long, if they are lucky enough to be alive. ";

				temp +=
				(Characters.Player.stats.PainTolerance < 40) ? "You have no tolerance for pain, even stubbing your toe could knock you out. It is truely legendary how little you can tolerate pain. " :
				(Characters.Player.stats.PainTolerance < 55) ? "Your pain tolerance is incredibly low, a single punch could take you down, you should be careful out there. " :
				(Characters.Player.stats.PainTolerance < 70) ? "Your tolerance for pain is far lower than most, you'll go down easily, so watch your step. " :
				(Characters.Player.stats.PainTolerance < 85) ? "Your pain tolerance is lower than most, it's not detrimental, but you should still be a little more careful. " :
				(Characters.Player.stats.PainTolerance < 115) ? "You have an average pain tolerance. " :
				(Characters.Player.stats.PainTolerance < 130) ? "Your pain tolerance is higher than most, it'll take a little more to knock you out. " :
				(Characters.Player.stats.PainTolerance < 145) ? "Your pain tolerance is far higher than most, you'll shrug off damage like it's a flick in many casses. " :
				(Characters.Player.stats.PainTolerance < 160) ? "Your pain tolerance is incredibly high, it'll take quite the event to take you down. " :
				"You have a legendary pain tolerance, you'll laugh off even massive hits. ";

				temp +=
				(Characters.Player.stats.Flexibility < 40) ? "Your lack of flexibility is legendary. A literal wooden board is more limber than you are. You won't be dodging anything any time soon. " :
				(Characters.Player.stats.Flexibility < 55) ? "Your flexibility is incredibly lower than most. you can just about turn your head maybe 5 degrees, and not much else. Get used to pain, enemies will hit you often. " :
				(Characters.Player.stats.Flexibility < 70) ? "You are far less flexible than most, you can turn your head, but not much else. You'll be easier to hit. " :
				(Characters.Player.stats.Flexibility < 85) ? "You are less flexible than most, your movements aren't that great, so don't go trying to do any parkour, and don't get frustrated if you get hit a lot. " :
				(Characters.Player.stats.Flexibility < 115) ? "You have average flexibility, nothing to brag about, but nothing to be sad about. " :
				(Characters.Player.stats.Flexibility < 130) ? "Your flexibility is higher than most, you are a slippery one, but don't let it get to your head. " :
				(Characters.Player.stats.Flexibility < 145) ? "You have far higher flexibility, you are quite slippery and hard to hit. " :
				(Characters.Player.stats.Flexibility < 160) ? "You have incredible flexibility, near contortionist status in fact. It'll be hard for you to get stuck in small places or for someone to hold onto you for any amount of time. " :
				"You have legendary flexibility, nearly impossible to hit, won't get trapped, you are seriously a thing of legend, contortionists don't even have a dream of being as flexible as you. You could literally kiss your own ass if you so pleased. ";

				temp +=
				(Characters.Player.stats.FineMoter < 40) ? "Your fine moter skills are non-existant. You really can't manipulate small object at all, and can't exactly do anything that involves precision. " :
				(Characters.Player.stats.FineMoter < 55) ? "Your fine moter skills are incredibly low, you have a hard time holding small things and manipulating them. " :
				(Characters.Player.stats.FineMoter < 70) ? "Your fine moter skills are far less than average, it's dificult to hold onto things, and to manipulate them. " :
				(Characters.Player.stats.FineMoter < 85) ? "You have lower fine moter skills than most, you have a harder time doing precission work than your peers. " :
				(Characters.Player.stats.FineMoter < 115) ? "You have average fine moter skills. " :
				(Characters.Player.stats.FineMoter < 130) ? "Your fine moter skills are higher than most. " :
				(Characters.Player.stats.FineMoter < 145) ? "You have far higher fine moter skills than average. " :
				(Characters.Player.stats.FineMoter < 160) ? "You have incredible fine moter skills. " :
				"Your fine moter skills are legendary ";

				temp +=
				(Characters.Player.stats.Analysis < 40) ? "Your lack of analytical skills is legendary. " :
				(Characters.Player.stats.Analysis < 55) ? "You have incredibly low analytical skills. " :
				(Characters.Player.stats.Analysis < 70) ? "You have far lower analytical skills. " :
				(Characters.Player.stats.Analysis < 85) ? "You have lower than average analytical skills. " :
				(Characters.Player.stats.Analysis < 115) ? "You have average analytical skills. " :
				(Characters.Player.stats.Analysis < 130) ? "You have higher than average analytical skills. " :
				(Characters.Player.stats.Analysis < 145) ? "You have far higher analytical skills. " :
				(Characters.Player.stats.Analysis < 160) ? "You have incredible analytical skills. " :
				"Your analytical skills are legendary. ";

				temp +=
				(Characters.Player.stats.Reflex < 40) ? "Your lack of reflexes is legendary. " :
				(Characters.Player.stats.Reflex < 55) ? "You have incredibly low reflexes. " :
				(Characters.Player.stats.Reflex < 70) ? "You have far lower reflexes. " :
				(Characters.Player.stats.Reflex < 85) ? "You have lower than average reflexes. " :
				(Characters.Player.stats.Reflex < 115) ? "You have average reflexes. " :
				(Characters.Player.stats.Reflex < 130) ? "You have higher than average reflexes. " :
				(Characters.Player.stats.Reflex < 145) ? "You have far higher reflexes. " :
				(Characters.Player.stats.Reflex < 160) ? "You have incredible reflexes. " :
				"Your reflexes are legendary. ";

				temp +=
				(Characters.Player.stats.Intelegence < 40) ? "Your lack of intelegence is legendary. " :
				(Characters.Player.stats.Intelegence < 55) ? "You have incredibly low intelegence. " :
				(Characters.Player.stats.Intelegence < 70) ? "You have far lower intelegence. " :
				(Characters.Player.stats.Intelegence < 85) ? "You have lower than average intelegence. " :
				(Characters.Player.stats.Intelegence < 115) ? "You have average intelegence. " :
				(Characters.Player.stats.Intelegence < 130) ? "You have higher than average intelegence. " :
				(Characters.Player.stats.Intelegence < 145) ? "You have far higher intelegence. " :
				(Characters.Player.stats.Intelegence < 160) ? "You have incredible intelegence. " :
				"Your intelegence is legendary. ";

				temp +=
				(Characters.Player.stats.Knowledge < 40) ? "Your lack of knowledge is legendary. " :
				(Characters.Player.stats.Knowledge < 55) ? "You have incredibly low knowledge. " :
				(Characters.Player.stats.Knowledge < 70) ? "You have far lower knowledge. " :
				(Characters.Player.stats.Knowledge < 85) ? "You have lower than average knowledge. " :
				(Characters.Player.stats.Knowledge < 115) ? "You have average knowledge. " :
				(Characters.Player.stats.Knowledge < 130) ? "You have higher than average knowledge. " :
				(Characters.Player.stats.Knowledge < 145) ? "You have far higher knowledge. " :
				(Characters.Player.stats.Knowledge < 160) ? "You have incredible knowledge. " :
				"Your knowledge is legendary. ";

				temp +=
				(Characters.Player.stats.Focus < 40) ? "Your lack of focus is legendary. " :
				(Characters.Player.stats.Focus < 55) ? "You have incredibly low focus. " :
				(Characters.Player.stats.Focus < 70) ? "You have far lower focus. " :
				(Characters.Player.stats.Focus < 85) ? "You have lower than average focus. " :
				(Characters.Player.stats.Focus < 115) ? "You have average focus. " :
				(Characters.Player.stats.Focus < 130) ? "You have higher than average focus. " :
				(Characters.Player.stats.Focus < 145) ? "You have far higher focus. " :
				(Characters.Player.stats.Focus < 160) ? "You have incredible focus. " :
				"Your focus is legendary. ";


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
