using System;
namespace Styles
{
	public partial struct ColorRGB : IRgb
	{
		public static readonly ColorRGB Empty = new ColorRGB(0, 0, 0, 0);

		double red;
		double green;
		double blue;
		double alpha;

		#region Operators
		public static bool operator ==(ColorRGB item1, ColorRGB item2)
		{
			return (
				item1.R == item2.R
				&& item1.G == item2.G
				&& item1.B == item2.B
				);
		}

		public static bool operator !=(ColorRGB item1, ColorRGB item2)
		{
			return (
				item1.R != item2.R
				|| item1.G != item2.G
				|| item1.B != item2.B
				);
		}

		#endregion

		#region Accessors
		public double R
		{
			get
			{
				return red;
			}
			set
			{
				red = (value > 1) ? 1 : ((value < 0) ? 0 : value);
			}
		}

		public double G
		{
			get
			{
				return green;
			}
			set
			{
				green = (value > 1) ? 1 : ((value < 0) ? 0 : value);
			}
		}

		public double B
		{
			get
			{
				return blue;
			}
			set
			{
				blue = (value > 1) ? 1 : ((value < 0) ? 0 : value);
			}
		}

		public double A
		{
			get
			{
				return alpha;
			}
			set
			{
				alpha = (value > 1) ? 1 : ((value < 0) ? 0 : value);
			}
		}
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Styles.Core.ColorRGB"/> struct.
		/// </summary>
		/// <param name="r">Red, from 0 to 1</param>
		/// <param name="g">Green, from 0 to 1</param>
		/// <param name="b">Blue, from 0 to 1</param>
		/// <param name="a">Alpha, from 0 to 1</param>
		public ColorRGB(double r, double g, double b, double a = 1)
		{
			red = (r > 1) ? 1 : ((r < 0) ? 0 : r);
			green = (g > 1) ? 1 : ((g < 0) ? 0 : g);
			blue = (b > 1) ? 1 : ((b < 0) ? 0 : b);
			alpha = (a > 1) ? 1 : ((a < 0) ? 0 : a);
		}

		public override bool Equals(Object obj)
		{
			if (obj == null || GetType() != obj.GetType()) return false;

			return (this == (ColorRGB)obj);
		}

		public override int GetHashCode()
		{
			return R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode();
		}

		#region IColorSpace implementation
		public void Initialize(IRgb color)
		{
			R = color.R;
			G = color.G;
			B = color.B;
		}

		public IRgb ToRgb()
		{
			return this;
		}
		#endregion

		public static ColorRGB FromHex(string hexString)
		{
			// TODO switch on whether this contains an alpha value

			var colorString = hexString.Replace("#", "");
			int red, green, blue;

			switch (colorString.Length)
			{
				case 3: // #RGB
					{
						red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16);
						green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16);
						blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16);
						return FromRGB(red, green, blue);
					}
				case 6: // #RRGGBB
					{
						red = Convert.ToInt32(colorString.Substring(0, 2), 16);
						green = Convert.ToInt32(colorString.Substring(2, 2), 16);
						blue = Convert.ToInt32(colorString.Substring(4, 2), 16);
						return FromRGB(red, green, blue);
					}

				default:
					throw new ArgumentOutOfRangeException(string.Format("Invalid color value {0} is invalid. It should be a hex value of the form #RBG, #RRGGBB", hexString));
			}
		}

		public static ColorRGB FromHex(int hexValue)
		{
			// TODO switch on whether this contains an alpha value

			return ColorRGB.FromRGB(
				(hexValue & 0xFF0000) >> 16,
				(hexValue & 0xFF00) >> 8,
				(hexValue & 0xFF)
			);
		}

		public string ToHex()
		{
			return string.Format("#{0:X2}{1:X2}{2:X2}", (int)(R * 255), (int)(G * 255), (int)(B * 255));
		}

		public static ColorRGB FromRGB(int r, int g, int b)
		{
			return new ColorRGB(
				r / 255d,
				g / 255d,
				b / 255d,
				1
			);
		}
	}
}

