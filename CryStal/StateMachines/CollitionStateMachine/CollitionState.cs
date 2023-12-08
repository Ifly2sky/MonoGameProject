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
        internal abstract void OnCollided(GameObject obj, GameObject target);
    }
}
