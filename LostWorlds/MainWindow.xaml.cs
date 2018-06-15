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

		/* Logic for the map:
		 * + load in image
		 * - on mouse down, move image the same ammount as the mouse moves
		 */

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
				Strength = 7.5,
				PainTolerance = -7.5,
				Flexibility = -7.5,
				FineMoter = 7.5,
				Analysis = -7.5,
				Reflex = 7.5,
				Intelegence = 0,
				Knowledge = 0,
				Focus = 0,
			}
		};

		public static Gender Female = new Gender()
		{
			mod = new Stats()
			{
				Strength = -7.5,
				PainTolerance = 7.5,
				Flexibility = 7.5,
				FineMoter = -7.5,
				Analysis = 7.5,
				Reflex = -7.5,
				Intelegence = 0,
				Knowledge = 0,
				Focus = 0,
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
			else if(stats.Damage < stats.PainTolerance - 60)
			{
				return DT.healthy;
			}
			else if(stats.Damage < stats.PainTolerance)
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
			/* Combat Logic:
			 * are you faster than the opponent? <- not handled here!
			 * do you know the enemie's weakness? [knowledge] <- figure out a way to handle this
			 *    if you do, then how accurately can you hit the weakness [fine moter]
			 * can you analyse the movements of your opponent? [analysis]
			 * can you hit your opponent? [fine motar]
			 *    will they dodge? [flexibility]
			 * how hard can you hit them? [strength]
			 *    how well can they take the hit? [pain tolerance]    
			 */
			
			var a = Utils.Gaussian(stats.Analysis, 15) - target.stats.Focus;
			var fm = Utils.Gaussian(stats.FineMoter, 15) - target.stats.Flexibility;
			var s = Utils.Gaussian(stats.Strength, 15) - target.stats.PainTolerance;

			var dmg = Math.Max(a + s + fm, 0);

			target.stats.Damage += dmg;

			target.isAlive = (Utils.Gaussian(target.stats.PainTolerance, 15) > target.stats.Damage);

			return (dmg > 0) ? AT.hit[Utils.rand.Next(AT.hit.Count())] : AT.miss[Utils.rand.Next(AT.miss.Count())];
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

	public static class MapInfo
	{
		public static Utils.Vec origin = new Utils.Vec(500, 500);
		public static Utils.Vec position = new Utils.Vec(0, 0);
		public static Utils.Vec chunkPos = new Utils.Vec(0, 0);
		public static bool doesDrag = false;

		public static double PixelTime = Time.Hour / 200;

		public static Utils.Vec OldMousPos;
		internal static Utils.Vec OldPos;

		public static void Update()
		{
			MainWindow.App.TopLeft.Source = new BitmapImage(new Uri(@"Map/chunk," + (chunkPos.X - 1) + "," + (chunkPos.Y - 1) + ".jpg", UriKind.Relative));
			MainWindow.App.Top.Source = new BitmapImage(new Uri(@"Map/chunk," + (chunkPos.X) + "," + (chunkPos.Y - 1) + ".jpg", UriKind.Relative));
			MainWindow.App.TopRight.Source = new BitmapImage(new Uri(@"Map/chunk," + (chunkPos.X + 1) + "," + (chunkPos.Y - 1) + ".jpg", UriKind.Relative));
			MainWindow.App.Left.Source = new BitmapImage(new Uri(@"Map/chunk," + (chunkPos.X - 1) + "," + (chunkPos.Y) + ".jpg", UriKind.Relative));
			MainWindow.App.Center.Source = new BitmapImage(new Uri(@"Map/chunk," + (chunkPos.X) + "," + (chunkPos.Y) + ".jpg", UriKind.Relative));
			MainWindow.App.Right.Source = new BitmapImage(new Uri(@"Map/chunk," + (chunkPos.X + 1) + "," + (chunkPos.Y) + ".jpg", UriKind.Relative));
			MainWindow.App.BottomLeft.Source = new BitmapImage(new Uri(@"Map/chunk," + (chunkPos.X - 1) + "," + (chunkPos.Y + 1) + ".jpg", UriKind.Relative));
			MainWindow.App.Bottom.Source = new BitmapImage(new Uri(@"Map/chunk," + (chunkPos.X) + "," + (chunkPos.Y + 1) + ".jpg", UriKind.Relative));
			MainWindow.App.BottomRight.Source = new BitmapImage(new Uri(@"Map/chunk," + (chunkPos.X + 1) + "," + (chunkPos.Y + 1) + ".jpg", UriKind.Relative));

			Console.WriteLine("Position: " + position.X + ", " + position.Y);
		}
	}

	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			App = this;
			Areas.Start.Load();
			Update();
			MapInfo.Update();
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

		private Utils.Vec oldDeltaPos;

		private void Map_MouseDown(object sender, MouseButtonEventArgs e)
		{
			oldDeltaPos = (Utils.Vec)e.GetPosition(Map);

			MapInfo.OldMousPos = (Utils.Vec)e.GetPosition(Map) - new Utils.Vec(100, 100);
			MapInfo.OldPos = MapInfo.position;
			MapInfo.doesDrag = true;

			Console.WriteLine("clicked!");
		}

		private void Map_MouseUp(object sender, MouseButtonEventArgs e)
		{
			MapInfo.doesDrag = false;
		}

		private void Map_MouseMove(object sender, MouseEventArgs e)
		{
			/* if you can drag, then drag
			 * when draging, capture the relative position of the mouse to the map image
			 * set the position of the map image to the position of the mouse 
			 */
			if(MapInfo.doesDrag)
			{
				MapInfo.position = MapInfo.OldPos + ((Utils.Vec)e.GetPosition(Map) - new Utils.Vec(100, 100) - MapInfo.OldMousPos);
				
				if(MapInfo.position.X < -200)
				{
					MapInfo.OldPos.X += 400;
					MapInfo.chunkPos.X += 1;
					MapInfo.Update();
				}
				if (MapInfo.position.X > 200)
				{
					MapInfo.OldPos.X -= 400;
					MapInfo.chunkPos.X -= 1;
					MapInfo.Update();
				}

				if (MapInfo.position.Y < -200)
				{
					MapInfo.OldPos.Y += 400;
					MapInfo.chunkPos.Y += 1;
					MapInfo.Update();
				}
				if (MapInfo.position.Y > 200)
				{
					MapInfo.OldPos.Y -= 400;
					MapInfo.chunkPos.Y -= 1;
					MapInfo.Update();
				}

				Canvas.SetLeft(Chunks, MapInfo.position.X - MapInfo.origin.X);
				Canvas.SetTop(Chunks, MapInfo.position.Y - MapInfo.origin.Y);

				Utils.Vec deltaPos = (Utils.Vec)e.GetPosition(Map) - oldDeltaPos;

				Time.Delta = (uint)(Math.Sqrt(deltaPos | deltaPos) * MapInfo.PixelTime);

				Update();

				oldDeltaPos = (Utils.Vec)e.GetPosition(Map);
			}
		}

		private void Map_MouseLeave(object sender, MouseEventArgs e)
		{
			MapInfo.doesDrag = false;
		}
	}
}
