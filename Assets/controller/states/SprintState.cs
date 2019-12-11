using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSimple;

public class SprintState : State
{
    
    public SprintState(StateMachine stateMachine) : base(stateMachine)
    {}

    #region components to enable
    private Motor _motor;
    private PlayerStats _stats;
    private Grounded _grounded;
    private Jump _jump;
    private Crouch _crouch;
    private InputReader _inputReader;
    #endregion

    #region components to disable
    
    #endregion

    public override void OnStateEnter()
    {
        _stats = stateMachine.GetComponent<StatHolder>().held;
        _motor = stateMachine.GetComponent<Motor>();
        _inputReader = stateMachine.GetComponent<InputReader>();
        _grounded = stateMachine.GetComponent<Grounded>();
        _jump = stateMachine.GetComponent<Jump>();
        _crouch = stateMachine.GetComponent<Crouch>();

        #region change motor vals
        _motor.speed = _stats.sprintSpeed;
        _motor.maxVelocityChange = _stats.maxSprintVelocityChange;
        _motor.sprintHorizontalInputReductionMult = _stats.sprintHorizontalInputReduction;
        #endregion
    }

    public override void OnStateExit()
    {
        
    }

    public override void Tick()
    {
        #region jump & air goto
        // not grounded
        // turn off ability to jump
        // change to air state
        if (!_grounded.check)
        {
            _inputReader.wasSprinting = true;
            stateMachine.SetState(new AirState(stateMachine));
        }
        else if(_inputReader.jump) // ability to jump is enabled (note this SHOULD grounded = false and exit state)
        {
            _jump.enabled = true;
        }
        #endregion jump

        #region normal goto
        // not holding down forward
        if (_inputReader.moveVertical <= 0)
        {
            stateMachine.SetState(new NormalState(stateMachine));
        }
        #endregion normal goto

        #region slide goto
        // pressed crouch while sprinting
        if (_inputReader.crouch)
        {
            _crouch.toHeight = _stats.crouchHeight;
            _crouch.speedOverride = _stats.crouchTime / 1.5f;
            _crouch.enabled = true;
            stateMachine.SetState(new SlideState(stateMachine));
        }
        #endregion slide goto
    }
}
