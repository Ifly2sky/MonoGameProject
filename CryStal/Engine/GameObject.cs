using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine
{
    public class GameObject
    {
        private Vector2 _LastPos = Vector2.Zero;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public Vector2 Acceleration { get; set; } = Vector2.Zero;

        public Hitbox Hitbox { get; set; } = new Hitbox();

        public virtual void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Velocity = Position - _LastPos;

            _LastPos = Position;

            Position = Position + Velocity + Acceleration * deltaTime * deltaTime;

            Acceleration = Vector2.Zero;
        }

        public void Accelerate(Vector2 acceleration)
        {
            Acceleration = acceleration;
        }
    }
}
