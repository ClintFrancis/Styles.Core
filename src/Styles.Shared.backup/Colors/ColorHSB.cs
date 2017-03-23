using System;
namespace Styles
{
	public struct ColorHSB : IHsb
	{
		public static readonly ColorHSB Empty = new ColorHSB();

		double hue;
		double saturation;
		double brightness;

		#region Operators
		public static bool operator ==(ColorHSB item1, ColorHSB item2)
		{
			return (
				item1.H == item2.H
				&& item1.S == item2.S
				&& item1.B == item2.B
				);
		}

		public static bool operator !=(ColorHSB item1, ColorHSB item2)
		{
			return (
				item1.H != item2.H
				|| item1.S != item2.S
				|| item1.B != item2.B
				);
		}
		#endregion

		#region Accessors
		public double H
		{
			get
			{
				return hue;
			}
			set
			{
				hue = MathUtils.Wrap(Math.Round(value), 360);
			}
		}

		public double S
		{
			get
			{
				return saturation;
			}
			set
			{
				saturation = (value > 1) ? 1 : ((value < 0) ? 0 : value);
			}
		}

		public double B
		{
			get
			{
				return brightness;
			}
			set
			{
				brightness = (value > 1) ? 1 : ((value < 0) ? 0 : value);
			}
		}
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Styles.Core.ColorHSB"/> struct.
		/// </summary>
		/// <param name="h">Hue value, from 0 to 360</param>
		/// <param name="s">Saturation, from 0 to 1</param>
		/// <param name="b">Brightness, from 0 to 1</param>
		public ColorHSB(double h, double s, double b)
		{
			hue = MathUtils.Wrap(Math.Round(h), 360);
			saturation = (s > 1) ? 1 : ((s < 0) ? 0 : s);
			brightness = (b > 1) ? 1 : ((b < 0) ? 0 : b);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Styles.Core.ColorHSB"/> struct.
		/// </summary>
		/// <param name="h">Hue value, from 0 to 360</param>
		/// <param name="s">Saturation, from 0 to 100</param>
		/// <param name="b">Brightness, from 0 to 100</param>
		public ColorHSB(int h, int s, int b)
		{
			hue = MathUtils.Wrap(h, 360);
			saturation = s / 100d;
			brightness = b / 100d;
		}

		public override bool Equals(Object obj)
		{
			if (obj == null || GetType() != obj.GetType()) return false;

			return (this == (ColorHSB)obj);
		}

		public override int GetHashCode()
		{
			return H.GetHashCode() ^ S.GetHashCode() ^ B.GetHashCode();
		}

		#region IColorSpace implementation
		public void Initialize(IRgb color)
		{
			var hsb = ConvertHSB.ToColorSpace(color);
			this.H = hsb.H;
			this.S = hsb.S;
			this.B = hsb.B;
		}

		public IRgb ToRgb()
		{
			return ConvertHSB.ToColor(this);
		}
		#endregion

		public static IHsb FromColor(IRgb color)
		{
			var hsb = ColorHSB.Empty;
			hsb.Initialize(color);
			return hsb;
		}

		public static IRgb ToColor(double hue, double saturation, double brightness)
		{
			return new ColorHSB(hue, saturation, brightness).ToRgb();
		}
	}
}

