using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTkEngine
{
    internal class RenderItem
    {
        private readonly int _vertexArrayObject;
        private readonly int _vertexBufferObject;
        private int _vertices;

        public Matrix4 ModelMatrix { get; set; }

        public RenderItem(string modelPath, Matrix4 initialModelMatrix, Vector3 Color)
        {
            Mesh mesh = new();
            if (!mesh.LoadFromObjectFile(modelPath))
            {
                throw new Exception("Failed to load obj file!");
            }

            ModelMatrix = initialModelMatrix;

            // Bind this items vertex array
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            // Vertices
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // Normals
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            // Colors
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            List<float> vertices = new();
            foreach (var triangle in mesh.Triangles)
            {
                var line1 = triangle.Point2 - triangle.Point1;
                var line2 = triangle.Point3 - triangle.Point1;
                var normal = Vector3.Cross(line1, line2);
                normal = Vector3.Normalize(normal);

                // Point 1 vertices
                vertices.Add(triangle.Point1.X);
                vertices.Add(triangle.Point1.Y);
                vertices.Add(triangle.Point1.Z);
                // Point 1 Normal
                vertices.Add(normal.X);
                vertices.Add(normal.Y);
                vertices.Add(normal.Z);
                // Point 1 Color
                vertices.Add(Color.X);
                vertices.Add(Color.Y);
                vertices.Add(Color.Z);
                // Point 2 Vertices
                vertices.Add(triangle.Point2.X);
                vertices.Add(triangle.Point2.Y);
                vertices.Add(triangle.Point2.Z);
                // Point 2 Normal
                vertices.Add(normal.X);
                vertices.Add(normal.Y);
                vertices.Add(normal.Z);
                // Point 2 Color
                vertices.Add(Color.X);
                vertices.Add(Color.Y);
                vertices.Add(Color.Z);
                // Point 3 Vertices
                vertices.Add(triangle.Point3.X);
                vertices.Add(triangle.Point3.Y);
                vertices.Add(triangle.Point3.Z);
                // Point 3 Normal
                vertices.Add(normal.X);
                vertices.Add(normal.Y);
                vertices.Add(normal.Z);
                // Point 3 Color
                vertices.Add(Color.X);
                vertices.Add(Color.Y);
                vertices.Add(Color.Z);
            }
            var verticesArr = vertices.ToArray();
            _vertices = verticesArr.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, verticesArr.Length * sizeof(float), verticesArr, BufferUsageHint.StaticDraw);
        }

        public void Draw(Shader shader)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BindVertexArray(_vertexArrayObject);
            shader.Use();
            var model = ModelMatrix;
            shader.SetMatrix4("model", ref model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices);
        }
    }
}
