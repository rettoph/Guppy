namespace Microsoft.Xna.Framework
{
    public static class MatrixExtensions
    {
        public static Matrix Invert(this Matrix matrix) => Matrix.Invert(matrix);

        public static float Radians(this Matrix matrix) => (float)Math.Atan2(matrix.M12, matrix.M11);

        public static Vector2 Transformation(this Matrix matrix) => new(matrix.M41, matrix.M42);

        public static BoundingSphere GetBoudingSphere(this Matrix matrix, float radius) => new(new Vector3(matrix.M41, matrix.M42, 0), radius);
    }
}