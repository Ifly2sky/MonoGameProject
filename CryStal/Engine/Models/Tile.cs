using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;

namespace CryStal.Engine.Models
{
    public class Tile : GameObject
    {
        public Tile() { }
        public Tile(World world, Texture2D texture, Vector2 position, float width, float height, string id) : base(world, true, position, width, height, id)
        {
            this.texture = texture;
        }
        public Tile(World world, bool collides, Texture2D texture, Texture2D specular = null, float width = Game1.TILESIZE, float height = Game1.TILESIZE) : base(world, true, collides, width, height)
        {
            this.texture = texture;
            this.specularMap = specular;
        }
        public Tile(World world, Texture2D texture, Texture2D specular, Vector2 position, float width, float height, string id) : base(world, true, position, width, height, id)
        {
            this.texture = texture;
            this.specularMap = specular;
        }
    }
}
