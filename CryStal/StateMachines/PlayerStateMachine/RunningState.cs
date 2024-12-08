using CryStal.Engine.Models;
using CryStal.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CryStal.StateMachines.PlayerStateMachine
{
    internal class RunningState : PlayerState
    {
        public override string StateName
        {
            get { return nameof(RunningState); }
        }
        internal override void EnterState(Player player)
        {
            if (player.isCrouching)
            {
                //TODO: implement crouch logistics
            }
        }
        internal override void UpdateState(KeyboardState keyboardState, Player player, out PlayerState state)
        {
            if (keyboardState.IsKeyDown(Keys.A))
            {
                player.ChangeVelocity(new Vector2(-player.speed, player.Velocity.Y));
            }
            else if (keyboardState.IsKeyUp(Keys.A))
            {
                player.ChangeVelocity(new Vector2(0, player.Velocity.Y));
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                player.ChangeVelocity(new Vector2(player.speed, player.Velocity.Y));
            }
            else if (keyboardState.IsKeyUp(Keys.D))
            {
                player.ChangeVelocity(new Vector2(0, player.Velocity.Y));
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                ExitState(StateMachine.JumpingState, player);
                StateMachine.JumpingState.EnterState(player);
                state = StateMachine.JumpingState;
                return;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                ExitState(StateMachine.CrouchingState, player);
                StateMachine.CrouchingState.EnterState(player);
                state = StateMachine.CrouchingState;
                return;
            }
            if (!player.isGrounded)
            {
                ExitState(StateMachine.FallingState, player);
                StateMachine.FallingState.EnterState(player);
                state = StateMachine.FallingState;
                return;
            }
            if (player.Velocity.X == 0)
            {
                ExitState(StateMachine.StoppedState, player);
                StateMachine.StoppedState.EnterState(player);
                state = StateMachine.StoppedState;
                return;
            }
            state = this;
        }
        internal override void ExitState(PlayerState newState, Player player)
        {

        }
    }
}
