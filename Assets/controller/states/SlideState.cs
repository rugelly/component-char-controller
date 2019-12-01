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
    private InputReader _input;
    private Motor _motor;
    private Crouch _crouch;
    private Slide _slide;
    #endregion

    public override void OnStateEnter()
    {
        _stats = stateMachine.GetComponent<StatHolder>().held;
        _input = stateMachine.GetComponent<InputReader>();

        // turn off motor
        _motor = stateMachine.GetComponent<Motor>();
        _motor.enabled = false;

        // crouch but with slide stats
        _crouch = stateMachine.GetComponent<Crouch>();
        _crouch.toHeight = _stats.crouchHeight;
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

        // un crouch you can see below its kind of hacky but itll work for now
        // TODO: can "un crouching be handled at the entrance of each state?"
        // SHOULD IT BE? maybe not
        
    }

    public override void Tick()
    {
        if (_slide.enabled == false)
        {
            if (_input.moveVertical > 0 && _input.wasSprinting)
            {
                _crouch.toHeight = _stats.standHeight;
                _crouch.transitionCurve = _stats.crouchTransitionCurve;
                _crouch.enabled = true;
                stateMachine.SetState(new SprintState(stateMachine));
            }
            else if (_input.hasMoveInput)
            {
                _crouch.toHeight = _stats.standHeight;
                _crouch.transitionCurve = _stats.crouchTransitionCurve;
                _crouch.enabled = true;
                stateMachine.SetState(new NormalState(stateMachine));
            }
            else
            {
                stateMachine.SetState(new CrouchState(stateMachine));
            }
        }
    }
}
