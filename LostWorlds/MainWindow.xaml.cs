using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Dynamic;

/*
 * TODO:
 * This is one mess of code, but everything is fine the way it is for now. might re-work it a little later.
 * Might end up adding in a physical map, will think about how to impliment it and decide on that later.
 */

namespace LostWorlds
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 

	public class Gender
	{
		public Stats mod = new Stats();
	}
	public static class Genders
	{
		/*
		 * men build more muscle mass, and have faster knee-jerk reactions
		 * women have higher pain tolerences and stay more cognitive than men, and also tend to be more dexterouse than men
		 * 
		 * this will be very difficult to properly model... and I'll be working on this for quite some time.
		 */

		public static Gender Male = new Gender()
		{
			mod = new Stats()
			{
				Strength = 15,
				Constitution = 0,
				Dextarity = 0,
				Intelegence = 0,
				Focus = -15,
				Wisdom = 0,
			}
		};

		public static Gender Female = new Gender()
		{
			mod = new Stats()
			{
				Strength = -15,
				Constitution = 15,
				Dextarity = 0,
				Intelegence = 0,
				Focus = 0,
				Wisdom = 0,
			}
		};
	}

	public class Entity
	{
		public Stats stats = new Stats();
		public bool isAlive = true;
		public string name = "";
		public string description = "";
		public attackText AT = new attackText();
		public damageText DT = new damageText();
		public string init = "";

		public double knowlegeRateing = 100;
		public bool canLevel = false;
		public Gender gender = new Gender();

		public struct damageText
		{
			public string unhurt; // damage = 0
			public string healthy; // when damage is < constitution - 60
			public string damaged; // when damage is < constitution
			public string critical; // when damage is >= constitution
		}

		public struct attackText
		{
			public List<string> attacking;
			public List<string> hit;
			public List<string> miss;
			public List<string> death;
		}

		public string HealthState()
		{
			if(stats.Damage == 0)
			{
				return DT.unhurt;
			}
			else if(stats.Damage < stats.Constitution - 60)
			{
				return DT.healthy;
			}
			else if(stats.Damage < stats.Constitution)
			{
				return DT.damaged;
			}
			else
			{
				return DT.critical;
			}
		}
		
		public string Attack(Entity target)
		{
			var wdamage = Math.Max(Utils.Gaussian(stats.Wisdom, 15) - target.knowlegeRateing, 0);
			stats.Wisdom += (wdamage == 0 && canLevel) ? Utils.Gaussian(stats.Intelegence, 15) / 100 : 0;
			// TODO: allow the target enemy's knowlegeLevel be mutated at this point in some way or another. 
			// Goal is to have a "target.GetClass().knowlegeLevel -= [INSERT LOGIC HERE];" or some such thing.

			var fdamage = Math.Max(Utils.Gaussian(stats.Focus, 15) - (target.stats.Dextarity - wdamage), 0);

			var sdamage = Math.Max(Utils.Gaussian(stats.Strength, 15) - (target.stats.Constitution - wdamage), 0);
			stats.Strength += Math.Max((Utils.Gaussian(100, 15) - stats.Strength) / 100, 0);

			target.stats.Constitution += (target.canLevel) ? Math.Max((Utils.Gaussian(100, 15) - stats.Constitution) / 100, 0) : 0;

			var tdamage = (fdamage > 0) ? fdamage : 0;
			
			target.stats.Damage += tdamage;
			target.isAlive = Utils.Gaussian(target.stats.Damage, 15) < target.stats.Constitution;
			/*
			 * When attatcking, are you smart enough to find the week point?
			 * are you focused enough to hit the target?
			 * are you strong enough to hurt the target?
			 */

			return (tdamage > 0) ? AT.attacking[Utils.rand.Next(AT.attacking.Count)] + AT.hit[Utils.rand.Next(AT.hit.Count)]
				: AT.attacking[Utils.rand.Next(AT.attacking.Count)] + AT.miss[Utils.rand.Next(AT.miss.Count)];
		}		
	}

	public static class Time
	{
		public static uint T = 0;
		public static uint Day = 86400;
		public static uint Hour = 3600;
		public static uint Minute = 60;
		public static uint Delta = 0;
	}

	public static class Sun
	{
		public static Utils.Quat Rpos = new Utils.Quat(0, 0, 0, 1);
		public static double Hpd = 24; //hours per day
		public static double Dpy = 364; //days per year
		public static double Tilt = Math.PI * 20 / 180; //axial tilt of the planet
		public static double TiltFase = 0; //this is the offset of the tilt relative to the orbital position
		public static double Latitude = Math.PI * 45 / 180; //the current latitude of the viewer
		public static double Eccentricity = 0.5; //TODO: actually impliment this. No idea quite how to impliment the varying day-length imposed by eccentricity of the orbit. will likely have to use integrals for this, which will be not fun.


		public static Utils.Vec Origin = new Utils.Vec(100, 100);
		public static Utils.Vec Pos = new Utils.Vec(0,0);
		public static double Radius = 10;
		public static Ellipse Circle = new Ellipse();
		public static Ellipse Horizon = new Ellipse()
		{
			Width = 200,
			Height = 200,
			Stroke = Brushes.White,
			StrokeThickness = 3
		};

		public static void Draw()
		{
			var dvel = Math.PI * 2 / (Hpd * Time.Hour);
			var yvel = Math.PI * 2 / (Dpy * Hpd * Time.Hour);

			var insvel = yvel / (1 - Eccentricity * Math.Cos(yvel * Time.T)); //Deppricated code, this doesn't work the way I need it to. Eccentricity will likely need to be passed through an integral in order to get the proper function to shift the sun's position correctly due to eccentricity

			var dpos = dvel * Time.T;
			var ypos = yvel * Time.T;

			var ntilt = Utils.Quat.Rot(Tilt, new Utils.Vec3(1, 0, 0)); //stores a rotation bassed off of the axial tilt
			ntilt = ntilt ^ Utils.Quat.Rot(ypos, new Utils.Vec3(0, 1, 0)); //rotates the rotation bassed off of the orbital position

			var nrpos = Rpos ^ ntilt; //offsets the position given the current latitude

			var npos = nrpos ^ Utils.Quat.Rot(dpos, new Utils.Vec3(0, -1, 0)) ^ Utils.Quat.Rot(Latitude, (Utils.Vec3)(Rpos) * new Utils.Vec3(0, 1, 0)); //Rotates the sun around proper axis throught the day

			var v3 = (Utils.Vec3)npos; //casting the quaterneon to a vector
			var p3 = (Utils.Pol3)v3;//casting the vector to a polar 3
			var p = (Utils.Pol)p3; //this colapses the polar3 down to a polar2 to display current heading and altitude

			Pos = !p * (200 / Math.PI); //make the sun draw at the correct place on the canvas

			Circle.Width = Radius * 2;
			Circle.Height = Radius * 2;

            //ternary would simplify it to Circle.Stroke = (Pos | Pos) > Math.Pow(100 + Radius, 2) ? Brushes.Transparent : Brushes.White;
            if ((Pos | Pos) > Math.Pow(100 + Radius, 2)) //only display the sun if it is actually above the horizon
			{
				Circle.Stroke = Brushes.Transparent;
				Circle.Fill = Brushes.Transparent;
				Horizon.Fill = new SolidColorBrush(Color.FromRgb(20, 20, 30));
			}
			else
			{
				Circle.Stroke = Brushes.White;
				Circle.Fill = Brushes.White;
				Horizon.Fill = new SolidColorBrush(Color.FromRgb(40, 40, 100));
			}
			Circle.StrokeThickness = 3;
			Circle.SetValue(Canvas.LeftProperty, (Pos + Origin).X - Radius);
			Circle.SetValue(Canvas.TopProperty, (Pos + Origin).Y - Radius);
		}

		
	}

	public static class Larder
	{
		public static uint Volume = 0;
		public static uint Density = 0;
		public static uint Water = 0;

		public static void Add(Food item, uint volume)
		{
			double energy = Density * Larder.Volume;
			double wat = Water * Larder.Volume;

			Larder.Volume += volume;
			energy += item.Energy * volume;
			wat += item.Water * volume;

			Density =(uint)(energy / Larder.Volume);
			Water = (uint)(wat / Larder.Volume);
		}
	}

	public static class Home
	{

	}

	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			App = this;
			Areas.Start.Load();
			Update();
		}

		public static MainWindow App;

		public void Update()
		{
			Time.T += Time.Delta;

			var numDays = ((Time.T + Time.Hour * 12) / Time.Day) % 364;
			var numYears = (Time.T / Time.Day) / 364;

			Days.Content = "Days: " + numDays;
			Years.Content = "Years: " + numYears;

			Characters.Update();

			PlayerHunger.Foreground = Characters.Player.HungerColor();
			PlayerThirst.Foreground = Characters.Player.ThirstColor();
			PartnerHunger.Foreground = Characters.Partner.HungerColor();
			PartnerThirst.Foreground = Characters.Partner.ThirstColor();
			Tracker.Children.Clear();
			Tracker.Children.Add(Sun.Horizon);
			Sun.Draw();
			Tracker.Children.Add(Sun.Circle);
			
			Time.Delta = 0;
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
            //.Any() would do it without comparison <- how do you mean? not very familiar with C# to be frank, feel free to redo this code so I can see how to use that, seems a lot cleaner to me
			if (Areas.Back.Count() > 0)
			{
				Time.Delta = Areas.Curr.TravelTime;
				Areas.Back[Areas.Back.Count() - 1].Load();
				Areas.Back.RemoveAt(Areas.Back.Count() - 1);
			}
			Update();
		}

		private void home_Click(object sender, RoutedEventArgs e)
		{
			Time.Delta = Areas.Curr.ReturnTime;
			Areas.Back.Clear();
			Areas.Home.Load();
			Update();
		}
	}
}
