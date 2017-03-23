using System;
namespace Styles
{
	internal static class ConvertLAB
	{
		internal static ILab ToColorSpace (IRgb color)
		{
			var xyz = ColorXYZ.Empty;
			xyz.Initialize (color);

			var white = ConvertXYZ.WhiteReference;
			var x = PivotXyz (xyz.X / white.X);
			var y = PivotXyz (xyz.Y / white.Y);
			var z = PivotXyz (xyz.Z / white.Z);

			var result = ColorLAB.Empty;
			result.L = Math.Max (0, 116 * y - 16);
			result.A = 500 * (x - y);
			result.B = 200 * (y - z);

			return result;
		}

		internal static IRgb ToColor (ILab item)
		{
			var y = (item.L + 16.0) / 116.0;
			var x = item.A / 500.0 + y;
			var z = y - item.B / 200.0;

			var white = ConvertXYZ.WhiteReference;
			var x3 = x * x * x;
			var z3 = z * z * z;
			var xyz = new ColorXYZ {
				X = white.X * (x3 > ConvertXYZ.Epsilon ? x3 : (x - 16.0 / 116.0) / 7.787),
				Y = white.Y * (item.L > (ConvertXYZ.Kappa * ConvertXYZ.Epsilon) ? Math.Pow (((item.L + 16.0) / 116.0), 3) : item.L / ConvertXYZ.Kappa),
				Z = white.Z * (z3 > ConvertXYZ.Epsilon ? z3 : (z - 16.0 / 116.0) / 7.787)
			};

			return xyz.ToRgb ();
		}

		private static double PivotXyz (double n)
		{
			return n > ConvertXYZ.Epsilon ? CubicRoot (n) : (ConvertXYZ.Kappa * n + 16) / 116;
		}

		private static double CubicRoot (double n)
		{
			return Math.Pow (n, 1.0 / 3.0);
		}
	}
}

