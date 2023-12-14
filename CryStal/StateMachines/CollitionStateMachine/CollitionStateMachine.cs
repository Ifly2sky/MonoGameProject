﻿using CryStal.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryStal.StateMachines.CollitionStateMachine
{
    public class CollitionStateMachine
    {
        private static ImpassableState impassableState = new ImpassableState();
        private static PassableState passableState = new PassableState();
        private static PlatformState platformState = new PlatformState();
        private static SpikeState spikeState = new SpikeState();
        private static ImpassableTileState impassableTileState = new ImpassableTileState();
        private CollitionState currentState = passableState;

        public void HandleCollition(GameObject obj, GameObject target)
        {
            currentState.HandleCollition(obj, target);
        }
        public void SetCollitonState(string CollitionType)
        {
            switch (CollitionType)
            {
                case "Passable":
                    currentState = passableState;
                    return;
                case "Impassable":
                    currentState = impassableState;
                    return;
                case "ImpassableTile":
                    currentState = impassableTileState;
                    return;
                case "Platform":
                    currentState = platformState;
                    return;
                case "Spike":
                    currentState = spikeState;
                    return;
            }
        }
    }
}
