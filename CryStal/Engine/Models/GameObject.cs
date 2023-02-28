﻿using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Engine.Models
{
    public class GameObject
    {
        private Vector2 lastPos = Vector2.Zero;

        public float Drag = 0.9f;
        public bool HasGravity = true;

        private Vector2 _position = Vector2.Zero;
        private Vector2 _velocity = Vector2.Zero;
        private Vector2 _acceleration = Vector2.Zero;
        public Vector2 Position 
        {
            get { return _position; } 
            set { _position = value; }
        }
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
        public Vector2 Center
        {
            get
            {
                return Position + Hitbox.Position + Hitbox.Size * 0.5f;
            }
        }

        public Hitbox Hitbox { get; set; } = new Hitbox();
        public Texture2D texture;

        public virtual void Update(float deltaTime, GraphicsDevice graphics)
        {

            Velocity = (Position - lastPos) * Drag;
            lastPos = Position;

            Position = Position + Velocity + Acceleration * deltaTime;

            _position.X = MathHelper.Clamp(Position.X, 0, graphics.Viewport.Width - Game1.TileSize);
            _position.Y = MathHelper.Clamp(Position.Y, 0, graphics.Viewport.Height - Game1.TileSize);

            Acceleration = Vector2.Zero;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 0f);
        }

        public void Accelerate(Vector2 acceleration)
        {
            Acceleration += acceleration;
        }
        public void ResetVelocity()
        {
            lastPos = Position;
        }
    }
}
