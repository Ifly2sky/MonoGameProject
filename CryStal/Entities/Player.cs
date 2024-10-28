using CryStal.Engine;
using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using CryStal.StateMachines.PlayerStateMachine;
using nkast.Aether.Physics2D.Dynamics;

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

        Queue<float> velQueue = new Queue<float>();
        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        public Vector2 Velocity
        {
            get { return _body.LinearVelocity; }
            set { _body.LinearVelocity = value; }
        }
        public Player(World world, Vector2 position, float speed, float jumpForce = 100, string id = "P") : base(world, position, id: id)
        {
            _body.FixtureList[0].Friction = 0.1f;
            Position = position;
            this.speed = speed;
            this.jumpForce = jumpForce;

            isKillable = true;
        }

        public override void Update(float deltaTime)
        {
            _keyboardState = Keyboard.GetState();
            isGrounded = IsGrounded();
            _state.UpdateState(_keyboardState, this);

            base.Update(deltaTime);
        }
        public void Draw(SpriteBatch spriteBatch, Camera camera, Effect effect)
        {
            Vector2 drawPos = Position;
            drawPos.X -= camera.X;
            drawPos.Y -= camera.Y;
            if (drawPos.X < camera.Width &&
                drawPos.X > -Width &&
                drawPos.Y < camera.Height &&
                drawPos.Y > -Height)
            {
                effect.Parameters["PaletteTexture"].SetValue(Palette);
                spriteBatch.Draw(Texture, drawPos, null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, Game1.SCALE, SpriteEffects.None, 0f);
            }
        }
        public void DrawDebug(SpriteBatch spriteBatch)
        {
            //draw debug
            spriteBatch.DrawString(Game1.Arial, $"On ground: {isGrounded}, {_body.LinearVelocity.Y}", new Vector2(4, 48), Microsoft.Xna.Framework.Color.WhiteSmoke);
            //spriteBatch.DrawString(Game1.Arial, $"Grid Pos: {Grid.GetGridCoordinates(Position)}", new Vector2(4, 64), Microsoft.Xna.Framework.Color.WhiteSmoke);
            spriteBatch.DrawString(Game1.Arial, $"Current state: {_state.Name}", new Vector2(4, 80), Microsoft.Xna.Framework.Color.WhiteSmoke);
        }
        private bool IsGrounded()
        {
            velQueue.Enqueue(_body.LinearVelocity.Y);
            if (velQueue.Count > 3 )
            {
                velQueue.Dequeue();
            }
            foreach (float val in velQueue)
            {
                if (val > 0.0003 || val < -0.0003)
                {
                    return false;
                }
            }
            return true;
        }
        public override void Unload(World world)
        {
            Game1.OnPaletteDraw -= Draw;
            base.Unload(world);
        }
        public override void Load(World world)
        {
            _state.SetState("StoppedState");
            Game1.OnPaletteDraw += Draw;
            base.Load(world);
        }
    }
}
