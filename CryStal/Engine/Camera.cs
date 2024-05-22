using Microsoft.Xna.Framework;

namespace CryStal.Engine
{
    public class Camera
    {
        Vector2 Position;
        Vector2 Size;
        public float X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }
        public float Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }
        public float Width
        {
            get { return Size.X; }
            set { Size.X = value; }
        }
        public float Height
        {
            get { return Size.Y; }
            set { Size.Y = value; }
        }
        public float Zoom
        {
            set
            {
                float deltaX = Width * (1 - value);
                float deltaY = Height * (1 - value);
                X += deltaX;
                Y += deltaY;
                Width *= value;
                Height *= value;
            }
        }
        public Camera(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }
        public void setLocation(Vector2 postition)
        {
            X = postition.X;
            Y = postition.X;
        }
        public Vector2 getLocation()
        {
            return Position;
        } 
    }
}
