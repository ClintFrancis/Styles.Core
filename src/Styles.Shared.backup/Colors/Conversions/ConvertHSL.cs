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
				result.H = 0; // undefined
			} else if (max == color.R && color.G >= color.B) {
				result.H = 60 * (color.G - color.B) / (max - min);
			} else if (max == color.R && color.G < color.B) {
				result.H = 60 * (color.G - color.B) / (max - min) + 360;
			} else if (max == color.G) {
				result.H = 60 * (color.B - color.R) / (max - min) + 120;
			} else if (max == color.B) {
				result.H = 60 * (color.R - color.G) / (max - min) + 240;
			}

			// luminance
			result.L = (max + min) / 2;

			// saturation
			if (result.L == 0 || max == min) {
				result.S = 0;
			} else if (0 < result.L && result.L <= 0.5) {
				result.S = (max - min) / (max + min);
			} else if (result.L > 0.5) {
				result.S = (max - min) / (2 - (max + min)); //(max-min > 0)?
			}

			return result;
		}

		// works fine
		internal static IRgb ToColor (IHsl item)
		{
			if (item.S == 0) {
				// achromatic color (gray scale)
				return new ColorRGB (item.L, item.L, item.L, 1);
			} else {
				double q = (item.L < 0.5) ? (item.L * (1.0 + item.S)) : (item.L + item.S - (item.L * item.S));
				double p = (2 * item.L) - q;

				double Hk = item.H / 360;
				double [] T = new double [3];
				T [0] = Hk + (1f / 3f);     // Red
				T [1] = Hk;                 // Blue
				T [2] = Hk - (1f / 3f);     // Green

				for (int i = 0; i < 3; i++) {
					if (T [i] < 0) T [i] += 1;
					if (T [i] > 1) T [i] -= 1;

					if ((T [i] * 6) < 1) {
						T [i] = p + ((q - p) * 6 * T [i]);
					} else if ((T [i] * 2.0) < 1) {
						T [i] = q;
					} else if ((T [i] * 3.0) < 2) {
						T [i] = p + (q - p) * ((2 / 3) - T [i]) * 6;
					} else T [i] = p;
				}

				return new ColorRGB (T [0], T [1], T [2], 1);
			}
		}
	}
}

