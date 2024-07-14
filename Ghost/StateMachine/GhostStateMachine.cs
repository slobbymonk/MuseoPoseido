using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ghost.States
{
    public class GhostStateMachine
    {
        public GhostState CurrentEnemyState { get; set; }

        public void Initialize(GhostState startingState)
        {
            CurrentEnemyState= startingState;
            CurrentEnemyState.EnterState();
        }

        public void ChangeState(GhostState newState)
        {
            CurrentEnemyState.ExitState();
            CurrentEnemyState = newState;
            CurrentEnemyState.EnterState();
        }
    }
}