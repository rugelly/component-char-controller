using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSimple;

public class SlideState : State
{
    public SlideState(StateMachine stateMachine) : base(stateMachine)
    {}

    #region components
    private PlayerStats _stats;
    private Motor _motor;
    private Crouch _crouch;
    private Slide _slide;
    #endregion

    public override void OnStateEnter()
    {
        _stats = stateMachine.GetComponent<StatHolder>().held;

        // turn off motor
        _motor = stateMachine.GetComponent<Motor>();
        _motor.enabled = false;

        // crouch but with slide stats
        _crouch = stateMachine.GetComponent<Crouch>();
        _crouch.isCrouching = true; // we want to go from standing, to crouch
        _crouch.transitionCurve = _stats.sprintTransitionCurve;
        _crouch.enabled = true;

        // actually physics slide the player forward
        _slide = stateMachine.GetComponent<Slide>();
        _slide.enabled = true;
    }

    public override void OnStateExit()
    {
        // motor re enabled
        _motor.enabled = true;
    }

    public override void Tick()
    {
        // go to crouch down state when crouch component turns itself off
        if (!_crouch.enabled)
        {
            stateMachine.SetState(new CrouchState(stateMachine));
        }
    }
}
