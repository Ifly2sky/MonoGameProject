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
            if(!target.isKillable)
            {
                return;
            }
            if (target.Position.X + target.Hitbox.Left > obj.Position.X + obj.Hitbox.Right ||
                target.Position.X + target.Hitbox.Right < obj.Position.X + obj.Hitbox.Left ||
                target.Position.Y + target.Hitbox.Bottom < obj.Position.Y + obj.Hitbox.Top ||
                target.Position.Y + target.Hitbox.Top > obj.Position.Y + obj.Hitbox.Bottom)
            {
                return;
            }
             target.SetDead();
        }
    }
}
