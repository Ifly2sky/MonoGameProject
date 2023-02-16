using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.Entities
{
    public class Player
    {
        Texture2D _texture;
        KeyboardState keyboardState;

        float _speed;
        Vector2 _velocity;
        Vector2 _position;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public Player(Vector2 position, float speed)
        {
            Position = position;
            Speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            keyboardState = Keyboard.GetState();

            MovePlayer(deltaTime, keyboardState);

            Position += Velocity * Speed * deltaTime;

            Velocity = Vector2.Zero;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }
        void MovePlayer(float deltaTime, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W))
            {
                _velocity.Y -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                _velocity.Y += 1;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                _velocity.X -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                _velocity.X += 1;
            }
        }
    }
}
