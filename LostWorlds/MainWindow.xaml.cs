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
using Utils;
using MapFile;

/*
 * TODO:
 * This is one mess of code, but everything is fine the way it is for now. might re-work it a little later.
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
			Stroke = System.Windows.Media.Brushes.White,
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
				Circle.Stroke = System.Windows.Media.Brushes.Transparent;
				Circle.Fill = System.Windows.Media.Brushes.Transparent;
				Horizon.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(20, 20, 30));
			}
			else
			{
				Circle.Stroke = System.Windows.Media.Brushes.White;
				Circle.Fill = System.Windows.Media.Brushes.White;
				Horizon.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(40, 40, 100));
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

	public struct ChunkInfo
	{

		public static System.Drawing.Color[] biomeColor = new System.Drawing.Color[] 
												{System.Drawing.Color.Firebrick,
												 System.Drawing.Color.Gray,
												 System.Drawing.Color.Olive,
												 System.Drawing.Color.DodgerBlue,
												 System.Drawing.Color.ForestGreen,
												 System.Drawing.Color.AntiqueWhite,
												 System.Drawing.Color.Yellow,
												 System.Drawing.Color.GreenYellow};
		
		public byte[,] biomes;

		public BitmapSource Bmp(Bitmap gen)
		{
			BitmapSource output;
			
			for (int x = 0; x < gen.Width; x++)
			{
				for (int y = 0; y < gen.Height; y++)
				{
					gen.SetPixel(x, 255 - y, biomeColor[biomes[x, y]]);
				}
			}

			output =
			System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
			gen.GetHbitmap(),
			IntPtr.Zero,
			Int32Rect.Empty,
			BitmapSizeOptions.FromWidthAndHeight(256, 256));
			return output;
		}

		public ChunkInfo(GlobalData g, uint gx, uint gy)
		{
			biomes = new byte[256, 256];

			/* New voronoi steps
			 * 
			 * - check to see if voronoi is needed
			 * - check to see what voronoi quadrent the target point is in
			 *   - check to see if voronoi is needed for target quadrent
			 * 
			 * What We Know:
			 * - voronoi verteces are at the circomcenters of the vertecies of a Delaunay Triangulation
			 *   - check matrix =	{{ a.x, a.y, a|a, 1 },
			 *						 { b.x, b.y, b|b, 1 },
			 *						 { c.x, c.y, c|c, 1 },
			 *						 { d.x, d.y, d|d, 1 }}
			 *						
			 *   - with clockwise storage, the determanent will be negetive if the fourth point is invalid
			 * - we can create 4 quadrents of regions per voronoi cell by comparing against neighboring cells
			 * - each quadrent will have 4 quarter voronoi cells
			 * - each quarter cell will have either 4 or 5 verteces
			 * - each cell will have as it's verteces, it's center, two of it's midpoints to the adgacent centers, and either both or one of the circomcenters
			 * 
			 */
			var nbiomes = new byte[3, 3];
			var ncenters = new Vec[3, 3];


			// initialize the two arrays
			for(uint x = 0; x < 3; x++)
			{
				for (uint y = 0; y < 3; y++)
				{
					var cent = g.GetChunk(gx + x - 1, gy + y - 1).BiomeCenter;
					cent = cent.X * (Consts.Circ ^ (new Vec(0, cent.Y) / 64));
					cent += new Vec((double)x - 1, (double)y - 1) * 256;

					ncenters[x, y] = cent;
					nbiomes[x, y] = g.GetChunk(gx - 1 + x, gy - 1 + y).BiomeID;
				}
			}

			// Testing brute force
			for (int x = 0; x < 256; x++)
			{
				for (int y = 0; y < 256; y++)
				{
					double dist = int.MaxValue;
					byte sbiome = nbiomes[1, 1];

					for (int i = 0; i < 3; i++)
					{
						for (int j = 0; j < 3; j++)
						{
							var sample = new Vec(x, y);
							var sdist = sample | ncenters[i, j];

							sbiome = (sdist < dist) ? nbiomes[i, j] : sbiome;
							dist = Math.Min(dist, sdist);
						}
					}

					biomes[x, y] = sbiome;
				}
			}



		}
	}

	public static class MapInfo
	{
		/* Steps for the map stuffs
		 * 
		 * - map current location to world map
		 * - generate the 4 relavent chunks depending on current quadrent of current chunk
		 * - generate all 9 blocks from current chunks location
		 *   - MUST have some texture to ground the player with their suroundings
		 *   - Likely some sourt of anti-aliassing situation, or bluring system with a random gradiant, or a random gradiant at first
		 */
		 
		public static Vec chunkPos = new Vec(5, 3);
		public static Vec position = new Vec();
		public static Vec OldPos = new Vec();
		public static Vec OldMousPos = new Vec();
		public static Vec origin = new Vec(284, 284);
		public static double PixelTime = Time.Hour / 500;
		public static bool DoesDrag = false;

		public static GlobalData GlobalMap = new GlobalData("../../Map/world.gbd");

		public static void Update()
		{
			var chunks = new BitmapSource[3, 3];
			using (Bitmap t = new Bitmap(256, 256))
			{ 
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						var b = new ChunkInfo(GlobalMap, (uint)(chunkPos.X + i), (uint)(chunkPos.Y + j));
						chunks[i + 1, j + 1] = b.Bmp(t);
						t.Save("test" + (i + chunkPos.X) + "," + (j + chunkPos.Y) + ".bmp");
					}
				}
			}
			
			MainWindow.App.TopLeft.Source = chunks[0, 0];
			MainWindow.App.Top.Source = chunks[1, 0];
			MainWindow.App.TopRight.Source = chunks[2, 0];

			MainWindow.App.Left.Source = chunks[0, 1];
			MainWindow.App.Center.Source = chunks[1, 1];
			MainWindow.App.Right.Source = chunks[2, 1];

			MainWindow.App.BottomLeft.Source = chunks[0, 2];
			MainWindow.App.Bottom.Source = chunks[1, 2];
			MainWindow.App.BottomRight.Source = chunks[2, 2];
			
			Console.WriteLine(chunkPos.X + ", " + chunkPos.Y);
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

			MapInfo.OldMousPos = (Vec)e.GetPosition(Map) - new Vec(100, 100);
			MapInfo.OldPos = MapInfo.position;
			MapInfo.DoesDrag = true;

			Console.WriteLine("clicked!");
		}

		private void Map_MouseUp(object sender, MouseButtonEventArgs e)
		{
			MapInfo.DoesDrag = false;
		}

		private void Map_MouseMove(object sender, MouseEventArgs e)
		{
			/* if you can drag, then drag
			 * when draging, capture the relative position of the mouse to the map image
			 * set the position of the map image to the position of the mouse 
			 */
			if(MapInfo.DoesDrag)
			{
				MapInfo.position = MapInfo.OldPos + ((Vec)e.GetPosition(Map) - new Vec(100, 100) - MapInfo.OldMousPos);
				
				if(MapInfo.position.X < -128)
				{
					MapInfo.OldPos.X += 256;
					MapInfo.chunkPos.X += 1;
					MapInfo.Update();
				}
				if (MapInfo.position.X > 128)
				{
					MapInfo.OldPos.X -= 256;
					MapInfo.chunkPos.X -= 1;
					MapInfo.Update();
				}

				if (MapInfo.position.Y < -128)
				{
					MapInfo.OldPos.Y += 256;
					MapInfo.chunkPos.Y += 1;
					MapInfo.Update();
				}
				if (MapInfo.position.Y > 128)
				{
					MapInfo.OldPos.Y -= 256;
					MapInfo.chunkPos.Y -= 1;
					MapInfo.Update();
				}

				Canvas.SetLeft(Chunks, MapInfo.position.X - MapInfo.origin.X);
				Canvas.SetTop(Chunks, MapInfo.position.Y - MapInfo.origin.Y);

				Utils.Vec deltaPos = (Utils.Vec)e.GetPosition(Map) - oldDeltaPos;

				Time.Delta = (uint)(Math.Sqrt(deltaPos | deltaPos) * MapInfo.PixelTime);

				Update();

				oldDeltaPos = (Utils.Vec)e.GetPosition(Map);

				Console.WriteLine(MapInfo.position.X + ", " + MapInfo.position.Y);
			}
		}

		private void Map_MouseLeave(object sender, MouseEventArgs e)
		{
			MapInfo.DoesDrag = false;
		}
	}
}
