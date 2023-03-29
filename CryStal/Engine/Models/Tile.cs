using Microsoft.Xna.Framework.Graphics;

namespace CryStal.Engine.Models
{
    public class Tile : GameObject
    {
        public Tile(Texture2D texture, CollitionType collisionType)
        {
            this.texture = texture;
            CollisionType = collisionType;
        }
    }
}
