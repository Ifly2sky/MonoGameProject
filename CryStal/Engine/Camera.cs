using Microsoft.Xna.Framework;

namespace CryStal.Engine
{
    public class Camera
    {
        public Vector2 Position;
        public Rectangle Crop;
        public Vector2 Scale;

        public Camera(Vector2 position, Rectangle crop, Vector2 scale)
        {
            Position = position;
            Crop = crop;
            Scale = scale;
        }
    }
}
