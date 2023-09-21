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
        public Tile(Texture2D texture, CollitionType collisionType)
        {
            this.texture = texture;
            CollisionType = collisionType;

            allObjects.Add(this);
        }
        public Tile(Texture2D texture, CollitionType collisionType, Hitbox hitbox)
        {
            this.texture = texture;
            CollisionType = collisionType;
            Hitbox = hitbox;

            allObjects.Add(this);
        }
        public void UnloadTile()
        {
            allObjects.Remove(this);
        }
    }
}
