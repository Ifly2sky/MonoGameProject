﻿using CryStal.StateMachines.CollitionStateMachine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CryStal.Engine.Models
{
    /// <summary>
    /// Checks what kind of behavior an object has when colliding
    /// </summary>
    /*public enum CollitionType
    {
        /// <summary>
        /// A Passable object is an object with no collition
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An Impassable object is one with collition
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// A Platform is only passable from the sides and below but not from the top.
        /// </summary>
        Platform = 2,

        /// <summary>
        /// A Spike damages any entity that hits it.
        /// </summary>
        Spike = 3,
    }*/
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
            Vector2 drawPos = (Position - camera.Position) * camera.Scale;
            if (drawPos.X > (camera.Crop.Left) * camera.Scale.X - Hitbox.Size.X &&
                drawPos.X < (camera.Crop.Right) * camera.Scale.X &&
                drawPos.Y > (camera.Crop.Top) * camera.Scale.Y - Hitbox.Size.Y &&
                drawPos.Y < (camera.Crop.Bottom) * camera.Scale.Y)
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
