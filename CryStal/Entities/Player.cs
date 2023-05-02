using CryStal.Engine;
using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace CryStal.Entities
{
    public class Player : PhysicsObject
    {
        Texture2D _texture;
        KeyboardState keyboardState;

        float speed;
        float jumpForce;
        Vector2 _direction;
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

        public Player(Vector2 position, float speed, float jumpForce = 100):base()
        {
            Position = position;
            this.speed = speed;
            this.jumpForce = jumpForce;
            ResetVelocity();

            Hitbox.Size = new Vector2(Game1.TileSize, Game1.TileSize);
            Hitbox.Position = Vector2.Zero;
            Drag = new Vector2(0.7f, 1.01f);
            HasGravity = true;

            allObjects.Add(this);
        }

        public override void Update(float deltaTime)
        {
            keyboardState = Keyboard.GetState();
            MovePlayer(keyboardState);

            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Microsoft.Xna.Framework.Color.BlanchedAlmond, 0f, Vector2.Zero, Game1.Scale, SpriteEffects.None, 0f);
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
                Accelerate(new Vector2(0, -jumpForce * 10));
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
                Accelerate(new Vector2(-speed, 0));
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                Accelerate(new Vector2(speed, 0));
            }
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
    }
}
