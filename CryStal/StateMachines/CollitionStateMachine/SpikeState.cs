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
    internal class SpikeState : CollitionState
    {
        internal override string Name
        {
            get { return "Spike"; }
        }
        internal override void HandleCollition(GameObject obj, GameObject target)
        {
            if(target.isKillable)
            {
                target.Unload();
            }
        }
    }
}
