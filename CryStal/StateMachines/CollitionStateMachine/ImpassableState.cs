using CryStal.Engine;
using CryStal.Engine.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.StateMachines.CollitionStateMachine
{
    internal class ImpassableState:CollitionState
    {
        internal override void OnCollided(GameObject obj, GameObject target)
        {
            
        }
    }
}
