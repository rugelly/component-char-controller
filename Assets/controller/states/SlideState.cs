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
        _crouch = stateMachine.GetComponent<Crouch>();

        _motor = stateMachine.GetComponent<Motor>();
        //_motor.enabled = false;
        _motor.disabledWorkAround = true;

        // actually physics slide the player forward
        _slide = stateMachine.GetComponent<Slide>();
        _slide.duration = _stats.slideLength;
        _slide.strength = _stats.slideStrength;
        _slide.enabled = true;
    }

    public override void OnStateExit()
    {
        //_motor.enabled = true;
        _motor.disabledWorkAround = false;
    }

    public override void Tick()
    {
        if (_slide.enabled == false)
        {
            // if (_input.moveVertical > 0 && _input.wasSprinting)
            // {
            //     _crouch.crouching = false;
            //     stateMachine.SetState(new SprintState(stateMachine));
            // }
            if (_input.hasMoveInput && _crouch.hasHeadroom)
            {
                _crouch.crouching = false;
                stateMachine.SetState(new NormalState(stateMachine));
            }
            else
            {
                stateMachine.SetState(new CrouchState(stateMachine));
            }
        }
    }
}
