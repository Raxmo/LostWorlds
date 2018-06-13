using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

/*
 * TODO:
 * Encounters seem fine for now, might work on it later on, but shouldn't need anything else for the time being
 */

namespace LostWorlds
{
	public class Encounter
	{
		public Enemy enemy = new Enemy();
		public string Text;
		public Action Setup = null;
		public Action Attack = null;
		public Action Run = null;
		public Area Location = new Area();

		private void LoadActions()
		{
			MainWindow.App.Options.Children.Clear();

			var attack = new Button();
			var run = new Button();

			attack.Content = "Attack";
			attack.Name = "attack";
			attack.Foreground = Brushes.White;
			attack.Background = Brushes.Black;
			attack.BorderBrush = Brushes.White;
			attack.Click += new RoutedEventHandler(AttackClicked);

			run.Content = "Run";
			run.Name = "run";
			run.Foreground = Brushes.White;
			run.Background = Brushes.Black;
			run.BorderBrush = Brushes.White;
			run.Click += new RoutedEventHandler(RunClicked);

			MainWindow.App.Options.Children.Add(attack);
			Grid.SetColumn(attack, 0);
			Grid.SetRow(attack, 0);

			MainWindow.App.Options.Children.Add(run);
			Grid.SetColumn(run, 1);
			Grid.SetRow(run, 0);
		}

		private void LoadContinue()
		{
			MainWindow.App.Options.Children.Clear();

			var Continue = new Button();

			Continue.Content = "Continue";
			Continue.Name = "ContinueB";
			Continue.Foreground = Brushes.White;
			Continue.Background = Brushes.Black;
			Continue.BorderBrush = Brushes.White;
			Continue.Click += new RoutedEventHandler(ContinueClicked);

			MainWindow.App.Options.Children.Add(Continue);
			Grid.SetColumn(Continue, 0);
			Grid.SetRow(Continue, 0);
			Grid.SetColumnSpan(Continue, 2);
		}
		
		private void AttackClicked(object sender, EventArgs e)
		{
			MainWindow.App.MainText.SelectAll();
			MainWindow.App.MainText.Selection.Text = Characters.Player.Attack(enemy) + (enemy.isAlive ? enemy.HealthState() + enemy.Attack(Characters.Player) : enemy.AT.death[Utils.rand.Next(enemy.AT.death.Count)]) + Characters.Player.HealthState();

			// not sure if ther is a more elegant way of doing this, if there is, would be most appreciated if you could make it a thing ^-^, also, I wish there was a nand opperator T-T I don't think it would have been that hard to put in !& as an opperator to be honest...
			if (! (enemy.isAlive && Characters.Player.isAlive)) 
			{
				LoadContinue();
			}
		}

		private void RunClicked(object sender, EventArgs e)
		{
			Location?.Load();
		}

		private void ContinueClicked(object sender, EventArgs e)
		{
			if(!Characters.Player.isAlive)
			{
				Areas.Home.Load();
			}
			else
			{

				Location?.Load();
			}
		}

		public void Load()
		{
			LoadActions();
			Location = Areas.Curr;
			MainWindow.App.MainText.SelectAll();
			MainWindow.App.MainText.Selection.Text = enemy.init + " " + Characters.Player.HealthState();
		}
	}

	public static class Encounters
	{
		public class tester : Encounter
		{
			public tester()
			{
				enemy = new Enemies.test();
			}
		}

		public static Encounter Test = new Encounter()
		{
			enemy = new Enemies.test(),
		};
	}
}
