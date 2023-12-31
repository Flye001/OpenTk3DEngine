using OpenTK.Mathematics;

namespace OpenTkEngine
{
    internal class Camera
    {
        public Vector3 Position { get; set; }

        private Vector3 _front = -Vector3.UnitZ;
        public Vector3 Front => _front;

        private Vector3 _up = Vector3.UnitY;
        public Vector3 Up => _up;

        private Vector3 _right = Vector3.UnitX;
        public Vector3 Right => _right;

        private float _pitch;
        private float _yaw = float.Pi / 2f;

        public float Pitch
        {
            get => _pitch;
            set
            {
                var angle = float.Clamp(value, -89 * (float.Pi / 180f), 89 * (float.Pi / 180f));
                _pitch = angle;
                UpdateVectors();
            }
        }
        public float Yaw
        {
            get => _yaw;
            set
            {
                _yaw = value;
                UpdateVectors();
            }
        }

        private void UpdateVectors()
        {
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);
            _front = Vector3.Normalize(_front);
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }

        private float _aspectRation;

        public Camera(Vector3 position, float aspectRation)
        {
            Position = position;
            _aspectRation = aspectRation;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }
    }
}
