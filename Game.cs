using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTkEngine
{
    internal class Game : GameWindow
    {
        float[] _vertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
            0.5f, -0.5f, 0.0f, //Bottom-right vertex
            0.0f,  0.5f, 0.0f  //Top vertex
        };

        // OpenGL stuff
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private Shader _basicBlueShader;
        private Shader _basicRedShader;

        // FPS Stuff
        private double deltaTime;
        private int frameCount = 0;

        // Window Size
        private int _windowWidth;
        private int _windowHeight;

        // Window Stuff
        private Matrix4 _projectionMatrix;
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
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            _basicBlueShader = new Shader("Shaders/basicShader.vert", "Shaders/basicShaderBlue.frag");
            _basicRedShader = new Shader("Shaders/basicShader.vert", "Shaders/basicShaderRed.frag");

            // Load Mesh
            if (!_modelMesh.LoadFromObjectFile("GameModels/teapot.obj"))
            {
                throw new Exception("Failed to load obj file!");
            }

            var aspect = _windowHeight / (float)_windowWidth;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(float.Pi / 2f, aspect, 0.1f, 1000f);
            //var m = Matrix4.CreateTranslation(0.5f, 0.5f, 0f);
            var m = Matrix4.Identity;

            //_basicRedShader.Use();
            //var projMatLocationRed = GL.GetUniformLocation(_basicRedShader.Handle, "projMatrix");
            //GL.UniformMatrix4(projMatLocationRed, true, ref m);
            //_basicBlueShader.Use();
            //var projMatLocationBlue = GL.GetUniformLocation(_basicBlueShader.Handle, "projMatrix");
            //GL.UniformMatrix4(projMatLocationBlue, true, ref m);
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

            // Generate Cube vectors
            var translateMatrix = Matrix4.CreateTranslation(0f, 0f, 5f); // Move object backwards
            var worldMatrix = zRotationMatrix * xRotationMatrix;
            worldMatrix *= translateMatrix;

            List<float> vertices = new();

            // Set the transformation
            var finalTransformation = worldMatrix * _projectionMatrix;
            _basicRedShader.Use();
            var projMatLocationRed = GL.GetUniformLocation(_basicRedShader.Handle, "projMatrix");
            GL.UniformMatrix4(projMatLocationRed, true, ref finalTransformation);
            _basicBlueShader.Use();
            var projMatLocationBlue = GL.GetUniformLocation(_basicBlueShader.Handle, "projMatrix");
            GL.UniformMatrix4(projMatLocationBlue, true, ref finalTransformation);

            foreach (var triangle in _modelMesh.Triangles)
            {
                // Add vertices to 
                vertices.Add(triangle.Point1.X);
                vertices.Add(triangle.Point1.Y);
                vertices.Add(triangle.Point1.Z);
                //vertices.Add(0);
                vertices.Add(triangle.Point2.X);
                vertices.Add(triangle.Point2.Y);
                vertices.Add(triangle.Point2.Z);
                //vertices.Add(0);
                vertices.Add(triangle.Point3.X);
                vertices.Add(triangle.Point3.Y);
                vertices.Add(triangle.Point3.Z);
                //vertices.Add(0);
            }

            var verticesArr = vertices.ToArray();
            
            // Buffer custom model
            vertexBufferLength = verticesArr.Length * sizeof(float);
            _vertexCount = verticesArr.Length / 3;
            GL.BufferData(BufferTarget.ArrayBuffer, vertexBufferLength, verticesArr, BufferUsageHint.StaticDraw);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            
            GL.BindVertexArray(_vertexArrayObject);
            // Draw solid triangles
            _basicBlueShader.Use();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexCount);
            // Draw Wireframe
            _basicRedShader.Use();
            GL.DrawArrays(PrimitiveType.LineLoop, 0, _vertexCount);

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

            _basicBlueShader.Dispose();
            base.OnUnload();
        }
    }
}
