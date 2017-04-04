using System;
namespace Styles
{
	public struct ColorHSL : IHsl
	{
		public static readonly ColorHSL Empty = new ColorHSL();

		double hue;
		double saturation;
		double luminance;

		#region Operators
		public static bool operator ==(ColorHSL item1, ColorHSL item2)
		{
			return (
				item1.H == item2.H
				&& item1.S == item2.S
				&& item1.L == item2.L
				);
		}

		public static bool operator !=(ColorHSL item1, ColorHSL item2)
		{
			return (
				item1.H != item2.H
				|| item1.S != item2.S
				|| item1.L != item2.L
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
				hue = MathUtils.Wrap(Math.Round(value, 2), 360);
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
				value = Math.Round(value, 4);
				saturation = (value > 1) ? 1 : ((value < 0) ? 0 : value);
			}
		}

		public double L
		{
			get
			{
				return luminance;
			}
			set
			{
				value = Math.Round(value, 4);
				luminance = (value > 1) ? 1 : ((value < 0) ? 0 : value);

			}
		}

		#endregion
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Styles.Core.ColorHSL"/> struct.
		/// </summary>
		/// <param name="h">Hue value, from 0 to 360</param>
		/// <param name="s">Saturation, from 0 to 1</param>
		/// <param name="l">Luminance, from 0 to 1</param> 
		public ColorHSL(double h, double s, double l)
		{
			hue = MathUtils.Wrap(Math.Round(h), 360);
			saturation = (s > 1) ? 1 : ((s < 0) ? 0 : s);
			luminance = (l > 1) ? 1 : ((l < 0) ? 0 : l);
		}

		public override bool Equals(Object obj)
		{
			if (obj == null || GetType() != obj.GetType()) return false;

			return (this == (ColorHSL)obj);
		}

		public override int GetHashCode()
		{
			return H.GetHashCode() ^ S.GetHashCode() ^ L.GetHashCode();
		}

		#region IColorSpace implementation
		public void Initialize(IRgb color)
		{
			var hsl = ConvertHSL.ToColorSpace(color);
			this.H = hsl.H;
			this.S = hsl.S;
			this.L = hsl.L;
		}

		public IRgb ToRgb()
		{
			return ConvertHSL.ToColor(this);
		}
		#endregion

		public static IHsl FromColor(IRgb color)
		{
			var result = ColorHSL.Empty;
			result.Initialize(color);
			return result;
		}

		public static IRgb ToColor(double hue, double saturation, double luminosity)
		{
			return new ColorHSL(hue, saturation, luminosity).ToRgb();
		}
	}
}

