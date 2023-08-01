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

        PlayerState currentState = new StoppedState();

        static StoppedState stoppedState = new StoppedState();
        static RunningState runningState = new RunningState();
        static JumpingState jumpingState = new JumpingState();
        static FallingState fallingState = new FallingState();

        public string Name
        {
            get { return currentState.StateName; }
        }

        public StateMachine()
        {
            currentState = stoppedState;
        }
        private PlayerState GetState(Vector2 velocity, bool isGrounded)
        {
            Vector2 absVel = velocity.Abs();
            if (absVel.X <= 0.1 && isGrounded == true)
            {
                return stoppedState;
            }
            if (absVel.X > 0.1 && isGrounded == true)
            {
                return runningState;
            }
            if (velocity.Y <= 0 && isGrounded == false)
            {
                return jumpingState;
            }
            if (velocity.Y > 0 && isGrounded == false)
            {
                return fallingState;
            }
            return currentState;
        }
        public void UpdateState(bool isGrounded, KeyboardState keyboardState, Player player)
        {
            PlayerState newState = GetState(player.Velocity, isGrounded);
            if(newState != currentState)
            {
                currentState.ExitState(newState, player);
                currentState = newState;
                currentState.EnterState(player);
            }
            currentState.UpdateState(keyboardState, player);
        }
    }
}
