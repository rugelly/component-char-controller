using UnityEngine;

namespace StateMachineSimple
{ 
public class StateMachine : MonoBehaviour
{
    #region base
    // base state machine implementation starts
    private State currentState;

    private void OnEnable()
    {
        SetState(new NormalState(this));
    }

    private void Update()
    {
        currentState.Tick();
    }

    public void SetState(State state)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        currentState = state;
        gameObject.name = state.GetType().Name;

        if (currentState != null)
        {
            currentState.OnStateEnter();
        }
    }
    // base state machine implementation ends
    #endregion

    #region additional
    // added functionality
        
    #endregion
}
}
