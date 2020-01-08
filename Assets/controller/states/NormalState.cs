using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSimple;

public class NormalState : State
{
    
    public NormalState(StateMachine stateMachine) : base(stateMachine)
    {}

    private Motor _motor;
    private Grounded _grounded;
    private FallDamage _falldmg;
    private Crouch _crouch;
    private InputReader _inputReader;
    private PlayerStats _stats;

    public override void OnStateEnter()
    {
        _inputReader = stateMachine.GetComponent<InputReader>();
        _stats = stateMachine.GetComponent<StatHolder>().held;
        _motor = stateMachine.GetComponent<Motor>();
        _crouch = stateMachine.GetComponent<Crouch>();
        _grounded = stateMachine.GetComponent<Grounded>();
        _falldmg = stateMachine.GetComponent<FallDamage>();

        #region change motor vals
        _motor.speed = _stats.runSpeed;
        _motor.accelRate = _stats.runAccelRate;
        _motor.sprintHorizontalInputReductionMult = 1f;
        #endregion
    }

    public override void OnStateExit()
    {
        #region disable components
            
        #endregion
    }

    public override void Tick()
    {
        if (_falldmg.landingFirm || _falldmg.landingHard || _falldmg.landingSplat)
            stateMachine.SetState(new HardLandingState(stateMachine));
        
        #region jump & air goto
        // not grounded
        // turn off ability to jump
        // change to air state
        if (!_grounded.isGrounded)
        {
            _inputReader.wasSprinting = false;
            stateMachine.SetState(new AirState(stateMachine));
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
            _crouch.crouching = true;
            stateMachine.SetState(new CrouchState(stateMachine));
        }
        #endregion crouch goto
    }
}
