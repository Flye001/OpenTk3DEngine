using OpenTK.Mathematics;

namespace OpenTkEngine
{
    internal struct Triangle
    {
        public Vector3 Point1;
        public Vector3 Point2;
        public Vector3 Point3;
        public Vector2 TPoint1;
        public Vector2 TPoint2;
        public Vector2 TPoint3;
        public Vector4 Color;

        public Triangle()
        {
            Point1 = new Vector3();
            Point2 = new Vector3();
            Point3 = new Vector3();
        }

        public Triangle(Vector3[] points)
        {
            Point1 = points[0];
            Point2 = points[1];
            Point3 = points[2];
        }

        public Triangle(Vector3[] points, Vector2[] texts)
        {
            Point1 = points[0];
            Point2 = points[1];
            Point3 = points[2];
            TPoint1 = texts[0];
            TPoint2 = texts[1];
            TPoint3 = texts[2];
        }
    }
}
