﻿using CryStal.Engine;
using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using CryStal.StateMachines.PlayerStateMachine;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;

namespace CryStal.Entities
{
    public class Player : PhysicsObject
    {
        Texture2D _texture;
        KeyboardState _keyboardState;
        StateMachine _state = new StateMachine();

        public float speed;
        public float jumpForce;
        Vector2 _direction;
        public bool isGrounded;
        public bool isCrouching;

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

        public Player(Vector2 position, float speed, float jumpForce = 100, string id = "P"):base()
        {
            Position = position;
            this.speed = speed;
            this.jumpForce = jumpForce;
            ResetVelocity();

            Hitbox.Size = new Vector2(Game1.TileSize, Game1.TileSize);
            Hitbox.Position = Vector2.Zero;
            Drag = new Vector2(0.7f, 1.00f);
            HasGravity = true;
            ID = id;

            allObjects.Add(this);
        }

        public override void Update(float deltaTime)
        {
            _keyboardState = Keyboard.GetState();
            isGrounded = IsGrounded();
            _state.UpdateState(isGrounded, _keyboardState, this);

            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Game1.Arial, $"On ground: {isGrounded}", new Vector2(4, 48), Microsoft.Xna.Framework.Color.WhiteSmoke); 
            spriteBatch.DrawString(Game1.Arial, $"Grid Pos: {Grid.GetGridCoordinates(Position)}", new Vector2(4, 64), Microsoft.Xna.Framework.Color.WhiteSmoke);
            spriteBatch.DrawString(Game1.Arial, $"Current state: {_state.Name}", new Vector2(4, 82), Microsoft.Xna.Framework.Color.WhiteSmoke);
        }
        private bool IsGrounded()
        {
            Vector2 hitboxPos = Position + Hitbox.Position;
            Size leftCellOnGrid = Grid.GetGridCoordinates(new Vector2(hitboxPos.X, hitboxPos.Y + Hitbox.Size.Y));
            Size rightCellOnGrid = Grid.GetGridCoordinates(new Vector2(hitboxPos.X + Hitbox.Size.X-0.01f, hitboxPos.Y + Hitbox.Size.Y));
            Cell cell1 = Grid.GetCell(leftCellOnGrid.Width, leftCellOnGrid.Height);
            Cell cell2 = Grid.GetCell(rightCellOnGrid.Width, rightCellOnGrid.Height);
            if (cell1 != null )
            {
                return cell1.Objects.Any(x => x is Tile);
            }
            else if(cell2 != null)
            {
                return cell2.Objects.Any(x => x is Tile);
            }
            return false;
        }
        private void Destroy()
        {
            allObjects.Remove(this);
            allPhysicsObjects.Remove(this);
        }
    }
}
