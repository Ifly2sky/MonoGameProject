using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CryStal.Engine.Models
{
    /// <summary>
    /// Checks what kind of behavior an object has when colliding
    /// </summary>
    public enum CollitionType
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
    }
    public class GameObject
    {
        protected Vector2 _position = Vector2.Zero;

        protected Vector2 lastPos = Vector2.Zero;
        public Texture2D texture;
        public CollitionType CollisionType = CollitionType.Impassable;
        public virtual Vector2 Position 
        {
            get { return _position; } 
            set { _position = value; }
        }
        public Vector2 Center
        {
            get { return _position + Hitbox.Position + Hitbox.Size * 0.5f; }
        }
        public Vector2 LastCenter
        {
            get { return lastPos + Hitbox.Position + Hitbox.Size * 0.5f; }
        }

        public Hitbox Hitbox { get; set; } = new Hitbox();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 0f);
        }
    }
}
