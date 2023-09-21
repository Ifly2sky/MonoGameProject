﻿using CryStal.Engine.Models;
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
