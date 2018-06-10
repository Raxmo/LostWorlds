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

namespace LostWorlds
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 

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
		public static double Eccentricity = 0.5; //TODO: actually impliment this. No idea quite how to impliment the varying day-length imposed by eccentricity of the orbit.


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
