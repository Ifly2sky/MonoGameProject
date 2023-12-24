using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CryStal.Engine.Models
{
    public class PhysicsObject : GameObject
    {
        public Vector2 Drag = new Vector2(1f, 1f);
        public bool HasGravity = true;
        public static List<PhysicsObject> allPhysicsObjects = new();

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

        public PhysicsObject(Hitbox hitbox, Vector2 Position, string collitionState, string id = "B")
        {
            this.Position = Position;
            this.Hitbox = hitbox;
            ID = id;
            ResetVelocity();

            CollitionHandler.SetCollitonState(collitionState);
            allObjects.Add(this);
            allPhysicsObjects.Add(this);
        }
        public PhysicsObject()
        {
            allObjects.Add(this);
            allPhysicsObjects.Add(this);
        }

        public virtual void Update(float deltaTime)
        {
            Velocity = (Position - lastPos) * Drag;
            lastPos = Position;

            Position = Position + Velocity + Acceleration * deltaTime;

            Acceleration = Vector2.Zero;
        }

        public void Accelerate(Vector2 acceleration)
        {
            Acceleration += acceleration;
        }
        public void ResetVelocity()
        {
            lastPos = Position;
        }
        public void ResetVelocityY()
        {
            lastPos.Y = Position.Y;
        }
        public void Clamp(GraphicsDevice graphics)
        {
            float maxX = graphics.Viewport.Width - Game1.TileSize;
            float maxY = graphics.Viewport.Height - Game1.TileSize;

            Position = new Vector2(Math.Clamp(Position.X, 0, maxX), Math.Clamp(Position.Y, 0, maxY));

            //this code stops bouncing off the edges of the screen. also makes blocks stack there more often.
            //lastPos.X = (_position.X == maxX) ? _position.X : lastPos.X;
            //lastPos.Y = (_position.Y == maxY) ? _position.Y : lastPos.Y;
        }
        public override void Unload()
        {
            allObjects.Remove(this);
            allPhysicsObjects.Remove(this);
        }
        public override void Load()
        {
            allObjects.Add(this);
            allPhysicsObjects.Add(this);
        }
    }
}
