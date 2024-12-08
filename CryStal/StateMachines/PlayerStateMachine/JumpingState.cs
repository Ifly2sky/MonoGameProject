using CryStal.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.StateMachines.PlayerStateMachine
{
    internal class JumpingState : PlayerState
    {
        public override string StateName
        {
            get { return nameof(JumpingState); }
        }
        internal override void EnterState(Player player)
        {
            player.ApplyImpulse(new Vector2(0, -player.jumpForce));
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
