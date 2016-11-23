using System;
using System.Runtime.InteropServices;

namespace Game
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4 : IEquatable<Matrix4>
    {

        /// <summary>
        ///     Top row of the matrix.
        /// </summary>
        public Vector4 Row0;

        /// <summary>
        ///     2nd row of the matrix.
        /// </summary>
        public Vector4 Row1;

        /// <summary>
        ///     3rd row of the matrix.
        /// </summary>
        public Vector4 Row2;

        /// <summary>
        ///     Bottom row of the matrix.
        /// </summary>
        public Vector4 Row3;

        /// <summary>
        ///     The identity matrix.
        /// </summary>
        private static readonly Matrix4 Identity = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

        /// <summary>
        ///     The zero matrix.
        /// </summary>
        private static readonly Matrix4 Zero = new Matrix4(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero);
        

        /// <summary>
        ///     Constructs a new instance.
        /// </summary>
        /// <param name="row0">Top row of the matrix.</param>
        /// <param name="row1">Second row of the matrix.</param>
        /// <param name="row2">Third row of the matrix.</param>
        /// <param name="row3">Bottom row of the matrix.</param>
        private Matrix4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }
        
       
        
        

        /// <summary>
        ///     Creates an orthographic projection matrix.
        /// </summary>
        /// <param name="left">The left edge of the projection volume.</param>
        /// <param name="right">The right edge of the projection volume.</param>
        /// <param name="bottom">The bottom edge of the projection volume.</param>
        /// <param name="top">The top edge of the projection volume.</param>
        /// <param name="zNear">The near edge of the projection volume.</param>
        /// <param name="zFar">The far edge of the projection volume.</param>
        /// <param name="result">The resulting Matrix4 instance.</param>
        public static void CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear,
            float zFar, out Matrix4 result)
        {
            result = Identity;

            var invRL = 1.0f/(right - left);
            var invTB = 1.0f/(top - bottom);
            var invFN = 1.0f/(zFar - zNear);

            result.Row0.X = 2*invRL;
            result.Row1.Y = 2*invTB;
            result.Row2.Z = -2*invFN;

            result.Row3.X = -(right + left)*invRL;
            result.Row3.Y = -(top + bottom)*invTB;
            result.Row3.Z = -(zFar + zNear)*invFN;
        }

        /// <summary>
        ///     Creates an orthographic projection matrix.
        /// </summary>
        /// <param name="left">The left edge of the projection volume.</param>
        /// <param name="right">The right edge of the projection volume.</param>
        /// <param name="bottom">The bottom edge of the projection volume.</param>
        /// <param name="top">The top edge of the projection volume.</param>
        /// <param name="zNear">The near edge of the projection volume.</param>
        /// <param name="zFar">The far edge of the projection volume.</param>
        /// <returns>The resulting Matrix4 instance.</returns>
        public static Matrix4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNear,
            float zFar)
        {
            Matrix4 result;
            CreateOrthographicOffCenter(left, right, bottom, top, zNear, zFar, out result);
            return result;
        }
        

        /// <summary>
        ///     Adds two instances.
        /// </summary>
        /// <param name="left">The left operand of the addition.</param>
        /// <param name="right">The right operand of the addition.</param>
        /// <returns>A new instance that is the result of the addition.</returns>
        private static Matrix4 Add(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Add(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        ///     Adds two instances.
        /// </summary>
        /// <param name="left">The left operand of the addition.</param>
        /// <param name="right">The right operand of the addition.</param>
        /// <param name="result">A new instance that is the result of the addition.</param>
        private static void Add(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            result.Row0 = left.Row0 + right.Row0;
            result.Row1 = left.Row1 + right.Row1;
            result.Row2 = left.Row2 + right.Row2;
            result.Row3 = left.Row3 + right.Row3;
        }
        

        /// <summary>
        ///     Subtracts one instance from another.
        /// </summary>
        /// <param name="left">The left operand of the subraction.</param>
        /// <param name="right">The right operand of the subraction.</param>
        /// <returns>A new instance that is the result of the subraction.</returns>
        private static Matrix4 Subtract(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Subtract(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        ///     Subtracts one instance from another.
        /// </summary>
        /// <param name="left">The left operand of the subraction.</param>
        /// <param name="right">The right operand of the subraction.</param>
        /// <param name="result">A new instance that is the result of the subraction.</param>
        private static void Subtract(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            result.Row0 = left.Row0 - right.Row0;
            result.Row1 = left.Row1 - right.Row1;
            result.Row2 = left.Row2 - right.Row2;
            result.Row3 = left.Row3 - right.Row3;
        }
        

        /// <summary>
        ///     Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication.</returns>
        private static Matrix4 Mult(Matrix4 left, Matrix4 right)
        {
            Matrix4 result;
            Mult(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        ///     Multiplies two instances.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication.</param>
        private static void Mult(ref Matrix4 left, ref Matrix4 right, out Matrix4 result)
        {
            float lM11 = left.Row0.X,
                lM12 = left.Row0.Y,
                lM13 = left.Row0.Z,
                lM14 = left.Row0.W,
                lM21 = left.Row1.X,
                lM22 = left.Row1.Y,
                lM23 = left.Row1.Z,
                lM24 = left.Row1.W,
                lM31 = left.Row2.X,
                lM32 = left.Row2.Y,
                lM33 = left.Row2.Z,
                lM34 = left.Row2.W,
                lM41 = left.Row3.X,
                lM42 = left.Row3.Y,
                lM43 = left.Row3.Z,
                lM44 = left.Row3.W,
                rM11 = right.Row0.X,
                rM12 = right.Row0.Y,
                rM13 = right.Row0.Z,
                rM14 = right.Row0.W,
                rM21 = right.Row1.X,
                rM22 = right.Row1.Y,
                rM23 = right.Row1.Z,
                rM24 = right.Row1.W,
                rM31 = right.Row2.X,
                rM32 = right.Row2.Y,
                rM33 = right.Row2.Z,
                rM34 = right.Row2.W,
                rM41 = right.Row3.X,
                rM42 = right.Row3.Y,
                rM43 = right.Row3.Z,
                rM44 = right.Row3.W;

            result.Row0.X = lM11*rM11 + lM12*rM21 + lM13*rM31 + lM14*rM41;
            result.Row0.Y = lM11*rM12 + lM12*rM22 + lM13*rM32 + lM14*rM42;
            result.Row0.Z = lM11*rM13 + lM12*rM23 + lM13*rM33 + lM14*rM43;
            result.Row0.W = lM11*rM14 + lM12*rM24 + lM13*rM34 + lM14*rM44;
            result.Row1.X = lM21*rM11 + lM22*rM21 + lM23*rM31 + lM24*rM41;
            result.Row1.Y = lM21*rM12 + lM22*rM22 + lM23*rM32 + lM24*rM42;
            result.Row1.Z = lM21*rM13 + lM22*rM23 + lM23*rM33 + lM24*rM43;
            result.Row1.W = lM21*rM14 + lM22*rM24 + lM23*rM34 + lM24*rM44;
            result.Row2.X = lM31*rM11 + lM32*rM21 + lM33*rM31 + lM34*rM41;
            result.Row2.Y = lM31*rM12 + lM32*rM22 + lM33*rM32 + lM34*rM42;
            result.Row2.Z = lM31*rM13 + lM32*rM23 + lM33*rM33 + lM34*rM43;
            result.Row2.W = lM31*rM14 + lM32*rM24 + lM33*rM34 + lM34*rM44;
            result.Row3.X = lM41*rM11 + lM42*rM21 + lM43*rM31 + lM44*rM41;
            result.Row3.Y = lM41*rM12 + lM42*rM22 + lM43*rM32 + lM44*rM42;
            result.Row3.Z = lM41*rM13 + lM42*rM23 + lM43*rM33 + lM44*rM43;
            result.Row3.W = lM41*rM14 + lM42*rM24 + lM43*rM34 + lM44*rM44;
        }

        /// <summary>
        ///     Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <returns>A new instance that is the result of the multiplication</returns>
        private static Matrix4 Mult(Matrix4 left, float right)
        {
            Matrix4 result;
            Mult(ref left, right, out result);
            return result;
        }

        /// <summary>
        ///     Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="left">The left operand of the multiplication.</param>
        /// <param name="right">The right operand of the multiplication.</param>
        /// <param name="result">A new instance that is the result of the multiplication</param>
        private static void Mult(ref Matrix4 left, float right, out Matrix4 result)
        {
            result.Row0 = left.Row0*right;
            result.Row1 = left.Row1*right;
            result.Row2 = left.Row2*right;
            result.Row3 = left.Row3*right;
        }

        
       


        /// <summary>
        ///     Matrix multiplication
        /// </summary>
        /// <param name="left">left-hand operand</param>
        /// <param name="right">right-hand operand</param>
        /// <returns>A new Matrix4 which holds the result of the multiplication</returns>
        public static Matrix4 operator *(Matrix4 left, Matrix4 right)
        {
            return Mult(left, right);
        }

        /// <summary>
        ///     Matrix-scalar multiplication
        /// </summary>
        /// <param name="left">left-hand operand</param>
        /// <param name="right">right-hand operand</param>
        /// <returns>A new Matrix4 which holds the result of the multiplication</returns>
        public static Matrix4 operator *(Matrix4 left, float right)
        {
            return Mult(left, right);
        }

        /// <summary>
        ///     Matrix addition
        /// </summary>
        /// <param name="left">left-hand operand</param>
        /// <param name="right">right-hand operand</param>
        /// <returns>A new Matrix4 which holds the result of the addition</returns>
        public static Matrix4 operator +(Matrix4 left, Matrix4 right)
        {
            return Add(left, right);
        }

        /// <summary>
        ///     Matrix subtraction
        /// </summary>
        /// <param name="left">left-hand operand</param>
        /// <param name="right">right-hand operand</param>
        /// <returns>A new Matrix4 which holds the result of the subtraction</returns>
        public static Matrix4 operator -(Matrix4 left, Matrix4 right)
        {
            return Subtract(left, right);
        }

        /// <summary>
        ///     Compares two instances for equality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        public static bool operator ==(Matrix4 left, Matrix4 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Compares two instances for inequality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equal right; false otherwise.</returns>
        public static bool operator !=(Matrix4 left, Matrix4 right)
        {
            return !left.Equals(right);
        }
        

        /// <summary>
        ///     Returns a System.String that represents the current Matrix4.
        /// </summary>
        /// <returns>The string representation of the matrix.</returns>
        public override string ToString()
        {
            return $"{Row0}\n{Row1}\n{Row2}\n{Row3}";
        }
        
        
        /// <summary>
        ///     Returns the hashcode for this instance.
        /// </summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Row0.GetHashCode();
                hashCode = (hashCode*397) ^ Row1.GetHashCode();
                hashCode = (hashCode*397) ^ Row2.GetHashCode();
                hashCode = (hashCode*397) ^ Row3.GetHashCode();
                return hashCode;
            }
        }
        
        
        /// <summary>
        ///     Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare tresult.</param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix4))
            {
                return false;
            }

            return Equals((Matrix4) obj);
        }
        

 

        /// <summary>Indicates whether the current matrix is equal to another matrix.</summary>
        /// <param name="other">An matrix to compare with this matrix.</param>
        /// <returns>true if the current matrix is equal to the matrix parameter; otherwise, false.</returns>
        public bool Equals(Matrix4 other)
        {
            return
                (Row0 == other.Row0) &&
                (Row1 == other.Row1) &&
                (Row2 == other.Row2) &&
                (Row3 == other.Row3);
        }

    }
}