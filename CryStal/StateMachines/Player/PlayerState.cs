using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.StateMachines.Player
{
    public abstract class PlayerState
    {
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
    }
}
