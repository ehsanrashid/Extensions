namespace System.Drawing
{
    public static class PointExtension
    {
        /// <summary>
        ///   Gets the rectangle that sorrounds the given point by a specified distance.
        /// </summary>
        /// <param name="point"> Instance value. </param>
        /// <param name="distance"> Distance that will be used to surround the point. </param>
        /// <returns> Rectangle that sorrounds the given point by a specified distance. </returns>
        public static Rectangle Surround(this Point point, int distance)
        {
            return new Rectangle(point.X - distance, point.Y - distance, distance*2, distance*2);
        }

        public static double Distance(Point p1, Point p2)
        {
            var dX = p1.X - p2.X;
            var dY = p1.Y - p2.Y;
            return Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));
        }

        public static String ToString(Point p)
        {
            return String.Format("({0, 2},{1, 2})", p.X, p.Y);
        }
    }
}