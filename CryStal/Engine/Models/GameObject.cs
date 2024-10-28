using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;

namespace CryStal.Engine.Models
{
    public class GameObject
    {
        protected Body _body;
        protected Fixture _fixture;

        bool collides;

        float _width;
        float _height;

        public Texture2D texture;
        public Texture2D specularMap;

        public string ID;
        public bool isKillable = false;
        private bool _isAlive = true;

        public virtual Vector2 Position 
        {
            get 
            { 
                return collides ? _body.Position * Game1.TILESIZE : Vector2.Zero; 
            }
            set 
            {
                if (collides)
                    _body.Position = value * 0.02083333333333333333f;
            }
        }
        public float Width { 
            get { return _width; } 
            set { _width = value; } 
        }
        public float Height {
            get { return _height; }
            set { _height = value; }
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
        public GameObject() { }
        public GameObject(World world, bool isStatic, bool collides, float width = Game1.TILESIZE, float height = Game1.TILESIZE)
        {
            this.collides = collides;
            if (isStatic)
                _body = world.CreateBody(new Vector2(0f, 0f), 0, BodyType.Static);
            else
                _body = world.CreateBody(new Vector2(0f, 0f), 0, BodyType.Dynamic);
            if (collides)
                _fixture = _body.CreateRectangle(width * 0.02083333333333333333f, height * 0.02083333333333333333f, 1, Vector2.Zero);
        }
        public GameObject(World world, bool isStatic, Vector2 position, float width, float height, string id)
        {
            if (isStatic)
                _body = world.CreateBody(position, 0, BodyType.Static);
            else
                _body = world.CreateBody(position, 0, BodyType.Dynamic);
            _fixture = _body.CreateRectangle(width * 0.02083333333333333333f, height * 0.02083333333333333333f, 1, Vector2.Zero);
            this.collides = true;
            this.Width = width;
            this.Height = height;
            ID = id;
        }
        public virtual void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Vector2 drawPos = Position;
            drawPos.X -= camera.X;
            drawPos.Y -= camera.Y;
            if (drawPos.X < camera.Width &&
                drawPos.X > -Width &&
                drawPos.Y < camera.Height &&
                drawPos.Y > -Height)
            {
                spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, Vector2.Zero, Game1.SCALE, SpriteEffects.None, 0f);
            }
        }
        public virtual void Unload(World world)
        {
            world.Remove(_body);
        }
        public virtual void Load(World world)
        {
            world.Add(_body);
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
