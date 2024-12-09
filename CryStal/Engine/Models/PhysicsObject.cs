using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;

namespace CryStal.Engine.Models
{
    public class PhysicsObject : GameObject
    {
        private Body _body;
        private Fixture _fixture;
        public override Vector2 Position
        {
            get
            {
                return _body.Position * Game1.TILESIZE;
            }
            set
            {
                _body.Position = value * 0.02083333333333333333f;
            }
        }
        public PhysicsObject(World world, float width = Game1.TILESIZE, float height = Game1.TILESIZE, string id = "B") : 
            base(width, height, id)
        {
            _body = world.CreateBody(bodyType: BodyType.Dynamic);
            _fixture = _body.CreateRectangle(width * 0.02083333333333333333f, height * 0.02083333333333333333f, 1, Vector2.Zero);
        }
        public PhysicsObject(World world) : base()
        {
            _body = world.CreateBody(bodyType: BodyType.Dynamic);
            _fixture = _body.CreateRectangle(48 * 0.02083333333333333333f, 48 * 0.02083333333333333333f, 1, Vector2.Zero);
        }
        public virtual void Update(float deltaTime) { }
        public void Clamp(GraphicsDevice graphics)
        {
            float maxX = graphics.Viewport.Width - Game1.TILESIZE;
            float maxY = graphics.Viewport.Height - Game1.TILESIZE;

            Position = new Vector2(Math.Clamp(Position.X, 0, maxX), Math.Clamp(Position.Y, 0, maxY));
        }
        public void ApplyForce(Vector2 force)
        {
            _body.ApplyForce(force);
        }
        public void ApplyImpulse(Vector2 force)
        {
            _body.ApplyLinearImpulse(force);
        }
        public void ChangeVelocity(Vector2 desiredVel)
        {
            Vector2 vel = _body.LinearVelocity;
            Vector2 change = desiredVel - vel;
            Vector2 impulse = _body.Mass * change;
            _body.ApplyLinearImpulse(impulse);
        }
        public override void Unload(World world)
        {
            world.Remove(_body);
        }
        public override void Load(World world)
        {
            world.Add(_body);
        }
    }
}
