using CryStal.Engine;
using CryStal.Engine.Models;
using CryStal.Engine.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Entities
{
    public class Player : GameObject
    {
        Texture2D _texture;
        KeyboardState keyboardState;

        float _speed;
        Vector2 _direction;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public Player(Vector2 position, float speed):base()
        {
            Position = position;
            Speed = speed;
            ResetVelocity();

            Hitbox.Size = new Vector2(Game1.TileSize, Game1.TileSize);
            Hitbox.Position = Vector2.Zero;

            GameObjectFactory.AddGameObject(this);
        }

        public override void Update(float deltaTime)
        {
            keyboardState = Keyboard.GetState();
            MovePlayer(keyboardState);

            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 0f);
        }
        void MovePlayer(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W))
            {
                Accelerate(new Vector2(0, -Speed));
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                Accelerate(new Vector2(0, Speed));
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                Accelerate(new Vector2(-Speed, 0));
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                Accelerate(new Vector2(Speed, 0));
            }
        }
    }
}
