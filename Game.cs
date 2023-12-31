using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTkEngine
{
    internal class Game : GameWindow
    {

        // OpenGL stuff
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private Shader _basicShader;
        private Shader _basicRedShader;

        // FPS Stuff
        private double deltaTime;
        private int frameCount = 0;

        // Window Size
        private int _windowWidth;
        private int _windowHeight;

        // Window Stuff
        private Matrix4 _modelMatrix; // Moves local object into world space
        private Matrix4 _viewMatrix; // Transforms objects into camera view space
        private Matrix4 _projectionMatrix; // Transforms objects into clipping space
        private float fTheta;
        private int vertexBufferLength;
        private int _vertexCount;

        // Models
        private Mesh _modelMesh;

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() {ClientSize = (width, height), Title = title})
        {
            // Init mesh
            _modelMesh = new();
            _windowWidth = width;
            _windowHeight = height;
        }

        protected override void OnLoad()
        {
            // OpenGL Setup
            base.OnLoad();

            GL.ClearColor(0f, 0f, 0f, 1f);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            // Vertices
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // Colors
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            _basicShader = new Shader("Shaders/basicShader.vert", "Shaders/basicShader.frag");
            _basicRedShader = new Shader("Shaders/basicShader.vert", "Shaders/basicShaderRed.frag");

            // Load Mesh
            if (!_modelMesh.LoadFromObjectFile("GameModels/teapot.obj"))
            {
                throw new Exception("Failed to load obj file!");
            }

            var aspect = _windowHeight / (float)_windowWidth;
            _modelMatrix = Matrix4.CreateRotationZ(1f);
            _viewMatrix = Matrix4.CreateTranslation(0f, 0f, 5f);
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(float.Pi / 2f, aspect, 0.1f, 100f);


            _basicShader.SetMatrix4("model", ref _modelMatrix);
            _basicShader.SetMatrix4("view", ref _viewMatrix);
            _basicShader.SetMatrix4("projection", ref _projectionMatrix);


            // Load in points
            Vector3 Color = new(1f, 1f, 0f);
            List<float> vertices = new();
            foreach (var triangle in _modelMesh.Triangles)
            {
                // Point 1 vertices
                vertices.Add(triangle.Point1.X);
                vertices.Add(triangle.Point1.Y);
                vertices.Add(triangle.Point1.Z);
                // Point 1 Color
                vertices.Add(Color.X);
                vertices.Add(Color.Y);
                vertices.Add(Color.Z);
                // Point 2 Vertices
                vertices.Add(triangle.Point2.X);
                vertices.Add(triangle.Point2.Y);
                vertices.Add(triangle.Point2.Z);
                // Point 2 Color
                vertices.Add(Color.X);
                vertices.Add(Color.Y);
                vertices.Add(Color.Z);
                // Point 3 Vertices
                vertices.Add(triangle.Point3.X);
                vertices.Add(triangle.Point3.Y);
                vertices.Add(triangle.Point3.Z);
                // Point 3 Color
                vertices.Add(Color.X);
                vertices.Add(Color.Y);
                vertices.Add(Color.Z);
            }

            var verticesArr = vertices.ToArray();

            // Buffer custom model
            vertexBufferLength = verticesArr.Length * sizeof(float);
            _vertexCount = verticesArr.Length / 3;
            GL.BufferData(BufferTarget.ArrayBuffer, vertexBufferLength, verticesArr, BufferUsageHint.StaticDraw);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            // Print FPS
            deltaTime += args.Time;
            frameCount++;

            if (deltaTime >= 1)
            {
                var fps = frameCount / deltaTime;
                Console.WriteLine("FPS: " + fps);
                deltaTime = 0;
                frameCount = 0;
            }

            // Create rotation matrix
            fTheta += 1f * (float)args.Time;
            var zRotationMatrix = Matrix4.CreateRotationZ(fTheta);
            var xRotationMatrix = Matrix4.CreateRotationX(fTheta * 0.5f);

            _modelMatrix = zRotationMatrix * xRotationMatrix;
            _basicShader.SetMatrix4("model", ref _modelMatrix);

            //// Generate Cube vectors
            //var translateMatrix = Matrix4.CreateTranslation(0f, 0f, 5f); // Move object backwards
            //var worldMatrix = zRotationMatrix * xRotationMatrix;
            //worldMatrix *= translateMatrix;

            //List<float> vertices = new();

            //// Set the transformation
            //var finalTransformation = worldMatrix * _projectionMatrix;
            //_basicRedShader.Use();
            //var projMatLocationRed = GL.GetUniformLocation(_basicRedShader.Handle, "projMatrix");
            //GL.UniformMatrix4(projMatLocationRed, true, ref finalTransformation);
            //_basicShader.Use();
            //var projMatLocationBlue = GL.GetUniformLocation(_basicShader.Handle, "projMatrix");
            //GL.UniformMatrix4(projMatLocationBlue, true, ref finalTransformation);

            //Vector3 Color = new(1f, 1f, 0f);

            //foreach (var triangle in _modelMesh.Triangles)
            //{
            //    // Point 1 vertices
            //    vertices.Add(triangle.Point1.X);
            //    vertices.Add(triangle.Point1.Y);
            //    vertices.Add(triangle.Point1.Z);
            //    // Point 1 Color
            //    vertices.Add(Color.X);
            //    vertices.Add(Color.Y);
            //    vertices.Add(Color.Z);
            //    // Point 2 Vertices
            //    vertices.Add(triangle.Point2.X);
            //    vertices.Add(triangle.Point2.Y);
            //    vertices.Add(triangle.Point2.Z);
            //    // Point 2 Color
            //    vertices.Add(Color.X);
            //    vertices.Add(Color.Y);
            //    vertices.Add(Color.Z);
            //    // Point 3 Vertices
            //    vertices.Add(triangle.Point3.X);
            //    vertices.Add(triangle.Point3.Y);
            //    vertices.Add(triangle.Point3.Z);
            //    // Point 3 Color
            //    vertices.Add(Color.X);
            //    vertices.Add(Color.Y);
            //    vertices.Add(Color.Z);
            //}

            //var verticesArr = vertices.ToArray();

            //// Buffer custom model
            //vertexBufferLength = verticesArr.Length * sizeof(float);
            //_vertexCount = verticesArr.Length / 3;
            //GL.BufferData(BufferTarget.ArrayBuffer, vertexBufferLength, verticesArr, BufferUsageHint.StaticDraw);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            
            GL.BindVertexArray(_vertexArrayObject);
            // Draw solid triangles
            _basicShader.Use();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexCount);
            // Draw Wireframe
            //_basicRedShader.Use();
            //GL.DrawArrays(PrimitiveType.LineLoop, 0, _vertexCount);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);

            _basicShader.Dispose();
            base.OnUnload();
        }
    }
}
