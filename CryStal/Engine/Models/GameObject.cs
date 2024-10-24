using CryStal.StateMachines.CollitionStateMachine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;

namespace CryStal.Engine.Models
{
    public class GameObject
    {
        protected Body _body;
        protected Fixture _fixture;

        float _width;
        float _height;

        public Texture2D texture;
        public Texture2D specularMap;

        public string ID;
        public bool isKillable = false;
        private bool _isAlive = true;

        public virtual Vector2 Position 
        {
            get { return _body.Position; }
            set { _body.Position = value; }
        }
        public float Width { get; set; }
        public float Height { get; set; }
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
        public GameObject(World world)
        {
            _body = world.CreateBody(new Vector2(0f, 0f), 0, BodyType.Static);
            _fixture = _body.CreateRectangle(Game1.TILESIZE, Game1.TILESIZE, 1, Vector2.Zero);
        }
        public GameObject(World world, float width, float height)
        {
            _body = world.CreateBody(new Vector2(0f, 0f), 0, BodyType.Static);
            _fixture = _body.CreateRectangle(Game1.TILESIZE, Game1.TILESIZE, 1, Vector2.Zero);
        }
        public GameObject(World world, Vector2 position, float width, float height, string id)
        {
            _body = world.CreateBody(Position, 0, BodyType.Static);
            _fixture = _body.CreateRectangle(width, height, 1, Vector2.Zero);
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
