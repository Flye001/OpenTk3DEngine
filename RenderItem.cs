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
        private float _scale;
        private Matrix4 _modelMatrix;
        private Texture _texture;

        private bool _rotate;

        public RenderItem(string modelPath, Vector3 position, Vector3 color, Shader shader, bool hasTexture = false, bool rotate = false, float scale = 1f, Texture texture = null)
        {
            Mesh mesh = new();
            if (!mesh.LoadFromObjectFile(modelPath, hasTexture))
            {
                throw new Exception("Failed to load obj file!");
            }

            _shader = shader;
            _texture = texture;
            _rotate = rotate;
            _position = position;
            _scale = scale;
            _modelMatrix = Matrix4.CreateTranslation(_position) * Matrix4.CreateScale(_scale);

            // Bind this items vertex array
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            // Vertices
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // Normals
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            // Colors
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);
            // Textures
            GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 11 * sizeof(float), 9 * sizeof(float));
            GL.EnableVertexAttribArray(3);

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
                // Point 1 Texture
                vertices.Add(triangle.TPoint1.X);
                vertices.Add(triangle.TPoint1.Y);
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
                // Point 2 Texture
                vertices.Add(triangle.TPoint2.X);
                vertices.Add(triangle.TPoint2.Y);
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
                // Point 3 Texture
                vertices.Add(triangle.TPoint3.X);
                vertices.Add(triangle.TPoint3.Y);
            }
            var verticesArr = vertices.ToArray();
            _vertices = verticesArr.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, verticesArr.Length * sizeof(float), verticesArr, BufferUsageHint.StaticDraw);
        }

        private float _fTheta;
        public void Update(double time)
        {
            if (_rotate)
            {
                _fTheta += 1f * (float)time;
                var zRotationMatrix = Matrix4.CreateRotationZ(_fTheta);
                var xRotationMatrix = Matrix4.CreateRotationX(_fTheta * 0.5f);

                var rot = zRotationMatrix * xRotationMatrix;
                _modelMatrix = rot * Matrix4.CreateTranslation(_position) * Matrix4.CreateScale(_scale);
            }
        }

        public void Draw()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BindVertexArray(_vertexArrayObject);

            if (_texture != null)
            {
                _texture.Use();
                _shader.SetInt("texture0", 0);
            }
            _shader.Use();

            var model = _modelMatrix;
            var normalMatrix = new Matrix3(Matrix4.Transpose(Matrix4.Invert(model)));
            _shader.SetMatrix4("model", ref model);
            _shader.SetMatrix3("modelNormals", ref normalMatrix);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices);
        }
    }
}
