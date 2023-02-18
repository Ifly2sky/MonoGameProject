using CryStal.Engine;
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

        public Player(Vector2 position, float speed)
        {
            Position = position;
            Speed = speed;
            Hitbox.Size = new Vector2(Game1.TileSize, Game1.TileSize);
            Hitbox.Position = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            keyboardState = Keyboard.GetState();

            MovePlayer(deltaTime, keyboardState);

            base.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 0f);
        }
        void MovePlayer(float deltaTime, KeyboardState keyboardState)
        {
            CheckKey();

            Position += Direction * Speed * deltaTime;

            Direction = Vector2.Zero;
        }
        void CheckKey()
        {
            if (keyboardState.IsKeyDown(Keys.W))
            {
                _direction.Y -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                _direction.Y += 1;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                _direction.X -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                _direction.X += 1;
            }
        }
    }
}
