using System;
namespace Styles
{
	// Code lovingly copied from StackOverflow (and tweaked a bit)
	// Question/Answer: http://stackoverflow.com/questions/359612/how-to-change-rgb-color-to-hsv/1626175#1626175
	// Submitter: Greg http://stackoverflow.com/users/12971/greg
	// License: http://creativecommons.org/licenses/by-sa/3.0/
	internal static class ConvertHSB
	{
		internal static IHsb ToColorSpace (IRgb color)
		{
			var item = ColorHSB.Empty;
			var r = color.R;
			var g = color.G;
			var b = color.B;
			var max = Math.Max (r, Math.Max (g, b));

			if (max <= 0) {
				item.H = 0;
				item.S = 0;
				item.B = 0;
				return item;
			}

			var min = Math.Min (r, Math.Min (g, b));
			var dif = max - min;

			if (max > min) {
				if (g == max) {
					item.H = (b - r) / dif * 60 + 120;
				} else if (b == max) {
					item.H = (r - g) / dif * 60 + 240;
				} else if (b > g) {
					item.H = (g - b) / dif * 60 + 360;
				} else {
					item.H = (g - b) / dif * 60;
				}
			} else {
				item.H = 0;
			}

			item.S = (dif / max);
			item.B = max;
			return item;

		}

		internal static IRgb ToColor (IHsb item)
		{
			var r = item.B;
			var g = item.B;
			var b = item.B;

			if (item.S != 0) {
				var max = item.B;
				var dif = item.B * item.S;
				var min = item.B - dif;

				var h = MathUtils.Wrap (item.H, 360);

				if (h < 60) {
					r = max;
					g = h * dif / 60 + min;
					b = min;
				} else if (h < 120) {
					r = -(h - 120) * dif / 60 + min;
					g = max;
					b = min;
				} else if (h < 180) {
					r = min;
					g = max;
					b = (h - 120) * dif / 60 + min;
				} else if (h < 240) {
					r = min;
					g = -(h - 240) * dif / 60 + min;
					b = max;
				} else if (h < 300) {
					r = (h - 240) * dif / 60 + min;
					g = min;
					b = max;
				} else if (h <= 360) {
					r = max;
					g = min;
					b = -(h - 360) * dif / 60 + min;
				} else {
					r = 0;
					g = 0;
					b = 0;
				}
			}
			return new ColorRGB (r, g, b, 1);
		}
	}
}

