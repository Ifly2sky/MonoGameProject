using CryStal.Entities;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.StateMachines.PlayerStateMachine
{
    internal abstract class PlayerState
    {
        public abstract string StateName
        {
            get;
        }
        internal abstract void EnterState(Player player);
        internal abstract void UpdateState(KeyboardState keyboardState, Player player, out PlayerState state);
        internal abstract void ExitState(PlayerState newState, Player player);
    }
}
