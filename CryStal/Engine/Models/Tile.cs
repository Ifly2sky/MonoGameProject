using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CryStal.Engine.Models
{
    public class Tile : GameObject
    {
        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                lastPos = value;
                base.Position = value;
            }
        }
        public Tile(Texture2D texture, string collisionType)
        {
            this.texture = texture;
            CollitionHandler.SetCollitonState(collisionType);

            allObjects.Add(this);
        }
        public Tile(Texture2D texture, string collisionType, Hitbox hitbox)
        {
            this.texture = texture;
            CollitionHandler.SetCollitonState(collisionType);
            Hitbox = hitbox;

            allObjects.Add(this);
        }
    }
}
