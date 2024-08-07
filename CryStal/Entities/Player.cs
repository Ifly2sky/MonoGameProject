﻿using CryStal.Engine;
using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using CryStal.StateMachines.PlayerStateMachine;

namespace CryStal.Entities
{
    public class Player : PhysicsObject
    {
        public Texture2D Texture;
        public Texture2D Palette;
        KeyboardState _keyboardState;
        StateMachine _state = new StateMachine();

        public float speed;
        public float jumpForce;
        Vector2 _direction;
        public bool isGrounded;
        public bool isCrouching;
        public string stateName => _state.Name;
        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public Player(Vector2 position, float speed, float jumpForce = 100, string id = "P"):base()
        {
            Position = position;
            this.speed = speed;
            this.jumpForce = jumpForce;
            ResetVelocity();

            Hitbox.Size = new Vector2(Game1.TILESIZE, Game1.TILESIZE);
            Hitbox.Position = Vector2.Zero;
            Drag = new Vector2(0, 1.00f);
            HasGravity = true;
            isKillable = true;
            ID = id;
            CollitionHandler.SetCollitonState("Impassable");

            allObjects.Add(this);
        }

        public override void Update(float deltaTime)
        {
            if (IsAlive)
            {
                Physics.OnPhysicsFinalize -= Unload;
            }
            else
            {
                Physics.OnPhysicsFinalize += Unload;
            }
            _keyboardState = Keyboard.GetState();
            isGrounded = IsGrounded();
            _state.UpdateState(isGrounded, _keyboardState, this);

            base.Update(deltaTime);
        }
        public void Draw(SpriteBatch spriteBatch, Camera camera, Effect effect)
        {
            Vector2 drawPos = Position;
            drawPos.X -= camera.X;
            drawPos.Y -= camera.Y;
            if (drawPos.X < camera.Width &&
                drawPos.X > -Hitbox.Size.X &&
                drawPos.Y < camera.Height &&
                drawPos.Y > -Hitbox.Size.Y)
            {
                effect.Parameters["PaletteTexture"].SetValue(Palette);
                spriteBatch.Draw(Texture, drawPos, null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, Game1.SCALE, SpriteEffects.None, 0f);
            }

            //draw debug
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
                return cell1.Objects.Any(x => x.CollitionHandler.GetCollitionStateName() != "Passable" && x != this);
            }
            else if(cell2 != null)
            {
                return cell2.Objects.Any(x => x.CollitionHandler.GetCollitionStateName() != "Passable" && x != this);
            }
            return false;
        }
        public override void Unload()
        {
            Game1.OnPaletteDraw -= Draw;
            base.Unload();
        }
        public override void Load()
        {
            _state.SetState("StoppedState");
            Game1.OnPaletteDraw += Draw;
            base.Load();
        }
    }
}
