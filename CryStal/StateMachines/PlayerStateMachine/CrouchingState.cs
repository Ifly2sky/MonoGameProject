using CryStal.Engine.Models;
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
    internal class CrouchingState : PlayerState
    {
        public override string StateName => nameof(CrouchingState);

        internal override void EnterState(Player player)
        {
            player.Hitbox.Size.Y = Game1.TileSize * 0.5f;
            player.isCrouching = true;
        }

        internal override void UpdateState(KeyboardState keyboardState, Player player, out PlayerState state)
        {
            if (keyboardState.IsKeyDown(Keys.A))
            {
                player.Accelerate(new Vector2(-player.speed * 0.5f, 0));
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                player.Accelerate(new Vector2(player.speed * 0.5f, 0));
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                ExitState(StateMachine.JumpingState, player);
                StateMachine.JumpingState.EnterState(player);
                state = StateMachine.JumpingState;
                return;
            }
            if (keyboardState.IsKeyUp(Keys.S))
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
            player.Position -= new Vector2(0, player.Hitbox.Size.Y);
            player.ResetVelocityY();
            player.Hitbox.Size.Y = Game1.TileSize;
            player.isCrouching = false;
        }
    }
}
