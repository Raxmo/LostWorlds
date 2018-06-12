using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWorlds
{
	public static class Utils
	{
		public static Random rand = new Random();

		public static double Gaussian(double m, double d) //random numbers within normal distribution, don't ask how it works, don't care, it just does. it's probably something allong the lines of the derivative of cosine at 0 is similar to the derivative of the normal distribution and the edge casses look like negetive log function or something? not sure and again, don't really care.
		{
			var u1 = 1.0 - rand.NextDouble();
			var u2 = 1.0 - rand.NextDouble();

			var randNorm = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);

			return randNorm * d + m;
		}

		public struct Pol //polar vector
		{
			public double A;
			public double R;

			public Pol(double a, double r)
			{
				this.A = a;
				this.R = r;
			}

			public static Pol operator ~(Pol a) //normalize
			{
				return new Pol(a.A, 1);
			}

			public static Vec operator !(Pol a) //convert from polar to cartesian
			{
				return new Vec(Math.Cos(a.A) * a.R, Math.Sin(a.A) * a.R);
			}

			public static Pol operator +(Pol a, Pol b) //mostly used for adding angles, but could add an angle and distance simultaniously
			{
				return new Pol(a.A + b.A, a.R + b.R);
			}

			public static Pol operator -(Pol a, Pol b)
			{
				return new Pol(a.A - b.A, a.R - b.R);
			}

			public static Pol operator *(Pol a, Pol b) //polar version of complex multiplication
			{
				return new Pol(a.A + b.A, a.R * b.R);
			}

			public static Pol operator *(Pol p, double b) //scalar multiplication
			{
				return new Pol(p.A, p.R * b);
			}

			public static Pol operator /(Pol a, Pol b) //polar version of complex division
			{
				return new Pol(a.A - b.A, a.R / b.R);
			}

			public static double operator |(Pol a, Pol b) //polar version of dot product
			{
				return (a.R * b.R * Math.Cos(b.A - a.A));
			}

			public static explicit operator Pol(Pol3 p)
			{
				return new Pol(p.H, p.A); //casting from pol3 to pol
			}
		}


		public struct Vec
		{
			public double X;
			public double Y;

			public Vec(double x, double y)
			{
				this.X = x;
				this.Y = y;
			}

			public static Vec operator ~(Vec a) //normalize
			{
				return new Vec(Math.Pow(a.X, 2), Math.Pow(a.Y, 2)) / (a | a);
			}

			public static Pol operator !(Vec a) //convert from cartesian to polar
			{
				return new Pol(Math.Atan2(a.Y, a.X), Math.Sqrt(a.X * a.X + a.Y * a.Y));
			}

			public static Vec operator +(Vec a, Vec b)
			{
				return new Vec(a.X + b.X, a.Y + b.Y);
			}

			public static Vec operator -(Vec a, Vec b)
			{
				return new Vec(a.X - b.X, a.Y - b.Y);
			}

			public static Vec operator *(Vec a, Vec b) //complex multiplication
			{
				return new Vec(a.X * b.X - a.Y * b.Y, a.X * b.Y + a.Y * b.X);
			}

			public static Vec operator *(Vec a, double b) //scalar multiplication
			{
				return new Vec(a.X * b, a.Y * b);
			}

			public static Vec operator /(Vec a, Vec b) //complex division
			{
				return new Vec((a.X * b.X + a.Y * b.Y) / (b | b), (a.Y * b.X - a.X * b.Y) / (b | b));
			}

			public static Vec operator /(Vec a, double b)
			{
				return new Vec(a.X / b, a.Y / b);
			}

			public static double operator |(Vec a, Vec b) //dot product
			{
				return (a.X * b.X + a.Y * b.Y);
			}
		}

		public struct Pol3
		{
			public double H, A, R;

			public Pol3(double h, double a, double r)
			{
				this.H = h;
				this.A = a;
				this.R = r;
			}

			public static explicit operator Pol3(Vec3 v) //converting from cartesian to polar
			{
				return new Pol3(
					Math.Atan2(v.Y, v.X),
					Math.Acos(v.Z / Math.Sqrt(v | v)),
					Math.Sqrt(v | v));
			}
		}

		public struct Vec3
		{
			public double X, Y, Z;

			public Vec3(double x, double y, double z)
			{
				this.X = x;
				this.Y = y;
				this.Z = z;
			}

			public static Vec3 operator *(Vec3 a, double b) //scalar multiplication
			{
				return new Vec3(a.X * b, a.Y * b, a.Z * b);
			}

			public static Vec3 operator *(Vec3 a, Vec3 b)
			{
				return new Vec3(
					a.Y * b.Z - a.Z * b.Y,
					a.Z * b.X - a.X * b.Z,
					a.X * b.Y - a.Y * b.X);
			}

			public static Vec3 operator /(Vec3 a, double b) //scalar division
			{
				return new Vec3(a.X / b, a.Y / b, a.Z / b);
			}

			public static Vec3 operator ~(Vec3 a) //normalize
			{
				return a / Math.Sqrt(a | a);
			}

			public static double operator |(Vec3 a, Vec3 b) //dot product
			{
				return (a.X * b.X + a.Y * b.Y + a.Z * b.Z);
			}

			public static explicit operator Vec3(Quat q) //grabbing the vector component of a quaterneon
			{
				return new Vec3(q.I, q.J, q.K);
			}
		}

		public struct Quat
		{
			public double R, I, J, K;

			public Quat(double r, double i, double j, double k)
			{
				this.R = r;
				this.I = i;
				this.J = j;
				this.K = k;
			}

			public Quat(double a, Vec3 v) // angle vector construction
			{
				R = a;
				I = v.X;
				J = v.Y;
				K = v.Z;
			}

			public static Quat operator *(Quat a, Quat b) //quaterneon multiplication kinda sucks
			{
				return new Quat(
					a.R * b.R - a.I * b.I - a.J * b.J - a.K * b.K,
					a.R * b.I + a.I * b.R + a.J * b.K - a.K * b.J,
					a.R * b.J - a.I * b.K + a.J * b.R + a.K * b.I,
					a.R * b.K + a.I * b.J - a.J * b.I + a.K * b.R);
			}

			public static Quat operator !(Quat a) //congugation of quaterneon
			{
				return new Quat(a.R, -a.I, -a.J, -a.K);
			}

			public static Quat operator ^(Quat a, Quat b) //rotational multiplication of two quaterneons
			{
				return b * a * (!b);
			}

			public static Quat Rot(double a, Vec3 v)
			{

				return new Quat(Math.Cos(a / 2), ~v * Math.Sin(a / 2));
			}
		}
	}
}
