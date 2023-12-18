using CryStal.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.StateMachines.CollitionStateMachine
{
    internal abstract class CollitionState
    {
        internal abstract string Name
        {
            get;
        }
        internal abstract void HandleCollition(GameObject obj, GameObject target);
    }
}
