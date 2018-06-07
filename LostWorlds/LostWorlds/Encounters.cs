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
		public Area location = null;

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
		}

		private void attackClicked(object sender, EventArgs e)
		{
			string temptext = "You lash out in an attack! ";
			double damage = Utils.gaussian(Characters.player.stats.attack, 15) - Utils.gaussian(enemy.stats.dodge, 15);

			if (damage > 0)
			{
				temptext += "You manage to land the hit squarely, hurting the foe! ";
				enemy.stats.damage += damage;
				if(Utils.gaussian(enemy.stats.endurance, 15) < enemy.stats.damage)
				{
					temptext += "The foe reals back and collapses to the ground, motionless.";
				}
				else
				{
					temptext += "The foe recoils in pain but lashes out at you!";

					double edamage = Utils.gaussian(enemy.stats.attack, 15) - Utils.gaussian(Characters.player.stats.dodge, 15);

					if (edamage > 0)
					{
						temptext += "They land a good hit, damaging you in the prossess!";

						Characters.player.stats.damage += edamage;

						if(Utils.gaussian(Characters.player.stats.endurance, 15) < Characters.player.stats.damage && Utils.gaussian(Characters.player.stats.endurance, 15) < Characters.player.stats.damage && Utils.gaussian(Characters.player.stats.endurance, 15) < Characters.player.stats.damage)
						{
							temptext += "The pain of the hit become far too much, and you pass out, waking up at your little camp site.";
						}
					}
				}
			}
		}

		public void load()
		{
			MainWindow.app.mainText.SelectAll();
			MainWindow.app.mainText.Selection.Text = text;
		}
	}

	public static class Encounters
	{

	}
}
