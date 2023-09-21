using CryStal.Entities;
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
            throw new NotImplementedException();
        }

        internal override void ExitState(PlayerState newState, Player player)
        {
            throw new NotImplementedException();
        }

        internal override void UpdateState(KeyboardState keyboardState, Player player)
        {
            throw new NotImplementedException();
        }
    }
}
