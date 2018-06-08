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
		public Enemy Enemy = new Enemy();
		public string Text;
		public Action Setup = null;
		public Action Attack = null;
		public Action Run = null;
		public Area Location = new Area();

		private void LoadActions()
		{
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

		private string EnemyAttack()
		{
			var temptext = "";

            //maybe check whether Enemy is null and handle that for a good measure?
			var edamage = Utils.Gaussian(Enemy.Stats.Attack, 15) - Characters.Player.Stats.Dodge;

			if (edamage > 0)
			{
				temptext += "They land a good hit, damaging you in the prossess! ";

				Characters.Player.Stats.Damage += edamage;

				if (Utils.Gaussian(Characters.Player.Stats.Endurance, 15) < Characters.Player.Stats.Damage && Utils.Gaussian(Characters.Player.Stats.Endurance, 15) < Characters.Player.Stats.Damage && Utils.Gaussian(Characters.Player.Stats.Endurance, 15) < Characters.Player.Stats.Damage)
				{
					temptext += "The pain of the hit become far too much, and you pass out, waking up at your little camp site. ";
					Finish(false);
				}
				else
				{
					temptext += "you recoil from the hit but are still able to fight. ";
				}
			}
			else if (Math.Abs(edamage) < double.Epsilon) //proper way of comparing because of possible imprecision inherent to floating point numbers
            {
				temptext += "the enemy mearly grazes you. ";
			}
			else
			{
				temptext += "The enemy misses you completely! ";
			}
			return temptext;
		}

		private void AttackClicked(object sender, EventArgs e)
		{
			var temptext = "You lash out in an attack! ";
		    //maybe check whether Enemy is null and handle that for a good measure?
            var damage = Utils.Gaussian(Characters.Player.Stats.Attack, 15) - Enemy.Stats.Dodge;

			if (damage > 0)
			{
				temptext += "You manage to land the hit squarely, hurting the foe! ";
				Enemy.Stats.Damage += damage;
				if(Utils.Gaussian(Enemy.Stats.Endurance, 15) < Enemy.Stats.Damage)
				{
					temptext += "The foe reals back and collapses to the ground, motionless. ";
					Finish(true);
				}
				else
				{
					temptext += "The foe recoils in pain but lashes out at you! ";
					temptext += EnemyAttack();	
				}
			}
			else if(Math.Abs(damage) < double.Epsilon)  //proper way of comparing because of possible imprecision inherent to floating point numbers
            {
				temptext += "You mearly graze the opponent, and they lash back at you!";
				temptext += EnemyAttack();
			}
			else
			{
				temptext += "You completely miss the opponent, and they use this opportunity to counter with an attack of their own!";
				temptext += EnemyAttack();
			}
			MainWindow.App.MainText.SelectAll();
			MainWindow.App.MainText.Selection.Text = temptext;
			Time.Delta = 6;
			MainWindow.App.Update();
		}

		private void RunClicked(object sender, EventArgs e)
		{
			Location.Encounter = null;
			Time.Delta = Time.Hour * 4;
			Location.Load();
		}

		private void Finish(bool survived)
		{
			MainWindow.App.Options.Children.Clear();
            //initialisers are cleaner
		    var finished = new Button
		    {
		        Content = "Continue",
		        Name = "finished",
		        Foreground = Brushes.White,
		        Background = Brushes.Black,
		        BorderBrush = Brushes.White
		    };
		    if(survived)
			{
				finished.Click += new RoutedEventHandler(SurvivedClicked);
			}
			else
			{
				finished.Click += new RoutedEventHandler(DiedClicked);
			}
			MainWindow.App.Options.Children.Add(finished);
			Grid.SetColumn(finished, 0);
			Grid.SetRow(finished, 0);
			Grid.SetColumnSpan(finished, 2);

			Enemy.Stats.Damage = 0;
			Location.Encounter = null;
		}

        //does not access any dynamic data, might as well be static
		private static void DiedClicked(object sender, EventArgs e)
		{
			Areas.Home.Load();
		}

		private void SurvivedClicked(object sender, EventArgs e)
		{
			Location?.Load(); // ?. checks whether left operand is null and if it is it stops the operation (null conditional)
		}

		public void Load()
		{
			MainWindow.App.Options.Children.Clear();
			LoadActions();
			Location = Areas.Curr;
			MainWindow.App.MainText.SelectAll();
			MainWindow.App.MainText.Selection.Text = Text;
		}
	}

	public static class Encounters
	{
		public static Encounter Test = new Encounter()
		{
			Enemy = Enemies.Test,
			Text = "You've encountered a.... thing, it's a testing thing, don't read too far into it, but what do you do?",
		};
	}
}
