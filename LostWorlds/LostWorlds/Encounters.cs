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
	public class Encounter
	{
		public Enemy enemy = new Enemy();
		public string text;
		public Action setup = null;
		public Action attack = null;
		public Action run = null;
		public Area location = new Area();

		private void loadActions()
		{
			Button attack = new Button();
			Button run = new Button();

			attack.Content = "Attack";
			attack.Name = "attack";
			attack.Foreground = Brushes.White;
			attack.Background = Brushes.Black;
			attack.BorderBrush = Brushes.White;
			attack.Click += new RoutedEventHandler(attackClicked);

			run.Content = "Run";
			run.Name = "run";
			run.Foreground = Brushes.White;
			run.Background = Brushes.Black;
			run.BorderBrush = Brushes.White;
			run.Click += new RoutedEventHandler(runClicked);

			MainWindow.app.options.Children.Add(attack);
			Grid.SetColumn(attack, 0);
			Grid.SetRow(attack, 0);

			MainWindow.app.options.Children.Add(run);
			Grid.SetColumn(run, 1);
			Grid.SetRow(run, 0);
		}

		private string enemyAttack()
		{
			string temptext = "";

			double edamage = Utils.gaussian(enemy.stats.attack, 15) - Characters.player.stats.dodge;

			if (edamage > 0)
			{
				temptext += "They land a good hit, damaging you in the prossess! ";

				Characters.player.stats.damage += edamage;

				if (Utils.gaussian(Characters.player.stats.endurance, 15) < Characters.player.stats.damage && Utils.gaussian(Characters.player.stats.endurance, 15) < Characters.player.stats.damage && Utils.gaussian(Characters.player.stats.endurance, 15) < Characters.player.stats.damage)
				{
					temptext += "The pain of the hit become far too much, and you pass out, waking up at your little camp site. ";
					finish(false);
				}
				else
				{
					temptext += "you recoil from the hit but are still able to fight. ";
				}
			}
			else if (edamage == 0)
			{
				temptext += "the enemy mearly grazes you. ";
			}
			else
			{
				temptext += "The enemy misses you completely! ";
			}
			return temptext;
		}

		private void attackClicked(object sender, EventArgs e)
		{
			string temptext = "You lash out in an attack! ";
			double damage = Utils.gaussian(Characters.player.stats.attack, 15) - enemy.stats.dodge;

			if (damage > 0)
			{
				temptext += "You manage to land the hit squarely, hurting the foe! ";
				enemy.stats.damage += damage;
				if(Utils.gaussian(enemy.stats.endurance, 15) < enemy.stats.damage)
				{
					temptext += "The foe reals back and collapses to the ground, motionless. ";
					finish(true);
				}
				else
				{
					temptext += "The foe recoils in pain but lashes out at you! ";
					temptext += enemyAttack();	
				}
			}
			else if(damage == 0)
			{
				temptext += "You mearly graze the opponent, and they lash back at you!";
				temptext += enemyAttack();
			}
			else
			{
				temptext += "You completely miss the opponent, and they use this opportunity to counter with an attack of their own!";
				temptext += enemyAttack();
			}
			MainWindow.app.mainText.SelectAll();
			MainWindow.app.mainText.Selection.Text = temptext;
			Time.delta = 6;
			MainWindow.app.update();
		}

		private void runClicked(object sender, EventArgs e)
		{
			location.encounter = null;
			Time.delta = Time.hour * 4;
			location.load();
		}

		private void finish(bool survived)
		{
			MainWindow.app.options.Children.Clear();
			Button finished = new Button();
			finished.Content = "Continue";
			finished.Name = "finished";
			finished.Foreground = Brushes.White;
			finished.Background = Brushes.Black;
			finished.BorderBrush = Brushes.White;
			if(survived)
			{
				finished.Click += new RoutedEventHandler(survivedClicked);
			}
			else
			{
				finished.Click += new RoutedEventHandler(diedClicked);
			}
			MainWindow.app.options.Children.Add(finished);
			Grid.SetColumn(finished, 0);
			Grid.SetRow(finished, 0);
			Grid.SetColumnSpan(finished, 2);

			enemy.stats.damage = 0;
			location.encounter = null;
		}

		private void diedClicked(object sender, EventArgs e)
		{
			Areas.home.load();
		}

		private void survivedClicked(object sender, EventArgs e)
		{
			location.load();
		}

		public void load()
		{
			MainWindow.app.options.Children.Clear();
			loadActions();
			location = Areas.curr;
			MainWindow.app.mainText.SelectAll();
			MainWindow.app.mainText.Selection.Text = text;
		}
	}

	public static class Encounters
	{
		public static Encounter test = new Encounter()
		{
			enemy = Enemies.test,
			text = "You've encountered a.... thing, it's a testing thing, don't read too far into it, but what do you do?",
		};
	}
}
