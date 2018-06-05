using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWorlds
{
	public static class Utils
	{
		public static double gaussian(double m, double d) //random numbers within normal distribution, don't ask how it works, don't care, it just does. it's probably something allong the lines of the derivative of cosine at 0 is similar to the derivative of the normal distribution and the edge casses look like negetive log function or something? not sure and again, don't really care.
		{
			Random rand = new Random();
			double u1 = 1.0 - rand.NextDouble();
			double u2 = 1.0 - rand.NextDouble();

			double randNorm = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);

			return randNorm * d + m;
		}

		public struct pol //polar vector
		{
			public double a;
			public double r;

			public pol(double a, double r)
			{
				this.a = a;
				this.r = r;
			}

			public static pol operator ~(pol a) //normalize
			{
				return new pol(a.a, 1);
			}

			public static vec operator !(pol a) //convert from polar to cartesian
			{
				return new vec(Math.Cos(a.a) * a.r, Math.Sin(a.a) * a.r);
			}

			public static pol operator +(pol a, pol b) //mostly used for adding angles, but could add an angle and distance simultaniously
			{
				return new pol(a.a + b.a, a.r + b.r);
			}

			public static pol operator -(pol a, pol b)
			{
				return new pol(a.a - b.a, a.r - b.r);
			}

			public static pol operator *(pol a, pol b) //polar version of complex multiplication
			{
				return new pol(a.a + b.a, a.r * b.r);
			}

			public static pol operator *(pol p, double b) //scalar multiplication
			{
				return new pol(p.a, p.r * b);
			}

			public static pol operator /(pol a, pol b) //polar version of complex division
			{
				return new pol(a.a - b.a, a.r / b.r);
			}

			public static double operator |(pol a, pol b) //polar version of dot product
			{
				return (a.r * b.r * Math.Cos(b.a - a.a));
			}

			public static explicit operator pol(pol3 p)
			{
				return new pol(p.h, p.a); //casting from pol3 to pol
			}
		}


		public struct vec
		{
			public double x;
			public double y;

			public vec(double x, double y)
			{
				this.x = x;
				this.y = y;
			}

			public static vec operator ~(vec a) //normalize
			{
				return new vec(Math.Pow(a.x, 2), Math.Pow(a.y, 2)) / (a | a);
			}

			public static pol operator !(vec a) //convert from cartesian to polar
			{
				return new pol(Math.Atan2(a.y, a.x), Math.Sqrt(a.x * a.x + a.y * a.y));
			}

			public static vec operator +(vec a, vec b)
			{
				return new vec(a.x + b.x, a.y + b.y);
			}

			public static vec operator -(vec a, vec b)
			{
				return new vec(a.x - b.x, a.y - b.y);
			}

			public static vec operator *(vec a, vec b) //complex multiplication
			{
				return new vec(a.x * b.x - a.y * b.y, a.x * b.y + a.y * b.x);
			}

			public static vec operator *(vec a, double b) //scalar multiplication
			{
				return new vec(a.x * b, a.y * b);
			}

			public static vec operator /(vec a, vec b) //complex division
			{
				return new vec((a.x * b.x + a.y * b.y) / (b | b), (a.y * b.x - a.x * b.y) / (b | b));
			}

			public static vec operator /(vec a, double b)
			{
				return new vec(a.x / b, a.y / b);
			}

			public static double operator |(vec a, vec b) //dot product
			{
				return (a.x * b.x + a.y * b.y);
			}
		}

		public struct pol3
		{
			public double h, a, r;

			public pol3(double h, double a, double r)
			{
				this.h = h;
				this.a = a;
				this.r = r;
			}

			public static explicit operator pol3(vec3 v) //converting from cartesian to polar
			{
				return new pol3(
					Math.Atan2(v.y, v.x),
					Math.Acos(v.z / Math.Sqrt(v | v)),
					Math.Sqrt(v | v));
			}
		}

		public struct vec3
		{
			public double x, y, z;

			public vec3(double x, double y, double z)
			{
				this.x = x;
				this.y = y;
				this.z = z;
			}

			public static vec3 operator *(vec3 a, double b) //scalar multiplication
			{
				return new vec3(a.x * b, a.y * b, a.z * b);
			}

			public static vec3 operator *(vec3 a, vec3 b)
			{
				return new vec3(
					a.y * b.z - a.z * b.y,
					a.z * b.x - a.x * b.z,
					a.x * b.y - a.y * b.x);
			}

			public static vec3 operator /(vec3 a, double b) //scalar division
			{
				return new vec3(a.x / b, a.y / b, a.z / b);
			}

			public static vec3 operator ~(vec3 a) //normalize
			{
				return a / Math.Sqrt(a | a);
			}

			public static double operator |(vec3 a, vec3 b) //dot product
			{
				return (a.x * b.x + a.y * b.y + a.z * b.z);
			}

			public static explicit operator vec3(quat q) //grabbing the vector component of a quaterneon
			{
				return new vec3(q.i, q.j, q.k);
			}
		}

		public struct quat
		{
			public double r, i, j, k;

			public quat(double r, double i, double j, double k)
			{
				this.r = r;
				this.i = i;
				this.j = j;
				this.k = k;
			}

			public quat(double a, vec3 v) // angle vector construction
			{
				r = a;
				i = v.x;
				j = v.y;
				k = v.z;
			}

			public static quat operator *(quat a, quat b) //quaterneon multiplication kinda sucks
			{
				return new quat(
					a.r * b.r - a.i * b.i - a.j * b.j - a.k * b.k,
					a.r * b.i + a.i * b.r + a.j * b.k - a.k * b.j,
					a.r * b.j - a.i * b.k + a.j * b.r + a.k * b.i,
					a.r * b.k + a.i * b.j - a.j * b.i + a.k * b.r);
			}

			public static quat operator !(quat a) //congugation of quaterneon
			{
				return new quat(a.r, -a.i, -a.j, -a.k);
			}

			public static quat operator ^(quat a, quat b) //rotational multiplication of two quaterneons
			{
				return b * a * (!b);
			}

			public static quat Rot(double a, vec3 v)
			{

				return new quat(Math.Cos(a / 2), ~v * Math.Sin(a / 2));
			}
		}
	}
}
