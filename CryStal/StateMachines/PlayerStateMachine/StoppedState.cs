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
            
        }
        internal override void UpdateState(KeyboardState keyboardState, Player player, out PlayerState state)
        {
            if (keyboardState.IsKeyDown(Keys.A))
            {
                player.Accelerate(new Vector2(-player.speed, 0));
                state = StateMachine.RunningState;
                return;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                player.Accelerate(new Vector2(player.speed, 0));
                state = StateMachine.RunningState;
                return;
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
            if (player.Velocity.Y > 0)
            {
                ExitState(StateMachine.FallingState, player);
                StateMachine.FallingState.EnterState(player);
                state = StateMachine.FallingState;
                return;
            }
            state = this;
        }
        internal override void ExitState(PlayerState newState, Player player)
        {
            
        }
    }
}
