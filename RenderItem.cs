using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTkEngine
{
    internal class RenderItem
    {
        private readonly int _vertexArrayObject;
        private readonly int _vertexBufferObject;
        private readonly int _vertices;
        private readonly Shader _shader;

        private Vector3 _position;
        private Matrix4 _modelMatrix;

        private bool _rotate;

        public RenderItem(string modelPath, Vector3 position, Vector3 color, Shader shader, bool rotate = false)
        {
            Mesh mesh = new();
            if (!mesh.LoadFromObjectFile(modelPath))
            {
                throw new Exception("Failed to load obj file!");
            }

            _shader = shader;
            _rotate = rotate;
            _position = position;
            _modelMatrix = Matrix4.CreateTranslation(_position);

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

                // Point 1 vertices
                vertices.Add(triangle.Point1.X);
                vertices.Add(triangle.Point1.Y);
                vertices.Add(triangle.Point1.Z);
                // Point 1 Normal
                vertices.Add(normal.X);
                vertices.Add(normal.Y);
                vertices.Add(normal.Z);
                // Point 1 Color
                vertices.Add(color.X);
                vertices.Add(color.Y);
                vertices.Add(color.Z);
                // Point 2 Vertices
                vertices.Add(triangle.Point2.X);
                vertices.Add(triangle.Point2.Y);
                vertices.Add(triangle.Point2.Z);
                // Point 2 Normal
                vertices.Add(normal.X);
                vertices.Add(normal.Y);
                vertices.Add(normal.Z);
                // Point 2 Color
                vertices.Add(color.X);
                vertices.Add(color.Y);
                vertices.Add(color.Z);
                // Point 3 Vertices
                vertices.Add(triangle.Point3.X);
                vertices.Add(triangle.Point3.Y);
                vertices.Add(triangle.Point3.Z);
                // Point 3 Normal
                vertices.Add(normal.X);
                vertices.Add(normal.Y);
                vertices.Add(normal.Z);
                // Point 3 Color
                vertices.Add(color.X);
                vertices.Add(color.Y);
                vertices.Add(color.Z);
            }
            var verticesArr = vertices.ToArray();
            _vertices = verticesArr.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, verticesArr.Length * sizeof(float), verticesArr, BufferUsageHint.StaticDraw);
        }

        private float fTheta;
        public void Update(double time)
        {
            if (_rotate)
            {
                fTheta += 1f * (float)time;
                var zRotationMatrix = Matrix4.CreateRotationZ(fTheta);
                var xRotationMatrix = Matrix4.CreateRotationX(fTheta * 0.5f);

                var rot = zRotationMatrix * xRotationMatrix;
                _modelMatrix = rot * Matrix4.CreateTranslation(_position);
            }
        }

        public void Draw()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BindVertexArray(_vertexArrayObject);
            _shader.Use();
            var model = _modelMatrix;
            var normalMatrix = new Matrix3(Matrix4.Transpose(Matrix4.Invert(model)));
            _shader.SetMatrix4("model", ref model);
            _shader.SetMatrix3("modelNormals", ref normalMatrix);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices);
        }
    }
}
