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
		public static uint t = 0;
		public static uint day = 86400;
		public static uint hour = 3600;
		public static uint minute = 60;
		public static uint delta = 0;
	}

	public static class Sun
	{
		public static Utils.quat rpos = new Utils.quat(0, 0, 0, 1);
		public static double hpd = 24; //hours per day
		public static double dpy = 364; //days per year
		public static double tilt = Math.PI * 20 / 180; //axial tilt of the planet
		public static double tiltFase = 0; //this is the offset of the tilt relative to the orbital position
		public static double latitude = Math.PI * 45 / 180; //the current latitude of the viewer
		public static double eccentricity = 0.5; //TODO: actually impliment this. No idea quite how to impliment the varying day-length imposed by eccentricity of the orbit.


		public static Utils.vec origin = new Utils.vec(100, 100);
		public static Utils.vec pos = new Utils.vec(0,0);
		public static double radius = 10;
		public static Ellipse circle = new Ellipse();
		public static Ellipse horizon = new Ellipse()
		{
			Width = 200,
			Height = 200,
			Stroke = Brushes.White,
			StrokeThickness = 3
		};

		public static void draw()
		{
			double dvel = Math.PI * 2 / (hpd * Time.hour);
			double yvel = Math.PI * 2 / (dpy * hpd * Time.hour);

			double insvel = yvel / (1 - eccentricity * Math.Cos(yvel * Time.t)); //Deppricated code, this doesn't work the way I need it to. Eccentricity will likely need to be passed through an integral in order to get the propper function to shift the sun's position correctly due to eccentricity

			double dpos = dvel * Time.t;
			double ypos = yvel * Time.t;

			Utils.quat ntilt = Utils.quat.Rot(tilt, new Utils.vec3(1, 0, 0)); //stores a rotation bassed off of the axial tilt
			ntilt = ntilt ^ Utils.quat.Rot(ypos, new Utils.vec3(0, 1, 0)); //rotates the rotation bassed off of the orbital position

			Utils.quat nrpos = rpos ^ ntilt; //offsets the position given the current latitude

			Utils.quat npos = nrpos ^ Utils.quat.Rot(dpos, new Utils.vec3(0, -1, 0)) ^ Utils.quat.Rot(latitude, (Utils.vec3)(rpos) * new Utils.vec3(0, 1, 0)); //Rotates the sun around proper axis throught the day

			Utils.vec3 v3 = (Utils.vec3)npos; //casting the quaterneon to a vector
			Utils.pol3 p3 = (Utils.pol3)v3;//casting the vector to a polar 3
			Utils.pol p = (Utils.pol)p3; //this colapses the polar3 down to a polar2 to display current heading and altitude

			pos = !p * (200 / Math.PI); //make the sun draw at the correct place on the canvas

			circle.Width = radius * 2;
			circle.Height = radius * 2;

			if ((pos | pos) > Math.Pow(100 + radius, 2)) //only display the sun if it is actually above the horizon
			{
				circle.Stroke = Brushes.Transparent;
			}
			else
			{
				circle.Stroke = Brushes.White;
			}
			circle.StrokeThickness = 3;
			circle.SetValue(Canvas.LeftProperty, (pos + origin).x - radius);
			circle.SetValue(Canvas.TopProperty, (pos + origin).y - radius);
		}

		
	}

	public static class Larder
	{
		public static uint volume = 0;
		public static uint density = 0;
		public static uint water = 0;

		public static void add(Food item, uint volume)
		{
			double energy = density * Larder.volume;
			double wat = water * Larder.volume;

			Larder.volume += volume;
			energy += item.energy * volume;
			wat += item.water * volume;

			density =(uint)(energy / Larder.volume);
			water = (uint)(wat / Larder.volume);
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
			app = this;
			Areas.start.load();
			update();
		}

		public static MainWindow app;

		public void update()
		{
			Time.t += Time.delta;

			uint numDays = (Time.t / Time.day) % 364;
			uint numYears = (Time.t / Time.day) / 364;

			days.Content = "Days: " + numDays;
			years.Content = "Years: " + numYears;

			Characters.update();

			playerHunger.Foreground = Characters.player.hungerColor();
			playerThurst.Foreground = Characters.player.thirstColor();
			partnerHunger.Foreground = Characters.partner.hungerColor();
			partnerThurst.Foreground = Characters.partner.thirstColor();
			tracker.Children.Clear();
			tracker.Children.Add(Sun.horizon);
			Sun.draw();
			tracker.Children.Add(Sun.circle);

			Time.delta = 0;
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			if (Areas.back.Count() > 0)
			{
				Time.delta = Areas.curr.travelTime;
				Areas.back[Areas.back.Count() - 1].load();
				Areas.back.RemoveAt(Areas.back.Count() - 1);
			}
			update();
		}

		private void home_Click(object sender, RoutedEventArgs e)
		{
			Time.delta = Areas.curr.returnTime;
			Areas.back.Clear();
			Areas.home.load();
			update();
		}
	}
}
