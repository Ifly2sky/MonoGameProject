using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryStal.StateMachines.PlayerStateMachine;
using CryStal.Entities;
using Microsoft.Xna.Framework;
using CryStal.Engine;
using Microsoft.Xna.Framework.Input;

namespace CryStal.StateMachines.PlayerStateMachine
{
    public class StateMachine
    {
        PlayerState _currentState;

        internal static StoppedState StoppedState = new StoppedState();
        internal static RunningState RunningState = new RunningState();
        internal static JumpingState JumpingState = new JumpingState();
        internal static FallingState FallingState = new FallingState();
        internal static CrouchingState CrouchingState = new CrouchingState();

        public string Name
        {
            get { return _currentState.StateName; }
        }

        public StateMachine()
        {
            _currentState = StoppedState;
        }
        public void UpdateState(bool isGrounded, KeyboardState keyboardState, Player player)
        {
            _currentState.UpdateState(keyboardState, player, out _currentState);
        }
        public void SetState(string stateName)
        {
            switch (stateName)
            {
                case "StoppedState":
                    _currentState = StoppedState;
                    break;
                case "RunningState":
                    _currentState = RunningState;
                    break;
                case "CrouchingState":
                    _currentState = CrouchingState;
                    break;
                case "JumpingState":
                    _currentState = JumpingState;
                    break;
                case "FallingState":
                    _currentState = FallingState;
                    break;
            }
        }
    }
}
