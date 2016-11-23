using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Game
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4 : IEquatable<Vector4>
    {
        /// <summary>
        ///     The X component of the Vector4.
        /// </summary>
        public float X;

        /// <summary>
        ///     The Y component of the Vector4.
        /// </summary>
        public float Y;

        /// <summary>
        ///     The Z component of the Vector4.
        /// </summary>
        public float Z;

        /// <summary>
        ///     The W component of the Vector4.
        /// </summary>
        public float W;

        /// <summary>
        ///     Defines a unit-length Vector4 that points towards the X-axis.
        /// </summary>
        public static readonly Vector4 UnitX = new Vector4(1, 0, 0, 0);

        /// <summary>
        ///     Defines a unit-length Vector4 that points towards the Y-axis.
        /// </summary>
        public static readonly Vector4 UnitY = new Vector4(0, 1, 0, 0);

        /// <summary>
        ///     Defines a unit-length Vector4 that points towards the Z-axis.
        /// </summary>
        public static readonly Vector4 UnitZ = new Vector4(0, 0, 1, 0);

        /// <summary>
        ///     Defines a unit-length Vector4 that points towards the W-axis.
        /// </summary>
        public static readonly Vector4 UnitW = new Vector4(0, 0, 0, 1);

        /// <summary>
        ///     Defines a zero-length Vector4.
        /// </summary>
        public static readonly Vector4 Zero = new Vector4(0, 0, 0, 0);

        /// <summary>
        ///     Defines an instance with all components set to 1.
        /// </summary>
        public static readonly Vector4 One = new Vector4(1, 1, 1, 1);

        /// <summary>
        ///     Defines the size of the Vector4 struct in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf(new Vector4());

        /// <summary>
        ///     Constructs a new Vector4.
        /// </summary>
        /// <param name="x">The x component of the Vector4.</param>
        /// <param name="y">The y component of the Vector4.</param>
        /// <param name="z">The z component of the Vector4.</param>
        /// <param name="w">The w component of the Vector4.</param>
        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>Transform a Vector by the given Matrix</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed vector</param>
        public static void Transform(ref Vector4 vec, ref Matrix4 mat, out Vector4 result)
        {
            result = new Vector4(
                vec.X*mat.Row0.X + vec.Y*mat.Row1.X + vec.Z*mat.Row2.X + vec.W*mat.Row3.X,
                vec.X*mat.Row0.Y + vec.Y*mat.Row1.Y + vec.Z*mat.Row2.Y + vec.W*mat.Row3.Y,
                vec.X*mat.Row0.Z + vec.Y*mat.Row1.Z + vec.Z*mat.Row2.Z + vec.W*mat.Row3.Z,
                vec.X*mat.Row0.W + vec.Y*mat.Row1.W + vec.Z*mat.Row2.W + vec.W*mat.Row3.W);
        }

        /// <summary>Transform a Vector by the given Matrix using right-handed notation</summary>
        /// <param name="mat">The desired transformation</param>
        /// <param name="vec">The vector to transform</param>
        /// <param name="result">The transformed vector</param>
        public static void Transform(ref Matrix4 mat, ref Vector4 vec, out Vector4 result)
        {
            result = new Vector4(
                mat.Row0.X*vec.X + mat.Row0.Y*vec.Y + mat.Row0.Z*vec.Z + mat.Row0.W*vec.W,
                mat.Row1.X*vec.X + mat.Row1.Y*vec.Y + mat.Row1.Z*vec.Z + mat.Row1.W*vec.W,
                mat.Row2.X*vec.X + mat.Row2.Y*vec.Y + mat.Row2.Z*vec.Z + mat.Row2.W*vec.W,
                mat.Row3.X*vec.X + mat.Row3.Y*vec.Y + mat.Row3.Z*vec.Z + mat.Row3.W*vec.W);
        }

        /// <summary>
        ///     Adds two instances.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static Vector4 operator +(Vector4 left, Vector4 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            left.W += right.W;
            return left;
        }

        /// <summary>
        ///     Subtracts two instances.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static Vector4 operator -(Vector4 left, Vector4 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            left.W -= right.W;
            return left;
        }

        /// <summary>
        ///     Negates an instance.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static Vector4 operator -(Vector4 vec)
        {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            vec.Z = -vec.Z;
            vec.W = -vec.W;
            return vec;
        }

        /// <summary>
        ///     Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>The result of the calculation.</returns>
        public static Vector4 operator *(Vector4 vec, float scale)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        /// <summary>
        ///     Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="scale">The scalar.</param>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the calculation.</returns>
        public static Vector4 operator *(float scale, Vector4 vec)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        /// <summary>
        ///     Component-wise multiplication between the specified instance by a scale vector.
        /// </summary>
        /// <param name="scale">Left operand.</param>
        /// <param name="vec">Right operand.</param>
        /// <returns>Result of multiplication.</returns>
        public static Vector4 operator *(Vector4 vec, Vector4 scale)
        {
            vec.X *= scale.X;
            vec.Y *= scale.Y;
            vec.Z *= scale.Z;
            vec.W *= scale.W;
            return vec;
        }

        /// <summary>
        ///     Transform a Vector by the given Matrix.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector4 operator *(Vector4 vec, Matrix4 mat)
        {
            Vector4 result;
            Transform(ref vec, ref mat, out result);
            return result;
        }

        /// <summary>
        ///     Transform a Vector by the given Matrix using right-handed notation
        /// </summary>
        /// <param name="mat">The desired transformation</param>
        /// <param name="vec">The vector to transform</param>
        /// <returns>The transformed vector</returns>
        public static Vector4 operator *(Matrix4 mat, Vector4 vec)
        {
            Vector4 result;
            Transform(ref mat, ref vec, out result);
            return result;
        }

        /// <summary>
        ///     Divides an instance by a scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>The result of the calculation.</returns>
        public static Vector4 operator /(Vector4 vec, float scale)
        {
            var mult = 1.0f/scale;
            vec.X *= mult;
            vec.Y *= mult;
            vec.Z *= mult;
            vec.W *= mult;
            return vec;
        }

        /// <summary>
        ///     Compares two instances for equality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        public static bool operator ==(Vector4 left, Vector4 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Compares two instances for inequality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equa lright; false otherwise.</returns>
        public static bool operator !=(Vector4 left, Vector4 right)
        {
            return !left.Equals(right);
        }

        private static readonly string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
        /// <summary>
        ///     Returns a System.String that represents the current Vector4.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0}{4} {1}{4} {2}{4} {3})", X, Y, Z, W, listSeparator);
        }

        /// <summary>
        ///     Returns the hashcode for this instance.
        /// </summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode*397) ^ Y.GetHashCode();
                hashCode = (hashCode*397) ^ Z.GetHashCode();
                hashCode = (hashCode*397) ^ W.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Vector4))
            {
                return false;
            }

            return Equals((Vector4) obj);
        }

        /// <summary>Indicates whether the current vector is equal to another vector.</summary>
        /// <param name="other">A vector to compare with this vector.</param>
        /// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
        public bool Equals(Vector4 other)
        {
            return
                (X == other.X) &&
                (Y == other.Y) &&
                (Z == other.Z) &&
                (W == other.W);
        }
    }
}