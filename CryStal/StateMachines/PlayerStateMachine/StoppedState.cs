﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.StateMachines.PlayerStateMachine
{
    internal class StoppedState : PlayerState
    {
        public override string StateName
        {
            get { return nameof(StoppedState); }
        }
        internal override void EnterState()
        {

        }
        internal override void UpdateState()
        {

        }
        internal override void ExitState()
        {

        }
    }
}
