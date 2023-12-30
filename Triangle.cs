using OpenTK.Mathematics;

namespace OpenTkEngine
{
    internal struct Triangle
    {
        public Vector3 Point1;
        public Vector3 Point2;
        public Vector3 Point3;

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
    }
}
