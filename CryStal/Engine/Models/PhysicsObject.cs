using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine.Models
{
    public class PhysicsObject : GameObject
    {
        private Vector2 lastPos = Vector2.Zero;

        public float Drag = 1.0f;
        public bool HasGravity = true;
        private float responceStrength = 0.9f;

        private Vector2 _velocity = Vector2.Zero;
        private Vector2 _acceleration = Vector2.Zero;

        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        public Vector2 Acceleration
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }

        public virtual void Update(float deltaTime)
        {
            Velocity = (Position - lastPos) * Drag * responceStrength;
            lastPos = Position;

            Position = Position + Velocity + Acceleration * deltaTime;

            Acceleration = Vector2.Zero;
            responceStrength = 1.0f;
        }

        public void Accelerate(Vector2 acceleration)
        {
            Acceleration += acceleration;
        }
        public void ResetVelocity()
        {
            lastPos = Position;
        }
        public void halfVelocity()
        {
            responceStrength = 0.5f;
        }
        public void Clamp(GraphicsDevice graphics)
        {
            float maxX = graphics.Viewport.Width - Game1.TileSize;
            float maxY = graphics.Viewport.Height - Game1.TileSize;

            _position.X = Math.Clamp(Position.X, 0, maxX);
            _position.Y = Math.Clamp(Position.Y, 0, maxY);

            //this code stops bouncing off the edges of the screen. also makes blocks stack there more often.
            //lastPos.X = (_position.X == maxX) ? _position.X : lastPos.X;
            //lastPos.Y = (_position.Y == maxY) ? _position.Y : lastPos.Y;
        }
    }
}
