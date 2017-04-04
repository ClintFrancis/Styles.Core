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
			get { return red; }
			set {
				value = Math.Round(value, 4);
				red = (value > 1) ? 1 : ((value < 0) ? 0 : value); 
			}
		}

		public double G
		{
			get { return green; }
			set { 
				value = Math.Round(value, 4);
				green = (value > 1) ? 1 : ((value < 0) ? 0 : value); 
			}
		}

		public double B
		{
			get { return blue; }
			set { 
				value = Math.Round(value, 4);
				blue = (value > 1) ? 1 : ((value < 0) ? 0 : value); 
			}
		}

		public double A
		{
			get { return Math.Round(alpha, 2); }
			set { alpha = (value > 1) ? 1 : ((value < 0) ? 0 : value); }
		}

		public double AlphaRaw
		{
			get { return alpha;}
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
			var colorString = hexString.Replace("#", "");
			int alpha, red, green, blue;

			switch (colorString.Length)
			{
				case 3: // #RGB
					{
						red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16);
						green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16);
						blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16);
						return new ColorRGB(red, green, blue);
					}
				case 6: // #RRGGBB
					{
						red = Convert.ToInt32(colorString.Substring(0, 2), 16);
						green = Convert.ToInt32(colorString.Substring(2, 2), 16);
						blue = Convert.ToInt32(colorString.Substring(4, 2), 16);
						return new ColorRGB(red, green, blue);
					}
				case 8: // #AARRGGBB
					{
						alpha = Convert.ToInt32(colorString.Substring(0, 2), 16);
						red = Convert.ToInt32(colorString.Substring(2, 2), 16);
						green = Convert.ToInt32(colorString.Substring(4, 2), 16);
						blue = Convert.ToInt32(colorString.Substring(6, 2), 16);
						return new ColorRGB(red, green, blue, (alpha / 255d));
					}
				default:
					throw new ArgumentOutOfRangeException(string.Format("Invalid color value {0} is invalid. It should be a hex value of the form #RBG, #RRGGBB", hexString));
			}
		}

		public static ColorRGB FromRGB(uint rgb)
		{
			return new ColorRGB() { ValueRGB = rgb	};
		}

		public static ColorRGB FromRGB(int r, int g, int b)
		{
			var red = ((r > 255) ? 255 : ((r < 0) ? 0 : r)) / 255d;
			var green = ((g > 255) ? 255 : ((g < 0) ? 0 : g)) / 255d;
			var blue = ((b > 255) ? 255 : ((b < 0) ? 0 : b)) / 255d;

			return new ColorRGB(red, green, blue);
		}

		public static ColorRGB FromRGBA(int r, int g, int b, double a)
		{
			var red = ((r > 255) ? 255 : ((r < 0) ? 0 : r)) / 255d;
			var green = ((g > 255) ? 255 : ((g < 0) ? 0 : g)) / 255d;
			var blue = ((b > 255) ? 255 : ((b < 0) ? 0 : b)) / 255d;
			var alpha = (a > 1) ? 1 : ((a < 0) ? 0 : a);

			return new ColorRGB(red, green, blue, alpha);
		}

		public static ColorRGB FromARGB(uint argb)
		{
			return new ColorRGB() { ValueARGB = argb };
		}

		public string ToHex()
		{
			return string.Format("#{0:X2}{1:X2}{2:X2}", (int)(red * 255), (int)(green * 255), (int)(blue * 255));
		}

		public string ToAlphaHex()
		{
			return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", (int)Math.Ceiling(alpha * 255), (int)(red * 255), (int)(green * 255), (int)(blue * 255));
		}

		public uint ValueRGB
		{
			get{
				var r = red.ToByte();
				var g = green.ToByte();
				var b = blue.ToByte();

				return r << 16 | g << 8 | b;
			}

			set{
				red = ((value >> 16) & 0xFF)/255d;
				green = ((value >> 8) & 0xFF)/255d;
				blue = (value & 0xFF) / 255d;
				alpha = 1;
			}
		}

		public uint ValueARGB
		{
			get {
				var a = alpha.ToByte();
				var r = red.ToByte();
				var g = green.ToByte();
				var b = blue.ToByte();

				return a << 24 | r << 16 | g << 8 | b;
			}

			set {
				alpha = ((value >> 24) & 0xFF) / 255d;
				red = ((value >> 16) & 0xFF) / 255d;
				green = ((value >> 8) & 0xFF) / 255d;
				blue = (value & 0xFF)/255d;
			}
		}
	}
}

