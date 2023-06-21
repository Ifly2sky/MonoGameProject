using System;
using System.Collections.Generic;
using System.Linq;
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
        internal abstract void EnterState();
        internal abstract void UpdateState();
        internal abstract void ExitState();
    }
}
