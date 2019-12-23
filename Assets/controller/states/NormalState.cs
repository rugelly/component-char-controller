using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSimple;

public class NormalState : State
{
    
    public NormalState(StateMachine stateMachine) : base(stateMachine)
    {}

    #region components to enable
    private Motor _motor;
    private Grounded _grounded;
    private Jump _jump;
    private Crouch _crouch;
    private InputReader _inputReader;
    private PlayerStats _stats;
    #endregion

    #region components to disable
    
    #endregion

    public override void OnStateEnter()
    {
        _inputReader = stateMachine.GetComponent<InputReader>();
        _stats = stateMachine.GetComponent<StatHolder>().held;
        _motor = stateMachine.GetComponent<Motor>();
        _grounded = stateMachine.GetComponent<Grounded>();
        _jump = stateMachine.GetComponent<Jump>();
        _crouch = stateMachine.GetComponent<Crouch>();

        #region change motor vals
        _motor.speed = _stats.runSpeed;
        _motor.accelRate = _stats.runAccelRate;
        _motor.sprintHorizontalInputReductionMult = 1f;
        #endregion

        #region set default jump vals
        _jump.direction = _stats.jumpDirection;
        _jump.strength = _stats.jumpStrength;
        #endregion
    }

    public override void OnStateExit()
    {
        #region disable components
            
        #endregion
    }

    public override void Tick()
    {
        #region jump & air goto
        // not grounded
        // turn off ability to jump
        // change to air state
        if (!_grounded.isGrounded)
        {
            _inputReader.wasSprinting = false;
            stateMachine.SetState(new AirState(stateMachine));
        }
        else if (_inputReader.jump) // ability to jump is enabled (note this SHOULD grounded = false and exit state)
        {
            _jump.enabled = true;
        }
        #endregion jump & air goto

        #region sprint goto
        // forward input and pressed sprint
        if (_inputReader.moveVertical > 0 && _inputReader.sprint)
        {
            stateMachine.SetState(new SprintState(stateMachine));
        }
        #endregion sprint goto

        #region crouch goto
        // pressed crouch
        if (_inputReader.crouch)
        {
            _crouch.toHeight = _stats.crouchHeight;
            _crouch.speedOverride = 1f;
            _crouch.enabled = true;
            stateMachine.SetState(new CrouchState(stateMachine));
        }
        #endregion crouch goto
    }
}
