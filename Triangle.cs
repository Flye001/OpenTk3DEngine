using OpenTK.Mathematics;

namespace OpenTkEngine
{
    internal struct Triangle
    {
        public Vector4 Point1;
        public Vector4 Point2;
        public Vector4 Point3;

        public Triangle()
        {
            Point1 = new Vector4();
            Point2 = new Vector4();
            Point3 = new Vector4();
        }

        public Triangle(Vector4[] points)
        {
            Point1 = points[0];
            Point2 = points[1];
            Point3 = points[2];
        }
    }
}
