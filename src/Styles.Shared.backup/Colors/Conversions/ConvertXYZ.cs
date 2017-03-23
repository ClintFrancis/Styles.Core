using System;
namespace Styles
{
	internal static class ConvertXYZ
	{
		#region Constants/Helper methods for Xyz related spaces
		internal static readonly IXyz WhiteReference = new ColorXYZ {
			X = 95.047,
			Y = 100.000,
			Z = 108.883
		};
		internal const double Epsilon = 0.008856; // Intent is 216/24389
		internal const double Kappa = 903.3; // Intent is 24389/27

		internal static double CubicRoot (double n)
		{
			return Math.Pow (n, 1.0 / 3.0);
		}
		#endregion

		internal static IXyz ToColorSpace (IRgb color)
		{
			// convert to a sRGB form
			var r = (color.R > 0.04045) ? Math.Pow ((color.R + 0.055) / (1 + 0.055), 2.4) : (color.R / 12.92);
			var g = (color.G > 0.04045) ? Math.Pow ((color.G + 0.055) / (1 + 0.055), 2.4) : (color.G / 12.92);
			var b = (color.B > 0.04045) ? Math.Pow ((color.B + 0.055) / (1 + 0.055), 2.4) : (color.B / 12.92);

			var result = ColorXYZ.Empty;
			result.X = (r * 0.4124 + g * 0.3576 + b * 0.1805);
			result.Y = (r * 0.2126 + g * 0.7152 + b * 0.0722);
			result.Z = (r * 0.0193 + g * 0.1192 + b * 0.9505);

			return result;
		}

		internal static IRgb ToColor (IXyz item)
		{
			// (Observer = 2°, Illuminant = D65)
			var x = item.X;
			var y = item.Y;
			var z = item.Z;

			var r = x * 3.2406 + y * -1.5372 + z * -0.4986;
			var g = x * -0.9689 + y * 1.8758 + z * 0.0415;
			var b = x * 0.0557 + y * -0.2040 + z * 1.0570;

			r = r > 0.0031308 ? 1.055 * Math.Pow (r, 1 / 2.4) - 0.055 : 12.92 * r;
			g = g > 0.0031308 ? 1.055 * Math.Pow (g, 1 / 2.4) - 0.055 : 12.92 * g;
			b = b > 0.0031308 ? 1.055 * Math.Pow (b, 1 / 2.4) - 0.055 : 12.92 * b;

			return ColorRGB.FromRGB (ToRgb (r), ToRgb (g), ToRgb (b));
		}

		private static int ToRgb (double n)
		{
			var result = 255.0 * n;
			if (result < 0) return 0;
			if (result > 255) return 255;
			return (int)Math.Round (result);
		}
	}
}

