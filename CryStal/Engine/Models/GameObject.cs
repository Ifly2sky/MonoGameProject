using CryStal.StateMachines.CollitionStateMachine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CryStal.Engine.Models
{
    public class GameObject
    {
        protected Vector2 _position = Vector2.Zero;
        protected Vector2 _center = Vector2.Zero;
        protected Vector2 lastPos = Vector2.Zero;

        public Texture2D texture;
        public Texture2D specularMap;
        public CollitionStateMachine CollitionHandler = new CollitionStateMachine();

        public string ID;
        public bool isKillable = false;
        private bool _isAlive = true;

        public static List<GameObject> allObjects = new();

        public virtual Vector2 Position 
        {
            get { return _position; } 
            set 
            { 
                _position = value;
                _center = _position + Hitbox.Position + Hitbox.Size * 0.5f;
            }
        }
        public Vector2 Center
        {
            get { return _center; }
        }
        public Vector2 LastCenter
        {
            get { return lastPos + Hitbox.Position + Hitbox.Size * 0.5f; }
        }
        public bool IsAlive
        {
            get
            {
                return _isAlive;
            }
            protected set 
            { 
                _isAlive = value; 
            }
        }

        public Hitbox Hitbox { get; set; } = new Hitbox();
        public virtual void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Vector2 drawPos = Position;
            drawPos.X -= camera.X;
            drawPos.Y -= camera.Y;
            if (drawPos.X < camera.Width &&
                drawPos.X > -Hitbox.Size.X &&
                drawPos.Y < camera.Height &&
                drawPos.Y > -Hitbox.Size.Y)
            {
                spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, Vector2.Zero, Game1.SCALE, SpriteEffects.None, 0f);
            }
        }
        public float DistanceTo(Vector2 position)
        {
            return (Center - position).Abs().Length();
        }
        public Vector2 VectorDistanceTo(Vector2 position)
        {
            return (Center - position).Abs();
        }
        public virtual void Unload()
        {
            allObjects.Remove(this);
        }
        public virtual void Load()
        {
            allObjects.Add(this);
        }
        public void SetDead()
        {
            _isAlive = false;
        }
        public void SetAlive()
        {
            _isAlive = true;
        }
    }
}
