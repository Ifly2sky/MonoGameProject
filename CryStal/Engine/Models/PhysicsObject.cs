using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;

namespace CryStal.Engine.Models
{
    public class PhysicsObject : GameObject
    {
        public PhysicsObject(World world, Vector2 Position, float width = Game1.TILESIZE, float height = Game1.TILESIZE, string id = "B") : 
            base(world, Position, width, height, id) { }
        public PhysicsObject(World world) : base(world) { }
        public virtual void Update(float deltaTime) { }
        public void Clamp(GraphicsDevice graphics)
        {
            float maxX = graphics.Viewport.Width - Game1.TILESIZE;
            float maxY = graphics.Viewport.Height - Game1.TILESIZE;

            Position = new Vector2(Math.Clamp(Position.X, 0, maxX), Math.Clamp(Position.Y, 0, maxY));
        }
    }
}
