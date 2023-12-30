using OpenTK.Graphics.OpenGL4;
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

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private Shader _basicBlueShader;
        private Shader _basicRedShader;

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() {ClientSize = (width, height), Title = title})
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0f, 0f, 0f, 1f);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            _basicBlueShader = new Shader("Shaders/basicShader.vert", "Shaders/basicShaderBlue.frag");
            _basicRedShader = new Shader("Shaders/basicShader.vert", "Shaders/basicShaderRed.frag");
            _basicBlueShader.Use();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            
            GL.BindVertexArray(_vertexArrayObject);
            // Draw full circles
            _basicBlueShader.Use();
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            // Draw Wireframe
            _basicRedShader.Use();
            GL.DrawArrays(PrimitiveType.LineLoop, 0, 3);
            
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
