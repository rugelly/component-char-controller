using UnityEngine;

namespace StateMachineSimple
{
public abstract class State
{
    #region base
    // base implementation
    protected StateMachine stateMachine;

    // abstract must be implemented by all inheritors
    public abstract void Tick();

    // virtuals only have to be implemented if you want
    public virtual void OnStateEnter(){}
    public virtual void OnStateExit(){}

    public State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    #endregion base


    #region addition
    // custom additions

    #endregion
}
}