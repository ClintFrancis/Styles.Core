using System;
namespace Styles
{
	public static class ColorCompareExt
	{
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
		/*
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
		*/

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
	}
}
