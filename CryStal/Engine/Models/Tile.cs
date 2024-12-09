using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;

namespace CryStal.Engine.Models
{
    public class Tile : GameObject
    {
        private bool collides = false;
        private Body _body;
        private Fixture _fixture;
        public override Vector2 Position
        {
            get
            {
                if (collides)
                {
                    return _body.Position * Game1.TILESIZE;
                }
                else { return _position; }
            }
            set
            {
                if (collides)
                {
                    _body.Position = value * 0.02083333333333333333f;
                }
                else
                {
                    _position = value;
                }
            }
        }
        public Tile() { }
        // if no specular map exists
        /*public Tile(Texture2D texture, Vector2 position, float width, float height, string id, World world = null) : base(position, width, height, id)
        {
            this.texture = texture;
            _body = world.CreateBody(new Vector2(0f, 0f), 0, BodyType.Static);
            _fixture = _body.CreateRectangle(width * 0.02083333333333333333f, height * 0.02083333333333333333f, 1, Vector2.Zero);
        }*/
        public Tile(Texture2D texture, World world, Texture2D specular = null, float width = Game1.TILESIZE, float height = Game1.TILESIZE)
        {
            this.texture = texture;
            this.specularMap = specular;
            collides = true;
            _body = world.CreateBody();
            _fixture = _body.CreateRectangle(width * 0.02083333333333333333f, height * 0.02083333333333333333f, 1, Vector2.Zero);
        }
        public Tile(Texture2D texture, Texture2D specular = null, float width = Game1.TILESIZE, float height = Game1.TILESIZE)
        {
            this.texture = texture;
            this.specularMap = specular;
        }
        public override void Unload(World world)
        {
            world.Remove(_body);
        }
        public override void Load(World world)
        {
            world.Add(_body);
        }
    }
}
