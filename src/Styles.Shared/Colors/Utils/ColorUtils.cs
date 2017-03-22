using System;
namespace Styles
{
	public static class ColorUtils
	{
		#region Extension Methods
		public static bool IsDarkColor(this IColorSpace color)
		{
			var rgb = color.ToRgb();
			return (0.2126 * rgb.R + 0.7152 * rgb.G + 0.0722 * rgb.B) < 0.5;
		}

		public static bool IsBlackOrWhite(this IColorSpace color)
		{
			var rgb = color.ToRgb();
			return (rgb.R > 0.91 && rgb.G > 0.91 && rgb.B > 0.91) || (rgb.R < 0.09 && rgb.G < 0.09 && rgb.B < 0.09);
		}

		public static bool IsDistinct(this IColorSpace color, ColorRGB compare)
		{
			var rgb = color.ToRgb();
			var threshold = 0.25;

			if (Math.Abs(rgb.R - compare.R) > threshold || Math.Abs(rgb.G - compare.G) > threshold || Math.Abs(rgb.B - compare.B) > threshold)
			{
				if (Math.Abs(rgb.R - rgb.G) < 0.03 && Math.Abs(rgb.R - rgb.B) < 0.03)
				{
					if (Math.Abs(compare.R - compare.G) < 0.03 && Math.Abs(compare.R - compare.B) < 0.03)
					{
						return false;
					}
				}
				return true;
			}

			return false;
		}

		// TODO create methods for
		// colorWithComplementaryFlatColorOf
		// colorWithContrastingBlackOrWhiteColorOn
		public static ColorRGB NearestFlatColor(this IColorSpace color, IColorSpace[] flatColors)
		{
			var index = 0;

			double smallestDistance = 1000000;
			double previousDistance = 1000000;
			double currentDistance;

			var compare = new CieDe2000Comparison();
			for (int i = 0; i < flatColors.Length; i++)
			{

				//Check that index is not zero
				if (i != 0)
				{
					compare.Compare(color, flatColors[i - 1]);
				}

				//Extract LAB values from colors in array and store it as the current index
				currentDistance = compare.Compare(color, flatColors[i]);

				//We're only interested in the smallest difference
				if (currentDistance < previousDistance)
				{
					if (currentDistance < smallestDistance)
					{
						smallestDistance = currentDistance;
						index = i;
					}
				}
			}

			var nearestColor = (ColorRGB)flatColors[index].ToRgb();
			return nearestColor;
		}

		public static bool IsContrastingColor(this IColorSpace color, ColorRGB compare)
		{
			var rgb = color.ToRgb();
			var bgLum = 0.2126 * rgb.R + 0.7152 * rgb.G + 0.0722 * rgb.B;
			var fgLum = 0.2126 * compare.R + 0.7152 * compare.G + 0.0722 * compare.B;

			var bgGreater = bgLum > fgLum;
			var nom = bgGreater ? bgLum : fgLum;

			var denom = bgGreater ? fgLum : bgLum;
			var contrast = (nom + 0.05) / (denom + 0.05);

			return (1.6 < contrast);
		}

		public static ColorRGB ColorWithMinimumSaturation(this IColorSpace color, double minSaturation)
		{
			var hsb = ColorHSB.FromColor(color.ToRgb());

			if (hsb.S < minSaturation)
			{
				hsb.S = minSaturation;
			}
			return (ColorRGB)hsb.ToRgb();
		}

		public static ColorRGB Mix(this IColorSpace color, ColorRGB mix, float amount = 0.5f)
		{
			var rgb = color.ToRgb();
			var nomalizedWeight = (amount > 1) ? 1 : ((amount < 0) ? 0 : amount);

			var red = rgb.R + nomalizedWeight * (mix.R - rgb.R);
			var green = rgb.G + nomalizedWeight * (mix.G - rgb.G);
			var blue = rgb.B + nomalizedWeight * (mix.B - rgb.B);
			var alpha = 1;//rgb.A + nomalizedWeight * (mix.A - rgb.A);

			return new ColorRGB(red, green, blue, alpha);
		}

		public static ColorRGB Tinted(this IColorSpace color, float amount = 0.2f)
		{
			return color.Mix(new ColorRGB(1, 1, 1, 1), amount);
		}

		public static ColorRGB Shaded(this IColorSpace color, float amount = 0.2f)
		{
			return color.Mix(new ColorRGB(0, 0, 0, 1), amount);
		}

		public static ColorRGB AdjustHue(this IColorSpace color, int amount)
		{
			var rgb = color.ToRgb();
			var hsl = ColorHSL.FromColor(rgb);
			hsl.H += amount;

			return (ColorRGB)hsl.ToRgb();
		}

		public static ColorRGB Complementary(this IColorSpace color)
		{
			return color.AdjustHue(180);
		}

		public static ColorRGB Lightened(this IColorSpace color, double amount = 0.2)
		{
			var hsl = ColorHSL.FromColor(color.ToRgb());
			hsl.L += amount;

			return (ColorRGB)hsl.ToRgb();
		}

		public static ColorRGB Darkened(this IColorSpace color, double amount = 0.2)
		{
			return color.Lightened(-amount);
		}

		public static ColorRGB Saturated(this IColorSpace color, double amount = 0.2)
		{
			var hsl = ColorHSL.FromColor(color.ToRgb());
			hsl.S += amount;

			return (ColorRGB)hsl.ToRgb();
		}

		public static ColorRGB Desaturated(this IColorSpace color, double amount = 0.2)
		{
			return color.Saturated(amount * -1);
		}

		public static ColorRGB GrayScale(this IColorSpace color)
		{
			return color.Desaturated(1);
		}

		public static ColorRGB Inverted(this IColorSpace color)
		{
			var rgb = color.ToRgb();
			return new ColorRGB(
				1 - rgb.R,
				1 - rgb.G,
				1 - rgb.B,
				1
			);
		}
		#endregion

		#region Generic Methods

		public static T ColorWithMinimumSaturation<T>(this IColorSpace color, double minSaturation) where T : IColorSpace, new()
		{
			return color.ColorWithMinimumSaturation(minSaturation).To<T>();
		}

		public static T Mix<T>(this IColorSpace color, ColorRGB mix, float amount = 0.5f) where T : IColorSpace, new()
		{
			return color.Mix(mix, amount).To<T>();
		}

		public static T Tinted<T>(this IColorSpace color, float amount = 0.2f) where T : IColorSpace, new()
		{
			return color.Mix(new ColorRGB(1, 1, 1, 1), amount).To<T>();
		}

		public static T Shaded<T>(this IColorSpace color, float amount = 0.2f) where T : IColorSpace, new()
		{
			return color.Mix(new ColorRGB(0, 0, 0, 1), amount).To<T>();
		}

		public static T AdjustHue<T>(this IColorSpace color, int amount) where T : IColorSpace, new()
		{
			return color.AdjustHue(amount).To<T>();
		}

		public static T Complementary<T>(this IColorSpace color) where T : IColorSpace, new()
		{
			return color.AdjustHue(180).To<T>();
		}

		public static T Lightened<T>(this IColorSpace color, double amount = 0.2) where T : IColorSpace, new()
		{
			return color.Lightened(amount).To<T>();
		}

		public static T Darkened<T>(this IColorSpace color, double amount = 0.2) where T : IColorSpace, new()
		{
			return color.Lightened(-amount).To<T>();
		}

		public static T Saturated<T>(this IColorSpace color, double amount = 0.2) where T : IColorSpace, new()
		{
			return color.Saturated(amount).To<T>();
		}

		public static T Desaturated<T>(this IColorSpace color, double amount = 0.2) where T : IColorSpace, new()
		{
			return color.Desaturated(amount).To<T>();
		}

		public static T GrayScale<T>(this IColorSpace color) where T : IColorSpace, new()
		{
			return color.Desaturated(1).To<T>();
		}

		public static T Inverted<T>(this IColorSpace color) where T : IColorSpace, new()
		{
			return color.Inverted().To<T>();
		}
		#endregion
	}
}

