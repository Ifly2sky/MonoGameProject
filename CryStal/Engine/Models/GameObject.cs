using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CryStal.Engine.Models
{
    public class GameObject
    {
        protected Vector2 _position = Vector2.Zero;

        public Texture2D texture;
        public Vector2 Position 
        {
            get { return _position; } 
            set { _position = value; }
        }
        public Vector2 Center
        {
            get { return _position + Hitbox.Position + Hitbox.Size * 0.5f; }
        }

        public Hitbox Hitbox { get; set; } = new Hitbox();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 0f);
        }
    }
}
