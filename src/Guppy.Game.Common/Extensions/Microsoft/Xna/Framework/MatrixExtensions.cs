namespace Microsoft.Xna.Framework
{
    public static class MatrixExtensions
    {
        public static Matrix Invert(this Matrix matrix)
        {
            return Matrix.Invert(matrix);
        }

        public static float Radians(this Matrix matrix)
        {
            return (float)Math.Atan2(matrix.M12, matrix.M11);
        }

        public static Vector2 Transformation(this Matrix matrix)
        {
            return new Vector2(matrix.M41, matrix.M42);
        }

        public static BoundingSphere GetBoudingSphere(this Matrix matrix, float radius)
        {
            return new BoundingSphere(new Vector3(matrix.M41, matrix.M42, 0), radius);
        }
    }
}
