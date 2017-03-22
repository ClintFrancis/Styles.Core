using System;
namespace Styles
{
	public struct ColorXYZ : IXyz
	{
		/// <summary>
		/// Gets the CIE D65 (white) structure.
		/// </summary>
		public static readonly ColorXYZ D65 = new ColorXYZ(0.9505, 1.0, 1.0890);
		public static readonly ColorXYZ Empty = new ColorXYZ();


		#region Fields
		double x;
		double y;
		double z;
		#endregion

		#region Operators
		public static bool operator ==(ColorXYZ item1, ColorXYZ item2)
		{
			return (
				item1.X == item2.X
				&& item1.Y == item2.Y
				&& item1.Z == item2.Z
				);
		}

		public static bool operator !=(ColorXYZ item1, ColorXYZ item2)
		{
			return (
				item1.X != item2.X
				|| item1.Y != item2.Y
				|| item1.Z != item2.Z
				);
		}

		#endregion

		#region Accessors
		/// <summary>
		/// Gets or sets X component.
		/// </summary>
		public double X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = (value > 0.9505) ? 0.9505 : ((value < 0) ? 0 : value);
			}
		}

		/// <summary>
		/// Gets or sets Y component.
		/// </summary>
		public double Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = (value > 1.0) ? 1.0 : ((value < 0) ? 0 : value);
			}
		}

		/// <summary>
		/// Gets or sets Z component.
		/// </summary>
		public double Z
		{
			get
			{
				return this.z;
			}
			set
			{
				this.z = (value > 1.089) ? 1.089 : ((value < 0) ? 0 : value);
			}
		}

		#endregion
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Styles.Core.ColorXYZ"/> struct.
		/// </summary>
		/// <param name="x">X from 0 to 100</param>
		/// <param name="y">Y from 0 to 1</param>
		/// <param name="z">Z from 0 to 1</param>
		// TODO fix the vairable ranges
		public ColorXYZ(double x, double y, double z)
		{
			this.x = (x > 0.9505) ? 0.9505 : ((x < 0) ? 0 : x);
			this.y = (y > 1.0) ? 1.0 : ((y < 0) ? 0 : y);
			this.z = (z > 1.089) ? 1.089 : ((z < 0) ? 0 : z);
		}

		public override bool Equals(Object obj)
		{
			if (obj == null || GetType() != obj.GetType()) return false;

			return (this == (ColorXYZ)obj);
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		#region IColorSpace implementation
		public void Initialize(IRgb color)
		{
			var xyz = ConvertXYZ.ToColorSpace(color);
			this.X = xyz.X;
			this.Y = xyz.Y;
			this.Z = xyz.Z;
		}

		public IRgb ToRgb()
		{
			return ConvertXYZ.ToColor(this);
		}
		#endregion

		public static IXyz FromColor(IRgb color)
		{
			var result = ColorXYZ.Empty;
			result.Initialize(color);
			return result;
		}

		public static IRgb ToColor(double x, double y, double z)
		{
			return new ColorXYZ(x, y, z).ToRgb();
		}
	}
}

