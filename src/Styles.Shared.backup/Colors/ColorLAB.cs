using System;
namespace Styles
{
	public struct ColorLAB : ILab
	{
		public static readonly ColorLAB Empty = new ColorLAB();

		double l;
		double a;
		double b;

		#region Operators
		public static bool operator ==(ColorLAB item1, ColorLAB item2)
		{
			return (
				item1.L == item2.L
				&& item1.A == item2.A
				&& item1.B == item2.B
				);
		}

		public static bool operator !=(ColorLAB item1, ColorLAB item2)
		{
			return (
				item1.L != item2.L
				|| item1.A != item2.A
				|| item1.B != item2.B
				);
		}

		#endregion

		public double L
		{
			get
			{
				return l;
			}
			set
			{
				l = value;
			}
		}

		public double A
		{
			get
			{
				return a;
			}
			set
			{
				a = value;
			}
		}

		public double B
		{
			get
			{
				return b;
			}
			set
			{
				b = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Styles.Core.ColorLAB"/> struct.
		/// </summary>
		/// <param name="l">L, from 0 to 100</param>
		/// <param name="a">A, from -128 to 128</param>
		/// <param name="b">B, from -128 to 128</param>
		public ColorLAB(double l, double a, double b)
		{
			this.l = l;
			this.a = a;
			this.b = b;
		}

		public override bool Equals(Object obj)
		{
			if (obj == null || GetType() != obj.GetType()) return false;

			return (this == (ColorLAB)obj);
		}

		public override int GetHashCode()
		{
			return L.GetHashCode() ^ a.GetHashCode() ^ b.GetHashCode();
		}

		#region IColorSpace implementation
		public void Initialize(IRgb color)
		{
			var lab = ConvertLAB.ToColorSpace(color);
			this.L = lab.L;
			this.A = lab.A;
			this.B = lab.B;
		}

		public IRgb ToRgb()
		{
			return ConvertLAB.ToColor(this);
		}
		#endregion

		public static ILab FromColor(IRgb color)
		{
			var result = ColorLAB.Empty;
			result.Initialize(color);
			return result;
		}

		public static IRgb ToColor(double l, double a, double b)
		{
			return new ColorLAB(l, a, b).ToRgb();
		}
	}
}

