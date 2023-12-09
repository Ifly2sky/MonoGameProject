using CryStal.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.StateMachines.CollitionStateMachine
{
    internal class PassableState : CollitionState
    {
        internal override void HandleCollition(GameObject obj, GameObject target)
        {
            //nothing
        }
    }
}
