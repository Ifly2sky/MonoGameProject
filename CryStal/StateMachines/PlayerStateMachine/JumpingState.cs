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

        }
        internal override void UpdateState(KeyboardState keyboardState, Player player)
        {
            if (keyboardState.IsKeyDown(Keys.S))
            {
                player.Hitbox.Size.Y = Game1.TileSize * 0.5f;
                player.crouching = true;
            }
            else if (keyboardState.IsKeyUp(Keys.S))
            {
                player.Hitbox.Size.Y = Game1.TileSize;
                player.crouching = false;
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
