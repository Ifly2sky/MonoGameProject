using CryStal.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CryStal.StateMachines.PlayerStateMachine
{
    internal class StoppedState : PlayerState
    {
        public override string StateName
        {
            get { return nameof(StoppedState); }
        }
        internal override void EnterState(Player player)
        {
            if (player.crouching)
            {
                player.Position -= new Vector2(0, player.Hitbox.Size.Y * 0.5f);
                player.ResetVelocityY();
                player.Hitbox.Size.Y = Game1.TileSize;
            }
            //-TODO fix crouch bounce bug
        }
        internal override void UpdateState(KeyboardState keyboardState, Player player)
        {
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                player.ResetVelocityY();
                player.Accelerate(new Vector2(0, -player.jumpForce * 10));
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                player.Hitbox.Size.Y = Game1.TileSize * 0.5f;
                player.Hitbox.Position.Y = Game1.TileSize * 0.5f;
            }
            else if (keyboardState.IsKeyUp(Keys.S))
            {
                player.Hitbox.Size.Y = Game1.TileSize;
                player.Hitbox.Position.Y = 0;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                player.Accelerate(new Vector2(-player.speed, 0));
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                player.Accelerate(new Vector2(player.speed, 0));
            }
        }
        internal override void ExitState(PlayerState newState, Player player)
        {
            
        }
    }
}
