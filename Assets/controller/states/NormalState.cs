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
    private InputReader _inputReader;
    private PlayerStats _stats;
    #endregion

    #region components to disable
    
    #endregion

    public override void OnStateEnter()
    {
        _inputReader = stateMachine.GetComponent<InputReader>();
        StatHolder temp = stateMachine.GetComponent<StatHolder>();
        _stats = temp.held;
        _motor = stateMachine.GetComponent<Motor>();
        _grounded = stateMachine.GetComponent<Grounded>();
        _jump = stateMachine.GetComponent<Jump>();

        #region change motor vals
        _motor.speed = _stats.runSpeed;
        _motor.maxVelocityChange = _stats.maxRunVelocityChange;
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
        if (!_grounded.check)
        {
            _jump.enabled = false;

            _inputReader.wasSprinting = false;
            stateMachine.SetState(new AirState(stateMachine));
        }
        else // ability to jump is enabled (note this SHOULD grounded = false and exit state)
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
            stateMachine.SetState(new CrouchState(stateMachine));
        }
        #endregion crouch goto
    }
}
