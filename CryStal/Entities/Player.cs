﻿using CryStal.Engine;
using CryStal.Engine.Models;
using CryStal.Engine.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Drawing;

namespace CryStal.Entities
{
    public class Player : PhysicsObject
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
            Drag = new Vector2(0.8f, 1);
            HasGravity = true;

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
            spriteBatch.Draw(Texture, Position, null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Game1.Arial, $"On ground: {grounded}", new Vector2(4, 48), Microsoft.Xna.Framework.Color.WhiteSmoke); 
            spriteBatch.DrawString(Game1.Arial, $"Grid Pos: {Grid.GetGridCoordinates(Position)}", new Vector2(4, 64), Microsoft.Xna.Framework.Color.WhiteSmoke);
        }
        bool jumped = false;
        bool grounded;
        void MovePlayer(KeyboardState keyboardState)
        {
            grounded = IsGrounded();
            if (keyboardState.IsKeyDown(Keys.Space) && !jumped && grounded)
            {
                ResetVelocityY();
                Accelerate(new Vector2(0, -Speed * 10));
                jumped = true;
            }
            else if (keyboardState.IsKeyUp(Keys.Space) && jumped)
            {
                jumped = false;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                
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
        private bool IsGrounded()
        {
            Size cellOnGrid = Grid.GetGridCoordinates(Center);
            Cell cellBelow = Grid.GetCell(cellOnGrid.Width, cellOnGrid.Height + 1);
            if(cellBelow != null)
            {
                return cellBelow.Objects.Any(x => x is Tile && x.CollisionType == CollitionType.Impassable);
            }
            return false;
        }
    }
}
