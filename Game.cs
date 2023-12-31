using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTkEngine
{
    internal class Game : GameWindow
    {
        
        // Shaders
        private Shader _basicShader;
        private Shader _sunShader;
        private Shader _texShader;

        // Textures
        private Texture _planeTex;
        private Texture _spyroTex;

        // FPS Stuff
        private double deltaTime;
        private int frameCount = 0;

        // Window Size
        private int _windowWidth;
        private int _windowHeight;

        // Transformation matrices
        private Matrix4 _projectionMatrix; // Transforms objects into clipping space
        // Camera
        private Camera _camera;
        private Vector2 _lastMousePos;
        private bool _firstMove;
        // Misc
        private float fTheta;

        // Models
        private Mesh _modelMesh;
        private List<RenderItem> _renderItems = new();

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
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
            GL.Enable(EnableCap.DepthTest);
            

            _basicShader = new Shader("Shaders/basicShader.vert", "Shaders/basicShader.frag");
            _sunShader = new Shader("Shaders/basicShader.vert", "Shaders/noLightShader.frag");
            _texShader = new Shader("Shaders/basicShader.vert", "Shaders/textureShader.frag");

            var aspect = _windowWidth / (float)_windowHeight;

            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(float.Pi / 2f, aspect, 0.1f, 1000f);

            _camera = new Camera(Vector3.Zero, aspect);
            CursorState = CursorState.Grabbed;

            _basicShader.SetMatrix4("projection", ref _projectionMatrix);
            _sunShader.SetMatrix4("projection", ref _projectionMatrix);
            _texShader.SetMatrix4("projection", ref _projectionMatrix);
            var light = new Vector3(0, 30.0f, 0f);
            _basicShader.SetVector3("lightPos", ref light);
            _sunShader.SetVector3("lightPos", ref light);
            _texShader.SetVector3("lightPos", ref light);

            _planeTex = new Texture("GameModels/AirplaneTexture.png");

            //_renderItems.Add(new RenderItem("GameModels/mountains.obj", new Vector3(10f, -15f, 30f), new Vector3(0.7f, 0.4f, 0.1f), _basicShader));
            //_renderItems.Add(new RenderItem("GameModels/teapot.obj", new Vector3(-35f, -14.5f, 50f), new Vector3(0f, 1f, 0f), _basicShader));
            //_renderItems.Add(new RenderItem("GameModels/cube.obj", new Vector3(0, 30, 0), Vector3.One, _sunShader));

            _renderItems.Add(new RenderItem("GameModels/Airplane.obj", new Vector3(0, 0, 0), new(1, 0, 0), _texShader, hasTexture: true, texture: _planeTex, scale: 0.01f));

            //_spyroTex = new Texture("GameModels/spyro.png");
            //_renderItems.Add(new RenderItem("GameModels/spyro.obj", Vector3.Zero, Vector3.One, _texShader, true, false, 1f, _spyroTex));

            //_renderItems.Add(new RenderItem("GameModels/teapot.obj", new Vector3(-35f, 0f, 45f), new Vector3(0f, 1f, 0f), _basicShader, rotate: true));
            //_renderItems.Add(new RenderItem("GameModels/teapot.obj", new Vector3(-30f, 0f, 45f), new Vector3(0f, 1f, 0f), _basicShader, rotate: true));
            //_renderItems.Add(new RenderItem("GameModels/teapot.obj", new Vector3(-25f, 0f, 45f), new Vector3(0f, 1f, 0f), _basicShader, rotate: true));
            //_renderItems.Add(new RenderItem("GameModels/teapot.obj", new Vector3(-20f, 0f, 45f), new Vector3(0f, 1f, 0f), _basicShader, rotate: true));
            //_renderItems.Add(new RenderItem("GameModels/teapot.obj", new Vector3(-15f, 0f, 45f), new Vector3(0f, 1f, 0f), _basicShader, rotate: true));
            //_renderItems.Add(new RenderItem("GameModels/teapot.obj", new Vector3(-10f, 0f, 45f), new Vector3(0f, 1f, 0f), _basicShader, rotate: true));
            //_renderItems.Add(new RenderItem("GameModels/teapot.obj", new Vector3(-5f, 0f, 45f), new Vector3(0f, 1f, 0f), _basicShader, rotate: true));
            //_renderItems.Add(new RenderItem("GameModels/teapot.obj", new Vector3(0f, 0f, 45f), new Vector3(0f, 1f, 0f), _basicShader, rotate: true));

        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            
            var speed = 10f;
            var sensitivity = 0.2f * (float.Pi / 180f);

            if (KeyboardState.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * speed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * speed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * speed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * speed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * speed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * speed * (float)args.Time;
            }

            if (_firstMove)
            {
                _lastMousePos = new Vector2(MouseState.X, MouseState.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = MouseState.X - _lastMousePos.X;
                var deltaY = MouseState.Y - _lastMousePos.Y;
                _lastMousePos = new Vector2(MouseState.X, MouseState.Y);

                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
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

                Console.WriteLine($"Current Camera Pos: {_camera.Position.X},{_camera.Position.Y},{_camera.Position.Z}");
            }

            // Create rotation matrix
            fTheta += 1f * (float)args.Time;
            var zRotationMatrix = Matrix4.CreateRotationZ(fTheta);
            var xRotationMatrix = Matrix4.CreateRotationX(fTheta * 0.5f);

           // _modelMatrix = zRotationMatrix * xRotationMatrix;
            //_basicShader.SetMatrix4("model", ref _modelMatrix);
            var viewMatrix = _camera.GetViewMatrix();
            _basicShader.SetMatrix4("view", ref viewMatrix);
            _sunShader.SetMatrix4("view", ref viewMatrix);
            _texShader.SetMatrix4("view", ref viewMatrix);

            foreach (var renderItem in _renderItems)
            {
                renderItem.Update(args.Time);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _planeTex.Use();
            _texShader.SetInt("texture0", 0);

            foreach (var renderItem in _renderItems)
            {
                renderItem.Draw();
            }

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

            //GL.DeleteBuffer(_vertexBufferObject);
            //GL.DeleteVertexArray(_vertexArrayObject);

            _basicShader.Dispose();
            base.OnUnload();
        }
    }
}
