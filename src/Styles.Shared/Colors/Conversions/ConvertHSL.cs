using System;
namespace Styles
{
	internal static class ConvertHSL
	{

		internal static IHsl ToColorSpace (IRgb color)
		{
			var result = ColorHSL.Empty;
			var max = Math.Max (color.R, Math.Max (color.G, color.B));
			var min = Math.Min (color.R, Math.Min (color.G, color.B));

			// hue
			if (max == min) {
				result.H = 0d; // undefined
			} else if (max == color.R && color.G >= color.B) {
				result.H = 60d * (color.G - color.B) / (max - min);
			} else if (max == color.R && color.G < color.B) {
				result.H = 60d * (color.G - color.B) / (max - min) + 360d;
			} else if (max == color.G) {
				result.H = 60d * (color.B - color.R) / (max - min) + 120d;
			} else if (max == color.B) {
				result.H = 60d * (color.R - color.G) / (max - min) + 240d;
			}

			// luminance
			result.L = (max + min) / 2;

			// saturation
			if (result.L == 0 || max == min) {
				result.S = 0;
			} else if (0 < result.L && result.L <= 0.5) {
				result.S = (max - min) / (max + min);
			} else if (result.L > 0.5) {
				result.S = (max - min) / (2 - (max + min));
			}

			return result;
		}

		// works fine
		internal static IRgb ToColor(IHsl item)
		{
			if (item.S == 0)
			{
				// achromatic color (gray scale)
				return new ColorRGB(item.L, item.L, item.L, 1);
			}
			else
			{
				double q = (item.L < 0.5) ? (item.L * (1.0 + item.S)) : (item.L + item.S - (item.L * item.S));
				double p = (2 * item.L) - q;

				double Hk = item.H / 360d;
				double[] T = new double[3];
				T[0] = Hk + (1d / 3d);     // Red
				T[1] = Hk;                 // Blue
				T[2] = Hk - (1d / 3d);     // Green

				for (int i = 0; i < 3; i++)
				{
					if (T[i] < 0) T[i] += 1;
					if (T[i] > 1) T[i] -= 1;

					if ((T[i] * 6d) < 1)
					{
						T[i] = p + ((q - p) * 6d * T[i]);
					}
					else if ((T[i] * 2.0) < 1)
					{
						T[i] = q;
					}
					else if ((T[i] * 3d) < 2)
					{
						T[i] = p + (q - p) * ((2 / 3) - T[i]) * 6d;
					}
					else T[i] = p;
				}

				var r = (int)T[0].ToByte();
				var g = (int)T[1].ToByte();
				var b = (int)T[2].ToByte();

				return ColorRGB.FromRGB(r,g,b);
			}
		}

		// Contains some kind of rounding error
		internal static IRgb ToColor2(IHsl item)
		{
			double r, g, b;
			if (item.S == 0)
			{
				r = g = b = item.L.ToByte();
			}
			else
			{
				double t1, t2;
				double th = item.H / 6.0d;

				if (item.L < 0.5d)
				{
					t2 = item.L * (1d + item.S);
				}
				else
				{
					t2 = (item.L + item.S) - (item.L * item.S);
				}
				t1 = 2d * item.L - t2;

				double tr, tg, tb;
				tr = th + (1.0d / 3.0d);
				tg = th;
				tb = th - (1.0d / 3.0d);

				tr = ColorCalc(tr, t1, t2);
				tg = ColorCalc(tg, t1, t2);
				tb = ColorCalc(tb, t1, t2);
				r = tr.ToByte();
				g = tg.ToByte();
				b = tb.ToByte();
			}
			return new ColorRGB(r, g, b);
		}

		private static double ColorCalc(double c, double t1, double t2)
		{
			if (c < 0) c += 1d;
			if (c > 1) c -= 1d;
			if (6.0d * c < 1.0d) return t1 + (t2 - t1) * 6.0d * c;
			if (2.0d * c < 1.0d) return t2;
			if (3.0d * c < 2.0d) return t1 + (t2 - t1) * (2.0d / 3.0d - c) * 6.0d;
			return t1;
		}
	}
}

